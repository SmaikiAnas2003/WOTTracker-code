using System.Drawing;
using System.Windows.Forms;

namespace WOTTracker
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonMinimize;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTitle = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.buttonMinimize = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.HL_Send = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.HL_export = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.LabelSessionTotal = new System.Windows.Forms.Label();
            this.labelStartSession = new System.Windows.Forms.Label();
            this.lbl_status = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.lblTotalOvertime = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOvertimeNormalDays = new System.Windows.Forms.Label();
            this.labelPoucentageNormalDays = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblOvertimeSaturdays = new System.Windows.Forms.Label();
            this.labelPoucentageSaturdays = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblOvertimePublicHolidays = new System.Windows.Forms.Label();
            this.labelPoucentageHolidays = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOvertimeSundays = new System.Windows.Forms.Label();
            this.labelPoucentageSundays = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelDownTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HL_Send)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HL_export)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "WOTTracker";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToExcelToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToExcelToolStripMenuItem.Text = "Export to Excel";
            this.exportToExcelToolStripMenuItem.Click += new System.EventHandler(this.btn_ExportToExcel);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.btn_OpenSettings);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(56, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(267, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "my                  Over Time Tracker";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.progressBar1});
            this.statusStrip.Location = new System.Drawing.Point(0, 669);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(461, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "  Ready";
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 16);
            this.progressBar1.Visible = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.panelTop.Controls.Add(this.pictureBox2);
            this.panelTop.Controls.Add(this.pictureBox9);
            this.panelTop.Controls.Add(this.buttonMinimize);
            this.panelTop.Controls.Add(this.buttonClose);
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(461, 45);
            this.panelTop.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::WOTTracker.Properties.Resources.looogo;
            this.pictureBox2.Location = new System.Drawing.Point(11, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(38, 25);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox9.Image = global::WOTTracker.Properties.Resources.yazaki;
            this.pictureBox9.Location = new System.Drawing.Point(90, 4);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(80, 41);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox9.TabIndex = 4;
            this.pictureBox9.TabStop = false;
            // 
            // buttonMinimize
            // 
            this.buttonMinimize.FlatAppearance.BorderSize = 0;
            this.buttonMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinimize.Font = new System.Drawing.Font("Comic Sans MS", 18F);
            this.buttonMinimize.ForeColor = System.Drawing.Color.White;
            this.buttonMinimize.Location = new System.Drawing.Point(372, -9);
            this.buttonMinimize.Name = "buttonMinimize";
            this.buttonMinimize.Size = new System.Drawing.Size(43, 58);
            this.buttonMinimize.TabIndex = 2;
            this.buttonMinimize.Text = "-";
            this.buttonMinimize.UseVisualStyleBackColor = true;
            this.buttonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Comic Sans MS", 12F);
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(414, -1);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(47, 46);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.pictureBox3);
            this.panel8.Controls.Add(this.label3);
            this.panel8.Controls.Add(this.pictureBox1);
            this.panel8.Controls.Add(this.HL_Send);
            this.panel8.Controls.Add(this.pictureBox7);
            this.panel8.Controls.Add(this.label15);
            this.panel8.Controls.Add(this.HL_export);
            this.panel8.Controls.Add(this.pictureBox5);
            this.panel8.Controls.Add(this.label11);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 45);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(56, 624);
            this.panel8.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1, 349);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "Dashboard";
            this.label6.Click += new System.EventHandler(this.Dashboard_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(12, 298);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 53);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.Dashboard_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Settings";
            this.label3.Click += new System.EventHandler(this.Settings_ClickHandler);
            this.label3.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 214);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.Settings_ClickHandler);
            this.pictureBox1.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // HL_Send
            // 
            this.HL_Send.BackColor = System.Drawing.Color.Aqua;
            this.HL_Send.Location = new System.Drawing.Point(0, 144);
            this.HL_Send.Name = "HL_Send";
            this.HL_Send.Size = new System.Drawing.Size(2, 22);
            this.HL_Send.TabIndex = 9;
            this.HL_Send.TabStop = false;
            this.HL_Send.Visible = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox7.Image = global::WOTTracker.Properties.Resources.chart;
            this.pictureBox7.Location = new System.Drawing.Point(13, 142);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(28, 26);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 8;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Click += new System.EventHandler(this.ButtonSendSummary_Click);
            this.pictureBox7.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(15, 172);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 12);
            this.label15.TabIndex = 7;
            this.label15.Text = "Send";
            this.label15.Click += new System.EventHandler(this.ButtonSendSummary_Click);
            this.label15.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // HL_export
            // 
            this.HL_export.BackColor = System.Drawing.Color.Aqua;
            this.HL_export.Location = new System.Drawing.Point(0, 67);
            this.HL_export.Name = "HL_export";
            this.HL_export.Size = new System.Drawing.Size(2, 22);
            this.HL_export.TabIndex = 6;
            this.HL_export.TabStop = false;
            this.HL_export.Visible = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox5.Image = global::WOTTracker.Properties.Resources.excel;
            this.pictureBox5.Location = new System.Drawing.Point(13, 65);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(29, 22);
            this.pictureBox5.TabIndex = 5;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.ButtonExport_Click);
            this.pictureBox5.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(12, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 12);
            this.label11.TabIndex = 4;
            this.label11.Text = "Export";
            this.label11.Click += new System.EventHandler(this.ButtonExport_Click);
            this.label11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox5_MouseHover);
            this.label11.MouseHover += new System.EventHandler(this.picture_MouseHover);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            // 
            // LabelSessionTotal
            // 
            this.LabelSessionTotal.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.LabelSessionTotal.ForeColor = System.Drawing.Color.Aqua;
            this.LabelSessionTotal.Location = new System.Drawing.Point(4, 41);
            this.LabelSessionTotal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LabelSessionTotal.Name = "LabelSessionTotal";
            this.LabelSessionTotal.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LabelSessionTotal.Size = new System.Drawing.Size(350, 45);
            this.LabelSessionTotal.TabIndex = 5;
            this.LabelSessionTotal.Text = "00:00 Overtime";
            this.LabelSessionTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStartSession
            // 
            this.labelStartSession.BackColor = System.Drawing.Color.Purple;
            this.labelStartSession.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelStartSession.ForeColor = System.Drawing.Color.White;
            this.labelStartSession.Location = new System.Drawing.Point(2, 148);
            this.labelStartSession.Name = "labelStartSession";
            this.labelStartSession.Size = new System.Drawing.Size(352, 29);
            this.labelStartSession.TabIndex = 7;
            this.labelStartSession.Text = "Session started at 17:00:00";
            this.labelStartSession.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lbl_status.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_status.Location = new System.Drawing.Point(119, 13);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(110, 20);
            this.lbl_status.TabIndex = 6;
            this.lbl_status.Text = "Current Session";
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelMessage.ForeColor = System.Drawing.Color.Aqua;
            this.labelMessage.Location = new System.Drawing.Point(6, 91);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMessage.Size = new System.Drawing.Size(347, 45);
            this.labelMessage.TabIndex = 8;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel2.Controls.Add(this.labelMessage);
            this.panel2.Controls.Add(this.lbl_status);
            this.panel2.Controls.Add(this.pictureBox8);
            this.panel2.Controls.Add(this.labelStartSession);
            this.panel2.Controls.Add(this.LabelSessionTotal);
            this.panel2.Location = new System.Drawing.Point(90, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(355, 168);
            this.panel2.TabIndex = 7;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.Image = global::WOTTracker.Properties.Resources.back;
            this.pictureBox8.Location = new System.Drawing.Point(96, 11);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(25, 25);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 4;
            this.pictureBox8.TabStop = false;
            // 
            // lblTotalOvertime
            // 
            this.lblTotalOvertime.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.lblTotalOvertime.ForeColor = System.Drawing.Color.White;
            this.lblTotalOvertime.Location = new System.Drawing.Point(0, 32);
            this.lblTotalOvertime.Name = "lblTotalOvertime";
            this.lblTotalOvertime.Size = new System.Drawing.Size(355, 32);
            this.lblTotalOvertime.TabIndex = 8;
            this.lblTotalOvertime.Text = "3 day 1 hour 3 minutes";
            this.lblTotalOvertime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(119, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 19);
            this.label13.TabIndex = 7;
            this.label13.Text = "Total OverTime :";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.lblTotalOvertime);
            this.panel3.Location = new System.Drawing.Point(90, 245);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(355, 76);
            this.panel3.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Normal days";
            // 
            // lblOvertimeNormalDays
            // 
            this.lblOvertimeNormalDays.AutoSize = true;
            this.lblOvertimeNormalDays.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOvertimeNormalDays.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblOvertimeNormalDays.Location = new System.Drawing.Point(16, 16);
            this.lblOvertimeNormalDays.Name = "lblOvertimeNormalDays";
            this.lblOvertimeNormalDays.Size = new System.Drawing.Size(19, 21);
            this.lblOvertimeNormalDays.TabIndex = 2;
            this.lblOvertimeNormalDays.Text = "0";
            // 
            // labelPoucentageNormalDays
            // 
            this.labelPoucentageNormalDays.BackColor = System.Drawing.Color.Gray;
            this.labelPoucentageNormalDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPoucentageNormalDays.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPoucentageNormalDays.ForeColor = System.Drawing.Color.Aqua;
            this.labelPoucentageNormalDays.Location = new System.Drawing.Point(123, 75);
            this.labelPoucentageNormalDays.Name = "labelPoucentageNormalDays";
            this.labelPoucentageNormalDays.Size = new System.Drawing.Size(48, 22);
            this.labelPoucentageNormalDays.TabIndex = 3;
            this.labelPoucentageNormalDays.Text = "0%";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel4.Controls.Add(this.labelPoucentageNormalDays);
            this.panel4.Controls.Add(this.lblOvertimeNormalDays);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(90, 434);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(174, 100);
            this.panel4.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(16, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 20);
            this.label9.TabIndex = 7;
            this.label9.Text = "Saturdays";
            // 
            // lblOvertimeSaturdays
            // 
            this.lblOvertimeSaturdays.AutoSize = true;
            this.lblOvertimeSaturdays.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOvertimeSaturdays.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblOvertimeSaturdays.Location = new System.Drawing.Point(16, 18);
            this.lblOvertimeSaturdays.Name = "lblOvertimeSaturdays";
            this.lblOvertimeSaturdays.Size = new System.Drawing.Size(19, 21);
            this.lblOvertimeSaturdays.TabIndex = 8;
            this.lblOvertimeSaturdays.Text = "0";
            // 
            // labelPoucentageSaturdays
            // 
            this.labelPoucentageSaturdays.BackColor = System.Drawing.Color.Gray;
            this.labelPoucentageSaturdays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPoucentageSaturdays.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPoucentageSaturdays.ForeColor = System.Drawing.Color.Aqua;
            this.labelPoucentageSaturdays.Location = new System.Drawing.Point(123, 75);
            this.labelPoucentageSaturdays.Name = "labelPoucentageSaturdays";
            this.labelPoucentageSaturdays.Size = new System.Drawing.Size(48, 20);
            this.labelPoucentageSaturdays.TabIndex = 5;
            this.labelPoucentageSaturdays.Text = "0%";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel5.Controls.Add(this.labelPoucentageSaturdays);
            this.panel5.Controls.Add(this.lblOvertimeSaturdays);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(90, 542);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(174, 100);
            this.panel5.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(11, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Public Holidays";
            // 
            // lblOvertimePublicHolidays
            // 
            this.lblOvertimePublicHolidays.AutoSize = true;
            this.lblOvertimePublicHolidays.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOvertimePublicHolidays.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblOvertimePublicHolidays.Location = new System.Drawing.Point(11, 16);
            this.lblOvertimePublicHolidays.Name = "lblOvertimePublicHolidays";
            this.lblOvertimePublicHolidays.Size = new System.Drawing.Size(19, 21);
            this.lblOvertimePublicHolidays.TabIndex = 4;
            this.lblOvertimePublicHolidays.Text = "0";
            // 
            // labelPoucentageHolidays
            // 
            this.labelPoucentageHolidays.BackColor = System.Drawing.Color.Gray;
            this.labelPoucentageHolidays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPoucentageHolidays.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelPoucentageHolidays.ForeColor = System.Drawing.Color.Aqua;
            this.labelPoucentageHolidays.Location = new System.Drawing.Point(126, 75);
            this.labelPoucentageHolidays.Name = "labelPoucentageHolidays";
            this.labelPoucentageHolidays.Size = new System.Drawing.Size(45, 22);
            this.labelPoucentageHolidays.TabIndex = 4;
            this.labelPoucentageHolidays.Text = "0%";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel7.Controls.Add(this.labelPoucentageHolidays);
            this.panel7.Controls.Add(this.lblOvertimePublicHolidays);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Location = new System.Drawing.Point(270, 434);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(174, 100);
            this.panel7.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(11, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Sunday";
            // 
            // lblOvertimeSundays
            // 
            this.lblOvertimeSundays.AutoSize = true;
            this.lblOvertimeSundays.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOvertimeSundays.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblOvertimeSundays.Location = new System.Drawing.Point(11, 19);
            this.lblOvertimeSundays.Name = "lblOvertimeSundays";
            this.lblOvertimeSundays.Size = new System.Drawing.Size(19, 21);
            this.lblOvertimeSundays.TabIndex = 6;
            this.lblOvertimeSundays.Text = "0";
            // 
            // labelPoucentageSundays
            // 
            this.labelPoucentageSundays.BackColor = System.Drawing.Color.Gray;
            this.labelPoucentageSundays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPoucentageSundays.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPoucentageSundays.ForeColor = System.Drawing.Color.Aqua;
            this.labelPoucentageSundays.Location = new System.Drawing.Point(126, 76);
            this.labelPoucentageSundays.Name = "labelPoucentageSundays";
            this.labelPoucentageSundays.Size = new System.Drawing.Size(45, 20);
            this.labelPoucentageSundays.TabIndex = 9;
            this.labelPoucentageSundays.Text = "0%";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel6.Controls.Add(this.labelPoucentageSundays);
            this.panel6.Controls.Add(this.lblOvertimeSundays);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Location = new System.Drawing.Point(270, 541);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(174, 100);
            this.panel6.TabIndex = 11;
            // 
            // labelDownTime
            // 
            this.labelDownTime.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.labelDownTime.ForeColor = System.Drawing.Color.White;
            this.labelDownTime.Location = new System.Drawing.Point(0, 32);
            this.labelDownTime.Name = "labelDownTime";
            this.labelDownTime.Size = new System.Drawing.Size(355, 32);
            this.labelDownTime.TabIndex = 8;
            this.labelDownTime.Text = "3 day 1 hour 3 minutes";
            this.labelDownTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(119, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Total DownTime :";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelDownTime);
            this.panel1.Location = new System.Drawing.Point(90, 340);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 76);
            this.panel1.TabIndex = 9;
            // 
            // panel9
            // 
            this.panel9.Location = new System.Drawing.Point(53, 45);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(408, 624);
            this.panel9.TabIndex = 13;
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
            this.panel10.Location = new System.Drawing.Point(120, 260);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(261, 168);
            this.panel10.TabIndex = 0;
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
            this.btnOk.Location = new System.Drawing.Point(179, 139);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label4.Location = new System.Drawing.Point(14, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 104);
            this.label4.TabIndex = 12;
            this.label4.Text = "Message for success or failed";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(38)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(461, 691);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "WOTTracker";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HL_Send)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HL_export)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.PictureBox HL_Send;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox HL_export;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label11;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Label label3;
        private Label LabelSessionTotal;
        private Label labelStartSession;
        private PictureBox pictureBox8;
        private Label lbl_status;
        private Label labelMessage;
        private Panel panel2;
        private Label lblTotalOvertime;
        private Label label13;
        private Panel panel3;
        private Label label2;
        private Label lblOvertimeNormalDays;
        private Label labelPoucentageNormalDays;
        private Panel panel4;
        private Label label9;
        private Label lblOvertimeSaturdays;
        private Label labelPoucentageSaturdays;
        private Panel panel5;
        private Label label5;
        private Label lblOvertimePublicHolidays;
        private Label labelPoucentageHolidays;
        private Panel panel7;
        private Label label7;
        private Label lblOvertimeSundays;
        private Label labelPoucentageSundays;
        private Panel panel6;
        private Label labelDownTime;
        private Label label1;
        private Panel panel1;
        private Panel panel9;
        private PictureBox pictureBox2;
        private Panel panel10;
        private Label label4;
        private Button btnOk;
        private Button button2;
        private PictureBox pictureBox3;
        private Label label6;
    }
}