using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient; //Import SQl Client
using Syncfusion.Windows.Shared;
using System.Dynamic;

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        // Event handler for the exit label click
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exit the application when the label is clicked
        }
       public void UserFill()
        {
            //try Catch Exception Handling 
            try
            {
                
                Con.Open();// Open the database connection
                string Myquery = "select * from UserTbl";// Define the SQL query
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a data adapter to execute the query
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a command builder to automatically generate SQL commands
                var ds = new DataSet();// Create a new DataSet to store the data
                da.Fill(ds);// Fill the DataSet with the result of the query
                UsersGV.DataSource = ds.Tables[0];// Set the DataGridView's data source to the first table of the DataSet
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);  // Display an error message if an exception occurs              
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            try
            {
                // Open the database connection
                Con.Open();
                // Define the SQL command to insert a new user into the UserTbl table
                SqlCommand cmd = new SqlCommand("insert into UserTbl values('"+UnameTb.Text+"','"+FnameTb.Text+"','"+PasswordTb.Text+"','"+PhoneTb.Text+"')",Con);
                cmd.ExecuteNonQuery();// Execute the SQL command
                MessageBox.Show("User Successfully Added");// Display a success message
                Con.Close();// Close the database connection
                UserFill(); // Update the DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs   
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            UserFill();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(PhoneTb.Text == "") //If Customer Phone number is null
            {
                MessageBox.Show("Enter The Users Phone Number"); //Display a message
            }
            else
            {
                Con.Open();// Open the database connection
               // Define the SQL command to Delete a user into the UserTbl table
                string myquery = "delete from UserTbl where UPhone = '" + PhoneTb.Text + "';";

                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();// Execute the SQL command
                MessageBox.Show("User Successfully Deleted");// Display a success message
                Con.Close();// Close the database connection
                UserFill(); //Delete the form Data by calling the UserFill method
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the database connection
                Con.Open();
                // Define the SQL command to Update a user into the UserTbl table
                SqlCommand cmd = new SqlCommand("update UserTbl set Uname='"+UnameTb.Text+"',Ufullname='"+FnameTb.Text+"',Upassword='"+PasswordTb.Text+"' where Uphone='"+PhoneTb.Text+"'", Con);
                cmd.ExecuteNonQuery();// Execute the SQL command
                MessageBox.Show("User Successfully Updated");// Display a success message
                Con.Close();// Close the database connection
                UserFill(); //Update the form Data by calling the UserFill method

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs   
            }
        }

        // Populate the text boxes with the selected user's information from the DataGridView
        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UnameTb.Text = UsersGV.SelectedRows[0].Cells[0].Value.ToString();
            FnameTb.Text = UsersGV.SelectedRows[0].Cells[1].Value.ToString();
            PasswordTb.Text = UsersGV.SelectedRows[0].Cells[2].Value.ToString();
            PhoneTb.Text = UsersGV.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();// Show the HomeForm
            this.Hide();// Hide the current form (ManageUsers)
        }

        /* When Click the Text Box using Click Event 
        * it Clear the all Text inside the Text*/
        private void UnameTb_Click(object sender, EventArgs e)
        {
            UnameTb.Text = "";
        }

        private void FnameTb_Click(object sender, EventArgs e)
        {
            FnameTb.Text = "";
        }

        private void PasswordTb_Click(object sender, EventArgs e)
        {
            PasswordTb.Text = "";
        }

        private void PhoneTb_Click(object sender, EventArgs e)
        {
            PhoneTb.Text = "";
        }
    }
}
