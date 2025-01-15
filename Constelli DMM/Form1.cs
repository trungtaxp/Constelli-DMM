using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Constelli_DMM.Helpers;
using Ivi.Visa; //This .NET assembly is installed with your NI VISA installation
using IviVisaExtended; //Custom extention functions for Ivi.Visa - all are defined in the IviVisaExtended Project

namespace Constelli_DMM
{
    public partial class Form1 : Form
    {
        private const int MAX_LINES = 100;
        private string unit_label = "";
        private string _ReceiveMonitor = "";
        string[] functions = { "DCV", "ACV", "DCI", "ACI", "2W Ω", "4W Ω", "Freq", "Temp", "Period", "Cap", "Cont", "Diode", "Ratio" };
        private double result = 0;
        private double expect_result = 0;
        private double error = 0;
        int funcX = 150, funcY = 320;
        int i = 0;
        Button[] functionButton = new Button[13];
        private XmlDocument itemDoc = new XmlDocument();

        string _BTC_Connection = FunctionForSystem._BTC_Connection;
        string _SFU_Connection = FunctionForSystem._SFU_Connection;
        string _SFC_Connection = FunctionForSystem._SFC_Connection;
        public static IMessageBasedSession _BTC = null;
        public static IMessageBasedSession _SFU = null;
        public static IMessageBasedSession _SFC = null;
        private double Start_Freq = 100;
        private double Stop_Freq = 800;
        public static List<double> _List_freq = new List<double>();
        public static List<double> _List_OffsetPower = new List<double>();
        private int Counting = 100;

        public Form1()
        {
            InitializeComponent();
            Ini_UI();
            BTC_Connect();
        }

        private void BTC_Connect()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                //-----------------------------------------------------------
                // Initialization:
                //-----------------------------------------------------------
                // Adjust the VISA Resource string to fit your instrument


