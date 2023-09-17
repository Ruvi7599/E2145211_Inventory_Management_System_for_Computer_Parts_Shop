using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class ManageOrders : Form
    {
        public ManageOrders()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        //Declare the variables to Use The Calculate Part
        int num = 0;
        int uprice, totprice, qty;
        string product;


        private void ManageOrders_Load(object sender, EventArgs e)
        {
            FillOrders(); // Fill the orders data
            populateproducts();// Populate the products data
            fillcategory();// Fill the category data

            // Create columns for the table to hold order details
            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Qty", typeof(int));
            table.Columns.Add("Uprice", typeof(int));
            table.Columns.Add("Totprice", typeof(int));

            OrderGv.DataSource = table; // Set the data source of the DataGridView to the table


        }
        public void FillOrders()
        {
            try
            {
                Con.Open();// Open the database connection
                string Myquery = "select * from CustomerTbl"; // Query to fetch orders data from the CustomerTbl table
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a SqlDataAdapter with the query and the database connection
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a SqlCommandBuilder to automatically generate SQL commands for updating the database
                var ds = new DataSet(); // Create a new DataSet to hold the data
                da.Fill(ds);// Fill the DataSet with data from the SqlDataAdapter
                CustomersGV.DataSource = ds.Tables[0]; // Set the data source of the CustomersGV DataGridView to the first table in the DataSet
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }

        }
        void populateproducts()
        {
            try
            {
                Con.Open(); // Open the database connection
                string Myquery = "select * from ProductTbl"; // Query to fetch product data from the ProductTbl table
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con); // Create a SqlDataAdapter with the query and the database connection
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a SqlCommandBuilder to automatically generate SQL commands for updating the database
                var ds = new DataSet();// Create a new DataSet to hold the data
                da.Fill(ds);// Fill the DataSet with data from the SqlDataAdapter
                ProductsGV.DataSource = ds.Tables[0];// Set the data source of the ProductsGV DataGridView to the first table in the DataSet
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }
        }
        void fillcategory()
        {
            string query = "select * from CategoryTbl"; // Query to fetch category data from the CategoryTbl table
            SqlCommand cmd = new SqlCommand(query, Con); // Create a SqlCommand with the query and the database connection
            SqlDataReader rdr;
            try
            {
                Con.Open();// Open the database connection
                DataTable dt = new DataTable(); // Create a new DataTable to hold the data
                dt.Columns.Add("CatName", typeof(string)); // Add a column named "CatName" to the DataTable
                rdr = cmd.ExecuteReader();// Execute the query and obtain a SqlDataReader
                dt.Load(rdr); // Load the data from the SqlDataReader into the DataTable
                SearchCombo.ValueMember = "CatName"; // Set the value member of the SearchCombo ComboBox to "CatName"
                SearchCombo.DataSource = dt; // Set the data source of the SearchCombo ComboBox to the DataTable
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Display an error message if an exception occurs
            }
        }
        void updateproduct()
        {
          
             int id = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[0].Value.ToString());// Get the selected product ID from the ProductsGV DataGridView
             int newQty = stock - Convert.ToInt32(QtyTb.Text);  // Calculate the new quantity by subtracting the entered quantity from the current stock
           
            if (newQty < 0)
                MessageBox.Show("Operation Failed");// Display a message if the new quantity is less than zero (not enough stock)

            else
            {
                try
                {
                    Con.Open();// Open the database connection
                   // Query to update the product quantity in the ProductTbl table
                    string query = "update ProductTbl set ProdQty = " + newQty + " where ProdId = " + id + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);// Create a SqlCommand with the query and the database connection
                    cmd.ExecuteNonQuery();// Execute the update query
                    Con.Close();// Close the database connection
                    populateproducts();// Update the products data in the ProductsGV DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs
                }
            }
        }

        int flag = 0; // Flag to indicate if a product is selected
        int stock; // Variable to store the stock quantity of the selected product;

        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the selected product name from the ProductsGV DataGridView
            product = ProductsGV.SelectedRows[0].Cells[1].Value.ToString();

            // Get the stock quantity of the selected product
            stock = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[2].Value.ToString());

            // Get the unit price of the selected product
            uprice = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[3].Value.ToString());  
            
            flag = 1; // Set the flag to indicate that a product is selected
        }

        private void CustomersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the selected customer ID from the CustomersGV DataGridView
            CidTb.Text = CustomersGV.SelectedRows[0].Cells[0].Value.ToString();

            // Get the selected customer name from the CustomersGV DataGridView
            CnameTb.Text = CustomersGV.SelectedRows[0].Cells[1].Value.ToString();
        }

        // Event handler for the exit label click
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void insertOrderBtn_Click(object sender, EventArgs e)
        {

            if (OrderIdTb.Text == "" || CidTb.Text == "" || CnameTb.Text == "" || TotAmount.Text == "")
            {
                MessageBox.Show("Fill the data Correctly"); // Show a message if any of the required fields is empty
            }
            else
            {
                try
                {
                    Con.Open(); // Open the database connection
                    // Define the SQL command to insert a data into the Order table
                    SqlCommand cmd = new SqlCommand("insert into OrderTbl values(" + OrderIdTb.Text + "," + CidTb.Text + ",'" + CnameTb.Text + "','" + orderdate.Value.ToString("yyyy-MM-dd") + "'," + TotAmount.Text + ")", Con);
                    cmd.ExecuteNonQuery();// Execute the SQL query to insert the order details into the OrderTbl table
                    MessageBox.Show("Order Added Successfully"); // Show a success message
                    Con.Close();// Close the database connection     
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);  // Display an error message if an exception occurs
                }

            }

        }

        int sum = 0;

        private void viewOrderBtn_Click(object sender, EventArgs e)
        {
            ViewOrders view = new ViewOrders();
            view.Show(); // Open the "ViewOrders" form to view orders
            this.Hide();//Hide OrderForm

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();// Create an instance of the HomeForm
            home.Show();// Show the HomeForm
            this.Hide();// Hide the current ManageOrders form
        }

        /* When Click the Text Box using Click Event 
          * it Clear the all Text inside the Text*/
        private void OrderIdTb_Click(object sender, EventArgs e)
        {
            OrderIdTb.Text = "";
        }

       

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            if (QtyTb.Text == "") // Check if the quantity is not entered
                MessageBox.Show("Enter the Quantity of Product");

            else if (flag == 0) // Check if the product is not selected
                MessageBox.Show("Select the Product");

            else if (Convert.ToInt32(QtyTb.Text) > stock) // Check if the entered quantity exceeds the available stock
                MessageBox.Show("No Enough Stocks Available");

            else
            {
                num = num + 1;  // Increment the item number
                qty = Convert.ToInt32(QtyTb.Text);  // Get the quantity from the Userinput
                totprice = qty * uprice; // Calculate the total price
                table.Rows.Add(num, product, qty, uprice, totprice); // Add a new row to the order table
                OrderGv.DataSource = table;// Set the data source of the DataGridView to the updated table
                flag = 0; // Reset the product selection flag
            }

            sum = sum + totprice; // Update the total amount
            TotAmount.Text = sum.ToString(); 

            updateproduct();// Call the updateproduct() method
        }

        private void SearchCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Con.Open(); // Open the database connection
                // Construct the SQL query to retrieve products based on the selected category
                string Myquery = "select * from ProductTbl where ProdCat='" + SearchCombo.SelectedValue.ToString() + "'";

                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();// Create a new DataSet to store the data
                da.Fill(ds);// Fill the dataset with the data from the database
                ProductsGV.DataSource = ds.Tables[0];// Set the data source of the DataGridView to the retrieved data
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);  // Display an error message if an exception occurs
            }
        }
    }
}
