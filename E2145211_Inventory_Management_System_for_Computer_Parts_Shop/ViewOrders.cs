using System;
using System.Collections;
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
    public partial class ViewOrders : Form
    {
        public ViewOrders()
        {
            InitializeComponent();
        }
        // Establish the database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ruvin\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");
        void populateorders()
        {
            try
            {
                Con.Open();// Open the database connection
                string Myquery = "select * from OrderTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);// Create a data adapter to execute the query
                SqlCommandBuilder builder = new SqlCommandBuilder(da);// Create a command builder to automatically generate SQL commands
                var ds = new DataSet();// Create a new DataSet to store the data
                da.Fill(ds); //Fill the DataSet with the result of the query
                OrderGv.DataSource = ds.Tables[0];// Set the data source of the OrderGv DataGridView
                Con.Close();// Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);// Display an error message if an exception occurs
            }
        }
        private void ViewOrders_Load(object sender, EventArgs e)
        {
            populateorders();// Call the populateorders() method to populate the orders in the OrderGv DataGridView
        }

        private void viewOrderBtn_Click(object sender, EventArgs e)
        {
            this.Hide();// Hide the ViewOrders form
            HomeForm home = new HomeForm();
            home.Show();

        }

        private void OrderGv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // Change the Bill Print into Button ////////this is Additional Info...........
           
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Draw the title "Order Summary" at the specified location using a bold Century font in red color
            e.Graphics.DrawString("Order Summary", new Font("Century", 30, FontStyle.Bold),Brushes.Red, new Point(230));

            // Draw the order details using a Century font in black color at the specified locations
            e.Graphics.DrawString("01.Order Id: " +OrderGv.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century", 20, FontStyle.Bold), Brushes.Black, new Point(80,100));
            e.Graphics.DrawString("02.Customer Id: " + OrderGv.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century", 20, FontStyle.Bold), Brushes.Black, new Point(80, 133));
            e.Graphics.DrawString("03.Customer Name: " + OrderGv.SelectedRows[0].Cells[2].Value.ToString(), new Font("Century", 20, FontStyle.Bold), Brushes.Black, new Point(80, 166));
            e.Graphics.DrawString("04.Order Date: " + OrderGv.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century", 20, FontStyle.Bold), Brushes.Black, new Point(80, 199));
            e.Graphics.DrawString("05.Order Amount: " + OrderGv.SelectedRows[0].Cells[4].Value.ToString(), new Font("Century", 20, FontStyle.Bold), Brushes.Black, new Point(80, 232));

            // Draw the additional information using a smaller Century font in dark green color at the specified location
            e.Graphics.DrawString("CreatedBy_@E2145211_Manoshitha_HPR_@BIT_@UOM", new Font("Century", 15, FontStyle.Bold), Brushes.DarkGreen, new Point(120,350));
        }

        private void btnPB_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();// Print the order summary when the user Select on a cell  in the OrderGv DataGridView and Click the Button 
            }
        }
    }
}
