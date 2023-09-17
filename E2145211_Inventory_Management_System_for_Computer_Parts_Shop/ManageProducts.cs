using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Import SQL Client
using Syncfusion.Windows.Shared;
using System.CodeDom.Compiler;

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class ManageProducts : Form
    {
        public ManageProducts()
        {
            InitializeComponent();
        }
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        void fillcategory()
        {
            string query = "select * from CategoryTbl"; // Query to select all categories from the CategoryTbl table
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader rdr;
            try
            {
                Con.Open();// Open the database connection
                DataTable dt = new DataTable();
                dt.Columns.Add("CatName", typeof(string));
                rdr = cmd.ExecuteReader(); // Execute the SQL query and retrieve the data
                dt.Load(rdr); // Load the data into the DataTable

                // Bind the DataTable to the CatCombo combo box
                CatCombo.ValueMember = "CatName";
                CatCombo.DataSource = dt;
                SearchCombo.ValueMember = "CatName";
                SearchCombo.DataSource = dt;

                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs   
            }
        }

        private void ManageProducts_Load(object sender, EventArgs e)
        {
            fillcategory(); // Call the fillcategory() method to populate the category combo box
            FillProducts(); // Call the FillProducts() method to populate the products grid view
        }

        public void FillProducts()
        {
            try
            {
                Con.Open(); //Open the DatabAse Connetion
                string Myquery = "select * from ProductTbl"; // SQL query to select all products
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a data adapter to retrieve data from the database
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a command builder to generate SQL commands
                var ds = new DataSet();// Create a new data set to hold the retrieved data
                da.Fill(ds);// Fill the data set with the product data
                ProductsGV.DataSource = ds.Tables[0];// Set the data source of the products grid view to the data table in the data set
                Con.Close();//Close the DatabAse Connetion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs   
            }
        }
        void filterbycategory()
        {
            try
            {
                Con.Open();//Open the Database Connetion
                // SQL query to select products based on the selected category
                string Myquery = "select * from ProductTbl where ProdCat='"+SearchCombo.SelectedValue.ToString()+"'";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a data adapter to retrieve data from the database
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a command builder to generate SQL commands
                var ds = new DataSet();// Create a new data set to hold the retrieved data
                da.Fill(ds);// Fill the data set with the product data
                ProductsGV.DataSource = ds.Tables[0];// Set the data source of the products grid view to the data table in the data set
                Con.Close();//Close the Database Connetion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs   
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();//Open the database Connection
                // SQL command to insert a new product with the provided details
                SqlCommand cmd = new SqlCommand("insert into ProductTbl values('" + PidTb.Text + "','" + PnameTb.Text + "','" + PqTb.Text + "','" + PpTb.Text + "','" + PdTb.Text + "','" + CatCombo.SelectedValue.ToString()+ "')", Con);
                cmd.ExecuteNonQuery();// Execute the SQL command to insert the product into the database
                MessageBox.Show("Product Successfully Added"); //Display Success Message 
                Con.Close();//Close the database Connection
                FillProducts();// Refresh the products grid view to display the updated list of products


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs   
            }
        }

        // Event handler for the exit label click
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();// Exit the application when the label is clicked
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (PidTb.Text == "")
            {
                MessageBox.Show("Enter the Product ID"); // Display an error message if the product ID is not entered
            }
            else
            {
                Con.Open();// Open the database connection
                // SQL query to delete the product with the specified ID
                string myquery = "delete from ProductTbl where ProdId = '" + PidTb.Text + "';";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();// Execute the SQL command to delete the product from the database
                MessageBox.Show("Product Successfully Deleted");// Display a success message
                Con.Close();// Close the database connection
                FillProducts();// Refresh the products grid view to display the updated list of products
            }
        }

        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PidTb.Text = ProductsGV.SelectedRows[0].Cells[0].Value.ToString(); // Get the product ID from the selected row and display it in the corresponding text box
            PnameTb.Text = ProductsGV.SelectedRows[0].Cells[1].Value.ToString(); // Get the product name from the selected row and display it in the corresponding text box
            PqTb.Text = ProductsGV.SelectedRows[0].Cells[2].Value.ToString(); // Get the product quantity from the selected row and display it in the corresponding text box
            PpTb.Text = ProductsGV.SelectedRows[0].Cells[3].Value.ToString(); // Get the product price from the selected row and display it in the corresponding text box
            PdTb.Text = ProductsGV.SelectedRows[0].Cells[4].Value.ToString(); // Get the product description from the selected row and display it in the corresponding text box
            CatCombo.SelectedValue = ProductsGV.SelectedRows[0].Cells[5].Value.ToString(); // Get the product category from the selected row and set it as the selected value in the category combo box

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();//Open the database Connetion
                // Construct the SQL command to update the product
                SqlCommand cmd = new SqlCommand("update ProductTbl set ProdName='" + PnameTb.Text + "',ProdQty='" + PqTb.Text + "',ProdPrice='" +PpTb.Text + "',ProdDesc='" + PdTb.Text + "',ProdCat='" + CatCombo.SelectedValue.ToString() + "' where ProdId='" + PidTb.Text + "'", Con);
                cmd.ExecuteNonQuery();// Execute the SQL command to update the product
                MessageBox.Show("Product Successfully Updated");// Display a success message
                Con.Close();//Close the database Connetion
                FillProducts();// Refresh the product list

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs   
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            filterbycategory();// Perform category-based filtering of products
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillProducts();// Refresh the product list
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();// Create an instance of the HomeForm
            home.Show();// Show the HomeForm
            this.Hide();// Hide the current ManageProducts form
        }

        /* When Click the Text Box using Click Event 
       * it Clear the all Text inside the Text*/
        private void PidTb_Click(object sender, EventArgs e)
        {
            PidTb.Text = "";
        }

        private void PnameTb_Click(object sender, EventArgs e)
        {
            PnameTb.Text = "";
        }

        private void PqTb_Click(object sender, EventArgs e)
        {
            PqTb.Text = "";
        }

        private void PpTb_Click(object sender, EventArgs e)
        {
            PpTb.Text = "";
        }

        private void PdTb_Click(object sender, EventArgs e)
        {
            PdTb.Text = "";
        }
    }
}
