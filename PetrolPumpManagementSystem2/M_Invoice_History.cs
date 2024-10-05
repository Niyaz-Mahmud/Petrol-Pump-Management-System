using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Invoice_History : Form
    {
        private string connectionString = @"Data Source=SUN;Initial Catalog=PatrolPumpManagementSystem;Integrated Security=True";

        public M_Invoice_History()
        {
            InitializeComponent();
            ShowAllDataInGrid();
            datePicker.Value = DateTime.Now.Date;


        }

        private void BackBT_Click(object sender, EventArgs e)
        {
            M_Home M_Home = new M_Home();
            M_Home.Show();
            this.Hide();
        }

        private void SearchByCustomerNameAndDate(string UserName, DateTime Date)
        {
            try
            {
                string selectQuery = "SELECT * FROM invoice WHERE UserName LIKE @UserName AND CONVERT(DATE, [Date]) = @Date";
               

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {

                        DateTime startDate = Date.Date;
                        DateTime endDate = Date.Date.AddDays(1).AddSeconds(-1);
                        adapter.SelectCommand.Parameters.AddWithValue("@UserName", "%" + UserName + "%");
                        adapter.SelectCommand.Parameters.AddWithValue("@Date", Date.Date);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        invoiceDataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching data: " + ex.Message);
            }
        }

        private void ShowAllDataInGrid()
        {
            try
            {
                string selectAllQuery = "SELECT * FROM invoice";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectAllQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        invoiceDataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching all data: " + ex.Message);
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            DateTime selectedDate = datePicker.Value;


            SearchByCustomerNameAndDate(UserName, selectedDate);
        }

        private void datePicker_ValueChanged_1(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            DateTime selectedDate = datePicker.Value;
            SearchByCustomerNameAndDate(UserName, selectedDate);
        }

        private void invoiceDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowAllDataInGrid();
        }
    }
}
