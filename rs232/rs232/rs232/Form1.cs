using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rs232
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            f2 = new Form2();
            f2.Show();
            
        }

        public bool[][] ZamianaNaBinarny(string text)
        {
            string t = text;
            bool[][] bin= new bool [t.Length][];
            for (int i = 0; i < t.Length; i++)
            {
                int kod = (int)t[i];//zamiana znakk a kod asci
                bool[] znakBIN = new bool[16];
                for (int j = 15; j >= 0; j--)//zamiana kodu na zapis binarny
                {
                    if (kod % 2 == 1) znakBIN[j] = true;
                    else znakBIN[j] = false;

                    kod /= 2;
                }
                bool[] odwroconyBIN = new bool[19];
                odwroconyBIN[0] = false;//bit startu
                for (int j = 0; j < znakBIN.Length; j++)//odwracanie zapisu znaku
                {
                    odwroconyBIN[j+1] = znakBIN[znakBIN.Length - 1 - j];
                }
                odwroconyBIN[17] = true;//bity stopu
                odwroconyBIN[18] = true;
                bin[i] = odwroconyBIN;
            
            }
            return bin;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            binarnyTekst=ZamianaNaBinarny(textBox1.Text);
            textBox2.Text = "";
            foreach (bool[] b in binarnyTekst)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (b[i]) textBox2.Text += "1";
                    else textBox2.Text += "0";
                }
                textBox2.Text += " ";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox2.Text.Equals("")) {
                f2.przyslanyBIN = binarnyTekst;
                f2.textBox1.Text = "";
                foreach (bool[] b in binarnyTekst)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        if (b[i]) f2.textBox1.Text += "1";
                        else f2.textBox1.Text += "0";
                    }
                    f2.textBox1.Text += " ";
                }

            }
            
        }
    }
}
