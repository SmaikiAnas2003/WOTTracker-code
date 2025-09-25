using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;
using WOTTracker.Models;

namespace WOTTracker
{
    public partial class Configuration : UserControl
    {
        // --- 1. DÉCLARATION DES ÉVÉNEMENTS ---
        // Événement pour notifier que la sauvegarde a réussi. Il transporte les données de configuration.
        public event Action<AppSettingsHistory> OnSave;
        // Événement pour notifier que l'utilisateur a annulé.
        public event Action OnCancel;

        private bool isPanel10Closed = false;




        public Configuration()
        {
            InitializeComponent();

            panel10.Visible = false;


            // On assigne les événements après que les composants soient créés
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // On peut mettre les valeurs par défaut ici
            this.timePickerWorkStart.Value = System.DateTime.Today.AddHours(8);
            this.timePickerWorkEnd.Value = System.DateTime.Today.AddHours(17);
        }


        public Configuration(IConfiguration config) : this()
        {
            if (config != null)
            {
                this.comboUserRole.Text = config["UserRole"];
                this.timePickerWorkStart.Value = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:Start"]));
                this.timePickerWorkEnd.Value = DateTime.Today.Add(TimeSpan.Parse(config["WorkingHours:End"]));

                int.TryParse(config["Tolerance:StartMinutes"], out int startMinutes);
                this.numericToleranceStart.Value = startMinutes;

                int.TryParse(config["Tolerance:EndMinutes"], out int endMinutes);
                this.numericToleranceEnd.Value = endMinutes;

                this.timePickerBreakStart.Value = DateTime.Today.Add(TimeSpan.Parse(config["BreakHours:Start"]));
                this.timePickerBreakEnd.Value = DateTime.Today.Add(TimeSpan.Parse(config["BreakHours:End"]));
                this.txtEmailRecipient.Text = config["Email:Recipient"];
                this.txtEmailSender.Text = config["Email:Sender"];

                if (int.TryParse(config["ExpirationOverTimePeriod"], out int period))
                    this.numericExpirationPeriod.Value = Math.Max(this.numericExpirationPeriod.Minimum, Math.Min(period, this.numericExpirationPeriod.Maximum));

                string unit = config["ExpirationOverTimeUnit"];
                if (!string.IsNullOrWhiteSpace(unit) && this.comboExpirationUnit.Items.Contains(unit))
                    this.comboExpirationUnit.SelectedItem = unit;



            }
        }


        public AppSettingsHistory GetConfiguration()
        {
            var config = new AppSettingsHistory
            {
                UserRole = this.comboUserRole.Text,
                WorkingHoursStart = this.timePickerWorkStart.Value.ToString("HH:mm"),
                WorkingHoursEnd = this.timePickerWorkEnd.Value.ToString("HH:mm"),
                ToleranceStartMinutes = (int)this.numericToleranceStart.Value,
                ToleranceEndMinutes = (int)this.numericToleranceEnd.Value,
                BreakHoursStart = this.timePickerBreakStart.Value.ToString("HH:mm"),
                BreakHoursEnd = this.timePickerBreakEnd.Value.ToString("HH:mm"),
                EmailRecipient = this.txtEmailRecipient.Text,
                EmailSender = this.txtEmailSender.Text,
                ExpirationOverTime = CalculateExpirationDate(),
                ActivationDate = DateTime.Now,
            };

            return config;
        }

        private DateTime CalculateExpirationDate()
        {
            int duration = (int)this.numericExpirationPeriod.Value;
            string unit = this.comboExpirationUnit.SelectedItem?.ToString();

            if (unit == "Month")
                return DateTime.Today.AddMonths(duration);
            else if (unit == "Year")
                return DateTime.Today.AddYears(duration);
            else
                return DateTime.Today; // fallback
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // --- 1. Role Validation ---
                if (string.IsNullOrWhiteSpace(this.comboUserRole.Text))
                {
                    ShowCustomMessage("Please select a role.");
                    return;
                }

                // --- 2. Work and Break Hours Validation ---
                TimeSpan workStart = timePickerWorkStart.Value.TimeOfDay;
                TimeSpan workEnd = timePickerWorkEnd.Value.TimeOfDay;
                TimeSpan breakStart = timePickerBreakStart.Value.TimeOfDay;
                TimeSpan breakEnd = timePickerBreakEnd.Value.TimeOfDay;

                if (workEnd <= workStart)
                {
                    ShowCustomMessage("Work end time must be after work start time.");
                    return;
                }

                if (breakEnd <= breakStart)
                {
                    ShowCustomMessage("Break end time must be after break start time.");
                    return;
                }

                if (breakStart <= workStart)
                {
                    ShowCustomMessage("Break start time must be after work start time.");
                    return;
                }

                if (breakEnd >= workEnd)
                {
                    ShowCustomMessage("Break end time must be before work end time.");
                    return;
                }

                // --- 3. Tolerance Validation ---
                if (!int.TryParse(numericToleranceStart.Text, out _))
                {
                    ShowCustomMessage("Start tolerance must be a valid integer (in minutes).");
                    return;
                }

                if (!int.TryParse(numericToleranceEnd.Text, out _))
                {
                    ShowCustomMessage("End tolerance must be a valid integer (in minutes).");
                    return;
                }

                // --- 4. Email Validation ---
                if (string.IsNullOrWhiteSpace(this.txtEmailRecipient.Text))
                {
                     ShowCustomMessage("Please enter the recipient email address.");
                    return;
                }
                if (!IsValidEmail(this.txtEmailRecipient.Text))
                {
                    ShowCustomMessage("The recipient email format is invalid.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.txtEmailSender.Text))
                {
                    ShowCustomMessage("Please enter the sender email address.");
                    return;
                }
                if (!IsValidEmail(this.txtEmailSender.Text))
                {
                    ShowCustomMessage("The sender email format is invalid.");
                    return;
                }

                if (CalculateExpirationDate().Date <= DateTime.Today)
                {
                    ShowCustomMessage("The ExpirationOverTime must be in the future.");
                    return;
                }

                // --- 5. If all is valid, raise the OnSave event ---
                ShowCustomMessage("Configuration saved successfully.");

                var config = GetConfiguration();
                OnSave?.Invoke(config);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Vérifie si une chaîne de caractères a un format d'email valide.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // On essaie de créer une adresse email. Si ça ne lance pas d'exception, le format est bon.
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // On notifie simplement le MainForm que l'opération a été annulée.
            OnCancel?.Invoke();
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
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != panel10)
                    ctrl.Enabled = false;
            }

            while (!isPanel10Closed)
            {
                Application.DoEvents(); // Permet à l'UI de continuer à répondre
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;

            // Réactiver les autres contrôles
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = true;
            }

            isPanel10Closed = true; // Débloque la boucle
        }

    }
}
