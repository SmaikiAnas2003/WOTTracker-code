using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WOTTracker.Models;

namespace WOTTracker
{
    public partial class CompensationDownTime : UserControl
    {
        // Variables pour stocker les données nécessaires au formulaire
        private List<DownTimeRecord> _unresolvedDownTimes;
        private int _availableOvertime;
        private IConfiguration _config;
        private string _databasePath;

        private DownTimeRecord _selectedDownTimeForPermission; // Stockage temporaire

        private bool isPanel10Closed = false;


        public event Action OnClose;

        public CompensationDownTime(List<DownTimeRecord> unresolvedDownTimes, int availableOvertime, IConfiguration config, string databasePath)
        {
            InitializeComponent();

            panel10.Visible = false;


            // On stocke les données passées en paramètre
            _unresolvedDownTimes = unresolvedDownTimes;
            _availableOvertime = availableOvertime;
            _config = config;
            _databasePath = databasePath;

            // On attache les gestionnaires d'événements aux boutons
            this.Load += RecoveryForm_Load;
            this.btnMarkAsLeave.Click += BtnMarkAsLeave_Click;
            this.btnJustifyPermission.Click += BtnJustifyPermission_Click;
            this.btnRecover.Click += BtnRecover_Click;

            this.dataGridViewDownTimes.SelectionChanged += DataGridViewDownTimes_SelectionChanged;

            panel3.Visible = false; // On cache le panel au démarrage

        }

        private void UpdateSelectedDuration()
        {
            var selectedRows = dataGridViewDownTimes.SelectedRows;
            if (selectedRows.Count < 2) // On ne montre le panel que pour 2 sélections ou plus
            {
                panel4.Visible = false;
                return;
            }

            // Calculer la durée totale en minutes
            int totalMinutes = 0; 
            foreach (DataGridViewRow row in selectedRows)
            {
                // --- CORRECTION ICI ---
                // On récupère l'objet depuis la propriété Tag
                if (row.Tag is DownTimeRecord dw)
                {
                    totalMinutes += (int)(dw.EndTime - dw.StartTime).TotalMinutes;
                }
                // --------------------
            }

            // On met à jour le label et on affiche le panel
            label2.Text = $"You have selected : {FormatDuration(totalMinutes)} of DownTime";
            panel4.Visible = true;
        }

        private void DataGridViewDownTimes_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedDuration();

            // On cache le bouton par défaut
            if (dataGridViewDownTimes.SelectedRows.Count == 0)
            {
                btnMarkAsLeave.Visible = false;
                btnJustifyPermission.Visible = false;
                return;
            }

            // On récupère l'objet DownTimeRecord associé à la ligne sélectionnée
            var selectedDownTime = (DownTimeRecord)dataGridViewDownTimes.SelectedRows[0].Tag;
            if (selectedDownTime == null) return;

            // On récupère les heures de travail depuis la configuration
            var workStart = TimeSpan.Parse(_config["WorkingHours:Start"]);
            var workEnd = TimeSpan.Parse(_config["WorkingHours:End"]);

            var role = _config["UserRole"];


            // --- LA LOGIQUE DE VÉRIFICATION ---
            // On vérifie si l'heure de début et de fin de l'absence correspondent
            // exactement aux heures de travail définies.

            if (dataGridViewDownTimes.SelectedRows.Count == 2)
            {
                var selectedDownTimes = dataGridViewDownTimes.SelectedRows
                                          .Cast<DataGridViewRow>()
                                          .Select(r => (DownTimeRecord)r.Tag)
                                          .OrderBy(d => d.StartTime)
                                          .ToList();

                var breakStart = TimeSpan.Parse(_config["BreakHours:Start"]);
                var breakEnd = TimeSpan.Parse(_config["BreakHours:End"]);



                bool isMorningAbsence = selectedDownTimes[0].StartTime.TimeOfDay == workStart &&
                            selectedDownTimes[0].EndTime.TimeOfDay == breakStart;

                bool isAfternoonAbsence = selectedDownTimes[1].StartTime.TimeOfDay == breakEnd &&
                                          selectedDownTimes[1].EndTime.TimeOfDay == workEnd;

                if (isMorningAbsence && isAfternoonAbsence)
                {
                    // Si c'est une journée complète, on affiche le bouton
                    btnMarkAsLeave.Visible = true;
                }
                else
                {
                    btnMarkAsLeave.Visible = false;
                    btnJustifyPermission.Visible = false;
                }
            }
            else
            {
                btnMarkAsLeave.Visible = false;
            }

