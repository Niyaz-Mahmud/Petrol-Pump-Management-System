using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class Login : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public Login()
        {
            InitializeComponent();
        }

        private void LoginBT_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password are required fields. Please fill them in.");
                return;
            }
            if (IsValidUser(username, password))
            {
                string userRole = GetUserRole(username);


                if (userRole == "admin")
                {
                    M_Home adminHomePage = new M_Home();
                    adminHomePage.Show();
                    this.Hide();
                }
                else if (userRole == "Manager")
                {
                    M_Home managerHomePage = new M_Home();
                    managerHomePage.Show();
                    this.Hide();
                }
                else if (userRole == "Staff")
                {
                    Staff staffForm = new Staff(username);
                    staffForm.Show();
                    this.Hide();
                }
                else if (userRole == "Customer")
                {
                    Customer customerForm = new Customer(username);
                    customerForm.Show();
                    this.Hide();
                }
                else if (userRole == "Cashier")
                {
                    cashier cashierForm = new cashier(username);
                    cashierForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool IsValidUser(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM UserLogin WHERE Username = @Username AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

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

        private string GetUserRole(string username)
        {
            string userRole = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Role FROM UserLogin WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            userRole = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return userRole;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginBT.PerformClick();
            }
        }
    }
}