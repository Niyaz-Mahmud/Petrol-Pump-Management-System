using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class mAtmBoot : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public mAtmBoot()
        {
            InitializeComponent();
            ShowFuelData();
        }

        private void backbt_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void ShowFuelData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FuelType, AvailableQuantity FROM FuelManagement";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string type = reader["FuelType"].ToString();
                                object availableQuantityObj = reader["AvailableQuantity"];
                                int availableQuantity = (availableQuantityObj != DBNull.Value) ? Convert.ToInt32(availableQuantityObj) : 0;

                                if (type == "Petrol")
                                {
                                    label2.Text = availableQuantity.ToString() + " Litre"; ;
                                }
                                else if (type == "Octane")
                                {
                                    label6.Text = availableQuantity.ToString()+" Litre";
                                }
                                else if (type == "Diesel")
                                {
                                    label11.Text = availableQuantity.ToString() + " Litre"; ;
                                }
                            }
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
