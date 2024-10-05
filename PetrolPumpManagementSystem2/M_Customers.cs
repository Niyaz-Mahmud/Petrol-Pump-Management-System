using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Customers : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public M_Customers()
        {
            InitializeComponent();
            LoadCustomerDetails();
            guna2DateTimePicker1.Value = DateTime.Now.Date;
            guna2DateTimePicker2.Value = DateTime.Now.Date;
        }

        private void backBT_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string vehicleNumber = txtVehicleNumber.Text;
            string name = txtName.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string gender = ComGender.Text;
            DateTime selectedDate = guna2DateTimePicker1.Value;
            string DOB = selectedDate.ToString();
            DateTime join = guna2DateTimePicker2.Value;
            string JoinDate = join.ToString();
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string role = "Customer";

            if (string.IsNullOrWhiteSpace(vehicleNumber) || string.IsNullOrWhiteSpace(name)
                  || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address)
                  || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(DOB)
                  || string.IsNullOrWhiteSpace(JoinDate) || string.IsNullOrWhiteSpace(username)
                  || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            if (!IsValidUser(username))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Customer_Information " +
                                       "(vehicleNumber, Name, Phone, Address, Gender, DateOfBirth, JoinDate, UserName, Password) " +
                                       "VALUES (@vehicleNumber, @Name, @Phone, @Address, @Gender, @DateOfBirth, @JoinDate, @UserName, @Password)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@vehicleNumber", vehicleNumber);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@Address", address);
                            command.Parameters.AddWithValue("@Gender", gender);
                            command.Parameters.AddWithValue("@DateOfBirth", DOB);
                            command.Parameters.AddWithValue("@JoinDate", JoinDate);
                            command.Parameters.AddWithValue("@UserName", username);
                            command.Parameters.AddWithValue("@Password", password);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Account created successfully...");
                                ClearFields();
                                LoadCustomerDetails();
                            }
                            else
                            {
                                MessageBox.Show("Failed to create account...");
                            }
                        }

                        string s = "INSERT INTO UserLogin (Username, Password, Role) VALUES (@UserName, @Password, @Role)";

                        using (SqlCommand command = new SqlCommand(s, connection))
                        {
                            command.Parameters.AddWithValue("@UserName", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int rowsAffected = command.ExecuteNonQuery();
                        }
                    }

                    LoadCustomerDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Username already exists...");
            }
        }

        private void LoadCustomerDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT [VehicleNumber], [Name], [Phone], [Address], [Gender], [DateOfBirth], [JoinDate], [UserName] FROM [dbo].[Customer_Information]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridViewCustomerDetails.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading customer details: " + ex.Message);
            }
        }

        private bool IsValidUser(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM UserLogin WHERE LOWER(Username) = LOWER(@Username)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void ClearFields()
        {
            txtVehicleNumber.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            ComGender.SelectedIndex = -1;
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2DateTimePicker2.Value = DateTime.Now;
            txtUserName.Text = "";
            txtPassword.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Customer_Information WHERE Name LIKE @searchText";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchText", "%" + txtSearch.Text + "%");
                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        dataGridViewCustomerDetails.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
