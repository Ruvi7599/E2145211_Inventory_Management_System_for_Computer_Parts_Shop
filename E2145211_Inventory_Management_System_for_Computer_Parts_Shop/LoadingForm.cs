using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; //Import  InteropServices

namespace E2145211_Inventory_Management_System_for_Computer_Parts_Shop
{
    public partial class LoadingForm : Form
    {
        sbyte progressBarRed = 2; //sets the value of the progressBarRed variable to 2

        public LoadingForm()
        {
            InitializeComponent();
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Check if progress is less than 100
            if (progress.Value < 100)
            {
                // Increment the progress value by 1
                progress.Value += 1;

                // Update the label to show the progress percentage
                PercentageLbl.Text = progress.Value.ToString() + "%";
            }
            else
            {
                // Stop the timer
                timer1.Stop();

                // Create an instance of the LoginForm
                LoginForm login = new LoginForm();
                this.Hide();// Hide the current loading form
                login.Show();// Show the LoginForm
            }
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            timer1.Start();// Start the timer
            ModifyProgressBarColor.SetState(progress, progressBarRed);// Set the state of the progress bar to the specified color
        }
    }

    //Modify Progress Bar Values and Colours
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr one);
        public static void SetState(this ProgressBar pBar , int state)
        {
            SendMessage(pBar.Handle,1040, (IntPtr) state, IntPtr.Zero);
        }

    }

}