                _BTC = GlobalResourceManager.Open(_BTC_Connection) as IMessageBasedSession;
                _BTC.TimeoutMilliseconds = 60000; //Timeout for VISA Read Operations
                _BTC.Clear();
                var idnResponse = _BTC.QueryString("*IDN?");
                int index = idnResponse.IndexOf("/");
                string Serialnumber = idnResponse.Substring(index + 1, 6);
                MessageBox.Show("Connected! \n" + idnResponse + "\n " + Serialnumber);
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void FetchMeasurement()
        {

            if (timer1.Enabled == false) { timer1.Enabled = true; fetchButton.Text = "Measuring"; }
            else { timer1.Enabled = false; fetchButton.Text = "Measure"; }
            
        }
        private void Read()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {              
                string _result = _BTC.QueryString(":READ?");
                result = Convert.ToDouble(_result);
                measurementLabel.Text = result.ToString() + " " + unit_label;
                //_BTC.Write("");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }

        #region Set Measurement type
        private void SetVDC()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"VOLT:DC\"\r");
                _BTC.Write(":SENS:VOLT:RANG:AUTO ON");
                _BTC.Write(":SENS:VOLT:INP AUTO");
                _BTC.Write(":SENS:VOLT:NPLC 10");
                _BTC.Write(":SENS:VOLT:AZER ON");
                _BTC.Write(":SENS:VOLT:AVER:TCON REP");
                _BTC.Write(":SENS:VOLT:AVER:COUN 1");
                _BTC.Write(":SENS:VOLT:AVER ON");
                
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetDCI()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"CURR:DC\"\r");
                _BTC.Write(":SENS:CURR:RANG:AUTO ON");
                //_BTC.Write(":SENS:CURR:INP AUTO");
                _BTC.Write(":SENS:CURR:NPLC 10");
                _BTC.Write(":SENS:CURR:AZER ON");
                _BTC.Write(":SENS:CURR:AVER:TCON REP");
                _BTC.Write(":SENS:CURR:AVER:COUN 1");
                _BTC.Write(":SENS:CURR:AVER ON");

            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void Set2Wire()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"RES\"\r");
                _BTC.Write(":SENS:FRES:RANG:AUTO ON");
                _BTC.Write(":SENS:FRES:OCOM ON");
                _BTC.Write(":SENS:FRES:AZER ON");
                _BTC.Write(":SENS:FRES:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetFreq()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"FREQ:VOLT\"\r");
                //_BTC.Write(":SENS:FREQ:RANG:AUTO ON");
                //_BTC.Write(":SENS:FREQ:OCOM ON");
                //_BTC.Write(":SENS:FREQ:AZER ON");
                //_BTC.Write(":SENS:FREQ:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetTemp()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"TEMP\"\r");
                //_BTC.Write(":SENS:TEMP:RANG:AUTO ON");
                //_BTC.Write(":SENS:TEMP:OCOM ON");
                _BTC.Write(":SENS:TEMP:AZER ON");
                _BTC.Write(":SENS:TEMP:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetACV()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"VOLT:AC\"\r");
                _BTC.Write(":SENS:VOLT:RANG:AUTO ON");
                //_BTC.Write(":SENS:VOLT:OCOM ON");
                _BTC.Write(":SENS:VOLT:AZER ON");
                _BTC.Write(":SENS:VOLT:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetACI()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"CURR:AC\"\r");
                _BTC.Write(":SENS:CURR:RANG:AUTO ON");
                //_BTC.Write(":SENS:CURR:OCOM ON");
                _BTC.Write(":SENS:CURR:AZER ON");
                _BTC.Write(":SENS:CURR:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void Set4Wire()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"FRES\"\r");
                _BTC.Write(":SENS:FRES:RANG:AUTO ON");
                _BTC.Write(":SENS:FRES:OCOM ON");
                _BTC.Write(":SENS:FRES:AZER ON");
                _BTC.Write(":SENS:FRES:NPLC 1");               
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetPeriod()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"PER:VOLT\"\r");
                //_BTC.Write(":SENS:PER:RANG:AUTO ON");
                //_BTC.Write(":SENS:PER:OCOM ON");
                //_BTC.Write(":SENS:PER:AZER ON");
                //_BTC.Write(":SENS:PER:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetCap()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"CAP\"\r");
                _BTC.Write(":SENS:CAP:RANG:AUTO ON");
                //_BTC.Write(":SENS:CAP:OCOM ON");
                //_BTC.Write(":SENS:CAP:AZER ON");
                //_BTC.Write(":SENS:CAP:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetCont()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"CONT\"\r");
                //_BTC.Write(":SENS:CONT:RANG:AUTO ON");
                //_BTC.Write(":SENS:CONT:OCOM ON");
                //_BTC.Write(":SENS:CONT:AZER ON");
                //_BTC.Write(":SENS:CONT:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetDiode()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"DIOD\"\r");
                //_BTC.Write(":SENS:DIOD:RANG:AUTO ON");
                //_BTC.Write(":SENS:DIOD:OCOM ON");
                //_BTC.Write(":SENS:DIOD:AZER ON");
                //_BTC.Write(":SENS:DIOD:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        private void SetRatio()
        {
            try //separate try-catch for specan initialization prevents accessing uninitialized object
            {
                _BTC.Write("*RST");
                _BTC.Write(":SENS:FUNC \"VOLT:DC:RAT\"\r");
                //_BTC.Write(":SENS:VOLT:RANG:AUTO ON");
                //_BTC.Write(":SENS:VOLT:OCOM ON");
                //_BTC.Write(":SENS:VOLT:AZER ON");
                //_BTC.Write(":SENS:VOLT:NPLC 1");
            }
            catch (Ivi.Visa.NativeVisaException ex)
            {
                MessageBox.Show("Error initializing the specan session:\n{0}", ex.Message);
            }
        }
        #endregion 
        private void timer1_Tick(object sender, EventArgs e)
        {
            Read();
        }
        private void Switch_Meas(object sender, EventArgs e)
        {
            timer1.Enabled = false;            
            string mess = sender.ToString();
            mess = mess.Substring(35).Trim();
            Switch_Meas_for_run(mess);
        }
        private void Switch_Meas_for_run(string measuremnt)
        {
            foreach (var funbutton in functionButton) { funbutton.Enabled = false; funbutton.BackColor = Color.White; }
            switch (measuremnt)//functions = { "DCV", "ACV", "DCI", "ACI", "2W Ω", "4W Ω", "Freq", "Temp", "Period", "Cap", "Cont", "Diode", "Ratio"}
            {
                case "DCV":
                    unit_label = "V";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.DCV;
                    SetVDC();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[0].BackColor = Color.Yellow;
                    break;
                case "ACV":
                    unit_label = "V";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.ACV;
                    SetACV();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[1].BackColor = Color.Yellow;
                    break;
                case "DCI":
                    unit_label = "A";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.DCI;
                    SetDCI();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[2].BackColor = Color.Yellow;
                    break;
                case "ACI":
                    unit_label = "A";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.ACI;
                    SetACI();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[3].BackColor = Color.Yellow;
                    break;
                case "2W Ω":
                    unit_label = "Ω";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources._2WOhms;
                    Set2Wire();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[4].BackColor = Color.Yellow;
                    break;
                case "4W Ω":
                    unit_label = "Ω";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources._4WOhms;
                    Set4Wire();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[5].BackColor = Color.Yellow;
                    break;
                case "Freq":
                    unit_label = "Hz";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.FreqPer;
                    SetFreq();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[6].BackColor = Color.Yellow;
                    break;
                case "Temp":
                    unit_label = "℃";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources._2WTemp;
                    SetTemp();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[7].BackColor = Color.Yellow;
                    break;
                case "Period":
                    unit_label = "s";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.FreqPer;
                    SetPeriod();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[8].BackColor = Color.Yellow;
                    break;
                case "Cap":
                    unit_label = "F";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.Capacitance;
                    SetCap();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[9].BackColor = Color.Yellow;
                    break;
                case "Cont":
                    unit_label = "Ω";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.Cont;
                    SetCont();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[10].BackColor = Color.Yellow;
                    break;
                case "Diode":
                    unit_label = "V";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.Diode;
                    SetDiode();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[11].BackColor = Color.Yellow;
                    break;
                case "Ratio":
                    unit_label = "V/V";
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.Ratio;
                    SetRatio();
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    functionButton[12].BackColor = Color.Yellow;
                    break;
                default:
                    foreach (var funbutton in functionButton) funbutton.Enabled = true;
                    this.pictureBox1.Image = global::Constelli_DMM.Properties.Resources.DCV;
                    break;
            }
        }
        private void LogMeasurement(string measurementType, double result, double expected, double error, string status)
        {
            string logMessage = $"Measurement Type: {measurementType}, Result: {result}, Expected: {expected}, Error: {error}, Status: {status}";
            Console.WriteLine(logMessage);

            string csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoadConfiguration/Output/measurement_log.csv");
            string directoryPath = Path.GetDirectoryName(csvFilePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string csvLine = $"{measurementType},{result},{expected},{error},{status}";

            int retryCount = 3;
            while (retryCount > 0)
            {
                try
                {
                    using (var fileStream = new FileStream(csvFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                    using (var writer = new StreamWriter(fileStream))
                    {
                        if (fileStream.Length == 0)
                        {
                            writer.WriteLine("Measurement Type,Result,Expected,Error,Status");
                        }
                        writer.WriteLine(csvLine);
                    }
                    break; // Exit loop if successful
                }
                catch (IOException)
                {
                    retryCount--;
                    System.Threading.Thread.Sleep(1000); // Wait before retrying
                }
            }
        }

        private void btnMeasureByPost_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoadConfiguration/Input");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string _measurement = "";

                try
                {
                    itemDoc.Load(openFileDialog1.FileName);
                    foreach (XmlNode specialNode in itemDoc.DocumentElement.ChildNodes)
                    {
                        foreach (XmlNode specialNode2 in specialNode.ChildNodes)
                        {
                            if (specialNode2.Name == "SET")
                            {
                                _measurement = specialNode2.InnerText.Trim();
                                Switch_Meas_for_run(_measurement);
                                System.Threading.Thread.Sleep(1000);
                                Read();
                                SetReceiveMonitor("Result: " + measurementLabel.Text);
                            }
                            else if (specialNode2.Name == "TC_StatusMsg")
                            {
                                SetReceiveMonitor(specialNode2.InnerText.Trim());
                                System.Threading.Thread.Sleep(1000);
                            }
                            else if (specialNode2.Name == "TC_ExpectResult")
                            {
                                SetReceiveMonitor("Expect result: " + specialNode2.InnerText.Trim() + " " + _measurement);
                                expect_result = Convert.ToDouble(specialNode2.InnerText.Trim());
                                System.Threading.Thread.Sleep(1000);
                            }
                            else if (specialNode2.Name == "TC_Error")
                            {
                                SetReceiveMonitor("Error: " + specialNode2.InnerText.Trim() + "%");
                                error = Convert.ToDouble(specialNode2.InnerText.Trim());
                                System.Threading.Thread.Sleep(1000);
                            }

                        }
                        string status = (expect_result * (1 - error * 0.01) < result && expect_result * (1 + error * 0.01) > result) ? "Pass" : "Fail";
                        SetReceiveMonitor(status);
                        LogMeasurement(_measurement, result, expect_result, error, status);
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        private void SetReceiveMonitor(string data)
        {
            if (data.IndexOf("\n") != -1)
            {
                data = data.Substring(0, data.IndexOf("\n"));
            }
            //when lines count over 100, delete the oldest one.
            if (ReceiveMntText.Lines.Length >= MAX_LINES)
            {
                ReceiveMntText.Text = ReceiveMntText.Text.Remove(0, ReceiveMntText.Lines[0].Length + 2);
            }
            int selPos = ReceiveMntText.Text.Length;
            if (ReceiveMntText.Text != "")
            {
                if (selPos < 32700)
                {
                    //AppendText() has a limit of 32,766.
                    ReceiveMntText.AppendText("\r\n");
                }
                else
                {
                    ReceiveMntText.Text += "\r\n";
                }
            }
            //scroll the window to button.
            if (selPos < 32700)
            {
                //AppendText() has a limit of 32,766.
                ReceiveMntText.AppendText(data);
            }
            else
            {
                ReceiveMntText.Text += data;
            }

            //scroll the window to bottom.
            ReceiveMntText.Select(selPos + 2, 0);
            ReceiveMntText.ScrollToCaret();
        }

        private void fetchButton_Click(object sender, EventArgs e)
        {
            FetchMeasurement();
        }

        private void smnuConstelli_Click(object sender, EventArgs e)
        {
            
        }

        private void smnuAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.StartPosition = FormStartPosition.CenterScreen;
            aboutForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Ini_UI()
        {

            /*string[] leftButtons = { "HOME", "MENU", "APPS", "HELP" };
            int leftButtonY = 90;

            foreach (var text in leftButtons)
            {
                Button leftButton = new Button
                {
                    Text = text,
                    Size = new Size(80, 30),
                    Location = new Point(20, leftButtonY),
                    FlatStyle = FlatStyle.Flat
                };
                this.Controls.Add(leftButton);
                leftButtonY += 50;
            }

            // USB Icon Placeholder
            Label usbLabel = new Label
            {
                Text = "USB",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(40, 30),
                Location = new Point(20, leftButtonY + 10),
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            this.Controls.Add(usbLabel);*/

            // Display Panel
            Panel displayPanel = new Panel
            {
                Location = new Point(150, 65),
                Size = new Size(700, 250),
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(displayPanel);

            measurementLabel = new Label
            {
                Text = "-000.0000mV",
                ForeColor = Color.LimeGreen,
                BackColor = Color.Black,
                Font = new Font("Consolas", 36, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            displayPanel.Controls.Add(measurementLabel);

            /*Label rangeLabel = new Label
            {
                Text = "Range: 100mV",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(550, 20)
            };
            displayPanel.Controls.Add(rangeLabel);*/

            // Function Buttons (e.g., DCV, ACV, etc.)
            
            foreach (var func in functions)
            {
                functionButton[i] = new Button
                {
                    Text = func,
                    Size = new Size(80, 40),
                    Location = new Point(funcX, funcY),
                    BackColor = func == "DCV" ? Color.Yellow : Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                functionButton[i].Click += Switch_Meas;
                this.Controls.Add(functionButton[i]);
                funcX += 90;
                if (funcX > 700)
                {
                    funcX = 150;
                    funcY += 50;
                }
                i++;
            }


            /*// Button to fetch measurement value
            Button fetchButton = new Button
            {
                Text = "Fetch Measurement",
                Location = new Point(100, 600),
                Size = new Size(200, 40),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            fetchButton.Click += FetchMeasurement;
            this.Controls.Add(fetchButton);*/

        }
    }
}
