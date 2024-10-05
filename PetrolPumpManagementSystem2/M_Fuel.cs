using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Fuel : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";
        private DataTable fuelDataTable = new DataTable();

        public M_Fuel()
        {
            InitializeComponent();
        }

        private void M_Fuel_Load(object sender, EventArgs e)
        {
            ShowFuelData();
        }

        private void ShowFuelData(string fuelType = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM FuelManagement";

                    if (!string.IsNullOrEmpty(fuelType))
                    {
                        query = "SELECT * FROM FuelManagement WHERE FuelType = @FuelType";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(fuelType))
                        {
                            command.Parameters.AddWithValue("@FuelType", fuelType);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            fuelDataTable.Clear();
                            adapter.Fill(fuelDataTable);
                            dataGridView1.DataSource = fuelDataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void backBT_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void comFuelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFuelType = fuelTypeComboBox.SelectedItem.ToString();
            ShowFuelData(selectedFuelType);
        }
    }
}
