using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetThing
{
    static class Database
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database.accdb; Persist Security Info=False;";

        public static void StoreData(BudgetItem budgetItem)
        {
            DateTime biCopy = budgetItem.StartDate;
            if (budgetItem.Frequency.Equals("Once"))
                StoreInDatabase(budgetItem);
            else if (budgetItem.Frequency.Equals("Daily"))
            {
                for (int i = 0; biCopy.Year < DateTime.Today.Year+1; i++)
                {
                    budgetItem.StartDate = biCopy;
                    StoreInDatabase(budgetItem);
                    biCopy = biCopy.AddDays(1);
                }
            }
            else if (budgetItem.Frequency.Equals("Weekly"))
            {
                for (int i=0; biCopy.Year < DateTime.Today.Year + 1; i++)
                {
                    budgetItem.StartDate = biCopy;
                    StoreInDatabase(budgetItem);
                    biCopy = biCopy.AddDays(7);
                }
            }
            else if (budgetItem.Frequency.Equals("Bi-Weekly"))
            {
                for (int i = 0; biCopy.Year < DateTime.Today.Year + 1; i++)
                {

                    budgetItem.StartDate = biCopy;
                    StoreInDatabase(budgetItem);
                    biCopy = biCopy.AddDays(14);
                }
            }
            else if (budgetItem.Frequency.Equals("Monthly"))
            {
                for (int i = 0; biCopy.Year < DateTime.Today.Year + 1; i++)
                {
                    budgetItem.StartDate = biCopy;
                    StoreInDatabase(budgetItem);
                    biCopy = biCopy.AddMonths(1);
                }
            }

        }

        public static void StoreInDatabase(BudgetItem budgetItem)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Budget(Name,Amount,Frequency,StartDate) VALUES(@name,@amount,@freq,@start)";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.Parameters.AddWithValue("@name", budgetItem.Name);
                command.Parameters.AddWithValue("@amount", budgetItem.Amount);
                command.Parameters.AddWithValue("@freq", budgetItem.Frequency);
                command.Parameters.AddWithValue("@start", budgetItem.StartDate);

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateDatabase(DataGridView dgvMain)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                int columnIndex = dgvMain.CurrentCell.ColumnIndex;
                string columnName = dgvMain.Columns[columnIndex].Name;
                connection.Open();
                string sql = "UPDATE Budget SET " + columnName + "=" + dgvMain.CurrentCell.Value + " WHERE BudgetItemId=" + dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells[0].Value;
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteByGroup(string name)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {   
                connection.Open();
                Console.WriteLine(name);
                string sql = "DELETE * FROM Budget WHERE Name='"+name+"'";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetDataForDate(DateTime date)
        {
            DataTable BudgetTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine(date.ToString("M/d/yyyy"));
                string sql = "SELECT * FROM Budget WHERE StartDate = #" + date.ToString("M/d/yyyy") + "#";
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader = command.ExecuteReader();
                BudgetTable.Load(reader);
            }
            return BudgetTable;
        }

        public static double GetRemainder(DateTime date, bool GetActual)
        {
            double income = 0;
            double expenses = 0;
            string sql;
            DataTable BudgetTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                if (GetActual)
                    sql = "SELECT * FROM Budget WHERE StartDate <= #" + date.ToString("M/d/yyyy") + "# AND Name='Income' AND Paid = TRUE";
                else
                    sql = "SELECT * FROM Budget WHERE StartDate <= #" + date.ToString("M/d/yyyy") + "# AND Name='Income'";
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    income += Convert.ToDouble(reader["Amount"]);
                }
                if (GetActual)
                    sql = "SELECT * FROM Budget WHERE StartDate <= #" + date.ToString("M/d/yyyy") + "# AND Name<>'Income' AND Paid = TRUE";
                else
                    sql = "SELECT * FROM Budget WHERE StartDate <= #" + date.ToString("M/d/yyyy") + "# AND Name<>'Income'";
                command = new OleDbCommand(sql, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    expenses += Convert.ToDouble(reader["Amount"]);
                }
            }
            return income - expenses;
        }

        public static List<string> GetNames(string search)
        {
            List<string> tempList = new List<string>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT Name FROM Budget WHERE Name LIKE('%" + search + "%')";
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tempList.Add(reader["Name"].ToString());
                }
            }
            return tempList;
        }
    }
}
