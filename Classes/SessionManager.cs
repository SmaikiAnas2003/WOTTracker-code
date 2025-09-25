using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using WOTTracker.Models;

namespace WOTTracker
{
    public static class SessionManager
    {
        private static IConfiguration config;
        private static string _databasePath;

        public static event Action<List<DownTimeRecord>, int, IConfiguration> OnRecoveryNeeded;


        // On initialise le manager avec le chemin de la BDD.
        public static void Initialize(string databasePath, IConfiguration _config)
        {
            _databasePath = databasePath;
            config = _config;
        }

        public static void OnSleep()
        {
            Log.Information("Processing the 'OnSleep' event.");
            EndCurrentSession();
        }

        public static void OnShutDown()
        {
            Log.Information("Processing the 'OnShutDown' event.");
            EndCurrentSession();
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// Trouve la session de travail active, la clôture en y ajoutant une EndTime,
        /// la découpe par jour, et calcule l'Overtime.
        /// </summary>
        private static void EndCurrentSession()
        {
            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // Trouver la session active (EndTime est NULL)
                    var activeSession = db.WorkSessions.FirstOrDefault(s => s.EndTime == null);


                    if (activeSession == null)
                    {
                        Log.Information("No active session to close.");
                        return;
                    }

                    // Récupérer la config associée à cette session
                    if (config == null)
                    {
                        Log.Error("Unable to close session {Id}: the associated configuration cannot be found.", activeSession.Id);
                        return;
                    }

                    // Mettre à jour l'enregistrement et sauvegarder
                    // La méthode ci-dessous va gérer le découpage, le calcul de l'overtime, etc.
                    String Notes = "session normale";
                    ProcessWorkSegment(activeSession.StartTime, DateTime.Now, db, activeSession, Notes);

                    db.SaveChanges();
                    Log.Information("Session (Id: {Id}) successfully closed at {EndTime}", activeSession.Id, activeSession.EndTime);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical error during the closure of the work session.");
            }
        }

        public static void OnWakeUp()
        {
            Log.Information("Wake-up detected: recovering sessions...");
            RecoverSessions();
        }

        /// <summary>
        /// Méthode principale qui orchestre la récupération des sessions manquantes
        /// en analysant les événements système.
        /// </summary>
        public static void RecoverSessions()
        {
            Log.Information("Start of the session recovery procedure.");
            using (var db = new YotTrackerDbContext(_databasePath))
            {
                DateTime cursor = GetLastProcessedTime(db);
                var listEvents = SystemEventManager.GetEventsSince(cursor);



            if (!listEvents.Any())
            {
                Log.Information("No new system events to process for recovery.");
                return;
            }

            
                if (config == null) return;

                var sessionToUpdate = db.WorkSessions.FirstOrDefault(s => s.EndTime == null);

                foreach (var ev in listEvents)
                {
                    DateTime segmentStart = cursor;
                    DateTime segmentEnd = ev.Timestamp;

                    String Notes = "session récupérée";

                    if (ev.EventType == "Veille" || ev.EventType == "Arrêt")
                    {
                        // Le PC était allumé. C'est une période de travail à analyser.
                        ProcessWorkSegment(segmentStart, segmentEnd,db,sessionToUpdate, Notes);
                        sessionToUpdate = null;
                    }
                    else // L'événement est "Réveil" ou "Démarrage"
                    {
                        // Le PC était éteint/en veille. C'est une période d'inactivité (DownTime).
                        ProcessDownTimeSegment(segmentStart, segmentEnd, db);
                    }

                    cursor = ev.Timestamp;
                }

                // Après avoir traité tous les événements, on crée la nouvelle session active
                var lastEvent = listEvents.Last();
                if (lastEvent.EventType == "Réveil" || lastEvent.EventType == "Démarrage")
                {
                    StartNewActiveSession(lastEvent.Timestamp, db);
                }
            }
        }