            if (selectedDownTime.IsPermission != -1 && IsPermissionRole(role))
            {
                // Si l'absence est déjà marquée comme permission ou si l'utilisateur a le rôle approprié
                // on affiche le bouton de justification de permission
                btnJustifyPermission.Visible = true;
            }
            else
            {
                btnMarkAsLeave.Visible = false;
                btnJustifyPermission.Visible = false;
            }
        }

        private void RecoveryForm_Load(object sender, EventArgs e)
        {
            // Au chargement du formulaire, on met à jour l'affichage
            UpdateDisplay();
        }

        /// <summary>
        /// Met à jour tous les affichages du formulaire (solde et liste des absences).
        /// </summary>
        private void UpdateDisplay()
        {
            lblOvertimeBalance.Text = $"Available balance of overtime : {FormatDuration(_availableOvertime)}";
            PopulateDownTimesGrid();
        }

        /// <summary>
        /// Remplit la grille (DataGridView) avec les absences non résolues.

        private void PopulateDownTimesGrid()
        {
            dataGridViewDownTimes.Rows.Clear();

            using (var db = new YotTrackerDbContext(_databasePath))
            {
                // --- OPTIMISATION ---
                // 1. On récupère toutes les compensations en une seule fois et on les groupe par DownTimeId.
                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                // On ne récupère que les compensations du mois en cours
                var compensationsByDownTimeId = db.Compensations
                    .Where(c => c.TransactionDate >= startOfMonth) // <-- FILTRE AJOUTÉ
                    .GroupBy(c => c.DownTimeRecordId)
                    .ToDictionary(g => g.Key, g => g.Sum(c => c.CompensationMinutes));

                // --------------------
                var orderedDownTimes = _unresolvedDownTimes
                .OrderBy(dt => dt.StartTime)
                .ToList();

                foreach (var dt in orderedDownTimes)
                {
                    // 2. On calcule la durée totale de l'absence (en minutes entières).
                    int totalDuration = (int)(dt.EndTime - dt.StartTime).TotalMinutes;

                    // 3. On récupère les minutes déjà compensées depuis notre dictionnaire (0 si rien n'est trouvé).
                    int compensatedMinutes = 0;
                    if (compensationsByDownTimeId.ContainsKey(dt.Id))
                    {
                        compensatedMinutes = compensationsByDownTimeId[dt.Id];
                    }

                    // 4. On calcule la durée restante à récupérer.
                    int remainingDuration = totalDuration - compensatedMinutes;

                    // On n'affiche la ligne que s'il reste du temps à récupérer.
                    if (remainingDuration >= 1)
                    {
                        int rowIndex = dataGridViewDownTimes.Rows.Add(
                            dt.StartTime.ToShortDateString(),
                            dt.StartTime.ToShortTimeString(),
                            dt.EndTime.ToShortTimeString(),
                            $"{FormatDuration(remainingDuration):F0}" // On affiche la durée restante
                        );
                        dataGridViewDownTimes.Rows[rowIndex].Tag = dt;
                    }
                }
            }
            dataGridViewDownTimes.ClearSelection();

        }

        // --- GESTIONNAIRES D'ÉVÉNEMENTS DES BOUTONS ---

