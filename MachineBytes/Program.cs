using System;
using System.Windows.Forms;

namespace MachineBytes
{
   public static class Program
    {
        public static volatile Machine machine;
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(machine = new Machine());
        }
    }
}
