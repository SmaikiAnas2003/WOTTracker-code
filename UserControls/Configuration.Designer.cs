using System.Windows.Forms;

namespace WOTTracker
{
    partial class Configuration
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.comboUserRole = new System.Windows.Forms.ComboBox();
            this.lblWorkHours = new System.Windows.Forms.Label();
            this.lblWorkStart = new System.Windows.Forms.Label();
            this.timePickerWorkStart = new System.Windows.Forms.DateTimePicker();
            this.lblWorkEnd = new System.Windows.Forms.Label();
            this.timePickerWorkEnd = new System.Windows.Forms.DateTimePicker();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.lblToleranceStart = new System.Windows.Forms.Label();
            this.numericToleranceStart = new System.Windows.Forms.NumericUpDown();
            this.lblToleranceEnd = new System.Windows.Forms.Label();
            this.numericToleranceEnd = new System.Windows.Forms.NumericUpDown();
            this.lblBreakHours = new System.Windows.Forms.Label();
            this.lblBreakStart = new System.Windows.Forms.Label();
            this.timePickerBreakStart = new System.Windows.Forms.DateTimePicker();
            this.lblBreakEnd = new System.Windows.Forms.Label();
            this.timePickerBreakEnd = new System.Windows.Forms.DateTimePicker();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailRecipient = new System.Windows.Forms.Label();
            this.txtEmailRecipient = new System.Windows.Forms.TextBox();
            this.lblEmailSender = new System.Windows.Forms.Label();
            this.txtEmailSender = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblExpirationOverTime = new System.Windows.Forms.Label();
            this.numericExpirationPeriod = new System.Windows.Forms.NumericUpDown();
            this.comboExpirationUnit = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericToleranceStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericToleranceEnd)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericExpirationPeriod)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitle.Location = new System.Drawing.Point(15, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(344, 32);
            this.lblTitle.TabIndex = 25;
            this.lblTitle.Text = "Please enter the configuration";
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUserRole.ForeColor = System.Drawing.Color.Silver;
            this.lblUserRole.Location = new System.Drawing.Point(15, 15);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(74, 19);
            this.lblUserRole.TabIndex = 26;
            this.lblUserRole.Text = "User Role :";
            // 
            // comboUserRole
            // 
            this.comboUserRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUserRole.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboUserRole.FormattingEnabled = true;
            this.comboUserRole.Items.AddRange(new object[] {
            "Manager",
            "Coordinator",
            "Responsible",
            "HR"});
            this.comboUserRole.Location = new System.Drawing.Point(149, 15);
            this.comboUserRole.Name = "comboUserRole";
            this.comboUserRole.Size = new System.Drawing.Size(210, 21);
            this.comboUserRole.TabIndex = 27;
            // 
            // lblWorkHours
            // 
            this.lblWorkHours.AutoSize = true;
            this.lblWorkHours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblWorkHours.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWorkHours.ForeColor = System.Drawing.Color.Silver;
            this.lblWorkHours.Location = new System.Drawing.Point(3, 50);
            this.lblWorkHours.Name = "lblWorkHours";
            this.lblWorkHours.Size = new System.Drawing.Size(105, 19);
            this.lblWorkHours.TabIndex = 28;
            this.lblWorkHours.Text = "Working Hours ";
            // 
            // lblWorkStart
            // 
            this.lblWorkStart.AutoSize = true;
            this.lblWorkStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblWorkStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWorkStart.ForeColor = System.Drawing.Color.Silver;
            this.lblWorkStart.Location = new System.Drawing.Point(16, 80);
            this.lblWorkStart.Name = "lblWorkStart";
            this.lblWorkStart.Size = new System.Drawing.Size(78, 19);
            this.lblWorkStart.TabIndex = 29;
            this.lblWorkStart.Text = "Start Time :";
            // 
            // timePickerWorkStart
            // 
            this.timePickerWorkStart.CustomFormat = "HH:mm";
            this.timePickerWorkStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePickerWorkStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerWorkStart.Location = new System.Drawing.Point(165, 80);
            this.timePickerWorkStart.Name = "timePickerWorkStart";
            this.timePickerWorkStart.ShowUpDown = true;
            this.timePickerWorkStart.Size = new System.Drawing.Size(80, 22);
            this.timePickerWorkStart.TabIndex = 30;
            // 
            // lblWorkEnd
            // 
            this.lblWorkEnd.AutoSize = true;
            this.lblWorkEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblWorkEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWorkEnd.ForeColor = System.Drawing.Color.Silver;
            this.lblWorkEnd.Location = new System.Drawing.Point(17, 113);
            this.lblWorkEnd.Name = "lblWorkEnd";
            this.lblWorkEnd.Size = new System.Drawing.Size(72, 19);
            this.lblWorkEnd.TabIndex = 31;
            this.lblWorkEnd.Text = "End Time :";
            // 
            // timePickerWorkEnd
            // 
            this.timePickerWorkEnd.CustomFormat = "HH:mm";
            this.timePickerWorkEnd.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePickerWorkEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerWorkEnd.Location = new System.Drawing.Point(165, 113);
            this.timePickerWorkEnd.Name = "timePickerWorkEnd";
            this.timePickerWorkEnd.ShowUpDown = true;
            this.timePickerWorkEnd.Size = new System.Drawing.Size(80, 22);
            this.timePickerWorkEnd.TabIndex = 32;
            // 
            // lblTolerance
            // 
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblTolerance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTolerance.ForeColor = System.Drawing.Color.Silver;
            this.lblTolerance.Location = new System.Drawing.Point(3, 149);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(127, 19);
            this.lblTolerance.TabIndex = 33;
            this.lblTolerance.Text = "Tolerance (minutes)";
            // 
            // lblToleranceStart
            // 
            this.lblToleranceStart.AutoSize = true;
            this.lblToleranceStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblToleranceStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblToleranceStart.ForeColor = System.Drawing.Color.Silver;
            this.lblToleranceStart.Location = new System.Drawing.Point(16, 177);
            this.lblToleranceStart.Name = "lblToleranceStart";
            this.lblToleranceStart.Size = new System.Drawing.Size(105, 19);
            this.lblToleranceStart.TabIndex = 34;
            this.lblToleranceStart.Text = "Tolerance start :";
            // 
            // numericToleranceStart
            // 
            this.numericToleranceStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericToleranceStart.Location = new System.Drawing.Point(165, 177);
            this.numericToleranceStart.Name = "numericToleranceStart";
            this.numericToleranceStart.Size = new System.Drawing.Size(80, 22);
            this.numericToleranceStart.TabIndex = 35;
            // 
            // lblToleranceEnd
            // 
            this.lblToleranceEnd.AutoSize = true;
            this.lblToleranceEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblToleranceEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblToleranceEnd.ForeColor = System.Drawing.Color.Silver;
            this.lblToleranceEnd.Location = new System.Drawing.Point(16, 205);
            this.lblToleranceEnd.Name = "lblToleranceEnd";
            this.lblToleranceEnd.Size = new System.Drawing.Size(100, 19);
            this.lblToleranceEnd.TabIndex = 36;
            this.lblToleranceEnd.Text = "Tolerance end :";
            // 
            // numericToleranceEnd
            // 
            this.numericToleranceEnd.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericToleranceEnd.Location = new System.Drawing.Point(165, 206);
            this.numericToleranceEnd.Name = "numericToleranceEnd";
            this.numericToleranceEnd.Size = new System.Drawing.Size(80, 22);
            this.numericToleranceEnd.TabIndex = 37;
            // 
            // lblBreakHours
            // 
            this.lblBreakHours.AutoSize = true;
            this.lblBreakHours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblBreakHours.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBreakHours.ForeColor = System.Drawing.Color.Silver;
            this.lblBreakHours.Location = new System.Drawing.Point(3, 239);
            this.lblBreakHours.Name = "lblBreakHours";
            this.lblBreakHours.Size = new System.Drawing.Size(84, 19);
            this.lblBreakHours.TabIndex = 38;
            this.lblBreakHours.Text = "Break Hours";
            // 
            // lblBreakStart
            // 
            this.lblBreakStart.AutoSize = true;
            this.lblBreakStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblBreakStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBreakStart.ForeColor = System.Drawing.Color.Silver;
            this.lblBreakStart.Location = new System.Drawing.Point(16, 270);
            this.lblBreakStart.Name = "lblBreakStart";
            this.lblBreakStart.Size = new System.Drawing.Size(83, 19);
            this.lblBreakStart.TabIndex = 39;
            this.lblBreakStart.Text = "Break Start :";
            // 
            // timePickerBreakStart
            // 
            this.timePickerBreakStart.CustomFormat = "HH:mm";
            this.timePickerBreakStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePickerBreakStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerBreakStart.Location = new System.Drawing.Point(165, 269);
            this.timePickerBreakStart.Name = "timePickerBreakStart";
            this.timePickerBreakStart.ShowUpDown = true;
            this.timePickerBreakStart.Size = new System.Drawing.Size(80, 22);
            this.timePickerBreakStart.TabIndex = 40;
            // 
            // lblBreakEnd
            // 
            this.lblBreakEnd.AutoSize = true;
            this.lblBreakEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblBreakEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBreakEnd.ForeColor = System.Drawing.Color.Silver;
            this.lblBreakEnd.Location = new System.Drawing.Point(16, 301);
            this.lblBreakEnd.Name = "lblBreakEnd";
            this.lblBreakEnd.Size = new System.Drawing.Size(77, 19);
            this.lblBreakEnd.TabIndex = 41;
            this.lblBreakEnd.Text = "Break End :";
            // 
            // timePickerBreakEnd
            // 
            this.timePickerBreakEnd.CustomFormat = "HH:mm";
            this.timePickerBreakEnd.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePickerBreakEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerBreakEnd.Location = new System.Drawing.Point(165, 302);
            this.timePickerBreakEnd.Name = "timePickerBreakEnd";
            this.timePickerBreakEnd.ShowUpDown = true;
            this.timePickerBreakEnd.Size = new System.Drawing.Size(80, 22);
            this.timePickerBreakEnd.TabIndex = 42;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.ForeColor = System.Drawing.Color.Silver;
            this.lblEmail.Location = new System.Drawing.Point(3, 334);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(129, 19);
            this.lblEmail.TabIndex = 43;
            this.lblEmail.Text = "Email Configuration";
            // 
            // lblEmailRecipient
            // 
            this.lblEmailRecipient.AutoSize = true;
            this.lblEmailRecipient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblEmailRecipient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailRecipient.ForeColor = System.Drawing.Color.Silver;
            this.lblEmailRecipient.Location = new System.Drawing.Point(16, 365);
            this.lblEmailRecipient.Name = "lblEmailRecipient";
            this.lblEmailRecipient.Size = new System.Drawing.Size(116, 19);
            this.lblEmailRecipient.TabIndex = 44;
            this.lblEmailRecipient.Text = "Recipient\'s email :";
            // 
            // txtEmailRecipient
            // 
            this.txtEmailRecipient.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailRecipient.Location = new System.Drawing.Point(165, 365);
            this.txtEmailRecipient.Name = "txtEmailRecipient";
            this.txtEmailRecipient.Size = new System.Drawing.Size(210, 22);
            this.txtEmailRecipient.TabIndex = 45;
            // 
            // lblEmailSender
            // 
            this.lblEmailSender.AutoSize = true;
            this.lblEmailSender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblEmailSender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailSender.ForeColor = System.Drawing.Color.Silver;
            this.lblEmailSender.Location = new System.Drawing.Point(16, 398);
            this.lblEmailSender.Name = "lblEmailSender";
            this.lblEmailSender.Size = new System.Drawing.Size(103, 19);
            this.lblEmailSender.TabIndex = 46;
            this.lblEmailSender.Text = "Sender\'s email :";
            // 
            // txtEmailSender
            // 
            this.txtEmailSender.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailSender.Location = new System.Drawing.Point(165, 397);
            this.txtEmailSender.Name = "txtEmailSender";
            this.txtEmailSender.Size = new System.Drawing.Size(210, 22);
            this.txtEmailSender.TabIndex = 47;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Silver;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(173, 483);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 48;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Silver;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(295, 483);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel3.Controls.Add(this.lblExpirationOverTime);
            this.panel3.Controls.Add(this.numericExpirationPeriod);
            this.panel3.Controls.Add(this.comboExpirationUnit);
            this.panel3.Controls.Add(this.lblUserRole);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.txtEmailSender);
            this.panel3.Controls.Add(this.txtEmailRecipient);
            this.panel3.Controls.Add(this.timePickerBreakEnd);
            this.panel3.Controls.Add(this.timePickerBreakStart);
            this.panel3.Controls.Add(this.numericToleranceEnd);
            this.panel3.Controls.Add(this.numericToleranceStart);
            this.panel3.Controls.Add(this.timePickerWorkEnd);
            this.panel3.Controls.Add(this.timePickerWorkStart);
            this.panel3.Controls.Add(this.comboUserRole);
            this.panel3.Controls.Add(this.lblWorkHours);
            this.panel3.Controls.Add(this.lblEmailSender);
            this.panel3.Controls.Add(this.lblWorkStart);
            this.panel3.Controls.Add(this.lblEmailRecipient);
            this.panel3.Controls.Add(this.lblEmail);
            this.panel3.Controls.Add(this.lblWorkEnd);
            this.panel3.Controls.Add(this.lblBreakEnd);
            this.panel3.Controls.Add(this.lblBreakStart);
            this.panel3.Controls.Add(this.lblTolerance);
            this.panel3.Controls.Add(this.lblBreakHours);
            this.panel3.Controls.Add(this.lblToleranceStart);
            this.panel3.Controls.Add(this.lblToleranceEnd);
            this.panel3.Location = new System.Drawing.Point(14, 86);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(383, 521);
            this.panel3.TabIndex = 50;
            // 
            // lblExpirationOverTime
            // 
            this.lblExpirationOverTime.AutoSize = true;
            this.lblExpirationOverTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.lblExpirationOverTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblExpirationOverTime.ForeColor = System.Drawing.Color.Silver;
            this.lblExpirationOverTime.Location = new System.Drawing.Point(5, 441);
            this.lblExpirationOverTime.Name = "lblExpirationOverTime";
            this.lblExpirationOverTime.Size = new System.Drawing.Size(139, 19);
            this.lblExpirationOverTime.TabIndex = 51;
            this.lblExpirationOverTime.Text = "Expiration OverTime :";
            // 
            // numericExpirationPeriod
            // 
            this.numericExpirationPeriod.Location = new System.Drawing.Point(173, 441);
            this.numericExpirationPeriod.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericExpirationPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericExpirationPeriod.Name = "numericExpirationPeriod";
            this.numericExpirationPeriod.Size = new System.Drawing.Size(60, 20);
            this.numericExpirationPeriod.TabIndex = 52;
            this.numericExpirationPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboExpirationUnit
            // 
            this.comboExpirationUnit.Items.AddRange(new object[] {
            "Month",
            "Year"});
            this.comboExpirationUnit.Location = new System.Drawing.Point(259, 441);
            this.comboExpirationUnit.Name = "comboExpirationUnit";
            this.comboExpirationUnit.Size = new System.Drawing.Size(100, 21);
            this.numericExpirationPeriod.Value = 1;
            this.comboExpirationUnit.SelectedIndex = 0;
            this.comboExpirationUnit.TabIndex = 53;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Location = new System.Drawing.Point(14, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 58);
            this.panel1.TabIndex = 51;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Gainsboro;
            this.panel10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel10.Controls.Add(this.button2);
            this.panel10.Controls.Add(this.btnOk);
            this.panel10.Controls.Add(this.label4);
            this.panel10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel10.Location = new System.Drawing.Point(74, 241);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(261, 187);
            this.panel10.TabIndex = 52;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 12F);
            this.button2.ForeColor = System.Drawing.Color.Gainsboro;
            this.button2.Location = new System.Drawing.Point(223, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 29);
            this.button2.TabIndex = 13;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.DimGray;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(179, 157);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label4.Location = new System.Drawing.Point(14, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 122);
            this.label4.TabIndex = 12;
            this.label4.Text = "Message for success or failed";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "Configuration";
            this.Size = new System.Drawing.Size(408, 621);
            ((System.ComponentModel.ISupportInitialize)(this.numericToleranceStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericToleranceEnd)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericExpirationPeriod)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.ResumeLayout(false);




        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.ComboBox comboUserRole;
        private System.Windows.Forms.Label lblWorkHours;
        private System.Windows.Forms.Label lblWorkStart;
        private System.Windows.Forms.DateTimePicker timePickerWorkStart;
        private System.Windows.Forms.Label lblWorkEnd;
        private System.Windows.Forms.DateTimePicker timePickerWorkEnd;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.Label lblToleranceStart;
        private System.Windows.Forms.NumericUpDown numericToleranceStart;
        private System.Windows.Forms.Label lblToleranceEnd;
        private System.Windows.Forms.NumericUpDown numericToleranceEnd;
        private System.Windows.Forms.Label lblBreakHours;
        private System.Windows.Forms.Label lblBreakStart;
        private System.Windows.Forms.DateTimePicker timePickerBreakStart;
        private System.Windows.Forms.Label lblBreakEnd;
        private System.Windows.Forms.DateTimePicker timePickerBreakEnd;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblEmailRecipient;
        private System.Windows.Forms.TextBox txtEmailRecipient;
        private System.Windows.Forms.Label lblEmailSender;
        private System.Windows.Forms.TextBox txtEmailSender;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblExpirationOverTime;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label4;
        private NumericUpDown numericExpirationPeriod;
        private ComboBox comboExpirationUnit;
    }
}