        private static void ProcessWorkSegment(DateTime segmentStart, DateTime segmentEnd, YotTrackerDbContext db, WorkSession sessionToUpdate, String Notes)
        {
                DateTime startTimeToProcess;
                DateTime endTimeToProcess = segmentEnd;

                if (sessionToUpdate != null)
                {
                    // Cas 1: On finalise une session qui était déjà en cours (après un plantage)
                    Log.Information("Finalization of an active orphan session (Id: {Id})", sessionToUpdate.Id);
                    startTimeToProcess = sessionToUpdate.StartTime;
                    
                }
                else
                {
                    // Cas 2: On traite un nouveau segment de travail détecté entre les événements
                    startTimeToProcess = segmentStart;
                }
                if ((endTimeToProcess - startTimeToProcess).TotalMinutes < 1)
                {
                    Log.Warning("Session ignored because it is too short (<1min) : {Start} → {End}", startTimeToProcess, endTimeToProcess);
                    return;
                }
                var lastSession = db.WorkSessions
                                .OrderByDescending(s => s.EndTime)
                                .FirstOrDefault();

            if (lastSession != null && lastSession.EndTime.HasValue &&
                Math.Abs((startTimeToProcess - lastSession.EndTime.Value).TotalMinutes) < 1)
            {
                // Fusion avec la session précédente
                lastSession.EndTime = endTimeToProcess;
                CalculateAndSetOvertime(lastSession, db);
                Log.Information("Session too short merged with the previous one. New end. : {End}", lastSession.EndTime);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error merging the short session.");
                }
                return;
            }

            // --- Logique Unifiée de Découpage et Sauvegarde ---

            var activeConfig = db.AppSettingsHistory.FirstOrDefault(c => c.isActive);
            if (activeConfig == null)
            {
                Log.Error("Unable to process the work segment: no active configuration found.");
                    return;
            }

            // On parcourt chaque jour contenu dans l'intervalle
                for (var day = startTimeToProcess.Date; day <= endTimeToProcess.Date; day = day.AddDays(1))
                {
                    DateTime startOfSegmentForDay = (day == startTimeToProcess.Date) ? startTimeToProcess : day;
                    DateTime endOfSegmentForDay = (day < endTimeToProcess.Date) ? day.AddDays(1).AddTicks(-1) : endTimeToProcess;

                    WorkSession currentSegment;
                    
                    if(sessionToUpdate!= null)
                    {
                        sessionToUpdate.EndTime = endOfSegmentForDay;
                        sessionToUpdate.Notes = Notes;
                        CalculateAndSetOvertime(sessionToUpdate, db);
                        sessionToUpdate = null;

                    }
                    else
                    {
                    currentSegment = new WorkSession
                    {
                        StartTime = startOfSegmentForDay,
                        AppSettingsVersionId = activeConfig.VersionId,
                        EndTime = endOfSegmentForDay,
                        Notes = "Session récupérée"
                    };

                        // Calcul et assignation de l'overtime et du type
                        CalculateAndSetOvertime(currentSegment, db);
                        db.WorkSessions.Add(currentSegment);
                    }


            }
            try
            {
                db.SaveChanges();
                Log.Information("Work segment from {Start} to {End} processed and saved.", startTimeToProcess, endTimeToProcess);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to back up the work segment.");
            }


        }

        /// <summary>
        /// Analyse un intervalle d'inactivité et crée un DownTimeRecord si nécessaire.
        /// </summary>
        private static DateTime GetLastProcessedTime(YotTrackerDbContext db)
        {
          
        var activeSession = db.WorkSessions.FirstOrDefault(s => s.EndTime == null);
                    if (activeSession != null) return activeSession.StartTime;

                    var lastCompleted = db.WorkSessions.OrderByDescending(s => s.Id).FirstOrDefault();
                    if (lastCompleted != null) return lastCompleted.EndTime.Value;

            return DateTime.Today.AddDays(-4); // Cas du tout premier lancement
        } 

