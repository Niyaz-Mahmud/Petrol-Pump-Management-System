using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Staff : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";
        private int selectedRowIndex = -1;


        public M_Staff()
        {
            InitializeComponent();
            guna2DateTimePicker1.Value = DateTime.Now.Date;
            guna2DateTimePicker2.Value = DateTime.Now.Date;
        }

        private void backBT_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void M_Staff_Load(object sender, EventArgs e)
        {
            LoadStaffDetails();
        }

        private void ClearTextBoxes()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            ComGender.SelectedIndex = -1;
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2DateTimePicker2.Value = DateTime.Now;
            ComShift.SelectedIndex = -1;
            txtSalary.Text = "";
            ComRole.SelectedIndex = -1;
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtSearch.Text = "";
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string gender = ComGender.Text;
            string DOB = guna2DateTimePicker1.Value.ToString("yyyy-MM-dd");
            string JoinDate = guna2DateTimePicker2.Value.ToString("yyyy-MM-dd");
            string shift = ComShift.Text;
            string salary = txtSalary.Text;
            string role = ComRole.Text;
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(gender) ||
                string.IsNullOrWhiteSpace(DOB) || string.IsNullOrWhiteSpace(JoinDate) ||
                string.IsNullOrWhiteSpace(shift) || string.IsNullOrWhiteSpace(salary) ||
                string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!IsNumeric(salary))
            {
                MessageBox.Show("Salary must be a numeric value.");
                return;
            }

            if (!IsValidUser(username))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Staff_Information " +
                                       "(Name, Phone, Address, Gender, DateOfBirth, JoinDate, Shift, Salary, UserName, Password, Role) " +
                                       "VALUES (@Name, @Phone, @Address, @Gender, @DateOfBirth, @JoinDate, @Shift, @Salary, @UserName, @Password, @Role)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@Address", address);
                            command.Parameters.AddWithValue("@Gender", gender);
                            command.Parameters.AddWithValue("@DateOfBirth", DOB);
                            command.Parameters.AddWithValue("@JoinDate", JoinDate);
                            command.Parameters.AddWithValue("@Shift", shift);
                            command.Parameters.AddWithValue("@Salary", salary);
                            command.Parameters.AddWithValue("@UserName", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Account created successfully.");
                                ClearTextBoxes();
                                LoadStaffDetails();
                            }
                            else
                            {
                                MessageBox.Show("Failed to create account.");
                            }
                        }

                        string query2 = "INSERT INTO UserLogin (Username, Password, Role) VALUES (@UserName, @Password, @Role)";

                        using (SqlCommand command = new SqlCommand(query2, connection))
                        {
                            command.Parameters.AddWithValue("@UserName", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int rowsAffected = command.ExecuteNonQuery();

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Username already exists.");
            }
        }

        private void LoadStaffDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Staff_Information";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable staffTable = new DataTable();
                        adapter.Fill(staffTable);

                        dataGridView1.DataSource = staffTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff details: " + ex.Message);
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

        private bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string searchText = txtSearch.Text.Trim();
                    string query = "SELECT * FROM Staff_Information WHERE Name LIKE @SearchText OR Phone LIKE @SearchText";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable staffTable = new DataTable();
                        adapter.Fill(staffTable);

                        dataGridView1.DataSource = staffTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching staff details: " + ex.Message);
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0)
            {
                string phone = txtPhone.Text;
                string address = txtAddress.Text;
                string shift = ComShift.Text;
                string salary = txtSalary.Text;
                string role = ComRole.Text;

                if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address) ||
                    string.IsNullOrWhiteSpace(shift) || string.IsNullOrWhiteSpace(salary) ||
                    string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (!IsNumeric(salary))
                {
                    MessageBox.Show("Salary must be a numeric value.");
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Staff_Information SET Phone = @Phone, Address = @Address, " +
                                       "Shift = @Shift, Salary = @Salary, Role = @Role WHERE UserName = @UserName";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@Address", address);
                            command.Parameters.AddWithValue("@Shift", shift);
                            command.Parameters.AddWithValue("@Salary", salary);
                            command.Parameters.AddWithValue("@Role", role);
                            command.Parameters.AddWithValue("@UserName", dataGridView1.Rows[selectedRowIndex].Cells["UserName"].Value);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record updated successfully.");
                                ClearTextBoxes();
                                LoadStaffDetails();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record.");
                            }
                        }

                        string query2 = "UPDATE UserLogin SET Role = @Role WHERE LOWER(Username) = LOWER(@Username)";

                        using (SqlCommand command = new SqlCommand(query2, connection))
                        {
                            command.Parameters.AddWithValue("@Role", role);
                            command.Parameters.AddWithValue("@UserName", dataGridView1.Rows[selectedRowIndex].Cells["UserName"].Value);

                            int rowsAffected = command.ExecuteNonQuery();

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                txtPhone.Text = selectedRow.Cells["Phone"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                ComShift.Text = selectedRow.Cells["Shift"].Value.ToString();
                txtSalary.Text = selectedRow.Cells["Salary"].Value.ToString();
                ComRole.Text = selectedRow.Cells["Role"].Value.ToString();
            }
        }


    }
}
