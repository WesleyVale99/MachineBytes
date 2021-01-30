using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MachineBytes
{
    public partial class IP : Form
    {
        private static readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());
        public Machine m;
        public IP()
        {
            InitializeComponent();
            textBox2.Enabled = false;
        }
        #region Buttons
        private void Button1_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddrSite();
            }).Start();
        }
        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Você deseja Sair?", "Wesley Vale (C) 2019", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        #endregion
        #region IPElements
        public static object RandomElement(object[] array)
        {
            if (array == null || array.Length < 1)
                return null;
            if (array.Length == 1)
                return array[0];
            lock (rnd)
                return array[rnd.Next(array.Length)];
        }
        public void AddrSite()
        {
            try
            {
                string tURL = textBox1.Text.Trim().ToLowerInvariant();
                if (!tURL.Contains("://"))
                {
                    tURL = string.Concat("http://", tURL);
                }
                tURL = new Uri(tURL).Host;
                textBox2.Text = (RandomElement(Dns.GetHostEntry(tURL).AddressList) as IPAddress).ToString();
                textBox2.Enabled = true;
                label2.Text = "Sucesso";
                label2.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                label2.Text = "Error";
                label2.ForeColor = Color.Red;
            }
        }
        #endregion
        #region ReleaseCapture       
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_LBUTTONDOWN)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion
    }
}
