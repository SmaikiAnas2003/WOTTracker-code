using System.Windows.Forms;

namespace WOTTracker
{
    partial class WorkingType
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LabelSessionTotal = new System.Windows.Forms.Label();
            this.labelStartSession = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 240);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(0, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 240);
            this.panel2.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::WOTTracker.Properties.Resources.OfficeWork;
            this.pictureBox2.Location = new System.Drawing.Point(84, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(233, 193);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WOTTracker.Properties.Resources.remoteWork;
            this.pictureBox1.Location = new System.Drawing.Point(72, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(269, 197);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(54)))));
            this.panel3.Controls.Add(this.LabelSessionTotal);
            this.panel3.Location = new System.Drawing.Point(0, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(408, 65);
            this.panel3.TabIndex = 1;
            // 
            // LabelSessionTotal
            // 
            this.LabelSessionTotal.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.LabelSessionTotal.ForeColor = System.Drawing.Color.Aqua;
            this.LabelSessionTotal.Location = new System.Drawing.Point(32, 2);
            this.LabelSessionTotal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LabelSessionTotal.Name = "LabelSessionTotal";
            this.LabelSessionTotal.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LabelSessionTotal.Size = new System.Drawing.Size(350, 53);
            this.LabelSessionTotal.TabIndex = 6;
            this.LabelSessionTotal.Text = "Welcome";
            this.LabelSessionTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStartSession
            // 
            this.labelStartSession.BackColor = System.Drawing.Color.Purple;
            this.labelStartSession.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelStartSession.ForeColor = System.Drawing.Color.White;
            this.labelStartSession.Location = new System.Drawing.Point(0, 72);
            this.labelStartSession.Name = "labelStartSession";
            this.labelStartSession.Size = new System.Drawing.Size(408, 25);
            this.labelStartSession.TabIndex = 8;
            this.labelStartSession.Text = "Your current session is :";
            this.labelStartSession.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelStartSession.Click += new System.EventHandler(this.labelStartSession_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.MintCream;
            this.label1.Location = new System.Drawing.Point(0, 187);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Size = new System.Drawing.Size(408, 53);
            this.label1.TabIndex = 7;
            this.label1.Text = "HomeOffice";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.MintCream;
            this.label2.Location = new System.Drawing.Point(0, 187);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Size = new System.Drawing.Size(408, 53);
            this.label2.TabIndex = 8;
            this.label2.Text = "On-Site";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WorkingType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.labelStartSession);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "WorkingType";
            this.Size = new System.Drawing.Size(408, 624);
            this.Load += new System.EventHandler(this.WorkingType_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

            this.panel1.MouseEnter += new System.EventHandler(this.Panel1_MouseEnter);
            this.panel1.MouseLeave += new System.EventHandler(this.Panel1_MouseLeave);

            this.panel2.MouseEnter += new System.EventHandler(this.Panel2_MouseEnter);
            this.panel2.MouseLeave += new System.EventHandler(this.Panel2_MouseLeave);

            this.panel1.Cursor = Cursors.Hand;
            this.panel2.Cursor = Cursors.Hand;


        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LabelSessionTotal;
        private System.Windows.Forms.Label labelStartSession;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
