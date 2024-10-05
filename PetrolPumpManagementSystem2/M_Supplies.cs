using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Supplies : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public M_Supplies()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now.Date;
            LoadSupplierInformationDetails();
        }

        private void backBt_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string cName = txtCName.Text;
                string mNumber = txtMNumber.Text;
                string fuelType = ComFuel.Text;
                double quantity = 0;
                double cost = 0;
                string phone = txtPhone.Text;
                string address = txtAddress.Text;

                DateTime selectedDate = dateTimePicker1.Value.Date;

                DateTime currentTime = DateTime.Now;

                DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                                                         currentTime.Hour, currentTime.Minute, currentTime.Second);

                string dateTimeString = combinedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

               

                if (string.IsNullOrWhiteSpace(cName) || string.IsNullOrWhiteSpace(mNumber) ||
                    string.IsNullOrWhiteSpace(fuelType) || string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                    string.IsNullOrWhiteSpace(txtCost.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (!double.TryParse(txtQuantity.Text, out quantity))
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                if (!double.TryParse(txtCost.Text, out cost))
                {
                    MessageBox.Show("Please enter a valid cost.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string supplierInfoQuery = "INSERT INTO SupplierInformation (CompanyName, MemoNumber, FuelType, Quantity, Phone, Address, Cost, Date) VALUES (@CompanyName, @MemoNumber, @FuelType, @Quantity, @Phone, @Address, @Cost, @Date)";

                    using (SqlCommand command = new SqlCommand(supplierInfoQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CompanyName", cName);
                        command.Parameters.AddWithValue("@MemoNumber", mNumber);
                        command.Parameters.AddWithValue("@FuelType", fuelType);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Cost", cost);
                        command.Parameters.AddWithValue("@Date", dateTimeString);

                        int rowsAffectedSupplierInfo = command.ExecuteNonQuery();

                        if (rowsAffectedSupplierInfo > 0)
                        {
                            MessageBox.Show("Supplier information saved successfully.");
                            ClearTextBoxes();
                            LoadSupplierInformationDetails();

                            string checkEmptyQuery = "SELECT COUNT(*) FROM FuelManagement WHERE FuelType = @FuelType";
                            SqlCommand checkEmptyCommand = new SqlCommand(checkEmptyQuery, connection);
                            checkEmptyCommand.Parameters.AddWithValue("@FuelType", fuelType);

                            int count = Convert.ToInt32(checkEmptyCommand.ExecuteScalar());

                            if (count == 0)
                            {
                                string insertQuery = "INSERT INTO FuelManagement (FuelType, AvailableQuantity, [Date]) VALUES (@FuelType, @AvailableQuantity, @Date)";
                                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                                insertCommand.Parameters.AddWithValue("@FuelType", fuelType);
                                insertCommand.Parameters.AddWithValue("@AvailableQuantity", quantity);
                                insertCommand.Parameters.AddWithValue("@Date", dateTimeString);

                                int insertRowsAffected = insertCommand.ExecuteNonQuery();


                            }
                            else
                            {
                                string fetchAvailableQuantityQuery = "SELECT TOP 1 AvailableQuantity FROM FuelManagement WHERE FuelType = @FuelType ORDER BY [Date] DESC";
                                SqlCommand fetchAvailableQuantityCommand = new SqlCommand(fetchAvailableQuantityQuery, connection);
                                fetchAvailableQuantityCommand.Parameters.AddWithValue("@FuelType", fuelType);

                                double lastAvailableQuantity = Convert.ToDouble(fetchAvailableQuantityCommand.ExecuteScalar());
                                double updatedAvailableQuantity = lastAvailableQuantity + quantity;

                                string insertQuery = "INSERT INTO FuelManagement (FuelType, AvailableQuantity, [Date]) VALUES (@FuelType, @AvailableQuantity, @Date)";
                                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                                insertCommand.Parameters.AddWithValue("@FuelType", fuelType);
                                insertCommand.Parameters.AddWithValue("@AvailableQuantity", updatedAvailableQuantity);
                                insertCommand.Parameters.AddWithValue("@Date", dateTimeString);

                                int insertRowsAffected = insertCommand.ExecuteNonQuery();
 
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to save supplier information.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void LoadSupplierInformationDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM SupplierInformation";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable supplierTable = new DataTable();
                        adapter.Fill(supplierTable);

                        dataGridView1.DataSource = supplierTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading supplier details: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string searchText = txtSearch.Text.Trim();
                    string query = "SELECT * FROM SupplierInformation WHERE CompanyName LIKE @SearchText OR Phone LIKE @SearchText OR MemoNumber LIKE @SearchText";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable supplierTable = new DataTable();
                        adapter.Fill(supplierTable);

                        dataGridView1.DataSource = supplierTable; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching supplier details: " + ex.Message);
            }
        }
        private void ClearTextBoxes()
        {
            txtCName.Text = "";
            txtMNumber.Text = "";
            ComFuel.SelectedIndex = -1;
            txtQuantity.Text = "";
            txtCost.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            txtSearch.Text = "";
        }

      
    }
}
