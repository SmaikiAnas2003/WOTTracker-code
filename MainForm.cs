using ClosedXML.Excel; // Use ClosedXML for Excel operations
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WOTTracker.Models;

namespace WOTTracker
{
    public partial class MainForm : Form
    {
        private static IConfiguration config;
        private DateTime expirationOverTime;
        private DateTime startTime;
        private DateTime endTime;
        private string appSettingsPath;
        private string emailRecipient;
        private string emailSender;
        private string smtpServer;
        private int smtpPort;
        private string smtpUsername;
        private string smtpPassword;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem exportMenuItem;
        private ToolStripMenuItem settingsMenuItem;
        private string _appFolderPath;
        private string _databasePath;
        private string _logFilePath;
        private System.Threading.Timer _uiUpdateTimer;
        private static int afficheMessage = 0;
        private bool isPanel10Closed = false;

        public MainForm()
        {

            InitializeComponent();

            panel10.Visible = false;



            // Événement principal de démarrage de la logique applicative

            try
            {
                //_appFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOTTracker");
                //Directory.CreateDirectory(_appFolderPath); (If you want create a folder in %appdata%)
                
                
               _appFolderPath = AppDomain.CurrentDomain.BaseDirectory; //the appfolder is the same as the executable folder



                // 3. Define the absolute paths for the database and log files
                _databasePath = Path.Combine(_appFolderPath, "database.sdf");
                appSettingsPath = Path.Combine(_appFolderPath, "appsettings.json");
                _logFilePath = Path.Combine(_appFolderPath, "log.txt");



                if (string.IsNullOrWhiteSpace(_databasePath))
                {
                    MessageBox.Show("Error: the path to the database is empty.");
                    Application.Exit();
                    return;
                }

                InitializeDatabase(_databasePath);


                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    db.Database.EnsureCreated();

                }

                // ---- INITIALISATION DES SERVICES ----
                ConfigureLogging();
                Log.Information("--- Application WOTTracker Started ---");
                Log.Information("Configuration loaded successfully.");



                // ---- DÉMARRAGE DE LA LOGIQUE MÉTIER ET UI ----

                InitializeTrayIcon();
                StartupManager.SetStartup(true);


                SystemEvents.PowerModeChanged += OnPowerModeChanged;

                this.TopMost = true;
                Rectangle workingArea = Screen.GetWorkingArea(this);
                this.Location = new Point(workingArea.Right - this.Width, workingArea.Bottom - this.Height);

                panel9.Visible = false;

                this.Load += MainForm_Load;

                SessionManager.OnRecoveryNeeded += ShowDownTimeRecoveryControl;

            }
            catch (Exception ex)
            {
                MessageBox.Show("A critical error occurred at startup : " + ex.Message);
                Application.Exit();
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigManager.Initialize(_databasePath, appSettingsPath);
                config = ConfigManager.LoadAndSaveFile();

                if (config == null)
                {
                    ShowConfigurationControl();
                    return; // Attente de sauvegarde de la config
                }

                // Charger les paramètres
                LoadEmailAndWorkingHours();

                SessionManager.Initialize(_databasePath, config);
                SessionManager.RecoverSessions();

                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    var activeSession = db.WorkSessions.FirstOrDefault(ws => ws.EndTime == null);
                    if (activeSession?.WorkingType == null)
                    {
                        ShowWorkingTypeControl();
                        return;
                    }
                }

