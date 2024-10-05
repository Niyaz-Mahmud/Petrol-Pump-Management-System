using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Dashboard : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public M_Dashboard()
        {
            InitializeComponent();
            CalculateAndDisplayTotalCost();
            CalculateAndDisplayFullSalary();
        }

        private void backBT_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void CalculateAndDisplayTotalCost()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SUM(Cost) AS TotalCost FROM SupplierInformation";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            suppliersBill.Text = result.ToString() + " BDT";
                        }
                        else
                        {
                            suppliersBill.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CalculateAndDisplayFullSalary()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SUM(Salary) AS TotalSalary FROM Staff_Information";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            staffBill.Text = result.ToString() + " BDT";
                        }
                        else
                        {
                            staffBill.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
