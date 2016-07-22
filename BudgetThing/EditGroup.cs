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
    public partial class EditGroup : Form
    {
        public EditGroup()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstGroupList.DataSource = Database.GetNames(txtSearch.Text);
        }

        private void lstGroupList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmAddBudgetItem f = new frmAddBudgetItem(lstGroupList.SelectedItem.ToString());
            f.ShowDialog();
        }
    }
}