                ContinueAfterWorkingType();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in MainForm_Load : " + ex.Message);
                Application.Exit();
            }


        }

        private void ContinueAfterWorkingType()
        {
            // Charger les paramètres
            LoadEmailAndWorkingHours();

            SessionManager.Initialize(_databasePath, config);
            SessionManager.RecoverSessions();

            // Vérifier s'il y a des compensations
            if (SessionManager.HasPendingDownTime(_databasePath) && SessionManager.HasPendingOverTime(_databasePath))
            {
                SessionManager.CheckForDownTimeRecovery(config);
                return;
            }

            // Lancer la logique principale
            StartApplicationLogic();
        }
        private void LoadEmailAndWorkingHours()
        {
            // Charger les heures de travail et les paramètres d'email
            startTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:Start"]));
            endTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:End"]));
            emailRecipient = config["Email:Recipient"];
            emailSender = config["Email:Sender"];
            smtpServer = config["Email:SmtpServer"];
            smtpPort = int.Parse(config["Email:SmtpPort"]);
            smtpUsername = config["Email:SmtpUsername"];
            smtpPassword = config["Email:SmtpPassword"];
            expirationOverTime = DateTime.Parse(config["ExpirationOverTime"]);
        }


        private void ShowWorkingTypeControl()
        {
            panel9.Visible = true;
            panel9.BringToFront();  // S'assure que le panel est au-dessus du reste

            var workingTypeControl = new WorkingType();

            workingTypeControl.OnWorkingTypeSelected += WorkingTypeChosen;


            panel9.Controls.Clear();
            panel9.Controls.Add(workingTypeControl);
            workingTypeControl.Dock = DockStyle.Fill;
        }

        private void WorkingTypeChosen(string selectedType)
        {
            using (var db = new YotTrackerDbContext(_databasePath))
            {
                // Récupère la session active (celle qui n'a pas encore d'EndTime)
                var activeSession = db.WorkSessions.FirstOrDefault(ws => ws.EndTime == null);

                if (activeSession != null && activeSession.WorkingType == null)
                {
                    activeSession.WorkingType = selectedType;
                    db.SaveChanges();
                }

                // Vérifier si Home Office et pas d'OverTime
                if (selectedType == "Home Office" && !IsOvertime(DateTime.Now, db))
                {
                    SendHomeOfficeEmail(activeSession?.StartTime);
                }
                ContinueAfterWorkingType();
            }



            SessionManager.Initialize(_databasePath, config);

            SessionManager.RecoverSessions();
            SessionManager.CheckForDownTimeRecovery(config);

            if (!SessionManager.HasPendingDownTime(_databasePath) || (!SessionManager.HasPendingOverTime(_databasePath)))
            {
                StartApplicationLogic();
            }
        }

        private void SendHomeOfficeEmail(DateTime? startTime)
        {
            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    var message = new MailMessage
                    {
                        From = new MailAddress(emailSender),
                        Subject = "Home Office Notification",
                        Body = $"My session that starts at {startTime:HH:mm} is Home Office."
                    };
                    message.To.Add(emailRecipient);

                    client.Send(message);
                }

                Log.Information("Home Office email sent successfully.");
                ShowCustomMessage("Email sent successfully.");
            }
            catch
            {
                ShowCustomMessage("Failed to send email.");

            }
        }



        private void ShowConfigurationControl()
        {
            // On rend le panel visible et on y ajoute le UserControl
            panel9.Visible = true;
            panel9.BringToFront();  // S'assure que le panel est au-dessus du reste

            var configControl = new Configuration();

            // On s'abonne à l'événement "Save" du UserControl
            configControl.OnSave += ConfigControl_OnSave;

            panel9.Controls.Clear();
            panel9.Controls.Add(configControl);
            configControl.Dock = DockStyle.Fill;
        }

        private void ConfigControl_OnSave(AppSettingsHistory configData)
        {
            // On utilise la nouvelle méthode du ConfigManager pour sauvegarder les données
            config = ConfigManager.SaveNewConfigFromUserInput(configData);

            if (config != null)
            {
                // La sauvegarde a réussi, on cache le panel et on lance l'application

                startTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:Start"]));
                endTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:End"]));
                emailRecipient = config["Email:Recipient"];
                emailSender = config["Email:Sender"];
                smtpServer = config["Email:SmtpServer"];
                smtpPort = int.Parse(config["Email:SmtpPort"]);
                smtpUsername = config["Email:SmtpUsername"];
                smtpPassword = config["Email:SmtpPassword"];
                expirationOverTime = DateTime.Parse(config["ExpirationOverTime"]);

                SessionManager.Initialize(_databasePath, config);
                SessionManager.RecoverSessions();

                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    var activeSession = db.WorkSessions.FirstOrDefault(ws => ws.EndTime == null);
                    if (activeSession?.WorkingType == null)
                    {
                        ShowWorkingTypeControl();
                        return;
                    }
                }

                


                SessionManager.CheckForDownTimeRecovery(config);


                if (!SessionManager.HasPendingDownTime(_databasePath))
                {
                    StartApplicationLogic();
                }

            }
            else
            {
                MessageBox.Show("An error occurred while saving. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartApplicationLogic()
        {
            _uiUpdateTimer = new System.Threading.Timer(
                  callback: _ => UpdateLiveDisplay(),
                  state: null,
                  dueTime: 0,
                  period: 1000);

            Task.Delay(500).ContinueWith(_ =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => panel9.Controls.Clear()));
                    this.Invoke(new Action(() => panel9.Visible = false));
                }
                else
                {
                    panel9.Visible = false;
                    panel9.Controls.Clear();
                }
            });
        }

        private void ShowDownTimeRecoveryControl(List<DownTimeRecord> unresolved, int availableOvertimes, IConfiguration config)
        {
            // S'assurer que l'opération se fait sur le bon thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowDownTimeRecoveryControl(unresolved, availableOvertimes, config)));
                return;
            }

            panel9.Visible = true;

            var compensationControl = new CompensationDownTime(unresolved, availableOvertimes, config, _databasePath);

            compensationControl.OnClose += HideRecoveryControl;

            panel9.Controls.Clear();
            panel9.Controls.Add(compensationControl);
            compensationControl.Dock = DockStyle.Fill;
        }

        private void HideRecoveryControl()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HideRecoveryControl));
                return;
            }

            StartApplicationLogic();
        }


        public static TimeSpan ComputeOverTime(
            DateTime startSession, DateTime now,
            DateTime workStart, DateTime workEnd,
            DateTime breakStart, DateTime breakEnd)
        {
            TimeSpan overtime = TimeSpan.Zero;

            // --- Overtime avant heures normales ---
            if (startSession < workStart)
            {
                if (now < workStart)
                {
                    overtime = now - startSession;
                    return overtime;
                }
                else
                {

                    overtime = workStart - startSession;

                    if (now < breakStart)
                    {
                        return overtime;
                    }

                    if (now < breakEnd)
                    {
                        overtime += (now - breakStart);
                        return overtime;

                    }
                    else
                    {
                        overtime += (breakEnd - breakStart);
                        if (now > workEnd) overtime += (now - workEnd);
                        return overtime;

                    }
                }
            }

            else if(startSession < breakStart)
            {
                if (now < breakStart)
                {
                    return overtime;
                }
                if (now < breakEnd)
                {
                    overtime += (now - breakStart);
                    return overtime;

                }
                else
                {
                    overtime += (breakEnd - breakStart);
                    if (now > workEnd) overtime += (now - workEnd);
                    return overtime;

                }
            }

            else if(startSession < breakEnd)
            {
                if (now < breakEnd)
                {
                    overtime += (now - startSession);
                    return overtime;
                }
                else
                {
                    overtime += (breakEnd - startSession); 
                    if (now > workEnd) overtime += (now - workEnd);

                    return overtime;
                }
            }

            else if(startSession < workEnd)
            {
                if(now>workEnd) overtime = (now - workEnd);

                return overtime;
            }
            else
            {
                overtime = now - startSession;
                return overtime;

            }


        }




        private void UpdateLiveDisplay()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (this.InvokeRequired) { this.BeginInvoke(new Action(UpdateLiveDisplay)); return; }

            try
            {
                DateTime workStartTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:Start"]));
                DateTime workEndTime = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:End"]));

                DateTime breakStartTime = DateTime.Today.Add(TimeSpan.Parse(config["BreakHours:Start"]));
                DateTime breakEndTime = DateTime.Today.Add(TimeSpan.Parse(config["BreakHours:End"]));

                DateTime now = DateTime.Now;

                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // --- 1. Session en cours ---
                    var activeSession = db.WorkSessions.AsNoTracking().FirstOrDefault(s => s.EndTime == null);
                    if (activeSession == null)
                    {
                        labelStartSession.Text = "No active session";
                        LabelSessionTotal.Text = "00:00:00";
                        return;
                    }

                    // --- 2. Totaux DB ---
                    int totalOvertimeInDb = db.WorkSessions
                    .Where(s => db.AppSettingsHistory.Any(c => c.VersionId == s.AppSettingsVersionId &&
                                               c.ExpirationOverTime > DateTime.Now))
                    .Sum(s => s.OverTimeMinutes ?? 0);


                    int totalCompensation = db.Compensations
                        .Where(comp => db.WorkSessions
                            .Any(ws => ws.Id == comp.WorkSessionId &&
                                       db.AppSettingsHistory.Any(config =>
                                           config.VersionId == ws.AppSettingsVersionId &&
                                           config.ExpirationOverTime > DateTime.Now)))
                        .Sum(comp => (int)comp.CompensationMinutes);

                    int normalDaysInDb = db.WorkSessions.Where(s => s.TypeOverTime == "NormalDays" && 
                                                db.AppSettingsHistory.Any(c => c.VersionId == s.AppSettingsVersionId &&
                                               c.ExpirationOverTime > DateTime.Now)).Sum(s => s.OverTimeMinutes ?? 0);
                    int saturdaysInDb = db.WorkSessions.Where(s => s.TypeOverTime == "Saturdays" && 
                                                db.AppSettingsHistory.Any(c => c.VersionId == s.AppSettingsVersionId &&
                                               c.ExpirationOverTime > DateTime.Now)).Sum(s => s.OverTimeMinutes ?? 0);
                    int sundaysInDb = db.WorkSessions.Where(s => s.TypeOverTime == "Sundays" &&
                                                db.AppSettingsHistory.Any(c => c.VersionId == s.AppSettingsVersionId &&
                                               c.ExpirationOverTime > DateTime.Now)).Sum(s => s.OverTimeMinutes ?? 0);
                    int holidaysInDb = db.WorkSessions.Where(s => s.TypeOverTime == "PublicHolidays" && 
                                                db.AppSettingsHistory.Any(c => c.VersionId == s.AppSettingsVersionId &&
                                               c.ExpirationOverTime > DateTime.Now)).Sum(s => s.OverTimeMinutes ?? 0);

                    string typeAtStart = SessionManager.GetOvertimeType(activeSession.StartTime, db);
                    string typeNow = SessionManager.GetOvertimeType(now, db);

                    TimeSpan runningDuration = TimeSpan.Zero;

                    void AddMinutes(string type, int minutes)
                    {
                        switch (type)
                        {
                            case "NormalDays": normalDaysInDb += minutes; break;
                            case "Saturdays": saturdaysInDb += minutes; break;
                            case "Sundays": sundaysInDb += minutes; break;
                            case "PublicHolidays": holidaysInDb += minutes; break;
                        }
                    }

                    // --- 3. Calcul du runningDuration ---
                    if (typeAtStart == typeNow)
                    {
                        if (typeAtStart == "NormalDays")
                        {
                            var overtime = ComputeOverTime(
                                activeSession.StartTime, now,
                                workStartTime, workEndTime,
                                breakStartTime, breakEndTime);

                            runningDuration = overtime;
                            // L'overtime "avant ou après" doit être ajouté comme overtime du jour
                            AddMinutes("Overtime", (int)overtime.TotalMinutes);
                        }
                        else
                        {
                            runningDuration = now - activeSession.StartTime;
                            AddMinutes(typeAtStart, (int)runningDuration.TotalMinutes);
                        }
                    }

                    else
                    {
                        DateTime midnight = activeSession.StartTime.Date.AddDays(1);

                        TimeSpan durationPart1;
                        TimeSpan durationPart2;

                        if (typeAtStart == "NormalDays")
                        {
                            var overtimePart1 = ComputeOverTime(
                                activeSession.StartTime, midnight,
                                workStartTime, workEndTime,
                                breakStartTime, breakEndTime);

                            durationPart1 = overtimePart1;
                            AddMinutes("Overtime", (int)overtimePart1.TotalMinutes);
                        }
                        else
                        {
                            durationPart1 = midnight - activeSession.StartTime;
                            AddMinutes(typeAtStart, (int)durationPart1.TotalMinutes);
                        }

                        if (typeNow == "NormalDays")
                        {
                            var overtimePart2 = ComputeOverTime(
                                midnight, now,
                                workStartTime, workEndTime,
                                breakStartTime, breakEndTime);

                            durationPart2 =overtimePart2;
                            AddMinutes("Overtime", (int)overtimePart2.TotalMinutes);
                        }
                        else
                        {
                            durationPart2 = now - midnight;
                            AddMinutes(typeNow, (int)durationPart2.TotalMinutes);
                        }

                        runningDuration = durationPart1 + durationPart2;

                    }


                    // --- 4. Labels de session ---
                    labelStartSession.Text = "Session start at " + activeSession.StartTime.ToLongTimeString();

                    if (IsOvertime(now, db))
                    {
                        LabelSessionTotal.Text = runningDuration.ToString(@"hh\:mm\:ss") + " Overtime";
                        panel2.BackColor = Color.FromArgb(60, 0, 0);
                        lbl_status.Text = "Overtime Session";
                    }
                    else
                    {
                        LabelSessionTotal.Text = "Working Session";
                        panel2.BackColor = Color.FromArgb(45, 50, 54);
                        lbl_status.Text = "Current Session";
                    }

                    // --- 5. Totaux ---
                    int brutTotalOverTime = totalOvertimeInDb + (int)runningDuration.TotalMinutes;
                    int netTotalOvertime = normalDaysInDb + (int)runningDuration.TotalMinutes - totalCompensation;

                    normalDaysInDb += (int)runningDuration.TotalMinutes;

                    lblTotalOvertime.Text = FormatDuration(netTotalOvertime);
                    lblOvertimeNormalDays.Text = FormatDuration(normalDaysInDb);
                    lblOvertimeSaturdays.Text = FormatDuration(saturdaysInDb);
                    lblOvertimeSundays.Text = FormatDuration(sundaysInDb);
                    lblOvertimePublicHolidays.Text = FormatDuration(holidaysInDb);

                    if (brutTotalOverTime != 0)
                    {
                        labelPoucentageHolidays.Text = $"{(holidaysInDb * 100 / brutTotalOverTime):0}%";

                        int normalPct = (normalDaysInDb * 100 / brutTotalOverTime);
                        if ((normalDaysInDb * 100) % brutTotalOverTime != 0) normalPct++;
                        labelPoucentageNormalDays.Text = $"{normalPct:0}%";

                        labelPoucentageSaturdays.Text = $"{(saturdaysInDb * 100 / brutTotalOverTime):0}%";
                        labelPoucentageSundays.Text = $"{(sundaysInDb * 100 / brutTotalOverTime):0}%";
                    }

                    // --- 6. DownTime ---
                    int rawDownTime = db.DownTimeRecords
                        .Where(d => d.IsPermission != 1 && d.IsConge != 1 && d.StartTime.Month == now.Month && d.StartTime.Year == now.Year)
                        .ToList()
                        .Sum(d => (int)(d.EndTime - d.StartTime).TotalMinutes);

                    DateTime monthStart = new DateTime(now.Year, now.Month, 1);
                    int downTimeCompensated = db.Compensations
                        .Where(c => db.DownTimeRecords.Any(d =>
                            d.Id == c.DownTimeRecordId &&
                            d.StartTime >= monthStart))
                        .Sum(c => (int)(c.CompensationMinutes));

                    int netDownTime = rawDownTime - downTimeCompensated;
                    labelDownTime.Text = FormatDuration(netDownTime);

                    // --- 7. Message périodique ---
                    if (afficheMessage % 10 == 0)
                        ShowMessage(IsOvertime(now, db));
                    afficheMessage++;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while updating the display.");
            }
        }


        private bool IsOvertime(DateTime now, YotTrackerDbContext db)
        {
            // 1. Vérification du jour de la semaine
            DayOfWeek day = now.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                return true;

            // 2. Vérification des jours fériés (par jour + mois uniquement)
            int todayDay = now.Day;
            int todayMonth = now.Month;

            var fixedHolidays = db.FixedPublicHolidays
                .Any(h => h.Day == todayDay && h.Month == todayMonth);

            var movableHolidays = db.MovablePublicHolidays
                .Any(h => h.Day == todayDay && h.Month == todayMonth);

            if (fixedHolidays || movableHolidays)
                return true;

            // 3. Vérification de la plage horaire
            TimeSpan nowTime = now.TimeOfDay;

            TimeSpan workStart = TimeSpan.Parse(config["WorkingHours:Start"]);
            TimeSpan workEnd = TimeSpan.Parse(config["WorkingHours:End"]);

            TimeSpan breakStart = TimeSpan.Parse(config["BreakHours:Start"]);
            TimeSpan breakEnd = TimeSpan.Parse(config["BreakHours:End"]);

            // 4. Plages valides = [workStart -> breakStart) et [breakEnd -> workEnd]
            bool inFirstSlot = nowTime >= workStart && nowTime < breakStart;
            bool inSecondSlot = nowTime >= breakEnd && nowTime <= workEnd;

            bool isInWorkingPeriod = inFirstSlot || inSecondSlot;

            return !isInWorkingPeriod;
        }

        private void ShowMessage(bool isOvertime)
        {
            string message = isOvertime
                ? ShowBreakMessage()
                : ShowNormalHoursMessage();

            labelMessage.Text = message;
        }


        private string ShowBreakMessage()
        {
            var breakMessages = new List<string>
    {
        "Time for a break!",
        "Take a short break.",
        "Stretch and relax!",
        "Break time! Step away from your desk.",
        "Rest your eyes for a few minutes.",
        "Breathe deeply and relax.",
        "Enjoy a quick walk.",
        "Grab a healthy snack.",
        "Hydrate yourself!",
        "Take a moment to unwind.",
        "Pause and refresh your mind.",
        "Relax and recharge.",
        "Step outside for fresh air.",
        "Take a mental break.",
        "Give yourself a short rest.",
        "Take a few deep breaths.",
        "Clear your mind for a bit.",
        "Take a quick stretch.",
        "Enjoy a brief pause.",
        "Take a moment to relax.",
        "“Rest and be thankful.”",
        "“The most productive thing is to relax.”",
        "“A change of work is the best rest.”",
        "“Rest is not idleness.”",
        "“Rest is the sweet sauce of labor.”"
    };
            var random = new Random();
            int index = random.Next(breakMessages.Count);
            return breakMessages[index];
        }

        private bool isWelcomeMessageShown = false;

        private string ShowNormalHoursMessage()
        {
            var now = DateTime.Now;
            var remainingTime = endTime - now;
            var remainingMinutes = (int)remainingTime.TotalMinutes;

            var welcomeMessages = new List<string>
    {
        "Welcome! Let's make today productive!",
        "Good to see you! Ready to tackle today's tasks?",
        "Hello! Let's get started on a great day!",
        "Welcome back! Let's achieve some goals today!",
        "Hi there! Ready to make progress?",
        "Good day! Let's get things done!",
        "Welcome! Let's have a productive session!",
        "Hello! Time to get to work!",
        "Welcome back! Let's make today count!",
        "Hi! Ready to dive into today's work?",
        "Good to see you! Let's get started!",
        "Welcome! Let's make today amazing!",
        "Hello! Ready to accomplish great things?",
        "Welcome back! Let's focus and succeed!",
        "Hi there! Let's start the day strong!",
        "Good day! Ready to be productive?",
        "Welcome! Let's achieve our goals!",
        "Hello! Time to get things done!",
        "Welcome back! Let's make progress!",
        "Hi! Ready to work hard and succeed?"
    };

            var earlyMessages = new List<string>
    {
        "Within working hours.",
        "No overtime.",
        "Regular hours.",
        "On schedule!",
        "Great job!",
        "Keep it up!",
        $"Working hours: {startTime.ToShortTimeString()} - {endTime.ToShortTimeString()}.",
        "Regular hours in progress.",
        "Scheduled hours.",
        "Take breaks!",
        "Stay hydrated!",
        "Remember to stretch!",
        "You're doing great!",
        "Stay focused!",
        "Keep going!",
        "You're on track!",
        "Maintain your pace.",
        "Stay productive!",
        "Keep the momentum!",
        "You're making progress!",
        "“Do great work, love what you do.”",
        "“Success is not the key to happiness.”",
        "“Love your job, never work a day.”",
        "“Pleasure in the job puts perfection.”",
        "“Work hard, be kind, amazing things.”",
        "“Predict the future by creating it.”",
        "You're on fire!",
        "Keep the energy high!",
        "Stay on top of it!",
        "You're doing awesome!",
        "Keep the good work going!",
        "You're a star!",
        "Keep shining!",
        "You're rocking it!",
        "Stay awesome!",
        "Keep the pace steady!"
    };

            var midMessages = new List<string>
    {
        "Halfway there!",
        "Keep pushing!",
        "You're doing amazing!",
        "Stay strong!",
        "Keep up the great work!",
        "You're on track!",
        "Keep the momentum going!",
        "Stay motivated!",
        "You're making great progress!",
        "Keep your focus!",
        "You're halfway done!",
        "Stay determined!",
        "Keep your energy up!",
        "You're doing well!",
        "Stay on task!",
        "Keep your spirits high!",
        "You're almost there!",
        "Stay positive!",
        "Keep your head up!",
        "You're doing fantastic!",
        "“Hard work brings luck.”",
        "“Success is small efforts.”",
        "“Believe, you're halfway there.”",
        "“Perseverance is not a long race.”",
        "“Doubts limit our realization.”",
        "“Future depends on today.”",
        "You're halfway through, keep it up!",
        "Keep the momentum strong!",
        "You're doing great, stay focused!",
        "Keep pushing, you're doing fantastic!",
        "Stay strong, you're halfway there!",
        "Keep up the amazing work!",
        "You're making great progress, keep going!",
        "Stay motivated, you're doing well!",
        "Keep your spirits high, you're halfway done!",
        "You're doing fantastic, keep it up!"
    };

            var lateMessages = new List<string>
    {
        $"Only {remainingMinutes} minutes left!",
        $"Just {remainingMinutes} minutes to go, you got this!",
        $"Hang in there, {remainingMinutes} minutes remaining!",
        $"Almost done, {remainingMinutes} minutes left!",
        "Finish strong!",
        "You're almost there!",
        "Great job, nearly finished!",
        "Final stretch, keep going!",
        "You're about to wrap up!",
        "Almost at the finish line!",
        "Keep pushing, almost done!",
        "You're nearly there!",
        "Just a bit more to go!",
        "Stay focused, almost done!",
        "You're on the home stretch!",
        "Keep your eyes on the prize!",
        "You're so close!",
        "Almost there, keep going!",
        "You're finishing strong!",
        "Just a few more minutes!",
        "“The best way out is through.”",
        "“It seems impossible until done.”",
        "“The last mile is the longest.”",
        "“Finish line is the beginning.”",
        "“Hard battle, sweet victory.”",
        "“Success is not final.”",
        "Almost there, finish strong!",
        "You're nearly done, keep going!",
        "Just a little more, you got this!",
        "You're on the final stretch, keep pushing!",
        "Almost at the finish line, stay focused!",
        "You're about to wrap up, great job!",
        "Keep going, you're almost there!",
        "You're finishing strong, keep it up!",
        "Just a few more minutes, stay strong!",
        "You're so close, keep pushing!"
    };

            List<string> selectedMessages;
            if (remainingMinutes > 240) // More than 4 hours remaining
            {
                selectedMessages = earlyMessages;
            }
            else if (remainingMinutes > 60) // Between 1 and 4 hours remaining
            {
                selectedMessages = midMessages;
            }
            else // Less than 1 hour remaining
            {
                selectedMessages = lateMessages;
            }

            var random = new Random();
            if (!isWelcomeMessageShown)
            {
                isWelcomeMessageShown = true;
                int index = random.Next(welcomeMessages.Count);
                return welcomeMessages[index];
            }
            else
            {
                int index = random.Next(selectedMessages.Count);
                return selectedMessages[index];
            }
        }


        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                Log.Information("System is suspending. Closing active session.");
                SessionManager.OnSleep();
            }
            else if (e.Mode == PowerModes.Resume)
            {
                Log.Information("System is resuming. Creating new active session.");
                // When we wake up, the old session is closed, so we create a new one.
                SessionManager.OnWakeUp();
            }
        }



        private void BackgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            if (statusStrip.InvokeRequired)
            {
                progressBar1.Visible = true;
                progressBar1.Value = e.ProgressPercentage;
                statusStrip.Invoke(new Action(() => toolStripStatusLabel.Text = $"Progress: {e.ProgressPercentage}%"));
            }
            else
            {
                progressBar1.Visible = true;
                progressBar1.Value = e.ProgressPercentage;
                toolStripStatusLabel.Text = $"Progress: {e.ProgressPercentage}%";
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            if (e.Cancelled)
            {
                toolStripStatusLabel.Text = "Export canceled.";
            }
            else if (e.Error != null)
            {
                toolStripStatusLabel.Text = $"Error: {e.Error.Message}";
            }

        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string task = e.Argument as string;
            if (task == "ExportToExcel")
            {
                ExportToExcel();
                e.Result = "Export done.";
            }
            else if (task == "SendEmailSummary")
            {
                SendMonthlySummary();
                e.Result = "Report sent.";
            }
            else if (task == "OpenSettings")
            {
                OpenSettings();
                e.Result = "Settings opened.";
            }
            else if (task == "ShowDashboard")
            {
                ShowDashboard();
                e.Result = "Settings opened.";
            }
        }

        private void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day)
                        .CreateLogger();
        }

        private void InitializeDatabase(string databasePath)
        {
            // On ne change rien si la base de données existe déjà.
            if (File.Exists(databasePath))
            {
                Log.Information("The database already exists. No action is required.");
                return;
            }

            try
            {
                Log.Information("Database not found. Attempting to create with Entity Framework...");

                // On crée une instance de notre DbContext
                using (var db = new YotTrackerDbContext(databasePath))
                {
                    // CETTE LIGNE CRÉE LA BASE DE DONNÉES ET TOUTES LES TABLES
                    // en se basant sur votre configuration dans YotTrackerDbContext.cs
                    db.Database.EnsureCreated();
                    Log.Information("Database and tables created successfully via EF Core.");

                    // On remplit les données initiales
                    SeedInitialData(db);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "CRITICAL failure of database creation");
                MessageBox.Show($"A fatal error occurred while creating the database. The application will close. Details: {ex.Message}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        /// <summary>
        /// Remplit la base de données nouvellement créée avec les données statiques (jours fériés).
        /// </summary>
        private void SeedInitialData(YotTrackerDbContext context)
        {
            Log.Information("Filling in initial data (holidays)...");

            var fixedHolidays = new[]
            {
        new FixedPublicHoliday { Month = 1, Day = 1 },    // New Year's Day
        new FixedPublicHoliday { Month = 1, Day = 11 },   // Proclamation of Independence Day
        new FixedPublicHoliday { Month = 5, Day = 1 },    // Labour Day
        new FixedPublicHoliday { Month = 7, Day = 30 },   // Throne Day
        new FixedPublicHoliday { Month = 8, Day = 14 },   // Oued Ed-Dahab Day
        new FixedPublicHoliday { Month = 8, Day = 20 },   // Revolution Day
        new FixedPublicHoliday { Month = 8, Day = 21 },   // Youth Day
        new FixedPublicHoliday { Month = 11, Day = 6 },   // Green March Day
        new FixedPublicHoliday { Month = 11, Day = 18 }   // Independence Day
    };
            context.FixedPublicHolidays.AddRange(fixedHolidays);

            var movableHolidays = new[]
            {
        new MovablePublicHoliday { Month = 4, Day = 10 }, // Eid al-Fitr
        new MovablePublicHoliday { Month = 4, Day = 11 }, // Eid al-Fitr Holiday
        new MovablePublicHoliday { Month = 6, Day = 20 }, // Eid al-Adha
        new MovablePublicHoliday { Month = 6, Day = 21 }, // Eid al-Adha Holiday
        new MovablePublicHoliday { Month = 7, Day = 30 }, // Islamic New Year
        new MovablePublicHoliday { Month = 9, Day = 27 }, // The Prophet Muhammad's Birthday
        new MovablePublicHoliday { Month = 9, Day = 28 }  // The Prophet Muhammad's Birthday Holiday
    };
            context.MovablePublicHolidays.AddRange(movableHolidays);

            // On sauvegarde toutes ces nouvelles données en une seule fois.
            context.SaveChanges();
            Log.Information("Holidays successfully inserted.");
        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };
            contextMenu = new ContextMenuStrip();
            exportMenuItem = new ToolStripMenuItem("Export to Excel", null, btn_ExportToExcel);
            settingsMenuItem = new ToolStripMenuItem("Settings", null, btn_OpenSettings);
            contextMenu.Items.Add(exportMenuItem);
            contextMenu.Items.Add(settingsMenuItem);
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }




        public static string FormatDuration(int minutes)
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


        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void SendMonthlySummary()
        {
            if (panel9.Visible)
            {
                ShowCustomMessage("This feature is not accessible unless you are on the main page");
                return;
            }

            Log.Information("Start sending monthly summary e-mail...");

            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // --- 1) Périmètre : mois courant ---
                    var now = DateTime.Now;
                    var monthStart = new DateTime(now.Year, now.Month, 1);
                    var nextMonthStart = monthStart.AddMonths(1);

                    // --- 2) Totaux en temps réel (incluant éventuellement la session en cours) ---
                    var (netTotal, normalDaysInDb, saturdaysInDb, sundaysInDb, holidaysInDb) = CalculateRealTimeOvertimeTotals(db);

                    // --- 3) Récupérer les sessions terminées du mois courant ---
                    var completedSessions = db.WorkSessions
                        .Where(s => s.EndTime != null &&
                                    s.StartTime >= monthStart &&
                                    s.StartTime < nextMonthStart)
                        .OrderBy(s => s.StartTime)
                        .ToList();

                    // --- 4) Overtime par type sur le mois courant ---
                    int normalMonth = completedSessions
                        .Where(s => s.TypeOverTime == "NormalDays")
                        .Sum(s => s.OverTimeMinutes ?? 0);

                    int saturdayMonth = completedSessions
                        .Where(s => s.TypeOverTime == "Saturdays")
                        .Sum(s => s.OverTimeMinutes ?? 0);

                    int sundayMonth = completedSessions
                        .Where(s => s.TypeOverTime == "Sundays")
                        .Sum(s => s.OverTimeMinutes ?? 0);

                    int holidayMonth = completedSessions
                        .Where(s => s.TypeOverTime == "PublicHolidays")
                        .Sum(s => s.OverTimeMinutes ?? 0);

                    int totalMonth = normalMonth + saturdayMonth + sundayMonth + holidayMonth;

                    // --- 5) Overtime par jour de la semaine (mois courant) ---
                    var overtimeByDay = completedSessions
                        .Where(s => (s.OverTimeMinutes ?? 0) > 0)
                        .GroupBy(s => s.StartTime.DayOfWeek)
                        .Select(g => new { Day = g.Key, Minutes = g.Sum(x => x.OverTimeMinutes ?? 0) })
                        .OrderByDescending(x => x.Minutes)
                        .ToList();

                    // --- 6) Construire le corps du mail (HTML) ---
                    var sb = new StringBuilder();

                    sb.AppendLine("<h2 style='color:#151C26'>Monthly Overtime Summary</h2>");
                    sb.AppendLine($"<p>Period: <strong>{monthStart:yyyy-MM-dd}</strong> → <strong>{nextMonthStart.AddDays(-1):yyyy-MM-dd}</strong></p>");

                    sb.AppendLine("<h3>Real-time totals (all time)</h3>");
                    sb.AppendLine("<ul>");
                    sb.AppendLine($"<li><strong>Total Overtime (net):</strong> {FormatDuration(netTotal)}</li>");
                    sb.AppendLine($"<li>Normal Days: {FormatDuration(normalDaysInDb)}</li>");
                    sb.AppendLine($"<li>Saturdays: {FormatDuration(saturdaysInDb)}</li>");
                    sb.AppendLine($"<li>Sundays: {FormatDuration(sundaysInDb)}</li>");
                    sb.AppendLine($"<li>Public Holidays: {FormatDuration(holidaysInDb)}</li>");
                    sb.AppendLine("</ul>");

                    sb.AppendLine("<h3>Current month breakdown</h3>");
                    sb.AppendLine("<ul>");
                    sb.AppendLine($"<li><strong>Total Overtime (without compensation):</strong> {FormatDuration(totalMonth)}</li>");
                    if (normalMonth > 0) sb.AppendLine($"<li>Normal Days: {FormatDuration(normalMonth)}</li>");
                    if (saturdayMonth > 0) sb.AppendLine($"<li>Saturdays: {FormatDuration(saturdayMonth)}</li>");
                    if (sundayMonth > 0) sb.AppendLine($"<li>Sundays: {FormatDuration(sundayMonth)}</li>");
                    if (holidayMonth > 0) sb.AppendLine($"<li>Public Holidays: {FormatDuration(holidayMonth)}</li>");
                    sb.AppendLine("</ul>");

                    sb.AppendLine("<h3>Overtime by day-of-week (current month)</h3>");
                    if (overtimeByDay.Count == 0)
                    {
                        sb.AppendLine("<p>No overtime recorded this month.</p>");
                    }
                    else
                    {
                        sb.AppendLine("<ul>");
                        foreach (var d in overtimeByDay)
                        {
                            sb.AppendLine($"<li>{d.Day}: {FormatDuration(d.Minutes)}</li>");
                        }
                        sb.AppendLine("</ul>");
                    }

                    // --- 7) Envoi du mail ---
                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                        client.EnableSsl = true;

                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(emailSender);
                            message.To.Add(emailRecipient);
                            message.Subject = $"Monthly Overtime Summary - {monthStart:yyyy-MM}";
                            message.IsBodyHtml = true;
                            message.Body = sb.ToString();

                            client.Send(message);
                        }
                    }

                    Log.Information("Monthly summary sent successfully.");

                    if (statusStrip.InvokeRequired)
                        statusStrip.Invoke(new Action(() => toolStripStatusLabel.Text = "Monthly report sent."));
                    else
                        toolStripStatusLabel.Text = "Monthly report sent.";

                    ShowCustomMessage("Monthly report sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending monthly summary.");

                if (statusStrip.InvokeRequired)
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel.Text = "Sending report failed!"));
                else
                    toolStripStatusLabel.Text = "Sending report failed!";

                ShowCustomMessage("Sending report failed!");
            }
        }



        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync("ExportToExcel");
            }
        }

        private void Settings_ClickHandler(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync("OpenSettings");
            }
        }
        private void Dashboard_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync("ShowDashboard");
            }
        }

        private void ButtonSendSummary_Click(object sender, EventArgs e)
        {

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync("SendEmailSummary");
            }
        }

        /// <summary>
        /// Calcule les totaux d'heures supplémentaires en temps réel, en incluant la session active.
        /// </summary>
        /// <returns>Un tuple contenant les différents totaux d'overtime.</returns>
        private (int total, int normal, int saturdays, int sundays, int holidays) CalculateRealTimeOvertimeTotals(YotTrackerDbContext db)
        {
            // 1. Calculer les totaux des sessions DÉJÀ TERMINÉES
            int totalInDb = db.WorkSessions.Where(s => s.EndTime != null).Sum(s => s.OverTimeMinutes ?? 0);
            int normalInDb = db.WorkSessions.Where(s => s.EndTime != null && s.TypeOverTime == "NormalDays").Sum(s => s.OverTimeMinutes ?? 0);
            int saturdaysInDb = db.WorkSessions.Where(s => s.EndTime != null && s.TypeOverTime == "Saturdays").Sum(s => s.OverTimeMinutes ?? 0);
            int sundaysInDb = db.WorkSessions.Where(s => s.EndTime != null && s.TypeOverTime == "Sundays").Sum(s => s.OverTimeMinutes ?? 0);
            int holidaysInDb = db.WorkSessions.Where(s => s.EndTime != null && s.TypeOverTime == "PublicHolidays").Sum(s => s.OverTimeMinutes ?? 0);


            var activeSession = db.WorkSessions.FirstOrDefault(s => s.EndTime == null);
            if (IsOvertime(DateTime.Now, db) && activeSession != null)
            {
                TimeSpan runningDuration = DateTime.Now - activeSession.StartTime;
                int liveMinutes = (int)runningDuration.TotalMinutes;
                string liveType = SessionManager.GetOvertimeType(activeSession.StartTime, db); // Assurez-vous que cette méthode est accessible

                switch (liveType)
                {
                    case "NormalDays": normalInDb += liveMinutes; break;
                    case "Saturdays": saturdaysInDb += liveMinutes; break;
                    case "Sundays": sundaysInDb += liveMinutes; break;
                    case "PublicHolidays": holidaysInDb += liveMinutes; break;
                }
                totalInDb += liveMinutes;
            }

            // 3. Calculer le solde net en soustrayant les compensations
            int totalCompensation = db.Compensations.Sum(c => (int?)c.CompensationMinutes ?? 0);
            int netTotal = Math.Max(0, totalInDb - totalCompensation);

            normalInDb = Math.Max(0, normalInDb - totalCompensation);

            return (netTotal, normalInDb, saturdaysInDb, sundaysInDb, holidaysInDb);

        }

        private void ExportToExcel()
        {
            if (panel9.Visible == true)
            {
                ShowCustomMessage("this feature is not accessible unless you are on the main page");
                return;
            }
            Log.Information("Starting export to Excel...");
            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    // --- 1. Récupérer toutes les données nécessaires en une seule fois ---

                    // a) On calcule les totaux en temps réel (inclut la session en cours)
                    var (netTotal, normal, saturdays, sundays, holidays) = CalculateRealTimeOvertimeTotals(db);

                    // b) On récupère la liste des sessions terminées pour les détails
                    var completedSessions = db.WorkSessions.Where(s => s.EndTime != null).OrderBy(s => s.StartTime).ToList();

                    using (var workbook = new XLWorkbook())
                    {
                        // --- 2. Créer la Feuille de Résumé ---
                        var summarySheet = workbook.Worksheets.Add("Overtime Summary");
                        summarySheet.Cell(1, 1).Value = "Total OverTime";
                        summarySheet.Cell(1, 2).Value = "Normal Days";
                        summarySheet.Cell(1, 3).Value = "Saturdays";
                        summarySheet.Cell(1, 4).Value = "Sundays";
                        summarySheet.Cell(1, 5).Value = "Public Holidays";

                        summarySheet.Cell(2, 1).Value = FormatDuration(netTotal);
                        summarySheet.Cell(2, 2).Value = FormatDuration(normal);
                        summarySheet.Cell(2, 3).Value = FormatDuration(saturdays);
                        summarySheet.Cell(2, 4).Value = FormatDuration(sundays);
                        summarySheet.Cell(2, 5).Value = FormatDuration(holidays);

                        summarySheet.Row(1).Style.Font.Bold = true;
                        summarySheet.Columns().AdjustToContents();

                        // --- 3. Créer la Feuille de Vue Détaillée (basée sur les sessions terminées) ---
                        var detailedSheet = workbook.Worksheets.Add("Detailed View");
                        detailedSheet.Cell(1, 1).Value = "Start Time";
                        detailedSheet.Cell(1, 2).Value = "End Time";
                        detailedSheet.Cell(1, 3).Value = "OverTime";
                        detailedSheet.Cell(1, 4).Value = "Type";
                        detailedSheet.Cell(1, 5).Value = "Notes";
                        detailedSheet.Row(1).Style.Font.Bold = true;

                        int row = 2;
                        foreach (var session in completedSessions)
                        {
                            detailedSheet.Cell(row, 1).Value = session.StartTime;
                            detailedSheet.Cell(row, 2).Value = session.EndTime;
                            detailedSheet.Cell(row, 3).Value = FormatDuration(session.OverTimeMinutes ?? 0);
                            detailedSheet.Cell(row, 4).Value = session.TypeOverTime;
                            detailedSheet.Cell(row, 5).Value = session.Notes;
                            row++;
                        }
                        detailedSheet.Columns().AdjustToContents();
                        detailedSheet.Column(1).Width = 20;
                        detailedSheet.Column(2).Width = 20;


                        // --- 4. Créer la Feuille de Rapport Journalier (basée sur les sessions terminées) ---
                        var dailyReportSheet = workbook.Worksheets.Add("Daily Reports");
                        dailyReportSheet.Cell(1, 1).Value = "Date";
                        dailyReportSheet.Cell(1, 2).Value = "Total OverTime (minutes)";
                        dailyReportSheet.Row(1).Style.Font.Bold = true;

                        var dailySummary = completedSessions
                            .GroupBy(s => s.StartTime.Date)
                            .Select(g => new { Date = g.Key, TotalMinutes = g.Sum(s => s.OverTimeMinutes ?? 0) })
                            .OrderBy(x => x.Date);

                        row = 2;
                        foreach (var dailyData in dailySummary)
                        {
                            dailyReportSheet.Cell(row, 1).Value = dailyData.Date;
                            dailyReportSheet.Cell(row, 2).Value = FormatDuration(dailyData.TotalMinutes);
                            row++;
                        }
                        dailyReportSheet.Columns().AdjustToContents();
                        dailyReportSheet.Column(1).Width = 20;


                        // --- 5. Sauvegarder et ouvrir le fichier ---
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string fileName = $"Overtime_Report_{timestamp}.xlsx";
                        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                        workbook.SaveAs(filePath);
                        System.Diagnostics.Process.Start(filePath);
                    }
                    toolStripStatusLabel.Text = "Exportation réussie.";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during export to Excel.");
                toolStripStatusLabel.Text = "Export error.";
            }
        }

        private void OpenSettings()
        {
            if (panel9.Visible == true)
            {
                ShowCustomMessage("this feature is not accessible unless you are on the main page");
                return;
            }
            if (panel9.InvokeRequired)
            {
                panel9.Invoke(new Action(() => panel9.Visible = true));
                panel9.Invoke(new Action(() => panel9.BringToFront()));
            }
            else
            {
                panel9.Visible = true;
            }


            var configControl = new Configuration(config); 

            configControl.OnSave += (configDataFromForm) =>
            {
                try
                {
                    // On recrée la structure JSON complète...
                    var fullJsonStructure = ConfigManager.CreateFullJsonStructure(configDataFromForm);

                    // On l'écrit sur le disque...
                    File.WriteAllText(appSettingsPath, JsonConvert.SerializeObject(fullJsonStructure, Formatting.Indented));

                    // On sauvegarde en BDD...
                    using (var db = new YotTrackerDbContext(_databasePath))
                    {
                        var currentlyActiveConfigs = db.AppSettingsHistory.Where(c => c.isActive).ToList();
                        foreach (var c in currentlyActiveConfigs)
                        {
                            c.isActive = false;
                        }

                        configDataFromForm.isActive = true;
                        configDataFromForm.ActivationDate = DateTime.Now;
                        db.AppSettingsHistory.Add(configDataFromForm);
                        db.SaveChanges();
                    }

                    Log.Information("Configuration successfully created by the assistant.");

                    // Mettre à jour la config courante
                    config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                                .AddJsonFile(appSettingsPath, false, true)
                                .Build();

                    ShowCustomMessage("Configuration saved successfully.");


                    StartApplicationLogic();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the configuration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };

            configControl.OnCancel += () =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => { StartApplicationLogic(); }));
                else
                {
                    StartApplicationLogic();
                }
            };

            if (panel9.InvokeRequired)
            {
                panel9.Invoke(new Action(() =>
                {
                    panel9.Controls.Clear();
                    panel9.Controls.Add(configControl);
                    configControl.Dock = DockStyle.Fill;
                }));
            }
            else
            {
                panel9.Controls.Clear();
                panel9.Controls.Add(configControl);
                configControl.Dock = DockStyle.Fill;
            }
        }



        private void btn_OpenSettings(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void btn_ExportToExcel(object sender, EventArgs e)
        {
            ExportToExcel(); 
        }



        private void picture_MouseHover(object sender, EventArgs e)
        {
            HL_Send.Show();
            HL_export.Hide();
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            HL_Send.Hide();
            HL_export.Show();
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }


        private void ShowCustomMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowCustomMessage(message)));
                return;
            }

            label4.Text = message;
            panel10.BringToFront();
            panel10.Visible = true;

            isPanel10Closed = false;

            foreach (Control ctrl in this.Controls)
                if (ctrl != panel10) ctrl.Enabled = false;

            while (!isPanel10Closed)
                Application.DoEvents();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;

            foreach (Control ctrl in this.Controls)
                ctrl.Enabled = true;

            isPanel10Closed = true;
        }

        private void ShowDashboard()
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)ShowDashboard);
                return;
            }

            if (panel9.Visible)
            {
                ShowCustomMessage("This feature is not accessible unless you are on the main page");
                return;
            }

            panel9.Visible = true;
            panel9.BringToFront();

            var dashboard = new DashboardStats(_databasePath);

            dashboard.OnClose += () =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        panel9.Controls.Clear();
                        panel9.Visible = false;
                        StartApplicationLogic();
                    }));
                }
                else
                {
                    panel9.Controls.Clear();
                    panel9.Visible = false;
                    StartApplicationLogic();
                }
            };

            panel9.Controls.Clear();
            panel9.Controls.Add(dashboard);
            dashboard.Dock = DockStyle.Fill;
        }

    }
}