        private void BtnMarkAsLeave_Click(object sender, EventArgs e)
        {
            if (dataGridViewDownTimes.SelectedRows.Count != 2)
            {
                ShowCustomMessage("Please select exactly two absences to justify a leave.");
                return;
            }
            var selectedDownTimes = dataGridViewDownTimes.SelectedRows
                                      .Cast<DataGridViewRow>()
                                      .Select(r => (DownTimeRecord)r.Tag)
                                      .OrderBy(d => d.StartTime)
                                      .ToList();

            var workStart = TimeSpan.Parse(_config["WorkingHours:Start"]);
            var workEnd = TimeSpan.Parse(_config["WorkingHours:End"]);
            var breakStart = TimeSpan.Parse(_config["BreakHours:Start"]);
            var breakEnd = TimeSpan.Parse(_config["BreakHours:End"]);



            bool isMorningAbsence = selectedDownTimes[0].StartTime.TimeOfDay == workStart &&
                        selectedDownTimes[0].EndTime.TimeOfDay == breakStart;

            bool isAfternoonAbsence = selectedDownTimes[1].StartTime.TimeOfDay == breakEnd &&
                                      selectedDownTimes[1].EndTime.TimeOfDay == workEnd;

            if (!(isMorningAbsence && isAfternoonAbsence))
            {
                ShowCustomMessage("The selected absences do not correspond to a full day of leave.");
                btnMarkAsLeave.Visible = false;
                return;
            }

            ShowCustomMessage("The selected absences will to mark as 'Leave'");

                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    foreach (DataGridViewRow row in dataGridViewDownTimes.SelectedRows)
                    {
                        var downTime = (DownTimeRecord)row.Tag;
                        var dbEntry = db.DownTimeRecords.Find(downTime.Id);
                        if (dbEntry != null)
                        {
                            dbEntry.IsConge = 1;
                        }
                    }
                    db.SaveChanges();
                }
                // On rafraîchit la liste
                RefreshDataAndDisplay();
        }
        private static bool IsPermissionRole(string role)
        {
            return role != null && new[] { "supervisor", "deputy", "manager" }.Contains(role.ToLower());
        }


        private void BtnJustifyPermission_Click(object sender, EventArgs e)
        {
            if (dataGridViewDownTimes.SelectedRows.Count != 1)
            {
                ShowCustomMessage("Please select a single absence to justify.");
                return;
            }

            _selectedDownTimeForPermission = (DownTimeRecord)dataGridViewDownTimes.SelectedRows[0].Tag;

            if (_selectedDownTimeForPermission.IsPermission == -1)
            {
                ShowCustomMessage("This session cannot be a permission");
                btnJustifyPermission.Visible = false;
                return;
            }

            var role = _config["UserRole"];
            if (!IsPermissionRole(role))
            {
                ShowCustomMessage("You don't have the rights to justify a permission.");
                btnJustifyPermission.Visible = false;
                return;
            }

            // Afficher panel3 à la place de TimeRangeDialog
            dtpStartTime.Value = _selectedDownTimeForPermission.StartTime;
            dtpEndTime.Value = _selectedDownTimeForPermission.EndTime;

            SetControlsEnabled(false);
            panel3.BringToFront();
            panel3.Visible = true;
        }

        private void SetControlsEnabled(bool enabled)
        {
            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                if (ctrl != panel3 && ctrl != panel10)
                    ctrl.Enabled = enabled;
            }
        }


        private void BtnRecover_Click(object sender, EventArgs e)
        {
            // 1. Vérifier qu'au moins une ligne est sélectionnée
            if (dataGridViewDownTimes.SelectedRows.Count == 0)
            {
                ShowCustomMessage("Please select at least one absence to recover..");
                return;
            }
    

            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // 2. Récupérer les données à jour
                    var overtimesAvailable = SessionManager.GetAvailableOverTimeSessions(db); // On recharge les heures sup disponibles
                    if(overtimesAvailable.Count == 0)
                    {
                        ShowCustomMessage("No overtime available to recover the absences.");
                        return;
                    }
                    else
                    {
                        ShowCustomMessage("You are about to recover the selected absences with your balance of overtime hours.");

                    }
                    var selectedDownTimes = dataGridViewDownTimes.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Select(row => (DownTimeRecord)row.Tag)
                    .OrderBy(dw => dw.StartTime)
                    .ToList();

                    var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    int c = 0;
                    // 3. Boucler sur chaque absence à récupérer
                    foreach (var downTime in selectedDownTimes)
                    {
                        var dbDownTime = db.DownTimeRecords.Find(downTime.Id); // On travaille avec l'objet suivi par EF
                        if (dbDownTime == null || dbDownTime.IsPermission == 1 || dbDownTime.IsConge == 1) continue;


                        var now = DateTime.Now;
                        var monthStart = new DateTime(now.Year, now.Month, 1);

                        int alreadyCompensated = db.Compensations
                        .Where(c => c.DownTimeRecordId == downTime.Id && c.TransactionDate >= startOfMonth) // Filtre ajouté
                        .Sum(c => (int?)c.CompensationMinutes) ?? 0;

                        int minutesToRecover = (int)(dbDownTime.EndTime - dbDownTime.StartTime).TotalMinutes - alreadyCompensated;

                        if (minutesToRecover <= 0) continue;

                        // 4. Boucler sur les heures supplémentaires pour trouver des minutes à utiliser
                        for (int i = 0; i < overtimesAvailable.Count; i++)
                        {
                            var overtime = overtimesAvailable[i];

                            int usedOvertime = 0;

                            if (c == 1)
                            {
                                usedOvertime = db.Compensations
                                .Where(c => c.WorkSessionId == overtime.Id &&
                                            db.WorkSessions.Any(ws =>
                                                ws.Id == c.WorkSessionId &&
                                                db.AppSettingsHistory.Any(a =>
                                                    a.VersionId == ws.AppSettingsVersionId &&
                                                    a.ExpirationOverTime > now)))
                                .Sum(c => (int?)c.CompensationMinutes) ?? 0;
                            }

                            int availableMinutes = (overtime.OverTimeMinutes ?? 0) - usedOvertime;

                            if (availableMinutes <= 0)
                            {
                                if (i != overtimesAvailable.Count - 1)
                                {
                                    continue;
                                }
                            }

                            int compensationAmount = Math.Min(availableMinutes, minutesToRecover);

                            if (compensationAmount > 0)
                            {
                                // 5. Créer l'enregistrement de la compensation
                                db.Compensations.Add(new Compensation
                                {
                                    WorkSessionId = overtime.Id,
                                    DownTimeRecordId = dbDownTime.Id,
                                    CompensationMinutes = compensationAmount,
                                    TransactionDate = DateTime.Now
                                });
                                c = 1;
                            }

                            minutesToRecover -= compensationAmount;
                            if (minutesToRecover < 1)
                            {
                                dbDownTime.IsPermission = -1;
                                dbDownTime.IsConge = -1;

                                db.SaveChanges();

                                break; // Cette absence est entièrement compensée, on passe à la suivante
                            }
                            else
                            {
                                if (i == overtimesAvailable.Count - 1)
                                {
                                    // Si on arrive à la dernière heure supplémentaire et qu'il reste des minutes à récupérer
                                    ShowCustomMessage("It stays "+FormatDuration(minutesToRecover) + " to recover for the absence from " + dbDownTime.StartTime + " to " + dbDownTime.EndTime + ".");
                                }
                            }
                            db.SaveChanges();
                        }
                    }

                    ShowCustomMessage("The recovery was successfully completed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la récupération des DownTimes.");
                ShowCustomMessage("An error occurred during the operation. Please try again later.");
            }

            // 7. Rafraîchir l'affichage pour montrer le résultat
            RefreshDataAndDisplay();
        }

        /// <summary>
        /// Recharge les données depuis la base de données et met à jour l'affichage.
        /// </summary>
        private void RefreshDataAndDisplay()
        {
            using (var db = new YotTrackerDbContext(_databasePath))
            {
                // Charger tous les downTimes
                var allDownTimes = db.DownTimeRecords.ToList();

                _unresolvedDownTimes = new List<DownTimeRecord>();

                foreach (var downTime in allDownTimes)
                {
                    var now = DateTime.Now;
                    var monthStart = new DateTime(now.Year, now.Month, 1);

                    int? totalCompensated = db.Compensations
                        .Where(c => c.DownTimeRecord.StartTime >= monthStart &&
                                    c.DownTimeRecordId == downTime.Id)
                        .Sum(c => (int?)c.CompensationMinutes);

                    int duration = (int)(downTime.EndTime - downTime.StartTime).TotalMinutes;

                    bool isNotFullyResolved =
                        (downTime.IsConge != 1 && downTime.IsPermission != 1) &&
                        (duration > totalCompensated);

                    if (isNotFullyResolved)
                        _unresolvedDownTimes.Add(downTime);
                }

                // Calcul du total disponible en heures supplémentaires
                _availableOvertime = SessionManager.GetAvailableOverTimeSessions(db)
                    .Sum(o => o.OverTimeMinutes ?? 0);
            }

            UpdateDisplay();
        }



        /// <summary>
        /// Divise un DownTimeRecord en plusieurs morceaux en fonction de la plage de permission
        /// saisie par l'utilisateur.
        /// </summary>
        private void SplitDownTimeForPermission(DownTimeRecord original, DateTime permissionStart, DateTime permissionEnd)
        {
            // On valide que la plage de permission est bien à l'intérieur du DownTime original
            if (permissionStart < original.StartTime || permissionEnd > original.EndTime || permissionStart >= permissionEnd)
            {
                ShowCustomMessage("The time range for the permission is invalid or outside the absence period.");
                return;
            }

            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // On attache l'objet original au contexte pour pouvoir le supprimer
                    db.DownTimeRecords.Attach(original);
                    db.DownTimeRecords.Remove(original);

                    // 1. On crée la partie "Permission" (résolue)
                    db.DownTimeRecords.Add(new DownTimeRecord
                    {
                        StartTime = permissionStart,
                        EndTime = permissionEnd,
                        IsPermission = 1,
                        AppSettingsVersionId = original.AppSettingsVersionId
                    });

                    // 2. On crée la partie AVANT la permission (si elle existe)
                    if (permissionStart > original.StartTime)
                    {
                        db.DownTimeRecords.Add(new DownTimeRecord
                        {
                            StartTime = original.StartTime,
                            EndTime = permissionStart.AddTicks(-1),
                            IsPermission = -1,
                            IsConge = -1, // Reste à traiter
                            AppSettingsVersionId = original.AppSettingsVersionId
                        });
                    }

                    // 3. On crée la partie APRÈS la permission (si elle existe)
                    if (permissionEnd < original.EndTime)
                    {
                        db.DownTimeRecords.Add(new DownTimeRecord
                        {
                            StartTime = permissionEnd.AddTicks(1),
                            EndTime = original.EndTime,
                            IsPermission = -1,
                            IsConge = -1, // Reste à traiter
                            AppSettingsVersionId = original.AppSettingsVersionId
                        });
                    }

                    db.SaveChanges();
                    Log.Information("DownTime (Id original: {Id}) a été divisé pour une permission.", original.Id);
                    ShowCustomMessage("Your time range of the absence was justified successfully.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de la division du DownTimeRecord.");
                ShowCustomMessage("An error occurred while saving the permission. Please try again later.");
            }
        }

        public  string FormatDuration(int minutes)
        {
            minutes = (int)Math.Round((double)minutes); // Round to the nearest whole number
            if (minutes < 60)
            {
                return $"{minutes} minutes";
            }
            else if (minutes < 1440)
            {
                int hours = (int)minutes / 60;
                int remainingMinutes = (int)minutes % 60;
                return $"{hours} hours {remainingMinutes} minutes";
            }
            else
            {
                int days = (int)minutes / 1440;
                int remainingHours = ((int)minutes % 1440) / 60;
                int remainingMinutes = (int)minutes % 60;
                return $"{days} days {remainingHours} hours {remainingMinutes} minutes";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // On notifie le MainForm que l'utilisateur a terminé.
            OnClose?.Invoke();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {

                if (dtpEndTime.Value < dtpStartTime.Value)
                {
                   
                    ShowCustomMessage("The end time cannot be earlier than the start time.");
                    return;
                }

                if (_selectedDownTimeForPermission == null) return;

                var permissionStart = dtpStartTime.Value;
                var permissionEnd = dtpEndTime.Value;

                if (permissionStart == permissionEnd)
                {
                    panel3.Visible = false;
                    return;
                }

                // Appel de la méthode pour diviser le DownTime
                SplitDownTimeForPermission(_selectedDownTimeForPermission, permissionStart, permissionEnd);

                // Rafraîchir l'affichage
                RefreshDataAndDisplay();

                panel3.Visible = false;
                SetControlsEnabled(true); // Réactive les autres contrôles
                _selectedDownTimeForPermission = null;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            SetControlsEnabled(true); // Réactive les autres contrôles
            _selectedDownTimeForPermission = null;
        }

        private void ShowCustomMessage(string message)
        {
            // Mettre le texte du label
            label4.Text = message;

            // Afficher panel10 au-dessus de tout
            panel10.BringToFront();
            panel10.Visible = true;

            isPanel10Closed = false;


            // Bloquer les autres contrôles
            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                if (ctrl != panel10)
                    ctrl.Enabled = false;
            }

            while (!isPanel10Closed)
            {
                Application.DoEvents(); // Permet à l'UI de continuer à répondre
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;

            // Réactiver les autres contrôles
            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                ctrl.Enabled = true;
            }

            isPanel10Closed = true; // Débloque la boucle
        }

    }
}
