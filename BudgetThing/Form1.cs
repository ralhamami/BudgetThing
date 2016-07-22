using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetThing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void addBudgetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddBudgetItem form = new frmAddBudgetItem("");
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void dtpMain_ValueChanged(object sender, EventArgs e)
        {

            dgvMain.DataSource = Database.GetDataForDate(dtpMain.Value);
            dgvMain.Columns["Amount"].DefaultCellStyle.Format = "c2";
            UpdateAmounts();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dtpMain.Value = dtpMain.Value.AddDays(1);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            dtpMain.Value = dtpMain.Value.AddDays(-1);
        }

        private void dgvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Database.UpdateDatabase(dgvMain);
            UpdateAmounts();
        }

        public void UpdateAmounts()
        {
            lblRemainder.Text = Database.GetRemainder(dtpMain.Value, false).ToString("c2");
            lblActualRem.Text = Database.GetRemainder(dtpMain.Value, true).ToString("c2");
        }

        private void editGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditGroup eg = new EditGroup();
            eg.ShowDialog();
        }
    }
}
