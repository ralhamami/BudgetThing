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
    public partial class frmAddBudgetItem : Form
    {
        string name = "";
        //Empty string is regular, all else is for update
        public frmAddBudgetItem(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            BudgetItem item = new BudgetItem();
            item.Name = txtName.Text;
            item.Amount = Convert.ToDecimal(txtAmount.Text);
            item.Frequency = cboFrequency.SelectedItem.ToString();
            item.StartDate = dtpStartDate.Value.Date;
            Console.WriteLine(name);
            if (name.Length > 0)
                Database.DeleteByGroup(name);
            Database.StoreData(item);
            MessageBox.Show("Data Stored!");
            this.Close();
        }
    }
}
