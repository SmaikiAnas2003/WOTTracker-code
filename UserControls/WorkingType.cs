using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOTTracker
{
    public partial class WorkingType : UserControl
    {
        public event Action<string> OnWorkingTypeSelected;

        public WorkingType()
        {
            InitializeComponent();

            panel1.Click += (s, e) => SelectWorkingType("Home Office");
            panel2.Click += (s, e) => SelectWorkingType("On Site");
        }

        private void SelectWorkingType(string type)
        {
            OnWorkingTypeSelected?.Invoke(type); // Notifie MainForm
        }

        private void WorkingType_Load(object sender, EventArgs e)
        {

        }

        private void labelStartSession_Click(object sender, EventArgs e)
        {

        }

        private void Panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(60, 65, 70); // Couleur plus claire au survol
        }

        private void Panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(45, 50, 54); // Couleur par défaut
        }

        private void Panel2_MouseEnter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(60, 65, 70);
        }

        private void Panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(45, 50, 54);
        }

    }
}
