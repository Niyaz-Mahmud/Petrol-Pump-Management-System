using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class CashierInvoice : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";
        private string loggedInUsername;

        public CashierInvoice(string username)
        {
            InitializeComponent();
            ShowDataInGrid();
            dateTextBox.Value = DateTime.Now.Date;
            loggedInUsername = username;
        }

        private void CalculateReturnedAmount()
        {
            try
            {
                if (!string.IsNullOrEmpty(receivedAmountTextBox.Text) && !string.IsNullOrEmpty(priceTextBox.Text))
                {
                    int receivedAmount = Convert.ToInt32(receivedAmountTextBox.Text);
                    int price = Convert.ToInt32(priceTextBox.Text);
                    int returnedAmount = receivedAmount - price;
                    returnedAmountTextBox.Text = returnedAmount.ToString();
                }
                else
                {
                    returnedAmountTextBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating returned amount: " + ex.Message);
            }
        }


        private void CalculatePrice()
        {
            try
            {
                if (!string.IsNullOrEmpty(quantityTextBox.Text) && !string.IsNullOrEmpty(rateTextBox.Text))
                {
                    int quantity = Convert.ToInt32(quantityTextBox.Text);
                    int rate = Convert.ToInt32(rateTextBox.Text);
                    int price = quantity * rate;
                    priceTextBox.Text = price.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating price: " + ex.Message);
            }
        }
      
        private int FetchLastAvailableQuantityFromDatabase(string fuel, SqlConnection connection, SqlTransaction transaction)
        {
            int lastAvailableQuantity = 0;


            string selectQuery = "SELECT TOP 1 AvailableQuantity FROM FuelManagement WHERE FuelType = @Fuel ORDER BY [Date] DESC";

            using (SqlCommand cmd = new SqlCommand(selectQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Fuel", fuel);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    lastAvailableQuantity = Convert.ToInt32(result);
                }
            }

            return lastAvailableQuantity;
        }

        private void printBT_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = userNameTextBox.Text;
                string fuel = fuelTextBox.Text;
                int quantity = Convert.ToInt32(quantityTextBox.Text);
                int price = Convert.ToInt32(priceTextBox.Text);
                int receivedAmount = Convert.ToInt32(receivedAmountTextBox.Text);

                DateTime selectedDate = dateTextBox.Value.Date;


                DateTime currentTime = DateTime.Now;


                DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                                                         currentTime.Hour, currentTime.Minute, currentTime.Second);

                string dateTimeString = combinedDateTime.ToString("yyyy-MM-dd HH:mm:ss");


                int returnedAmount = receivedAmount - price;

                string insertQueryInvoice = "INSERT INTO invoice (UserName, Fuel, Quantity, Price, ReceivedAmount, ReturnedAmount, [Date]) VALUES (@UserName, @Fuel, @Quantity, @Price, @ReceivedAmount, @ReturnedAmount, @Date)";
                string insertQueryFuel = "INSERT INTO FuelManagement (FuelType, AvailableQuantity, [Date], SalesQuantity) VALUES (@Fuel, @AvailableQuantity, @Date, @SalesQuantity)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            int availableQuantity = FetchLastAvailableQuantityFromDatabase(fuel, connection, transaction);
                            int updatedAvailableQuantity = availableQuantity - quantity;

                            using (SqlCommand cmdInvoice = new SqlCommand(insertQueryInvoice, connection, transaction))
                            {
                                cmdInvoice.Parameters.AddWithValue("@UserName", userName);
                                cmdInvoice.Parameters.AddWithValue("@Fuel", fuel);
                                cmdInvoice.Parameters.AddWithValue("@Quantity", quantity);
                                cmdInvoice.Parameters.AddWithValue("@Price", price);
                                cmdInvoice.Parameters.AddWithValue("@ReceivedAmount", receivedAmount);
                                cmdInvoice.Parameters.AddWithValue("@ReturnedAmount", returnedAmount);
                                cmdInvoice.Parameters.AddWithValue("@Date", dateTimeString);

                                int rowsAffectedInvoice = cmdInvoice.ExecuteNonQuery();

                                if (rowsAffectedInvoice > 0)
                                {
                                    using (SqlCommand cmdFuel = new SqlCommand(insertQueryFuel, connection, transaction))
                                    {
                                        cmdFuel.Parameters.AddWithValue("@Fuel", fuel);
                                        cmdFuel.Parameters.AddWithValue("@AvailableQuantity", updatedAvailableQuantity);
                                        cmdFuel.Parameters.AddWithValue("@Date", dateTimeString);
                                        cmdFuel.Parameters.AddWithValue("@SalesQuantity", quantity);

                                        int rowsAffectedFuel = cmdFuel.ExecuteNonQuery();

                                        if (rowsAffectedFuel > 0)
                                        {
                                            transaction.Commit();
                                            MessageBox.Show("Purchase completed successfully. Invoice inserted, and available quantity updated.");
                                            ShowDataInGrid();
                                            ClearTextBoxes();
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            MessageBox.Show("Failed to insert data into FuelManagement.");
                                        }
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Failed to insert data into the invoice table.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }



        private void ShowDataInGrid()
        {
            try
            {
                string selectQuery = "SELECT * FROM invoice";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        invoiceDataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching data: " + ex.Message);
            }
        }

        private void rateTextBox_TextChanged(object sender, EventArgs e)
        {
            CalculatePrice();
        }

        private void receivedAmountTextBox_TextChanged_1(object sender, EventArgs e)
        {
            CalculateReturnedAmount();
        }

        private void DeleteBT(object sender, EventArgs e)
        {
            try
            {
                if (invoiceDataGridView.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = invoiceDataGridView.SelectedRows[0];
                    string userName = selectedRow.Cells["UserName"].Value.ToString();
                    string deleteQuery = "DELETE FROM invoice WHERE UserName = @UserName";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserName", userName);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Selected record deleted successfully from the invoice table.");
                                ClearTextBoxes();
                            }
                            else
                            {
                                MessageBox.Show("No records found for the selected UserName.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
                ShowDataInGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void ClearTextBoxes()
        {
  
            userNameTextBox.Text = "";
            fuelTextBox.Text = "";
            quantityTextBox.Text = "";
            priceTextBox.Text = "";
            receivedAmountTextBox.Text = "";
            returnedAmountTextBox.Text = "";

            dateTextBox.Value = DateTime.Now;
        }
        private void BackBT_Click(object sender, EventArgs e)
        {
            cashier cashierForm = new cashier(loggedInUsername);
            cashierForm.Show();
            this.Hide();
        }
    }
}
