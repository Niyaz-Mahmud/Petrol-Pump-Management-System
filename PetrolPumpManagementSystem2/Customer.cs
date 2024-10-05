using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class Customer : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";
        private string loggedInUsername;

        public Customer(string username)
        {
            InitializeComponent();
            loggedInUsername = username;
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM invoice WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", loggedInUsername);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable userData = new DataTable();
                            adapter.Fill(userData);
                            userDataGridView.DataSource = userData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdatePassword(string newPassword, string currentPassword)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string queryCheckPassword = "SELECT COUNT(*) FROM [UserLogin] WHERE [Username] = @Username AND [Password] = @CurrentPassword";

                    using (SqlCommand cmdCheckPassword = new SqlCommand(queryCheckPassword, connection))
                    {
                        cmdCheckPassword.Parameters.AddWithValue("@Username", loggedInUsername);
                        cmdCheckPassword.Parameters.AddWithValue("@CurrentPassword", currentPassword);

                        int passwordMatchCount = (int)cmdCheckPassword.ExecuteScalar();

                        if (passwordMatchCount > 0)
                        {
                            string queryUpdatePassword = "UPDATE [UserLogin] SET [Password] = @NewPassword WHERE [Username] = @Username";

                            using (SqlCommand cmdUpdatePassword = new SqlCommand(queryUpdatePassword, connection))
                            {
                                cmdUpdatePassword.Parameters.AddWithValue("@NewPassword", newPassword);
                                cmdUpdatePassword.Parameters.AddWithValue("@Username", loggedInUsername);

                                int rowsAffected = cmdUpdatePassword.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Password updated successfully!");
                                    ClearTextBoxes();
                                }
                                else
                                {
                                    MessageBox.Show("Password update failed.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Current password does not match.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private bool AreFieldsEmpty()
        {
            return string.IsNullOrWhiteSpace(newPasswordTextBox.Text)
                || string.IsNullOrWhiteSpace(currentPasswordTextBox.Text);
        }
        private void ClearTextBoxes()
        {
            newPasswordTextBox.Text = "";
            currentPasswordTextBox.Text = "";
        }
        private void updateBT_Click(object sender, EventArgs e)
        {
            if (AreFieldsEmpty())
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            string newPassword = newPasswordTextBox.Text;
            string currentPassword = currentPasswordTextBox.Text;

            UpdatePassword(newPassword, currentPassword);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