         /// <summary>
         /// Analyse un intervalle d'inactivité (où le PC était éteint ou en veille)
         /// et crée un DownTimeRecord si cet intervalle chevauche les heures de travail.
         /// </summary>
        private static void ProcessDownTimeSegment(DateTime segmentStart, DateTime segmentEnd, YotTrackerDbContext db)
        {
                // On récupère la configuration active pour cette période
                var activeConfig = db.AppSettingsHistory.FirstOrDefault(c => c.isActive);
                if (activeConfig == null)
                {
                    Log.Error("Unable to process the DownTime: no active configuration found.");
                    return;
                }

                // On découpe la période d'inactivité par jour
                for (var day = segmentStart.Date; day <= segmentEnd.Date; day = day.AddDays(1))
                {
                    // On vérifie si ce jour est un jour de travail (ni week-end, ni férié)
                    if (!IsWorkDay(day, db))
                    {
                        continue; // Si ce n'est pas un jour de travail, on passe au jour suivant
                    }

                    // Définir l'intervalle d'inactivité pour CE jour précis
                    DateTime startOfInterval = (day == segmentStart.Date) ? segmentStart : day;
                    DateTime endOfInterval = (day < segmentEnd.Date) ? day.AddDays(1).AddTicks(-1) : segmentEnd;

                    // Définir les heures de travail pour CE jour précis
                    DateTime workDayStart = day.Date.Add(TimeSpan.Parse(config["WorkingHours:Start"]));
                    DateTime workDayEnd = day.Date.Add(TimeSpan.Parse(config["WorkingHours:End"]));

                    // Définir les heures de pause déjeuner
                    TimeSpan breakStart = TimeSpan.Parse(config["BreakHours:Start"]);
                    TimeSpan breakEnd = TimeSpan.Parse(config["BreakHours:End"]);
                    DateTime breakStartTime = day.Date + breakStart;
                    DateTime breakEndTime = day.Date + breakEnd;

                    // --- Calcul de l'intersection (le cœur de la logique) ---
                    DateTime intersectionStart = (startOfInterval > workDayStart) ? startOfInterval : workDayStart;
                    DateTime intersectionEnd = (endOfInterval < workDayEnd) ? endOfInterval : workDayEnd;

                // S'il y a un chevauchement valide (plus d'une minute)
                if (intersectionEnd > intersectionStart && (intersectionEnd - intersectionStart).TotalMinutes > 1)
                {
                    // Si chevauchement total dans la pause → rien à faire
                    if (intersectionStart >= breakStartTime && intersectionEnd <= breakEndTime)
                        continue;

                    // Si chevauchement partiel ou total, on crée jusqu’à deux segments exclusifs
                    List<(DateTime start, DateTime end)> segments = new List<(DateTime start, DateTime end)>();

                    if (intersectionStart < breakStartTime)
                    {
                        DateTime end = (intersectionEnd <= breakStartTime) ? intersectionEnd : breakStartTime;
                        if ((end - intersectionStart).TotalMinutes > 1)
                            segments.Add((intersectionStart, end));
                    }

                    if (intersectionEnd > breakEndTime)
                    {
                        DateTime start = (intersectionStart >= breakEndTime) ? intersectionStart : breakEndTime;
                        if ((intersectionEnd - start).TotalMinutes > 1)
                            segments.Add((start, intersectionEnd));
                    }

                    foreach (var (segStart, segEnd) in segments)
                    {
                        var downTimeRecord = new DownTimeRecord
                        {
                            AppSettingsVersionId = activeConfig.VersionId,
                            StartTime = segStart,
                            EndTime = segEnd,
                        };

                        var lastDownTime = db.DownTimeRecords
                            .OrderByDescending(d => d.EndTime)
                            .FirstOrDefault();

                        if (lastDownTime != null && lastDownTime.EndTime >= segStart.AddMinutes(-1))
                        {
                            lastDownTime.EndTime = (segEnd > lastDownTime.EndTime) ? segEnd : lastDownTime.EndTime;
                            db.Update(lastDownTime);
                            Log.Information("DownTime merged with the previous recording. New ending. : {End}", lastDownTime.EndTime);
                        }
                        else
                        {
                            db.DownTimeRecords.Add(downTimeRecord);
                            Log.Information("Downtime detected and recorded from {Start} à {End}", segStart, segEnd);
                        }
                    }
                }

            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save the DownTime records.");
            }

            // On sauvegarde tous les DownTimeRecords créés pendant la boucle

        }

        /// <summary>
        /// Fonction d'aide pour déterminer si une date est un jour de travail.
        /// </summary>
        private static bool IsWorkDay(DateTime date, YotTrackerDbContext db)
        {
            // C'est un jour de travail s'il ne tombe PAS un week-end ET s'il n'est PAS un jour férié.
            DayOfWeek day = date.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            {
                return false;
            }

            bool isHoliday = db.FixedPublicHolidays.Any(h => h.Month == date.Month && h.Day == date.Day) ||
                             db.MovablePublicHolidays.Any(h => h.Month == date.Month && h.Day == date.Day);

            return !isHoliday;
        }

