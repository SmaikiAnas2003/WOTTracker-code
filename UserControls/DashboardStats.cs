using System;
using System.Drawing;
using System.Windows.Forms;
using WOTTracker.Models;
using System.Windows.Forms.DataVisualization.Charting;


namespace WOTTracker
{
    public partial class DashboardStats : UserControl
    {
        private readonly StatsService _stats;

        public event Action OnClose;


        public DashboardStats(string dbPath)
        {
            InitializeComponent();


            _stats = new StatsService(dbPath);
            Load += DashboardStats_Load;
        }

        private void DashboardStats_Load(object sender, EventArgs e)
        {
            LoadOvertimeBreakdown();
            LoadMostCommonOvertimeDays();
            LoadLongestSessions();
            LoadBurnoutRisk();
        }

        private void LoadOvertimeBreakdown()
        {
            var data = _stats.GetOvertimeBreakdown();

            chartOvertimeBreakdown.Series.Clear();
            var s = new Series("Overtime")
            {
                ChartType = SeriesChartType.Pie,
                ChartArea = "MainArea1",
                Legend = "MainLegend1"
            };
            chartOvertimeBreakdown.Series.Add(s);

            if (data.Total == 0)
            {
                s.Points.AddXY("Test", 1);
                return;
            }

            if (data.NormalDays > 0)
                s.Points.AddXY("NormalDays", data.NormalDays);
            if (data.Saturdays > 0)
                s.Points.AddXY("Saturdays", data.Saturdays);
            if (data.Sundays > 0)
                s.Points.AddXY("Sundays", data.Sundays);
            if (data.PublicHolidays > 0)
                s.Points.AddXY("PublicHolidays", data.PublicHolidays);

            SetChartTitle(chartOvertimeBreakdown, "Overtime Breakdown");


            chartOvertimeBreakdown.Invalidate();
            chartOvertimeByDay.Invalidate();
        }

        private void LoadMostCommonOvertimeDays()
        {
            var data = _stats.GetMostCommonOvertimeDays();

            chartOvertimeByDay.Series.Clear();
            var s = new Series("ByDay")
            {
                ChartType = SeriesChartType.Column,
                ChartArea = "MainArea2",
                Legend = "MainLegend2"
            };

            chartOvertimeByDay.Series.Add(s);

            foreach (var d in data)
            {
                s.Points.AddXY(d.Day.ToString(), d.Minutes / 60.0); // heures
            }

            SetChartTitle(chartOvertimeByDay, "Overtime Hours by Day");


            chartOvertimeBreakdown.Invalidate();
            chartOvertimeByDay.Invalidate();

        }

        private void SetChartTitle(Chart chart, string titleText)
        {
            chart.Titles.Clear();
            chart.Titles.Add(new Title
            {
                Text = titleText,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black
            });
        }


        public string FormatDuration(int minutes)
        {
            minutes = (int)Math.Round((double)minutes); // Round to the nearest whole number
            if (minutes < 60)
            {
                return $"{minutes} min";
            }
            else if (minutes < 1440)
            {
                int hours = (int)minutes / 60;
                int remainingMinutes = (int)minutes % 60;
                return $"{hours} h {remainingMinutes} min";
            }
            else
            {
                int days = (int)minutes / 1440;
                int remainingHours = ((int)minutes % 1440) / 60;
                int remainingMinutes = (int)minutes % 60;
                return $"{days} d {remainingHours} h {remainingMinutes} min";
            }
        }
        private void LoadLongestSessions()
        {
            var items = _stats.GetLongestSessions(5);
            lvLongestSessions.Items.Clear();

            foreach (var i in items)
            {
                int totalMinutes = (int)Math.Round(i.Hours * 60); // Convertir en minutes

                var lvi = new ListViewItem(new[]
                {
                i.Start.ToString("yyyy-MM-dd HH:mm"),
                i.End.ToString("yyyy-MM-dd HH:mm"),
                FormatDuration(totalMinutes),
                i.WorkingType ?? "-"
            });
                lvLongestSessions.Items.Add(lvi);
            }
        }

        private void LoadBurnoutRisk()
        {
            var result = _stats.GetBurnoutRisk(10.0, 3);
            if (result.IsAtRisk)
            {
                lblBurnoutRisk.Text = "⚠️ Burnout risk detected (+10h/week for ≥3 consecutive weeks)";
                lblBurnoutRisk.ForeColor = Color.OrangeRed;
            }
            else
            {
                lblBurnoutRisk.Text = "No Burnout risk detected";
                lblBurnoutRisk.ForeColor = Color.LightGreen;
            }
        }

        private Size originalSize;
        private Point originalLocation;
        private bool isZoomed = false;

        private void Chart_Click(object sender, EventArgs e)
        {
            if (sender is Chart chart)
            {
                if (!isZoomed)
                {
                    originalSize = chart.Size;
                    originalLocation = chart.Location;

                    chart.BringToFront();
                    chart.Size = new Size(400, 600); // taille zoomée
                    chart.Location = new Point((Width - chart.Width) / 2, (Height - chart.Height) / 2);
                }
                else
                {
                    chart.Size = originalSize;
                    chart.Location = originalLocation;
                }
                isZoomed = !isZoomed;
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
        }
    }

}
