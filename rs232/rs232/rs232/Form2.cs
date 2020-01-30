using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace rs232
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            if (przyslanyBIN != null)
            {
                foreach (bool[] b in przyslanyBIN)
                {
                    bool[] znakBIN = new bool[8];
                    for (int i = 1; i < 9; i++)//usuwanie bitów startu i stopu
                    {
                        znakBIN[i - 1] = b[i];
                    }
                    bool[] odwroconyBIN = new bool[8];
                    for (int j = 0; j < 8; j++)//odwracanie zapisu znaku
                    {
                        odwroconyBIN[j] = znakBIN[znakBIN.Length - 1 - j];
                    }
                    int kod = 0;
                    int n = 1;
                    for (int i = odwroconyBIN.Length - 1; i >= 0; i--)
                    {
                        if (odwroconyBIN[i])
                        {
                            kod += n;
                        }
                        n *= 2;
                    }
                    char znak = (char)kod;
                    textBox2.Text += znak;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    List<string> czarnalista = new List<string>();
                    try
                    {
                        StreamReader f1 = new StreamReader(openFileDialog1.FileName);

                        string line;
                        while ((line = f1.ReadLine()) != null)
                        {
                            czarnalista.Add(line);//zapisuje każdą linie z odczytanego pliku do listy

                        }
                        f1.Close();
                    }
                    catch(IOException ioex)
                    {
                        czarnalista.Add("krowy");
                    }
                    StringBuilder tekst = new StringBuilder(textBox2.Text);
                    foreach (string item in czarnalista)
                    {
                        tekst.Replace(item, "***");//zamienia string item na ***
                        tekst.Replace(item.ToLower(), "***");
                        tekst.Replace(item.ToUpper(), "***");
                    }
                    
                    textBox2.Text = tekst.ToString();
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
