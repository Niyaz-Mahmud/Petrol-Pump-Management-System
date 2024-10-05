using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class Staff : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";
        private string loggedInUsername;

        public Staff(string username)
        {
            InitializeComponent();
            loggedInUsername = username;
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            ShowStaffDetails();
        }

        private void ShowStaffDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Staff_Information WHERE Username = @Username";

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

        private void Update_Click(object sender, EventArgs e)
        {
            string newPassword = newPasswordTextBox.Text;
            string currentPassword = currentPasswordTextBox.Text;
            UpdatePassword(newPassword, currentPassword);
        }

        private void logOUTBT_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
