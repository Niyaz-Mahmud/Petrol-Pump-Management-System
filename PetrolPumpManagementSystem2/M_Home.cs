using System;
using System.Windows.Forms;

namespace PetrolPumpManagementSystem2
{
    public partial class M_Home : Form
    {
        public M_Home()
        {
            InitializeComponent();
        }

        private void StaffClick_Click(object sender, EventArgs e)
        {
            M_Staff M_Staff = new M_Staff();
            M_Staff.Show();
            this.Hide();
        }

        private void SupplierClick_Click(object sender, EventArgs e)
        {
            M_Supplies M_Supplies = new M_Supplies();
            M_Supplies.Show();
            this.Hide();
        }

        private void fuelclick_Click(object sender, EventArgs e)
        {
            M_Fuel Fuel = new M_Fuel();
            Fuel.Show();
            this.Hide();
        }

        private void MachineClick_Click(object sender, EventArgs e)
        {
            mAtmBoot PumpBooth = new mAtmBoot();
            PumpBooth.Show();
            this.Hide();
        }

        private void customer_Click(object sender, EventArgs e)
        {
            M_Customers M_Customer = new M_Customers();
            M_Customer.Show();
            this.Hide();
        }

        private void Invoice_Click(object sender, EventArgs e)
        {
            Invoice Invoice = new Invoice();
            Invoice.Show();
            this.Hide();
        }

        private void InvoiceHistory_Click(object sender, EventArgs e)
        {
            M_Invoice_History M_Invoice_History = new M_Invoice_History();
            M_Invoice_History.Show();
            this.Hide();
        }

        private void DashBoard_Click(object sender, EventArgs e)
        {
            M_Dashboard M_Dashboard = new M_Dashboard();
            M_Dashboard.Show();
            this.Hide();
        }

        private void logOut_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
        }
    }
}
