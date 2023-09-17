using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Import SQl Client

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        // Event handler for the checkbox state change
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Check if the checkbox is checked
            if (checkBox1.Checked == false)
                // Use system password character to hide password
                PasswordTb.UseSystemPasswordChar = true;
            else
                // Disable system password character to show password
                PasswordTb.UseSystemPasswordChar = false; 
        }

        // Event handler for clearing username and password
        private void label3_Click(object sender, EventArgs e)
        {
            UnameTb.Text = "";     //Clear User Name
            PasswordTb.Text = "";   //Clear Password
        }

        // Event handler for the login button click
        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            Con.Open();// Open the database connection
            // Create a SQL data adapter with the login query
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from UserTbl where Uname = '"+UnameTb.Text+"' and Upassword = '" + PasswordTb.Text + "'",Con);
            DataTable dt = new DataTable();   // Create a data table to hold the result
            sda.Fill(dt);// Fill the data table with the query result

            // Check if a matching record was found
            if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Login Succeed", "Login Message", MessageBoxButtons.OK); // If Login Success Display a Message 
                HomeForm home = new HomeForm();// Create an instance of the HomeForm
                home.Show();// Show the HomeForm
                this.Hide(); // Hide the current login form
            }
            else
            {
                MessageBox.Show("Incorret UserName or Password Try Again"); // If not Display error Message
            }
            Con.Close();
        }
        // Event handler for the exit label click
        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        /* When Click the Text Box using Click Event 
         * it Clear the all Text inside the Text*/
        private void UnameTb_Click(object sender, EventArgs e)    
        {
            UnameTb.Text = ""; 
        }                           
                                                                               
        private void PasswordTb_Click(object sender, EventArgs e)
        {
            PasswordTb.Text = "";
        }
    }
}