        /// <summary>
        /// Crée une nouvelle session active (avec EndTime à NULL).
        /// </summary>
        private static void StartNewActiveSession(DateTime startTime, YotTrackerDbContext db)
        {
                // On s'assure qu'il n'y a pas d'autre session active
                var existingActive = db.WorkSessions.FirstOrDefault(s => s.EndTime == null);
                if (existingActive == null)
                {
                    var lastSession = db.WorkSessions
                                .OrderByDescending(s => s.EndTime)
                                .FirstOrDefault();
                if (lastSession != null && lastSession.EndTime.HasValue &&
                    Math.Abs((startTime - lastSession.EndTime.Value).TotalMinutes) < 1)
                {
                    // Fusion avec la session précédente
                    lastSession.EndTime = null;
                }
                else
                {
                    var activeConfig = db.AppSettingsHistory.FirstOrDefault(c => c.isActive);
                    var newSession = new WorkSession
                    {
                        StartTime = startTime,
                        EndTime = null,
                        AppSettingsVersionId = activeConfig?.VersionId ?? 0
                    };
                    db.WorkSessions.Add(newSession);
                }
                    db.SaveChanges();
                }
        }

        /// <summary>
        /// Calcule l'Overtime pour une session et met à jour ses propriétés.
        /// </summary>
        private static void CalculateAndSetOvertime(WorkSession session, YotTrackerDbContext db)
        {
            TimeSpan workStart = TimeSpan.Parse(config["WorkingHours:Start"]);
            TimeSpan workEnd = TimeSpan.Parse(config["WorkingHours:End"]);
            TimeSpan breakStart = TimeSpan.Parse(config["BreakHours:Start"]);
            TimeSpan breakEnd = TimeSpan.Parse(config["BreakHours:End"]);

            DateTime workStartTime = session.StartTime.Date + workStart;
            DateTime workEndTime = session.StartTime.Date + workEnd;
            DateTime breakStartTime = session.StartTime.Date + breakStart;
            DateTime breakEndTime = session.StartTime.Date + breakEnd;

            int overtimeMinutes = 0;
            string overtimeType = GetOvertimeType(session.StartTime.Date, db);

            if (overtimeType != "NormalDays")
            {
                overtimeMinutes = (int)(session.EndTime.Value - session.StartTime).TotalMinutes;
            }
            else
            {
                // Heures sup avant le début
                if (session.StartTime < workStartTime)
                {
                    // CORRECTION : On trouve la date de fin de cette période
                    DateTime endOfMorningOvertime = session.EndTime.Value < workStartTime ? session.EndTime.Value : workStartTime;
                    overtimeMinutes += (int)(endOfMorningOvertime - session.StartTime).TotalMinutes;
                }


                // Pendant la pause déjeuner
                if (session.StartTime < breakEndTime && session.EndTime.Value > breakStartTime)
                {
                    DateTime breakOverlapStart = session.StartTime > breakStartTime ? session.StartTime : breakStartTime;
                    DateTime breakOverlapEnd = session.EndTime.Value < breakEndTime ? session.EndTime.Value : breakEndTime;

                    overtimeMinutes += (int)(breakOverlapEnd - breakOverlapStart).TotalMinutes;
                }

                // Heures sup après la fin
                if (session.EndTime.Value > workEndTime)
                {
                    // CORRECTION : On trouve la date de début de cette période
                    DateTime startOfEveningOvertime = session.StartTime > workEndTime ? session.StartTime : workEndTime;
                    overtimeMinutes += (int)(session.EndTime.Value - startOfEveningOvertime).TotalMinutes;
                }
            }

            session.OverTimeMinutes = (int)Math.Round((double)overtimeMinutes);
            session.TypeOverTime = overtimeType;
        }



        public static string GetOvertimeType(DateTime date, YotTrackerDbContext db)
        {
            // On vérifie d'abord si c'est un jour férié
            bool isHoliday = db.FixedPublicHolidays.Any(h => h.Month == date.Month && h.Day == date.Day) ||
                             db.MovablePublicHolidays.Any(h => h.Month == date.Month && h.Day == date.Day);

            if (isHoliday)
            {
                return "PublicHolidays";
            }

            // Ensuite, on vérifie si c'est un week-end
            if (date.DayOfWeek == DayOfWeek.Saturday) return "Saturdays";
            if (date.DayOfWeek == DayOfWeek.Sunday) return "Sundays";

            // Sinon, c'est un jour normal
            return "NormalDays";
        }

        // (Toujours dans SessionManager.cs)

