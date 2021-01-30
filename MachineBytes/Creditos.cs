using System;
using System.IO;
using System.Windows.Forms;

namespace MachineBytes
{
    public class Creditos
    {
        public void GerarLogger()
        {
            try
            {
                CheckarLogger();
                using (FileStream fileStream = new FileStream($"logs\\debug.txt", FileMode.Append))
                using (StreamWriter stream = new StreamWriter(fileStream))
                {
                    stream.Write($" [Copyright] https://www.facebook.com/wesley.vale.3192 ");
                    stream.Close();
                    fileStream.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void CheckarLogger()
        {
            try
            {
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");
                else
                {
                    Directory.Delete("logs", true);
                    Directory.CreateDirectory("logs");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
