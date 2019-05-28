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
using System.IO;
using System.Windows;
using System.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace adcLab1
{
    public class ADC
    {
        static System.IO.Ports.SerialPort Serial;
        private static string[] tool_name =
             {
            "ADCv2.0",
            "Устройство один",
            "устройство два"
                //здесь включить имена новых устройств если таковые будут
        };
        private static string[] chanels_adcV2 =
            {
            "1",
            "2",
            "3",
            "4"
                //здесь включить имена новых устройств если таковые будут
        };
        private static string[] chanels_def =
           {
            "0"
            
                //здесь включить имена новых устройств если таковые будут
        };
        public static Array GetChanels(string count)
        {
            switch (count)
            { 
                case "ADCv2.0":
                return chanels_adcV2;
                
            default:
                return chanels_def;
            }
        }
        public static Array Getname()
        {
            return tool_name;
        }
        
        public static string Com(string name)
        {
            string fport="";
            if (SerialPort.GetPortNames().Length != 0)
            {
                int i = 0;
                foreach (string portName in SerialPort.GetPortNames())
                {
                    

                    if (Call(portName) == name && fport=="")
                    {
                        fport = portName;
                        
                    }
                    
                }
                
            }
            else
            {
                if(name!="")
                MessageBox.Show("Error opening/writing to serial port :: Устройство не было подключено Error!");
                try { Serial.Close(); } catch { }
                
                

            }
            return fport;
        }
        static string Call(string port_name)
        {
           try
            {
                Serial = new SerialPort(port_name, 38400, System.IO.Ports.Parity.None, 8, StopBits.One);
            
            Serial.Open();
                Thread.Sleep(10);

                Serial.WriteLine("0");
                Serial.ReadTimeout = 1000;
            }
           catch {
                
            }
            try
            {
                string ans = Serial.ReadLine();
                Serial.Close();
                ans = ans.Trim();
                return (ans);
            }
            catch
            {
                
                return ("0");
            }
        }
        bool state = true;
        public string name;
        public bool constate;
        private object threadLock1 = new object();
        public void ComState()// вот этот классс
        {

           
                while (state == true && constate == true)
                {
                    Thread.Sleep(10);
                    if (SerialPort.GetPortNames().Length != 0)
                    {
                        foreach (string portName in SerialPort.GetPortNames())
                        {
                            if (constate == false)
                                break;
                            if (name == portName)
                            {
                                state = true;
                                break;
                            }
                            else
                            {
                                state = false;

                            }

                        }

                    }

                    else
                        state = false;
                    
                        

                }
                ComStateChanged(state);
            
            
        }
        public event Action<bool> ComStateChanged;//нашел в гайде. вроде как создается событие

        public static bool saveExcel(int cont, List<int> d1, List<int> d2, List<int> d3, List<int> d4)
        {
            try
            {
                // Opening the Excel template...
                FileStream fs = new FileStream(@"examples\example.xlsx", FileMode.Open, FileAccess.Read);

                // Getting the complete workbook...
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Getting the worksheet by its name...
                ISheet sheet = templateWorkbook.GetSheet("Лист1");

                for(int i=0; i<cont-1;i++)
                {
                    IRow dataRow = sheet.CreateRow(i+1);
                    dataRow.CreateCell(0).SetCellValue(i+1);
                    dataRow.CreateCell(1).SetCellValue(d1[i]);
                    dataRow.CreateCell(2).SetCellValue(d2[i]);
                    dataRow.CreateCell(3).SetCellValue(d3[i]);
                    dataRow.CreateCell(4).SetCellValue(d4[i]);

                }

                
                

                // Setting the value 77 at row 5 column 1
                

                // Forcing formula recalculation...
                sheet.ForceFormulaRecalculation = true;

                string addres = DateTime.Now.ToString();
                addres = addres.Replace(".", "-");
                addres = addres.Replace(" ", "_");
                addres = addres.Replace(":", "-");

                addres = "показания"+addres;

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Excel|*.xlsx";
                dialog.FileName = addres;
                dialog.Title = "Сохранение показаний как Excel файла";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fw = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        templateWorkbook.Write(fw);
                    }
                    MessageBox.Show("Запись успешна");
                    return true;
                }
                else
                {
                    MessageBox.Show("Ошибка записи");
                    return false;
                }

                // Writing the workbook content to the FileStream...



            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка записи");
                return false;
            }
        }
        public static bool saveText(int cont, List<int> d1, List<int> d2, List<int> d3, List<int> d4)
        {
            try
            {
               
                FileInfo fi1 = new FileInfo(@"finalData.txt");

                //Create a file to write to.
                using (StreamWriter sw = fi1.CreateText())
                {

                    for (int i = 0; i < cont - 1; i++)
                    {
                        sw.WriteLine(d1[i].ToString());
                        sw.WriteLine(d2[i].ToString());
                        sw.WriteLine(d3[i].ToString());
                        sw.WriteLine(d4[i].ToString());


                    }


                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = @"Project1.exe";
                    proc.Start();
                    proc.WaitForExit();

                    string addres = DateTime.Now.ToString();
                    addres = addres.Replace(".", "-");
                    addres = addres.Replace(" ", "_");
                    addres = addres.Replace(":", "-");

                    addres = "показания" + addres;

                    SaveFileDialog dialog = new SaveFileDialog();

                    dialog.FileName = addres;
                    dialog.Title = "Сохранение показаний как WAV файла";

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DirectoryInfo di = Directory.CreateDirectory(dialog.FileName);
                        string directory = @"buffer\";
                        string directoryTmp = dialog.FileName;

                        DirectoryInfo destin = new DirectoryInfo(directoryTmp);
                        DirectoryInfo source = new DirectoryInfo(directory);
                        foreach (var item in source.GetFiles())
                        {
                            item.CopyTo(directoryTmp + @"\" + item.Name, true);
                        }
                    }
                }
                    MessageBox.Show("Запись успешна");
                    return true;
               

            }
            catch
            {
                MessageBox.Show("Ошибка записи");
                return false;
            }
        }



    }
}