        /// <summary>
        /// Vérifie s'il y a des absences à traiter et, si c'est le cas,
        /// délègue l'affichage à la MainForm via un événement.
        /// </summary>
        public static void CheckForDownTimeRecovery(IConfiguration config)
        {
            using (var db = new YotTrackerDbContext(_databasePath))
            {
                // 1. On charge les données nécessaires.
                var downTimesNotResolved = GetUnresolvedDownTimes(db);
                var overTimesAvailable = GetAvailableOverTimeSessions(db);

                // 2. On vérifie si une action est requise.
                if (downTimesNotResolved.Any() && overTimesAvailable.Any())
                {
                    Log.Information("Unresolved Downtimes and available OverTime. Displaying the recovery interface.");

                    // 3. On déclenche un événement pour que le MainForm affiche le UserControl.
                    // Le MainForm s'abonnera à cet événement.
                    OnRecoveryNeeded?.Invoke(downTimesNotResolved, overTimesAvailable.Sum(s => s.OverTimeMinutes) ?? 0, config);
                }
                else
                {
                    Log.Information("No DownTime to recover or no OverTime balance available.");
                }
            }
        }


        public static List<DownTimeRecord> GetUnresolvedDownTimes(YotTrackerDbContext db)
        {
            DateTime now = DateTime.Now;
            DateTime monthStart = new DateTime(now.Year, now.Month, 1);

            var downTimes = db.DownTimeRecords
                .Where(d =>
                    d.IsPermission != 1 &&
                    d.IsConge != 1 &&
                    d.StartTime >= monthStart)
                .ToList();

            var compensationSums = db.Compensations
            .Where(c => db.DownTimeRecords.Any(d =>
                d.Id == c.DownTimeRecordId &&
                d.IsPermission != 1 &&
                d.IsConge != 1 &&
                d.StartTime >= monthStart))
            .GroupBy(c => c.DownTimeRecordId)
            .ToDictionary(g => g.Key, g => g.Sum(c => (int)(c.CompensationMinutes)));

            var unresolved = downTimes
                .Where(d =>
                {
                    int durationMinutes = (int)(d.EndTime - d.StartTime).TotalMinutes;
                    return compensationSums.TryGetValue(d.Id, out int totalComp)
                        ? durationMinutes > totalComp
                        : true; // Aucun enregistrement de compensation → non résolu
                })
                .ToList();

            return unresolved;
        }


        public static List<WorkSession> GetAvailableOverTimeSessions(YotTrackerDbContext db)
        {
            var now = DateTime.Now;

            // ÉTAPE 1 : Récupérer les IDs de toutes les configurations dont l'overtime n'a pas expiré.
            // C'est beaucoup plus efficace que de faire la jointure dans la requête principale.
            var validConfigIds = db.AppSettingsHistory
                .Where(a => a.ExpirationOverTime > now)
                .Select(a => a.VersionId)
                .ToList();

            // ÉTAPE 2 : Récupérer toutes les sessions de "NormalDays" qui ont de l'overtime et qui
            // sont liées à une configuration valide.
            var potentialOvertimeSessions = db.WorkSessions
            .AsNoTracking()
            .Where(w => w.OverTimeMinutes > 0 &&
                        w.TypeOverTime == "NormalDays" &&
                        validConfigIds.Contains(w.AppSettingsVersionId))
            .ToList();


            // ÉTAPE 3 : Récupérer toutes les compensations pertinentes en une seule requête.
            var allCompensations = db.Compensations
                .ToList()
                .GroupBy(c => c.WorkSessionId)
                .ToDictionary(g => g.Key, g => g.Sum(c => c.CompensationMinutes));

            // ÉTAPE 4 : Filtrer la liste finale en mémoire.
            // C'est très rapide car toutes les données sont déjà chargées.
            var availableSessions = new List<WorkSession>();
            foreach (var session in potentialOvertimeSessions)
            {
                int totalCompensated = 0;
                if (allCompensations.ContainsKey(session.Id))
                {
                    totalCompensated = allCompensations[session.Id];
                }

                if (session.OverTimeMinutes > totalCompensated)
                {
                    availableSessions.Add(session);
                    availableSessions[availableSessions.Count - 1].OverTimeMinutes -= totalCompensated;
                }
            }

            return availableSessions;
        }

        public static bool HasPendingDownTime(string databasePath)
        {
            using (var db = new YotTrackerDbContext(databasePath))
            {
                return GetUnresolvedDownTimes(db).Any();
            }
        }

        public static bool HasPendingOverTime(string databasePath)
        {
            using (var db = new YotTrackerDbContext(databasePath))
            {
                return GetAvailableOverTimeSessions(db).Any();
            }
        }



    }
}
