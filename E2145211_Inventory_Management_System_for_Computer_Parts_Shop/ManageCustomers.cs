using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; //Import SQl Client
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class ManageCustomers : Form
    {
        public ManageCustomers()
        {
            InitializeComponent();
        }
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();// Open the database connection
                // Define the SQL command to insert a new Customer into the Customer table
                SqlCommand cmd = new SqlCommand("insert into CustomerTbl values('" + CidTb.Text + "','" + CnameTb.Text + "','" + CphoneTb.Text + "')", Con);
                cmd.ExecuteNonQuery();// Execute the insert command
                MessageBox.Show("Customer Successfully Added");// Display a success message
                Con.Close();// Close the database connection
                FillCustomer();// Update the form by calling the FillCustomer method
         
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }
        }

        // Event handler for the exit label click
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();// Exit the application when the label is clicked
        }
        public void FillCustomer()
        {
            try
            {
                Con.Open();// Open the database connection
                string Myquery = "select * from CustomerTbl"; //Select the customer Table all Values
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a data adapter to execute the query
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a command builder to automatically generate SQL commands
                var ds = new DataSet();// Create a new DataSet to store the data
                da.Fill(ds);// Fill the DataSet with the result of the query
                CustomersGV.DataSource = ds.Tables[0];// Fill the CustomersGV DataGridView with data from the CustomerTbl
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs
            }
        }

        private void ManageCustomers_Load(object sender, EventArgs e)
        {
            FillCustomer();// Call the FillCustomer method to populate the form
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CidTb.Text == "") //If customer id is Null
            {
                MessageBox.Show("Enter the Customer ID");// Display a message if the CidTb is empty
            }
            else
            {
                Con.Open();// Open the database connection
                // Define the SQL query to delete a customer
                string myquery = "delete from CustomerTbl where Custid = '" + CidTb.Text + "';";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();// Execute the delete query
                MessageBox.Show("Customer Successfully Deleted"); // Display a Deleted message
                Con.Close();// Close the database connection
                FillCustomer();// Delete the form data by calling the FillCustomer method
            }
        }

        private void CustomersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate the text boxes with the selected customer's information from the DataGridView
            CidTb.Text = CustomersGV.SelectedRows[0].Cells[0].Value.ToString();
            CnameTb.Text = CustomersGV.SelectedRows[0].Cells[1].Value.ToString();
            CphoneTb.Text = CustomersGV.SelectedRows[0].Cells[2].Value.ToString();

            // Count the number of orders for the selected customer

            Con.Open();// Open the database connection
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from OrderTbl where CustId = " + CidTb.Text + "", Con);
            DataTable dt = new DataTable();    // Create a data table to hold the result
            sda.Fill(dt); // Fill the data table with the query result
            OrderLabel.Text = dt.Rows[0][0].ToString(); //OutPut Showing in Label

            // Calculate the total amount spent by the selected customer
            SqlDataAdapter sda1 = new SqlDataAdapter("Select Sum(TotalAmt) from OrderTbl where CustId = " + CidTb.Text + "", Con);
            DataTable dt1 = new DataTable();// Create a data table to hold the result
            sda1.Fill(dt1);// Fill the data table with the query result
            AmountLabel.Text = dt1.Rows[0][0].ToString();//OutPut Showing in Label

            // Get the latest order date for the selected customer
            SqlDataAdapter sda2 = new SqlDataAdapter("Select Max(OrderDate) from OrderTbl where CustId = " + CidTb.Text + "", Con);
            DataTable dt2 = new DataTable();// Create a data table to hold the result
            sda2.Fill(dt2);// Fill the data table with the query result
            DateLabel.Text = dt2.Rows[0][0].ToString();//OutPut Showing in Label
            Con.Close();// Close the database connection
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();// Open the database connection
               // Define the SQL command to Update a Customer into the Customer table
                SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName='" + CnameTb.Text + "',CustPhone='" + CphoneTb.Text + "' where CustId='" + CidTb.Text + "'", Con);
                cmd.ExecuteNonQuery();// Execute the Update query
                MessageBox.Show("Customer Successfully Updated");//Display Updated Message
                Con.Close();// Close the database connection
                FillCustomer();//Update the form Data by calling the FillCustomer method

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();// Create an instance of the HomeForm
            home.Show();// Show the HomeForm
            this.Hide();// Hide the current ManageCustomer form
        }

        /* When Click the Text Box using Click Event 
        * it Clear the all Text inside the Text*/
        private void CidTb_Click(object sender, EventArgs e)
        {
            CidTb.Text = "";
        }

        private void CnameTb_Click(object sender, EventArgs e)
        {
            CnameTb.Text = "";
        }

        private void CphoneTb_Click(object sender, EventArgs e)
        {
            CphoneTb.Text = "";
        }

        private void DateLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
