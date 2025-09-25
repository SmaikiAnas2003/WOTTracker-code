using System.Windows.Forms.DataVisualization.Charting;

namespace WOTTracker
{
    partial class DashboardStats
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chartOvertimeBreakdown = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartOvertimeByDay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lvLongestSessions = new System.Windows.Forms.ListView();
            this.colStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblBurnoutRisk = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLongestSessionsTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartOvertimeBreakdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOvertimeByDay)).BeginInit();
            this.SuspendLayout();
            // 
            // chartOvertimeBreakdown
            // 
            chartArea1.Name = "MainArea1";
            this.chartOvertimeBreakdown.ChartAreas.Add(chartArea1);
            this.chartOvertimeBreakdown.Cursor = System.Windows.Forms.Cursors.Hand;
            legend1.Name = "MainLegend1";
            this.chartOvertimeBreakdown.Legends.Add(legend1);
            this.chartOvertimeBreakdown.Location = new System.Drawing.Point(73, 16);
            this.chartOvertimeBreakdown.Name = "chartOvertimeBreakdown";
            this.chartOvertimeBreakdown.Size = new System.Drawing.Size(257, 173);
            this.chartOvertimeBreakdown.TabIndex = 0;
            this.chartOvertimeBreakdown.Text = "chartOvertimeBreakdown";
            this.chartOvertimeBreakdown.Click += new System.EventHandler(this.Chart_Click);
            // 
            // chartOvertimeByDay
            // 
            chartArea2.Name = "MainArea2";
            this.chartOvertimeByDay.ChartAreas.Add(chartArea2);
            this.chartOvertimeByDay.Cursor = System.Windows.Forms.Cursors.Hand;
            legend2.Name = "MainLegend2";
            this.chartOvertimeByDay.Legends.Add(legend2);
            this.chartOvertimeByDay.Location = new System.Drawing.Point(73, 204);
            this.chartOvertimeByDay.Name = "chartOvertimeByDay";
            this.chartOvertimeByDay.Size = new System.Drawing.Size(257, 173);
            this.chartOvertimeByDay.TabIndex = 1;
            this.chartOvertimeByDay.Text = "chartOvertimeByDay";
            this.chartOvertimeByDay.Click += new System.EventHandler(this.Chart_Click);
            // 
            // lvLongestSessions
            // 
            this.lvLongestSessions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStart,
            this.colEnd,
            this.colHours,
            this.colType});
            this.lvLongestSessions.FullRowSelect = true;
            this.lvLongestSessions.GridLines = true;
            this.lvLongestSessions.HideSelection = false;
            this.lvLongestSessions.Location = new System.Drawing.Point(3, 425);
            this.lvLongestSessions.Name = "lvLongestSessions";
            this.lvLongestSessions.Size = new System.Drawing.Size(402, 113);
            this.lvLongestSessions.TabIndex = 2;
            this.lvLongestSessions.UseCompatibleStateImageBehavior = false;
            this.lvLongestSessions.View = System.Windows.Forms.View.Details;
            // 
            // colStart
            // 
            this.colStart.Text = "Start";
            this.colStart.Width = 120;
            // 
            // colEnd
            // 
            this.colEnd.Text = "End";
            this.colEnd.Width = 120;
            // 
            // colHours
            // 
            this.colHours.Text = "Duration";
            this.colHours.Width = 78;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 80;
            // 
            // lblBurnoutRisk
            // 
            this.lblBurnoutRisk.AutoSize = true;
            this.lblBurnoutRisk.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBurnoutRisk.ForeColor = System.Drawing.Color.LightGreen;
            this.lblBurnoutRisk.Location = new System.Drawing.Point(17, 549);
            this.lblBurnoutRisk.Name = "lblBurnoutRisk";
            this.lblBurnoutRisk.Size = new System.Drawing.Size(177, 19);
            this.lblBurnoutRisk.TabIndex = 3;
            this.lblBurnoutRisk.Text = "No Burnout risk detected";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Silver;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(226, 576);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(162, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLongestSessionsTitle
            // 
            this.lblLongestSessionsTitle.AutoSize = true;
            this.lblLongestSessionsTitle.BackColor = System.Drawing.Color.White;
            this.lblLongestSessionsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLongestSessionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(38)))), ((int)(((byte)(45)))));
            this.lblLongestSessionsTitle.Location = new System.Drawing.Point(105, 399);
            this.lblLongestSessionsTitle.Name = "lblLongestSessionsTitle";
            this.lblLongestSessionsTitle.Size = new System.Drawing.Size(183, 21);
            this.lblLongestSessionsTitle.TabIndex = 4;
            this.lblLongestSessionsTitle.Text = "Top 5 Longest Sessions";
            // 
            // DashboardStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(38)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblBurnoutRisk);
            this.Controls.Add(this.chartOvertimeByDay);
            this.Controls.Add(this.chartOvertimeBreakdown);
            this.Controls.Add(this.lblLongestSessionsTitle);
            this.Controls.Add(this.lvLongestSessions);
            this.Name = "DashboardStats";
            this.Size = new System.Drawing.Size(408, 624);
            ((System.ComponentModel.ISupportInitialize)(this.chartOvertimeBreakdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOvertimeByDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartOvertimeBreakdown;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartOvertimeByDay;
        private System.Windows.Forms.ListView lvLongestSessions;
        private System.Windows.Forms.ColumnHeader colStart;
        private System.Windows.Forms.ColumnHeader colEnd;
        private System.Windows.Forms.ColumnHeader colHours;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.Label lblBurnoutRisk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblLongestSessionsTitle;

    }
}
