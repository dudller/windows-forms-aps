using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProceesorSymulator
{
    public partial class Form1 : Form
    {
        public List<String> param1; //parametry 1 rozkazu
        public List<String> param2; //parametry 2 rozkazu
        public List<String> instruction;//aktywne rozkazy
        private Boolean[] rejestr;//pamiec procesora rejestry


        // adresy pierwszych komórek danych rejestrów
        //AX
        public const int AH = 8;
        public const int AL = 0;
        //BX
        public const int BH = 24;
        public const int BL = 16;
        //CX
        public const int CH = 40;
        public const int CL = 32;
        //DX
        public const int DH = 56;
        public const int DL = 48;

        
        private static bool CF = false; //flaga przeniesienia
        private static bool OF = false; //flaga przepełnienia
        private static bool TF = false; //flaga pracy krokowej

        private int k = 0;//wskaznik zadania
        int paramcounter = 0;//wskaznik parametru 2
        int counter = 0; //wskaznik parametru 1

        private bool[][] stack; //stos do prechowywania danych
        private int sp = 0;//wskaznik stosu
        private String[] interuptvector= { "INT1Ah", "INT16h", "INT10h", "INT12h",  "INT09h", "INT06h", "INT00h", "INT05h", "INT08h",  "INT14h" }; //wektor przerwań
        Task task;
        public Form1()
        {
            InitializeComponent();
            instruction = new List<string>();
            param1 = new List<string>();
            param2 = new List<string>();
            stack = new bool[10][];
            stack[0] = new bool[16];
            stack[1] = new bool[16];
            stack[2] = new bool[16];
            stack[3] = new bool[16];
            stack[4] = new bool[16];
            stack[5] = new bool[16];
            stack[6] = new bool[16];
            stack[7] = new bool[16];
            stack[8] = new bool[16];
            stack[9] = new bool[16];
            rejestr = new bool[64];
            for (int i =0; i < 64; i++) { rejestr[i]=false; }

        }

        private void push(int komorka)//dodanie do stosu
        {
            
            for (int i=0; i < 16; i++)
            {
                stack[sp][i] = rejestr[i + komorka];
            }
            sp++;
        }
        private void pop(int komorka)//zdjecie ze stosu
        {
            sp--;
            for (int i = 0; i < 16; i++)
            {
                rejestr[i + komorka]= stack[sp][i];
            }
            
        }
        private void interrupt(int i)//funkcja przerwania
        {
            switch (i)
            {
                case 0:
                    //pobranie danych zegara
                    int h = DateTime.Now.Hour;
                    int m = DateTime.Now.Minute;
                    int s = DateTime.Now.Second;
                    push(CL);
                    push(DL);
                    //wypisanie ich w rejestrach
                    writeIntInRej(h, CL);
                    writeIntInRej(m, CH);
                    writeIntInRej(s, DH);
                    label14.Text = "godz:min:sek \n" + h + ":" + m + ":" + s + "\n przerwanie 1Ah funcja podania czasu zegara rzeczywistego";
                    displaydata();
                    //odczekanie 5 sekund zeby ladnie bylo widać że się zmienily dane
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        //przywrocenie danych
                        pop(DL);
                        pop(CL);
                        //zamrozenie watku zeby uzytkownik nie mogl zaklucic procesu
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;
                case 1:
                    int c =Console.In.Read();
                    push(CL);
                    writeIntInRej(c, CL);
                    displaydata();
                    label14.Text = "przerwanie 16h funkcja odczytanie klawisza z klawiatury klawisz o numerze : " +c ;
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(CL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;
                case 2:
                    Point mouse = System.Windows.Forms.Cursor.Position;
                    push(CL);
                    writeIntInRej(mouse.X, CL);
                    writeIntInRej(mouse.Y, CH);

                    displaydata();
                    label14.Text = "przerwanie 10h funkcja odczytania pozycji kursora: " + mouse.X +" , " + mouse.Y ;
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(CL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });

                    break;
                case 3:
                    label14.Text = "przerwanie 12h funkcja określenia wielkości pamięci podstawowej: 254 ";
                    push(CL);
                    writeIntInRej(254, CH);
                    displaydata();
                    label14.Text = "przerwanie 12h funkcja określenia wielkości pamięci podstawowej: 254 ";
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(CL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });

                    break;
                case 4:
                    label14.Text = "przerwanie 09h przepełnienie stosu koprocesora";
                    push(CL);
                    writeIntInRej(0, CH);
                    label14.Text = "przerwanie 09h przepełnienie stosu koprocesora";
                    displaydata();
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(CL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    

                    break;
                case 5:
                    label14.Text = "przerwanie 06h niepoprawny kod operacji";
                    push(AL);
                    push(BL);
                    push(CL);
                    push(DL);
                    writeIntInRej(0, AH);
                    writeIntInRej(1, BH);
                    writeIntInRej(2, CH);
                    writeIntInRej(3, DH);
                    displaydata();
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(DL);
                        pop(CL);
                        pop(BL);
                        pop(AL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                   
                    break;
                case 6:
                    label14.Text = "przerwanie 00h błąd dzielenia przez 0";
                    push(CL);
                    writeIntInRej(10, CH);
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        pop(CL);
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;
                case 7:
                    label14.Text = "przerwanie 05h przekroczenie wartości granicznych";
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;
                case 8:
                    label14.Text = "przerwanie 08h podwójny bląd";
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;
                case 9:
                    label14.Text = "przerwanie 14h funkcja inicjalizacji portu szeregowego";
                    task = Task.Run(() => {
                        Task.Delay(4000).Wait();
                        System.Threading.Thread.Sleep(5000);
                        displaydata();
                    });
                    break;

            }
            //sprawdzenie predkosci wykonania zadania
            TimeSpan ts = TimeSpan.FromMilliseconds(6000);
            if (!task.Wait(ts))
                Console.WriteLine("Zadanie zakończone w przedziale czasowym");

        }

        private void writeIntInRej(int value, int rej)
        {
            int v = value;
            for (int j = 7; j >= 0; j--)//zamiana kodu na zapis binarny
            {
                if (v % 2 == 1) rejestr[j+rej] = true;
                else rejestr[j + rej] = false;

                v /= 2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(k);
            if (k == 0)
            {
                try
                {
                    if (comboBox1.Enabled) instruction.Add((String)comboBox1.SelectedItem);
                    if (comboBox2.Enabled)
                    {
                        if (comboBox2.Text.Equals("stala")) param1.Add(textBox1.Text);
                        else param1.Add((String)comboBox2.SelectedItem);
                    }
                    if (comboBox3.Enabled) param2.Add((String)comboBox3.SelectedItem);

                    if (comboBox4.Enabled && comboBox4.SelectedItem != null) instruction.Add((String)comboBox4.SelectedItem);
                    if (comboBox5.Enabled)
                    {
                        if (comboBox5.Text.Equals("stala")) param1.Add(textBox2.Text);
                        else param1.Add((String)comboBox5.SelectedItem);
                    }
                    if (comboBox6.Enabled) param2.Add((String)comboBox6.SelectedItem);

                    if (comboBox7.Enabled && comboBox7.SelectedItem != null) instruction.Add((String)comboBox7.SelectedItem);
                    if (comboBox8.Enabled)
                    {
                        if (comboBox8.Text.Equals("stala")) param1.Add(textBox3.Text);
                        else param1.Add((String)comboBox8.SelectedItem);
                    }
                    if (comboBox9.Enabled) param2.Add((String)comboBox9.SelectedItem);

                    if (comboBox10.Enabled && comboBox10.SelectedItem != null) instruction.Add((String)comboBox10.SelectedItem);
                    if (comboBox11.Enabled)
                    {
                        if (comboBox11.Text.Equals("stala")) param1.Add(textBox4.Text);
                        else param1.Add((String)comboBox11.SelectedItem);
                    }
                    if (comboBox12.Enabled) param2.Add((String)comboBox12.SelectedItem);

                    if (comboBox13.Enabled && comboBox13.SelectedItem != null) instruction.Add((String)comboBox13.SelectedItem);
                    if (comboBox14.Enabled)
                    {
                        if (comboBox14.Text.Equals("stala")) param1.Add(textBox5.Text);
                        else param1.Add((String)comboBox14.SelectedItem);
                    }
                    if (comboBox15.Enabled) param2.Add((String)comboBox15.SelectedItem);

                    if (comboBox16.Enabled && comboBox16.SelectedItem != null) instruction.Add((String)comboBox16.SelectedItem);
                    if (comboBox17.Enabled)
                    {
                        if (comboBox17.Text.Equals("stala")) param1.Add(textBox6.Text);
                        else param1.Add((String)comboBox17.SelectedItem);
                    }
                    if (comboBox18.Enabled) param2.Add((String)comboBox18.SelectedItem);

                    if (comboBox19.Enabled && comboBox19.SelectedItem != null) instruction.Add((String)comboBox19.SelectedItem);
                    if (comboBox20.Enabled)
                    {
                        if (comboBox20.Text.Equals("stala")) param1.Add(textBox7.Text);
                        else param1.Add((String)comboBox20.SelectedItem);
                    }
                    if (comboBox21.Enabled) param2.Add((String)comboBox21.SelectedItem);

                }
                catch (Exception ex) { }
            }
            execute();


        }


        private void type1cbchanged(object sender, EventArgs e)
        {
            ComboBox boks = (ComboBox)sender;
            String tekst = boks.Text;
   
                    ComboBox boks2 = (ComboBox)GetNextControl(boks, true);
                    boks2.Enabled = true;//mozna dodac arg 1
                    GetNextControl(GetNextControl(GetNextControl(GetNextControl(boks, true), true), true), true).Enabled=true; //kolejny rozkaz mozna dodac
                    if (tekst.Equals("mov")) GetNextControl(boks2, true).Enabled = true; //mozna dodac arg 2
                    else GetNextControl(boks2, true).Enabled = false;

            if (!tekst.Equals("add")&& !tekst.Equals("sub") && !tekst.Equals("mov")) { boks2.Enabled = false; }
  
            
        }
        private void type2cbchanged(object sender, EventArgs e)
        {
            ComboBox boks = (ComboBox)sender;
            String tekst = boks.Text;
            button1.Enabled = true;
            if (tekst.Equals("stala")) { GetNextControl(GetNextControl(boks, true), true).Enabled = true; //mozna wpisac stala

            }
            else GetNextControl(GetNextControl(boks, true), true).Enabled = false;


        }
        private void type3cbchanged(object sender, EventArgs e)
        {
            ComboBox boks = (ComboBox)sender;
            String tekst = boks.Text;
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length != 0) //dla calego tekstu
            {

                string buff = "";

                for (int i = 0; i < tb.Text.Length; i++)
                {
                    if (i < 16)
                    {
                        if (tb.Text[i].Equals('1') || tb.Text[i].Equals('0'))
                        {
                            buff += tb.Text[i]; //tylko jesli wpiszemy 0 lub 1
                        }
                    }
                }

                tb.Text = buff; //dodajemy do wyswietlania
            }
        }
        private void execute()
        {
            
            
            
            
            while(k<instruction.Count)
            {
                String inst = instruction[k];
                Stack<Boolean> data = new Stack<bool>();
                int target=0;
                
                switch (inst)
                {
                    
                    case "mov":
                        if (param2[paramcounter].Equals("AX")) target = AL;
                        if (param2[paramcounter].Equals("BX")) target = BL;
                        if (param2[paramcounter].Equals("CX")) target = CL;
                        if (param2[paramcounter].Equals("DX")) target = DL;
                        paramcounter++;
                        data = changetoBIN(param1[paramcounter]);
                        if (data.Count > 8) OF = true;
                        for(int i =target; i < target + 8; i++)
                        {
                            try
                            {
                              rejestr[i]=data.Pop();
                            }
                            catch(Exception ex) { rejestr[i]= false; }
                        }
                        if (OF)
                        {
                            for (int i = target+8; i < target + 16; i++)
                            {
                                try
                                {
                                    rejestr[i] = data.Pop();
                                }
                                catch (Exception ex) { rejestr[i] = false; }
                            }
                        }
                        
                        break;
                    case "add":
                        CF = false;
                        if (!param1[paramcounter].Equals("AX")&&!param1[paramcounter].Equals("BX") && !param1[paramcounter].Equals("CX") && !param1[paramcounter].Equals("DX"))//wprowadzono stala
                        {
                            data = changetoBIN(param1[paramcounter]);
                            Stack<bool> newdata = new Stack<bool>();
                            while(data.Count>0) { newdata.Push(data.Pop()); } //odwrocenie stosu
                            for (int i = 15; i >= 0; i--)
                            {
                                bool dataElement = newdata.Pop();
                                bool buf;
                                if (rejestr[i] == true && dataElement) { buf = CF; CF = true; } //w obu danych jest 1 i przypisujemy wartosc przeniesienia oraz ustawiamy kolejne przeniesienie
                                else if (CF && ((rejestr[i] == true || dataElement))) { buf = false; CF = true; }//jest dodatnie przeniesienie z poprzedniego i choc jedna dana jest 1
                                else if (CF) { buf = CF; CF = false; }
                                else { buf = (rejestr[i] == true || dataElement); CF = false; }//przeniesienia nie ma a dane nie sa na raz 1
                                rejestr[i] = buf;
                            }
                        }
                        else
                        {


                            if (param1[paramcounter].Equals("AX")) target = AL;
                            if (param1[paramcounter].Equals("BX")) target = BL;
                            if (param1[paramcounter].Equals("CX")) target = CL;
                            if (param1[paramcounter].Equals("DX")) target = DL;
                            for (int i = 15; i >= 0; i--)
                            {
                                bool buf;
                                if (rejestr[i] == true && rejestr[target + i] == true) { buf = CF; CF = true; } //w obu danych jest 1 i przypisujemy wartosc przeniesienia oraz ustawiamy kolejne przeniesienie
                                else if (CF && ((rejestr[i] == true || rejestr[target + i] == true))) { buf = false; CF = true; }//jest dodatnie przeniesienie z poprzedniego i choc jedna dana jest 1
                                else if (CF) { buf = CF; CF = false; }
                                else { buf = (rejestr[i] == true || rejestr[target + i] == true); CF = false; }//przeniesienia nie ma a dane nie sa na raz 1
                                rejestr[i] = buf;
                            }
                        }
                        break;
                    case "sub":
                        CF = false;
                        if (!param1[paramcounter].Equals("AX") && !param1[paramcounter].Equals("BX") && !param1[paramcounter].Equals("CX") && !param1[paramcounter].Equals("DX"))
                        {
                            data = changetoBIN(param1[paramcounter]);
                            Stack<bool> newdata = new Stack<bool>();
                            while (data.Count > 0) { newdata.Push(data.Pop()); } //odwrocenie stosu

                            for (int i = 15; i >= 0; i--)
                            {
                                bool dataElement = newdata.Pop();
                                bool buf = false;
                                if (!dataElement)
                                {
                                    if (!CF)
                                    {
                                        buf = rejestr[i];
                                    }
                                    else
                                    {
                                        if (rejestr[i]) { buf = false; CF = false; }
                                        else { buf = false; }
                                    }
                                }
                                else
                                {
                                    if (rejestr[i]) { buf = false; }
                                    else { CF = true; buf = false; }
                                }
                                
                                rejestr[i] = buf;
                            }
                        }
                        else
                        {
                            if (param1[paramcounter].Equals("AX")) target = AL;
                            if (param1[paramcounter].Equals("BX")) target = BL;
                            if (param1[paramcounter].Equals("CX")) target = CL;
                            if (param1[paramcounter].Equals("DX")) target = DL;
                           for (int i = 15; i >= 0; i--)
                            {
                                bool dataElement = rejestr[i + target];
                                bool buf = false;
                                if (!dataElement)
                                {
                                    if (!CF)
                                    {
                                        buf = rejestr[i];
                                    }
                                    else
                                    {
                                        if (rejestr[i]) { buf = false; CF = false; }
                                        else { buf = false; }
                                    }
                                }
                                else
                                {
                                    if (rejestr[i]) { buf = false; }
                                    else { CF = true; buf = false; }
                                }
                                
                                rejestr[i] = buf;
                            }
                        }
                        
                        break;
                    default:
                        for(int i=0; i < 10; i++)
                        {
                            if (interuptvector[i].Equals(inst))
                            {
                                interrupt(i);
                            }
                        }
                        

                        break;
                }
                counter++;
                dispayFlag();
                displaydata();
                k++;
                if (TF) break;
               
            }
            if (!TF) //jak nie ma pracy krokowej to sam sie resetuje
            {
                k = 0;
                counter = 0;
                paramcounter = 0;
                instruction.Clear();
                param1.Clear();
                param2.Clear();
            }
        }
       public Stack<Boolean> changetoBIN(string s)
        {
            Stack<Boolean> buf = new Stack<bool>();
            int counter = 0;
            for(int i=s.Length; i>0;i--)
            {
                counter++;
                if (s[i-1] == '1')
                    buf.Push(true);
                else buf.Push(false);
            }
            while (counter < 16)
            {
                buf.Push(false);
                counter++;
            }
            return buf;
        }
        public String[] display(int start)
        {
            String[] s = new string[2];
            int n = 0;
            for (int i = start; i < start+16; i++)
            {
                if (i > start+ 7) n = 1;
                if (rejestr[i]) s[n] += "1";
                else s[n] += "0";
            }
            return s ;
        }

        public void displaydata()
        {
            String[] textbuf = new string[2];
            textbuf = display(0);
            try
            {
                axh.Text = textbuf[0];
                axl.Text = textbuf[1];
                textbuf = display(16);
                bxh.Text = textbuf[0];
                bxl.Text = textbuf[1];
                textbuf = display(32);
                cxh.Text = textbuf[0];
                cxl.Text = textbuf[1];
                textbuf = display(48);
                dxh.Text = textbuf[0];
                dxl.Text = textbuf[1];
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(2000);
                TimeSpan ts = TimeSpan.FromMilliseconds(2000);
                if (!task.Wait(ts))
                {
                    
                    displaydata();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button2.PerformClick();
            if (checkBox1.Checked) { TF = true; button2.Enabled = true; k = 0; }
            else { TF = false; button2.Enabled = false; k = 0; }
        }

        private void button2_Click(object sender, EventArgs e) //resetuje processor
        {
            CF = false;
            OF = false;
            k=0;
            counter = 0;
            paramcounter = 0;
            instruction.Clear();
            param1.Clear();
            param2.Clear();
            for (int i = 0; i < 64; i++) { rejestr[i] = false; }
            dispayFlag();
            String[] textbuf = new string[2];
            textbuf = display(0);
            axh.Text = textbuf[0];
            axl.Text = textbuf[1];
            textbuf = display(16);
            bxh.Text = textbuf[0];
            bxl.Text = textbuf[1];
            textbuf = display(32);
            cxh.Text = textbuf[0];
            cxl.Text = textbuf[1];
            textbuf = display(48);
            dxh.Text = textbuf[0];
            dxl.Text = textbuf[1];

        }
        private void dispayFlag()
        {
            string s = "";
            if (CF) s += 1;
            else s += 0;
            if (OF) s += 1;
            else s += 0;
            if (TF) s += 1;
            else s += 0;
            label8.Text = s;
        }
    }
}
