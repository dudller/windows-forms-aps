using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dzialanie = 0;
            newInput = true;
            changeTheme(0);
        }
        public void checkInput()
        {
            if (newInput)
            {
                newInput = false;
                label1.Text = "";
                label2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button1.Text;
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button6.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button7.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button8.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button9.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button10.Text;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            checkInput();
            label1.Text += button15.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            newInput = true;
            if (dzialanie == 0)
            {
                bufor = label1.Text;
                
                dzialanie = 1;
                
            }
            else
            {
                dzialanie = 1;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            newInput = true;
            if (dzialanie == 0)
            {
                bufor = label1.Text;
                dzialanie = 2;
            }
            else
            {
                dzialanie = 2;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            newInput = true;
            if (dzialanie == 0)
            {
                bufor = label1.Text;
                dzialanie = 3;
            }
            else
            {
                dzialanie = 3;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            newInput = true;
            if (dzialanie == 0)
            {
                bufor = label1.Text;
                dzialanie = 4;
            }
            else
            {
                dzialanie = 4;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            newInput = true;
            try
            {
                switch (dzialanie)
                {
                    case 0:

                        break;
                    case 1:
                        wynik = Convert.ToDouble(bufor) + Convert.ToDouble(label1.Text);
                        break;
                    case 2:
                        wynik = Convert.ToDouble(bufor) - Convert.ToDouble(label1.Text);
                        break;
                    case 3:
                        wynik = Convert.ToDouble(bufor) * Convert.ToDouble(label1.Text);
                        break;
                    case 4:
                        if (label1.Text.Equals("0")) { wynik = Convert.ToDouble(bufor); label2.Text = "Matematyka nie uznaje dzielenia przez zero \n operacja została anulowana"; }
                        else
                        {
                            wynik = Convert.ToDouble(bufor) / Convert.ToDouble(label1.Text);
                        }
                        break;

                }
                dzialanie = 0;
                label1.Text = Convert.ToString(wynik);

            }
            catch (Exception d) { label2.Text = d.Message; }
                
          
            

            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            newInput = true;
            checkInput();
            bufor = "";
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changeTheme(int themeNumber) //zmienia motyw kolorow zaleznie od wybranego
        {
            switch (themeNumber)
            {
                case 0:
                    foreach (Control control in this.Controls)
                    {
                        this.BackColor = Color.White;
                        if (control is Button)
                        {
                            control.BackColor = Color.LightGray;
                            control.ForeColor = Color.Black;
                        }
                        if (control is Label)
                        {
                            control.ForeColor = Color.Black;
                        }
                        if (control is MenuStrip)
                        {
                            control.BackColor = Color.Gray;

                        }
            
                    }
                        break;
                case 1:

                    foreach (Control control in this.Controls) //dla kazdego elementu na oknie sprawdza jakiej klasy to element i dla konkretnych zmienia kolory
                    {
                        this.BackColor = Color.AliceBlue;
                        if (control is Button)
                        {
                            control.BackColor = Color.Blue;
                            control.ForeColor = Color.LightYellow;
                        }
                        if(control is Label)
                        {
                            control.ForeColor= Color.DeepSkyBlue;
                        }
                        if (control is MenuStrip)
                        {
                            control.BackColor = Color.LightBlue;
                           
                        }
                       
                    }
                    break;
                case 2:
                    foreach (Control control in this.Controls) 
                    {
                        this.BackColor = Color.Wheat;
                        if (control is Button)
                        {
                            control.BackColor = Color.OrangeRed;
                            control.ForeColor = Color.DarkRed;
                        }
                        if (control is Label)
                        {
                            control.ForeColor = Color.Red;
                        }
                        if (control is MenuStrip)
                        {
                            control.BackColor = Color.OrangeRed;

                        }

                    }
                    break;
            }
            
        }

        private void domyślnyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeTheme(0);
            domyślnyToolStripMenuItem.Checked = true;
            niebieskiToolStripMenuItem.Checked = false;
            czerwonyToolStripMenuItem.Checked = false;
        }

        private void niebieskiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeTheme(1);
            niebieskiToolStripMenuItem.Checked = true;
            czerwonyToolStripMenuItem.Checked = false;
            domyślnyToolStripMenuItem.Checked = false;
        }

        private void czerwonyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeTheme(2);
            czerwonyToolStripMenuItem.Checked = true;
            niebieskiToolStripMenuItem.Checked = false;
            domyślnyToolStripMenuItem.Checked = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.currentH = DateTime.Now.Hour;
            this.currentMin = DateTime.Now.Minute;
            this.currentSec = DateTime.Now.Second;
            showtime();
            

        }
        private void showtimebase()
        {
            if (currentH < 10) { this.label3.Text = "0" + currentH + " : "; } else { this.label3.Text = currentH + " : "; }
            if (currentMin < 10) { this.label3.Text += "0" + currentMin + " : "; } else { this.label3.Text += currentMin + " : "; }
            if (currentSec < 10) { this.label3.Text += "0" + currentSec; } else { this.label3.Text += currentSec; }
        }
        private void showtime() //wyswietla czas w label3
        {
            if (czasangielski)
            {
                if (currentH < 12)
                {
                    showtimebase();
                    label3.Text += " am ";
           
                }
                else
                {
                    currentH -= 12;
                    showtimebase();
                    
                    label3.Text += " pm ";
                }
                
            }
            else
            {
                showtimebase();
            }
        }

        private void zmienFormatGodzinyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            czasangielski = !czasangielski;
        }

        private void ButtonKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '1') { this.button2.PerformClick(); }
            else if (e.KeyChar == '2') { this.button3.PerformClick(); }
            else if (e.KeyChar == '3') { this.button4.PerformClick(); }
            else if (e.KeyChar == '4') { this.button5.PerformClick(); }
            else if (e.KeyChar == '5') { this.button6.PerformClick(); }
            else if (e.KeyChar == '6') { this.button7.PerformClick(); }
            else if (e.KeyChar == '7') { this.button8.PerformClick(); }
            else if (e.KeyChar == '8') { this.button9.PerformClick(); }
            else if (e.KeyChar == '9') { this.button10.PerformClick(); }
            else if (e.KeyChar == '0') { this.button1.PerformClick(); }
            else if (e.KeyChar == '+') { this.button11.PerformClick(); }
            else if (e.KeyChar == '-') { this.button12.PerformClick(); }
            else if (e.KeyChar == '*') { this.button13.PerformClick(); }
            else if (e.KeyChar == '/') { this.button14.PerformClick(); }
            else if (e.KeyChar == '\\') { this.button14.PerformClick(); }
            else if (e.KeyChar == '=') { this.button16.PerformClick(); }
            else if (e.KeyChar == 'c') { this.button17.PerformClick(); }
            else if (e.KeyChar == ',') { this.button15.PerformClick(); }


        }

        
    }
}
