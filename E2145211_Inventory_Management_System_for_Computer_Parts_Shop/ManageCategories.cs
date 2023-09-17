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
using System.CodeDom.Compiler;

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class ManageCategories : Form
    {
        public ManageCategories()
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();//Open the database Connetion
                // Insert a new category into the CategoryTbl table
                SqlCommand cmd = new SqlCommand("insert into CategoryTbl values('" + CataIdTb.Text + "','" + CatanameTb.Text + "')", Con);
                cmd.ExecuteNonQuery(); //Execute the SQL command
                MessageBox.Show("Category Successfully Added"); // Display Success Message
                Con.Close();//Open the database Connetion
                FillCategory();// Add the category list

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }

        }
         public void FillCategory()
        {
            try
            {
                Con.Open();// Open the database connection
                string Myquery = "select * from CategoryTbl"; // Create a data adapter to retrieve data from the CategoryTbl table
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();// Create the dataset Store the data to CategoryTbl table
                da.Fill(ds);// Fill the dataset with data from the CategoryTbl table
                CategoriesGV.DataSource = ds.Tables[0]; // Set the data source of the CategoriesGV DataGridView to the first table in the dataset
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }
        }

        private void ManageCategories_Load(object sender, EventArgs e)
        {
            FillCategory(); // Call the FillCategory() method to populate the category list when the form is loaded
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(CataIdTb.Text  == "") //If Category id is null
            {
                MessageBox.Show("Enter the Category ID"); // Display a message box indicating that the Category ID is required
            }
            else
            {
                Con.Open();// Open the database connection

                // SQL query to delete a category from the CategoryTbl table based on the Category ID
                string myquery = "delete from CategoryTbl where Catid = '" + CataIdTb.Text + "';";

                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();// Execute the SQL command to delete the category
                MessageBox.Show("Category Successfully Deleted"); // Display a message box indicating that the category was successfully deleted
                Con.Close();// Close the database connection
                FillCategory();// Refresh the category list by calling the FillCategory() method
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();//Open the database Connection
                // Update the category name in the CategoryTbl table for the specified category ID
                SqlCommand cmd = new SqlCommand("update CategoryTbl set CatName='" + CatanameTb.Text + "' where CatId='" + CataIdTb.Text + "'", Con);
                cmd.ExecuteNonQuery();//Execute the SQL command
                MessageBox.Show("Category Successfully Updated");
                Con.Close();//Close the database Connection
                FillCategory();//Update the Category data from Dgv 

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs
            }
        }

        private void CategoriesGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // Set the CataIdTb TextBox with the value of the first cell in the selected row of the CategoriesGV DataGridView
            CataIdTb.Text = CategoriesGV.SelectedRows[0].Cells[0].Value.ToString();

           // Set the CataIdTb TextBox with the value of the first cell in the selected row of the CategoriesGV DataGridView
            CatanameTb.Text = CategoriesGV.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();// Show the HomeForm
            this.Hide();// Hide the current form (ManageCategories)
        }

        /* When Click the Text Box using Click Event 
        * it Clear the all Text inside the Text*/
        private void CataIdTb_Click(object sender, EventArgs e)
        {
            CataIdTb.Text = "";
        }

        private void CatanameTb_Click(object sender, EventArgs e)
        {
            CatanameTb.Text = "";
        }
    }
}
