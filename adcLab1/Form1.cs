using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace adcLab1
{
    public partial class Form1 : Form
    {
        string[] tool_name = new string[10];
        string[] port_name = new string[10];
        private ADC _ADC;
        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;

        }
        System.IO.Ports.SerialPort Serial;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string Name in ADC.Getname())
            {
                comboBox1.Items.Add(Name);

            }
            comboBox1.SelectedIndex = 0;
            button1.Text = "Подключение";
            button2.Text = "Снять показания";
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            if (SerialPort.GetPortNames().Length == 0)
            {
                this.Refresh();
            }

        }



        bool port_bool = false;


        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Отключение")
            {
                if (saved == false)
                {
                    DialogResult dr = MessageBox.Show("Отключение преведет к потере несохраненных данных", "Отключиться?", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            Serial.Close();
                            button1.Text = "Подключение";
                            port_bool = false;
                            _ADC.constate = false;
                            button2.Enabled = false;
                            comboBox1.Enabled = true;
                            Thread.Sleep(0);
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox5.Text = "";
                            textBox6.Text = "";

                            comboBox2.Items.Remove(comboBox2.SelectedItem);
                            comboBox2.Items.Clear();
                            saved = true;
                            break;
                        case DialogResult.No:
                            break;

                    }

                }
                else
                {
                    Serial.Close();
                    button1.Text = "Подключение";
                    port_bool = false;
                    _ADC.constate = false;
                    button2.Enabled = false;
                    comboBox1.Enabled = true;
                    Thread.Sleep(0);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                    comboBox2.Items.Remove(comboBox2.SelectedItem);
                    comboBox2.Items.Clear();
                    saved = true;
                }
               

            }
            else
            {
                _ADC = new ADC();
                Thread thread1 = new Thread(_ADC.ComState);//создаю поток здесь
                thread1.Priority = ThreadPriority.Lowest;
                thread1.IsBackground = true;


                String name = ((string)comboBox1.SelectedItem);
                String com = ADC.Com(name);
                textBox1.Text = "";
                try
                {
                    if (com != "")
                    {

                        Serial = new SerialPort(com, 38400, System.IO.Ports.Parity.None, 8, StopBits.One);


                        _ADC.name = com;
                        _ADC.constate = true;
                        _ADC.ComStateChanged += ADC_ProgressChanged;//подписка на событие

                        thread1.Start();//запускаю поток
                        Thread.Sleep(10);

                        measNum = 0;
                        d1.Clear();
                        d2.Clear();
                        d3.Clear();
                        d4.Clear();
                        write = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Error opening/writing to serial p11ort :: Отсутствует доступное устройство Error!");
                    port_bool = false;
                    try
                    {
                        Serial.Close();
                    }
                    catch { }
                    button1.Text = "Подключение";
                }
                try
                {


                    if (com != "")
                    {

                        Serial.Open();
                        button1.Text = "Отключение";
                        port_bool = true;
                        //Serial.WriteLine("1");
                        button2.Enabled = true;
                        comboBox1.Enabled = false;
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        Serial.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);


                        foreach (string count in ADC.GetChanels(comboBox1.Text))
                        {
                            comboBox2.Items.Add(count);

                        }
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                        MessageBox.Show("Error opening/writing to serial p11ort :: Отсутствует доступное устройство Error!");



                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error opening/writing to11 serial port ::  устройство не обнаружено");


                }
            }


        }
        private void ADC_ProgressChanged(bool chanded)//происходит действие здесь
        {
            Action action = () =>
            {
                if (chanded == false)
                {
                    textBox1.Text = "";
                    button2.Text = "Снять показания";
                    button1.Text = "Подключение";
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Clear();
                    measNum = 0;
                    d1.Clear();
                    d2.Clear();
                    d3.Clear();
                    d4.Clear();
                    comboBox2.Items.Remove(comboBox2.SelectedItem);
                    comboBox2.Items.Clear();
                    comboBox2.Enabled = true;
                    button3.BackColor = default(Color);
                    
                    button3.Text = "Запись";
                    port_bool = false;
                    MessageBox.Show("Error opening/writing to serial port :: устройство было отключено. Несохраненные данные утеряны!!");
                    //Serial.DataReceived -= sp_DataReceived;
                    Serial.Close();
                    //Form1 frm1 = new Form1();
                    // frm1.Show();
                    // this.Hide;

                }
            };
            try
            {
                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            }
            catch { }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }



        private delegate void SetIntDeleg1();


        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {



            // this.BeginInvoke(new SetIntDeleg1(si_DataReceived1));
            si_DataReceived1();



        }

        int i = 1;
        int measNum = 0;
        List<int> d1 = new List<int>();
        List<int> d2 = new List<int>();
        List<int> d3 = new List<int>();
        List<int> d4 = new List<int>();
        int numTextbox;
        private object threadLock = new object();
        private void si_DataReceived1()
        {
            lock (threadLock)
            {
                try
                {
                    int count = Serial.ReadByte();
                    if (numTextbox == 5)
                        numTextbox = 0;
                    int step = 1;
                    int data = 0;
                    for (int i = 0; i < count; i++)
                    {
                        data += Serial.ReadByte() * step;
                        step *= 10;
                    }
                    reserv(data);
                }
                catch { }
            }

        }
        private void reserv(int data)
        {
            Action action = () =>
            {
                switch (numTextbox)
                {
                    case 0:
                        textBox1.Text = data.ToString();
                        if (write == true)
                        {
                            measNum++;
                            textBox6.Text = measNum.ToString();
                            d1.Add(data);
                        }

                        break;
                    case 1:
                        textBox2.Text = data.ToString();
                        if (write == true)
                            d2.Add(data);
                        break;
                    case 2:
                        textBox3.Text = data.ToString();
                        if (write == true)
                            d3.Add(data);
                        break;
                    case 3:
                        textBox4.Text = data.ToString();
                        if (write == true)
                            d4.Add(data);
                        break;
                    case 4:
                        textBox5.Text = (1000 / data).ToString();
                        if (write == true && measNum == 20000)
                        {
                            write = false;
                            button3.BackColor = default(Color);
                            button2.Enabled = true;
                            button3.Text = "Запись";
                        }
                        break;
                }
                numTextbox++;
            };
            try
            {
                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            }
            catch { }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Снять показания")
            {
                switch (comboBox2.Text)
                {
                    case "1":
                        Serial.WriteLine("1");
                        break;
                    case "2":
                        Serial.WriteLine("2");
                        break;
                    case "3":
                        Serial.WriteLine("3");
                        break;
                    case "4":
                        Serial.WriteLine("4");
                        break;
                }

                button3.Enabled = true;



                comboBox2.Enabled = false;
                button1.Enabled = false;


                textBox1.Text = " ";
                textBox2.Text = " ";
                textBox3.Text = " ";
                textBox4.Text = " ";
                textBox5.Text = " ";




                button2.Text = "Остановить";
            }
            else
            {
                try
                {
                    Serial.WriteLine("1");
                    Thread.Sleep(10);


                    button3.Enabled = false;
                    comboBox2.Enabled = true;
                    button1.Enabled = true;
                    button2.Text = "Снять показания";
                }
                catch { }


            }
        }
        bool write = false;
        bool saved = true;
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Запись")
            {
                if (measNum > 0)
                {
                    DialogResult dr = MessageBox.Show("Предудущие показания не были сохренены", "При нажатии кнопки Да несохраненные показания будут удалены", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            saved = false;
                            write = true;
                            button3.BackColor = Color.Green;
                            textBox6.Clear();
                            measNum = 0;
                            d1.Clear();
                            d2.Clear();
                            d3.Clear();
                            d4.Clear();
                            button2.Enabled = false;
                            button3.Text = "Остановить";
                           
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    saved = false;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    write = true;
                    button3.BackColor = Color.Green;
                    textBox6.Clear();
                    button2.Enabled = false;
                    button3.Text = "Остановить";
                }
            }
            else
            {
                write = false;
                button3.BackColor = default(Color);
                button2.Enabled = true;
                button3.Text = "Запись";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.PerformClick();


            button4.Enabled = false;
            bool status = ADC.saveExcel(measNum, d1, d2, d3, d4);
            if (status == true)
            {
               /* measNum = 0;
                d1.Clear();
                d2.Clear();
                d3.Clear();
                d4.Clear();
                textBox6.Text = "";*/
            }

            button4.Enabled = true;
            saved = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            button2.PerformClick();


            button4.Enabled = false;
            bool status = ADC.saveText(measNum, d1, d2, d3, d4);
            if (status == true)
            {
               /* measNum = 0;
                d1.Clear();
                d2.Clear();
                d3.Clear();
                d4.Clear();
                textBox6.Text = "";*/
            }

            button4.Enabled = true;
            saved = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           if (button1.Text == "Отключение")
            {
                MessageBox.Show("Отключитесь от устройства");
                e.Cancel = true;
            }
            
        }
    }
   

   

}
