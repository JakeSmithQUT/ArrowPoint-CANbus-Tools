using ArrowPointCANBusTool.Canbus;
using ArrowPointCANBusTool.Configuration;
using ArrowPointCANBusTool.Forms;
using ArrowPointCANBusTool.Model;
using ArrowPointCANBusTool.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrowPointCANBusTool.Forms {
    public partial class MainFormPi : Form {

        private CarData carData;
        private NetworkDefinitionForm networkDefinitionForm;
        private BatteryService batteryService;
        private Timer timer;
        private int activeBMUId = 0;
        private String ipAddress = "10.16.16.78";
        private int port = 29536;
        private bool ConnectSwitch = true;
        private Timer timerCharge;
        private bool preCharge = false;


        public MainFormPi() {
            batteryService = BatteryService.Instance;
            InitializeComponent();

            // Charger init
            BatteryMonitoringService.Instance.BatteryMonitorUpdateEventHandler += new BatteryMonitorUpdateEventHandler(MonitoringDataReceived);
            maxSocketCurrent.SelectedIndex = maxSocketCurrent.FindStringExact("10");
            ChargerComboBox.SelectedIndex = 0;
            RequestedChargeCurrent.Maximum = 10;

        }

        private void MainFormPi_Load(object sender, EventArgs e) {
            //CanService.Instance.RequestConnectionStatusChange += FormMain_RequestConnectionStatusChange;

            // Setup as initially not connected
            //FormMain_RequestConnectionStatusChange(false);

            this.carData = new CarData();

            //ShowConnectionForm();

            if (UpdateService.Instance.IsUpdateAvailable)
                new NewReleaseForm(UpdateService.Instance).ShowDialog();

            // BATTERY VIEWER FORM LOAD

            /*
            // Setup Menu
            if (batteryService.BatteryData.GetBMUs() != null &&
                batteryService.BatteryData.GetBMUs().Count == 1)
                BMU2.Visible = false;
            */

            // Setup BMU Data
            DataGridViewRow sysStatus = new DataGridViewRow();
            sysStatus.CreateCells(BMUdataGridView);
            sysStatus.Cells[0].Value = "Sys Status";
            BMUdataGridView.Rows.Add(sysStatus);

            DataGridViewRow secondHeader = new DataGridViewRow();
            secondHeader.CreateCells(BMUdataGridView);
            secondHeader.Cells[7].Value = "Fan Speed (rpm)";
            secondHeader.Cells[8].Value = "SOC/BAL (Ah)";
            secondHeader.Cells[9].Value = "SOC/BAL (%)";
            secondHeader.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            BMUdataGridView.Rows.Add(secondHeader);

            DataGridViewRow prechgStatus = new DataGridViewRow();
            prechgStatus.CreateCells(BMUdataGridView);
            prechgStatus.Cells[0].Value = "Prechg Status";
            BMUdataGridView.Rows.Add(prechgStatus);

            DataGridViewRow flags = new DataGridViewRow();
            flags.CreateCells(BMUdataGridView);
            flags.Cells[0].Value = "Flags";
            BMUdataGridView.Rows.Add(flags);

            activeBMUId = 0;
            // BMUmenuStrip.Items[activeBMUId].BackColor = Color.LightBlue;

            DataGridViewRow twelveVStatus = new DataGridViewRow();
            twelveVStatus.CreateCells(TwelveVoltDataGridView);
            TwelveVoltDataGridView.Rows.Add(twelveVStatus);

            BMUdataGridView.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            CMUdataGridView.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            TwelveVoltDataGridView.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);

            // Connection Form

            ipAddressTb.Text = this.ipAddress;
            portTb.Text = this.port.ToString();

            ipAddressTb.Enabled = false;
            portTb.Enabled = false;
            InterfaceCheckedListBox.Enabled = false;

            Boolean isConnected = CanService.Instance.IsConnected();

            this.connectBtn.Enabled = !isConnected;
            this.disconnectBtn.Enabled = isConnected;
            this.radioButton1.Enabled = !isConnected;
            this.radioButton2.Enabled = !isConnected;

            foreach (KeyValuePair<string, string> entry in CanService.Instance.AvailableInterfaces) {
                IpDetails ipDetails = new IpDetails() {
                    IpAddress = entry.Key,
                    IpDescription = entry.Value
                };

                if (CanService.Instance.SelectedInterfaces != null && !CanService.Instance.SelectedInterfaces.Contains(ipDetails.IpAddress))
                    InterfaceCheckedListBox.Items.Add(ipDetails, false);
                else
                    InterfaceCheckedListBox.Items.Add(ipDetails, true);
            }

            // CHARGER 
            BatteryChargeService.Instance.SetCharger(ElconService.Instance);

            timerCharge = new Timer {
                Interval = (100)
            };
            timerCharge.Tick += new EventHandler(BatteryTimerTick);
            timerCharge.Start();

            //ChargeChart.DataSource = BatteryMonitoringService.Instance.ChargeDataSet;
            //ChargeChart.DataBind();


        }


        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            CanService.Instance.Disconnect();
            Application.Exit();
        }

        private void LoadConfigurationToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog;

            openFileDialog = new OpenFileDialog {
                RestoreDirectory = true,
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 2
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                if (networkDefinitionForm == null)
                    networkDefinitionForm = new NetworkDefinitionForm() {
                        MdiParent = this
                        // Dock = DockStyle.Left
                    };
                networkDefinitionForm.LoadConfig(openFileDialog.FileName);
                networkDefinitionForm.Show();
                //networkDefinitionForm.SendToBack();
            }
        }

        private void SaveConfigurationToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                RestoreDirectory = true,
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 2
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                ConfigService.Instance.SaveConfig(saveFileDialog.FileName);
            }
        }

        private void FormMain_SizeChanged(object sender, EventArgs e) {
            this.Refresh();
        }

        private void BatteryControllerToolStripMenuItem_Click(object sender, EventArgs e) {
            BatteryControllerForm batteryControlForm = new BatteryControllerForm() {
                MdiParent = this
            };
            batteryControlForm.Show();
        }

        private void Timer1_Tick(object sender, EventArgs e) {
            
            
            try {
                // Setup the BMU Panel

                BMU activeBMU = batteryService.BatteryData.GetBMU(activeBMUId);
                
                
                if (activeBMU != null) {
                    // Sys status
                    if (BatteryChargeService.Instance.IsFullyCharged) { TXTBatStatus.Text = "Full Charge"; }
                    else if (BatteryChargeService.Instance.IsCharging) { TXTBatStatus.Text = "Charging"; }
                    else { TXTBatStatus.Text = "Depleting"; }
                    if (CanService.Instance.IsConnected()) { ConnectedLBL.Text = "True"; ConnectedLBL.ForeColor = Color.Green; } else { ConnectedLBL.Text = "False"; ConnectedLBL.ForeColor = Color.Red; }
                    batteryPRO.Value = Convert.ToInt32(activeBMU.SOCPercentage);
                    TXTBatPercentage.Text = activeBMU.SOCPercentage.ToString() + "%";
                    TXTMinCellV.Text = activeBMU.MinCellVoltage.ToString();
                    TXTMinCellV.Text = activeBMU.MaxCellVoltage.ToString();
                    TXTMinCellVTemp.Text = ((double)activeBMU.MinCellTemp / 10).ToString();
                    TXTMaxCellVTemp.Text = ((double)activeBMU.MaxCellTemp / 10).ToString();
                    TXTBatTemp.Text = ((((double)activeBMU.MaxCellTemp / 10) - ((double)activeBMU.MinCellTemp / 10)) / 2).ToString();
                    //sysStatus.Cells[5].Value = activeBMU.BatteryVoltage;
                    //sysStatus.Cells[6].Value = activeBMU.BatteryCurrent;
                    //sysStatus.Cells[7].Value = activeBMU.BalanceVoltageThresholdRising;
                    //sysStatus.Cells[8].Value = activeBMU.BalanceVoltageThresholdFalling;
                    //sysStatus.Cells[9].Value = activeBMU.CMUCount;

                    /*

                    // preChgStatus
                    DataGridViewRow prechgStatus = BMUdataGridView.Rows[2];
                    prechgStatus.Cells[1].Value = activeBMU.PrechargeStateText;
                    prechgStatus.Cells[7].Value = activeBMU.FanSpeed0RPM;
                    prechgStatus.Cells[8].Value = Math.Round(activeBMU.SOCAh, 2);
                    prechgStatus.Cells[9].Value = activeBMU.SOCPercentage * 100;

                    // Flags
                    DataGridViewRow flags = BMUdataGridView.Rows[3];
                    flags.Cells[1].Value = activeBMU.StateMessage;
                    flags.Cells[7].Value = activeBMU.FanSpeed1RPM;

                    CMU[] cmus = batteryService.BatteryData.GetBMU(activeBMUId).GetCMUs();

                    for (int cmuIndex = 0; cmuIndex < cmus.Length; cmuIndex++) {
                        if (cmus[cmuIndex].SerialNumber != null && cmus[cmuIndex].SerialNumber != 0) {
                            if (CMUdataGridView.Rows.Count <= cmuIndex)
                                CMUdataGridView.Rows.Add(new DataGridViewRow());

                            DataGridViewRow cmuRow = CMUdataGridView.Rows[cmuIndex];
                            cmuRow.Cells[0].Value = "CMU " + (cmuIndex + 1);
                            cmuRow.Cells[1].Value = cmus[cmuIndex].SerialNumber;
                            cmuRow.Cells[2].Value = cmus[cmuIndex].PCBTemp;
                            cmuRow.Cells[3].Value = cmus[cmuIndex].CellTemp;
                            cmuRow.Cells[4].Value = cmus[cmuIndex].Cell0mV;
                            cmuRow.Cells[5].Value = cmus[cmuIndex].Cell1mV;
                            cmuRow.Cells[6].Value = cmus[cmuIndex].Cell2mV;
                            cmuRow.Cells[7].Value = cmus[cmuIndex].Cell3mV;
                            cmuRow.Cells[8].Value = cmus[cmuIndex].Cell4mV;
                            cmuRow.Cells[9].Value = cmus[cmuIndex].Cell5mV;
                            cmuRow.Cells[10].Value = cmus[cmuIndex].Cell6mV;
                            cmuRow.Cells[11].Value = cmus[cmuIndex].Cell7mV;

                            for (int cellIndex = 0; cellIndex <= 7; cellIndex++)
                                FormatCell(cmuRow.Cells[cellIndex + 4], cmuIndex, cellIndex);

                        }
                    }

                    BatteryTwelveVolt batteryTwelveVolt = batteryService.BatteryData.BatteryTwelveVolt;

                    // Sys status

                    double cellTemp = 0;
                    if (batteryTwelveVolt.CellTemp != null) cellTemp = (double)batteryTwelveVolt.CellTemp;

                    DataGridViewRow TwelveVStatus = TwelveVoltDataGridView.Rows[0];
                    TwelveVStatus.Cells[0].Value = batteryTwelveVolt.SerialNumber;
                    TwelveVStatus.Cells[1].Value = (double)cellTemp / 10;
                    TwelveVStatus.Cells[2].Value = batteryTwelveVolt.Cell0mV;
                    TwelveVStatus.Cells[3].Value = batteryTwelveVolt.Cell1mV;
                    TwelveVStatus.Cells[4].Value = batteryTwelveVolt.Cell2mV;
                    TwelveVStatus.Cells[5].Value = batteryTwelveVolt.Cell3mV;
                    TwelveVStatus.Cells[6].Value = batteryTwelveVolt.Net12vCurrent;
                    TwelveVStatus.Cells[7].Value = batteryTwelveVolt.HVDc2DcCurrent;
                    TwelveVStatus.Cells[8].Value = batteryTwelveVolt.StatusFlags;
                    TwelveVStatus.Cells[9].Value = batteryTwelveVolt.StatusEvents;
                    */


                    BatteryTimerTick(sender, e);
                }
            } catch {

                Console.WriteLine("Fail");

            }

            try {
                // Setup the BMU Panel

                BMU activeBMU = batteryService.BatteryData.GetBMU(activeBMUId);

                if (activeBMU != null) {
                    // Sys status
                    DataGridViewRow sysStatus = BMUdataGridView.Rows[0];
                    sysStatus.Cells[1].Value = activeBMU.MinCellVoltage;
                    sysStatus.Cells[2].Value = activeBMU.MaxCellVoltage;
                    sysStatus.Cells[3].Value = (double)activeBMU.MinCellTemp / 10;
                    sysStatus.Cells[4].Value = (double)activeBMU.MaxCellTemp / 10;
                    sysStatus.Cells[5].Value = activeBMU.BatteryVoltage;
                    sysStatus.Cells[6].Value = activeBMU.BatteryCurrent;
                    sysStatus.Cells[7].Value = activeBMU.BalanceVoltageThresholdRising;
                    sysStatus.Cells[8].Value = activeBMU.BalanceVoltageThresholdFalling;
                    sysStatus.Cells[9].Value = activeBMU.CMUCount;

                    // preChgStatus
                    DataGridViewRow prechgStatus = BMUdataGridView.Rows[2];
                    prechgStatus.Cells[1].Value = activeBMU.PrechargeStateText;
                    prechgStatus.Cells[7].Value = activeBMU.FanSpeed0RPM;
                    prechgStatus.Cells[8].Value = Math.Round(activeBMU.SOCAh, 2);
                    prechgStatus.Cells[9].Value = activeBMU.SOCPercentage * 100;

                    // Flags
                    DataGridViewRow flags = BMUdataGridView.Rows[3];
                    flags.Cells[1].Value = activeBMU.StateMessage;
                    flags.Cells[7].Value = activeBMU.FanSpeed1RPM;

                    CMU[] cmus = batteryService.BatteryData.GetBMU(activeBMUId).GetCMUs();

                    for (int cmuIndex = 0; cmuIndex < cmus.Length; cmuIndex++) {
                        if (cmus[cmuIndex].SerialNumber != null && cmus[cmuIndex].SerialNumber != 0) {
                            if (CMUdataGridView.Rows.Count <= cmuIndex)
                                CMUdataGridView.Rows.Add(new DataGridViewRow());

                            DataGridViewRow cmuRow = CMUdataGridView.Rows[cmuIndex];
                            cmuRow.Cells[0].Value = "CMU " + (cmuIndex + 1);
                            cmuRow.Cells[1].Value = cmus[cmuIndex].SerialNumber;
                            cmuRow.Cells[2].Value = cmus[cmuIndex].PCBTemp;
                            cmuRow.Cells[3].Value = cmus[cmuIndex].CellTemp;
                            cmuRow.Cells[4].Value = cmus[cmuIndex].Cell0mV;
                            cmuRow.Cells[5].Value = cmus[cmuIndex].Cell1mV;
                            cmuRow.Cells[6].Value = cmus[cmuIndex].Cell2mV;
                            cmuRow.Cells[7].Value = cmus[cmuIndex].Cell3mV;
                            cmuRow.Cells[8].Value = cmus[cmuIndex].Cell4mV;
                            cmuRow.Cells[9].Value = cmus[cmuIndex].Cell5mV;
                            cmuRow.Cells[10].Value = cmus[cmuIndex].Cell6mV;
                            cmuRow.Cells[11].Value = cmus[cmuIndex].Cell7mV;

                            for (int cellIndex = 0; cellIndex <= 7; cellIndex++)
                                FormatCell(cmuRow.Cells[cellIndex + 4], cmuIndex, cellIndex);

                        }
                    }

                    BatteryTwelveVolt batteryTwelveVolt = batteryService.BatteryData.BatteryTwelveVolt;

                    // Sys status

                    double cellTemp = 0;
                    if (batteryTwelveVolt.CellTemp != null) cellTemp = (double)batteryTwelveVolt.CellTemp;

                    DataGridViewRow TwelveVStatus = TwelveVoltDataGridView.Rows[0];
                    TwelveVStatus.Cells[0].Value = batteryTwelveVolt.SerialNumber;
                    TwelveVStatus.Cells[1].Value = (double)cellTemp / 10;
                    TwelveVStatus.Cells[2].Value = batteryTwelveVolt.Cell0mV;
                    TwelveVStatus.Cells[3].Value = batteryTwelveVolt.Cell1mV;
                    TwelveVStatus.Cells[4].Value = batteryTwelveVolt.Cell2mV;
                    TwelveVStatus.Cells[5].Value = batteryTwelveVolt.Cell3mV;
                    TwelveVStatus.Cells[6].Value = batteryTwelveVolt.Net12vCurrent;
                    TwelveVStatus.Cells[7].Value = batteryTwelveVolt.HVDc2DcCurrent;
                    TwelveVStatus.Cells[8].Value = batteryTwelveVolt.StatusFlags;
                    TwelveVStatus.Cells[9].Value = batteryTwelveVolt.StatusEvents;

                }
            } catch {
            }
        }

        private void TestBTN_Click(object sender, EventArgs e) {
            /*
            DriverControllerSimulatorForm driverControllerSimulatorForm = new DriverControllerSimulatorForm() {
                MdiParent = this
            };
            driverControllerSimulatorForm.Show();
            Console.WriteLine("BRUH");
            */

            Console.WriteLine("Sim active.");
        }


        private void BMUdataGridView_SelectionChanged(object sender, EventArgs e) {
            BMUdataGridView.ClearSelection();
        }

        private void CMUdataGridView_SelectionChanged(object sender, EventArgs e) {
            CMUdataGridView.ClearSelection();
        }

        private void TwelveVoltDataGridView_SelectionChanged(object sender, EventArgs e) {
            TwelveVoltDataGridView.ClearSelection();
        }


        private void FormatCell(DataGridViewCell cell, int cmuNo, int cellNo) {

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle {
                Font = new Font(CMUdataGridView.Font, FontStyle.Regular),
                BackColor = Color.White
            };

            cell.Style = defaultStyle;

            DataGridViewCellStyle boldStyle = new DataGridViewCellStyle {
                Font = new Font(CMUdataGridView.Font, FontStyle.Bold)
            };

            DataGridViewCellStyle blueBackground = new DataGridViewCellStyle {
                BackColor = Color.LightBlue
            };

            DataGridViewCellStyle italicStyle = new DataGridViewCellStyle {
                Font = new Font(CMUdataGridView.Font, FontStyle.Italic)
            };

            if (cell != null && cell.Value != null) {
                string cellTxtValue = cell.Value.ToString();

                if (int.TryParse(cellTxtValue, out int cellValue)) {
                    BMU activeBMU = batteryService.BatteryData.GetBMU(activeBMUId);

                    if (cellValue > activeBMU.BalanceVoltageThresholdFalling) cell.Style.ApplyStyle(blueBackground);
                    if (cellValue > activeBMU.BalanceVoltageThresholdRising) cell.Style.ApplyStyle(italicStyle);
                }
            }

            if (cmuNo + 1 == batteryService.BatteryData.GetBMU(0).CMUNumberMinCell && cellNo == batteryService.BatteryData.GetBMU(0).CellNumberMinCell)
                cell.Style.Font = new Font(cell.Style.Font, FontStyle.Bold);

            if (cmuNo + 1 == batteryService.BatteryData.GetBMU(0).CMUNumberMaxCell && cellNo == batteryService.BatteryData.GetBMU(0).CellNumberMaxCell)
                cell.Style.Font = new Font(cell.Style.Font, FontStyle.Bold);

        }

        private void Button1_Click(object sender, EventArgs e) {

            //Boolean ipAddressParsed = IPAddress.TryParse("10.16.16.78", out IPAddress notUsedIpAddress);
            //Boolean portParsed = Int32.TryParse("2953", out this.port);

            //Boolean canServiceConnected = CanService.Instance.ConnectOverSocketCan(this.ipAddress, this.port);

            string samplePacket = CANID.Text+NumOfBits.Text+Bit1.Text+Bit2.Text+Bit3.Text+Bit4.Text+Bit5.Text+Bit6.Text+Bit7.Text+Bit8.Text;
            CanPacket sendTest = new CanPacket(Convert.ToUInt16(CANID.Text, 10));
            //sendTest.CanId = Convert.ToUInt16(CANID.Text, 16);
            sendTest.SetByte(0, Convert.ToByte(Bit1.Text));
            sendTest.SetByte(1, Convert.ToByte(Bit2.Text));
            sendTest.SetByte(2, Convert.ToByte(Bit3.Text));
            sendTest.SetByte(3, Convert.ToByte(Bit4.Text));
            sendTest.SetByte(4, Convert.ToByte(Bit5.Text));
            sendTest.SetByte(5, Convert.ToByte(Bit6.Text));
            sendTest.SetByte(6, Convert.ToByte(Bit7.Text));
            sendTest.SetByte(7, Convert.ToByte(Bit8.Text));


            //string samplePacket = "12381122334455667788";
            //CanPacket canPacket = new CanPacket(sampleTest);
            //Debug.Print()
           // CanService.Instance.Connect("10.11.12.13",29536);//29536

            int sent = CanService.Instance.SendMessage(sendTest);

            /* CONNECTION LBL
            if (C == false) {
                connectionLBL.Text = "Not Connected";
            } else {
                connectionLBL.Text = "Connected";
            }
            */

            var lines = CanService.Instance.AvailableInterfaces.Select(kvp => kvp.Key.ToString() + " -> " + kvp.Value.ToString());
            Console.WriteLine(lines);
            //Console.WriteLine(CanService.Instance.AvailableInterfaces);
            //Consle.WriteLine();
            Console.WriteLine("sent");
            
        }

        private class IpDetails {
            public string IpAddress { get; set; }
            public string IpDescription { get; set; }
            public override string ToString() {
                return IpDescription;
            }
        }


        private void RadioButton1_CheckedChanged(object sender, EventArgs e) {
            if (radioButton1.Checked) {
                ipAddressTb.Enabled = true;
                portTb.Enabled = true;
                InterfaceCheckedListBox.Enabled = true;
            }
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e) {
            ipAddressTb.Enabled = false;
            portTb.Enabled = false;
            InterfaceCheckedListBox.Enabled = false;
        }

        private void ConnectBtn_Click_1(object sender, EventArgs e) {
            try {
                Boolean ipAddressParsed = IPAddress.TryParse(this.ipAddressTb.Text, out IPAddress notUsedIpAddress);
                Boolean portParsed = Int32.TryParse(this.portTb.Text, out this.port);

                List<string> selectedInterfaces = new List<String>();

                foreach (IpDetails ipDetails in InterfaceCheckedListBox.CheckedItems) {
                    selectedInterfaces.Add(ipDetails.IpAddress);
                }

                CanService.Instance.SelectedInterfaces = selectedInterfaces;

                Boolean canServiceConnected = CanService.Instance.ConnectOverSocketCan(this.ipAddress, this.port);
                Console.WriteLine(canServiceConnected);
                Console.WriteLine(CanService.Instance.IsConnected());
                if (ipAddressParsed && portParsed && canServiceConnected) {
                    this.connectBtn.Enabled = false;
                    this.disconnectBtn.Enabled = true;

                    this.ipAddressTb.Enabled = false;
                    this.portTb.Enabled = false;
                    this.radioButton1.Checked = false;
                    this.radioButton2.Checked = false;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = false;

                } else if (!ipAddressParsed) {
                    MessageBox.Show("Failed to parse IP address.");
                } else if (!portParsed) {
                    MessageBox.Show("Failed to parse port value. Port must be an integer");
                } else {
                    MessageBox.Show("Failed to connect, this is likely caused by another tool already listening on the CanBus Port.");
                }
            } catch { }
        }

        private void DisconnectBtn_Click_1(object sender, EventArgs e) {
            CanService.Instance.Disconnect();

            this.connectBtn.Enabled = true;
            this.disconnectBtn.Enabled = false;

            this.ipAddressTb.Enabled = false;
            this.portTb.Enabled = false;
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = true;
            this.radioButton1.Enabled = true;
            this.radioButton2.Enabled = true;
        }
        
        private void BtnFrame1_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(CANID.Text) + 1;
            if (tempInt > 999) { tempInt -= 1000; }
            CANID.Text = tempInt.ToString();
        }

        private void BtnFrame10_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(CANID.Text) + 10;
            if (tempInt > 999) { tempInt -= 1000; }
            CANID.Text = tempInt.ToString();
        }

        private void BtnFrame1Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(CANID.Text) - 1;
            if (tempInt < 0) { tempInt += 1000; }
            CANID.Text = tempInt.ToString();
        }

        private void BtnFrame10Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(CANID.Text) - 10;
            if (tempInt < 0) { tempInt += 1000;}
            CANID.Text = tempInt.ToString();
        }

        private void BTNb110_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit1.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit1.Text = tempInt.ToString();
        }

        private void BTNb210_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit2.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit2.Text = tempInt.ToString();
        }

        private void BTNb310_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit3.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit3.Text = tempInt.ToString();
        }

        private void BTNb410_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit4.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit4.Text = tempInt.ToString();
        }

        private void BTNb610_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit6.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit6.Text = tempInt.ToString();
        }

        private void BTNb710_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit7.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit7.Text = tempInt.ToString();
        }

        private void BTNb810_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit8.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit8.Text = tempInt.ToString();
        }

        private void BTNb11_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit1.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit1.Text = tempInt.ToString();
        }

        private void BTNb21_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit2.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit2.Text = tempInt.ToString();
        }

        private void BTNb31_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit3.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit3.Text = tempInt.ToString();
        }

        private void BTNb41_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit4.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit4.Text = tempInt.ToString();
        }

        private void BTNb51_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit5.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit5.Text = tempInt.ToString();
        }

        private void BTNb61_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit6.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit6.Text = tempInt.ToString();
        }

        private void BTNb71_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit7.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit7.Text = tempInt.ToString();
        }

        private void BTNb81_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit8.Text) + 1;
            if (tempInt > 99) { tempInt -= 100; }
            Bit8.Text = tempInt.ToString();
        }

        private void BTNb11Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit1.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit1.Text = tempInt.ToString();
        }

        private void BTNb21Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit2.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit2.Text = tempInt.ToString();
        }

        private void BTNb31Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit3.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit3.Text = tempInt.ToString();
        }

        private void BTNb41Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit4.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit4.Text = tempInt.ToString();
        }

        private void BTNb51Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit5.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit5.Text = tempInt.ToString();
        }

        private void BTNb61Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit6.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit6.Text = tempInt.ToString();
        }

        private void BTNb71Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit7.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit7.Text = tempInt.ToString();
        }

        private void BTNb81Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit8.Text) - 1;
            if (tempInt < 0) { tempInt += 100; }
            Bit8.Text = tempInt.ToString();
        }

        private void BTNb110Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit1.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit1.Text = tempInt.ToString();
        }

        private void BTNb210Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit2.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit2.Text = tempInt.ToString();
        }

        private void BTNb310Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit3.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit3.Text = tempInt.ToString();
        }

        private void BTNb410Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit4.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit4.Text = tempInt.ToString();
        }

        private void BTNb510Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit5.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit5.Text = tempInt.ToString();
        }

        private void BTNb610Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit6.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit6.Text = tempInt.ToString();
        }

        private void BTNb710Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit7.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit7.Text = tempInt.ToString();
        }

        private void BTNb810Neg_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit8.Text) - 10;
            if (tempInt < 0) { tempInt += 100; }
            Bit8.Text = tempInt.ToString();
        }

        private void BTNb510_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(Bit5.Text) + 10;
            if (tempInt > 99) { tempInt -= 100; }
            Bit5.Text = tempInt.ToString();
        }

        private void Label22_Click(object sender, EventArgs e) {

        }

        private void ChargeChart_Click(object sender, EventArgs e) {

        }


        // CHARGER TAB
        private async void StartCharge_ClickAsync(object sender, EventArgs e) {
            startCharge.Enabled = false;

            // This should never happen.  It is a safety just in case
            if (BatteryDischargeService.Instance.IsDischarging) {
                await BatteryChargeService.Instance.StopCharge();
                preCharge = false;
                return;
            }

            if (BatteryChargeService.Instance.IsCharging)
                await BatteryChargeService.Instance.StopCharge();
            else {
                //startDischarge.Enabled = false;
                preCharge = true;

                if ((BatteryChargeService.Instance.BatteryState == CanReceivingNode.STATE_WARNING || BatteryChargeService.Instance.BatteryState == CanReceivingNode.STATE_ON || BatteryChargeService.Instance.BatteryState == CanReceivingNode.STATE_IDLE) &&
                    (BatteryChargeService.Instance.ChargerState == CanReceivingNode.STATE_WARNING || BatteryChargeService.Instance.ChargerState == CanReceivingNode.STATE_ON || BatteryChargeService.Instance.ChargerState == CanReceivingNode.STATE_IDLE)) {
                    BatteryChargeService.Instance.RequestedCurrent = float.Parse(RequestedChargeCurrent.Value.ToString());
                    BatteryChargeService.Instance.RequestedVoltage = float.Parse(RequestedChargeVoltage.Value.ToString());
                    BatteryChargeService.Instance.SupplyCurrentLimit = float.Parse(maxSocketCurrent.SelectedItem.ToString());
                    BatteryChargeService.Instance.ChargeToPercentage = float.Parse(chargeToPercentage.Value.ToString());
                    await BatteryChargeService.Instance.StartCharge();
                } else
                    MessageBox.Show("Charger of battery is currently in an invalid state to start charging",
                     "Check Battery and Charger",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);

            }

            UpdateStartStopDetails();
        }


        private async void ChargerControlForm_FormClosingAsync(object sender, FormClosingEventArgs e) {
            timer.Stop();
            await BatteryChargeService.Instance.StopCharge();
            BatteryChargeService.Instance.BatteryService.ShutdownService();
        }

        private void UpdateStartStopDetails() {
            if (BatteryChargeService.Instance.IsCharging || preCharge) {
                //startDischarge.Enabled = false;
                startCharge.Text = "Stop Charge";
                maxSocketCurrent.Enabled = false;
            } else {
                ActualVoltageTxt.Text = "";
                ActualCurrentTxt.Text = "";

                //startDischarge.Enabled = true;
                startCharge.Text = "Start Charge";
                maxSocketCurrent.Enabled = true;
            }

            if (BatteryDischargeService.Instance.IsDischarging) {
                startCharge.Enabled = false;
                //startDischarge.Text = "Stop Discharge";
            } else {
                startCharge.Enabled = true;
                //startDischarge.Text = "Start Discharge";
            }
        }

        private void BatteryTimerTick(object sender, EventArgs e) {

            Battery battery = BatteryChargeService.Instance.BatteryService.BatteryData;

            SOCText.Text = (battery.SOCPercentage * 100).ToString() + "%";
            BatteryPackMaTxt.Text = battery.BatteryCurrent.ToString();
            BatteryPackMvTxt.Text = battery.BatteryVoltage.ToString();
            BatteryCellMinMvTxt.Text = battery.MinCellVoltage.ToString();
            BatteryCellMaxMvTxt.Text = battery.MaxCellVoltage.ToString();
            BatteryMinCTxt.Text = (battery.MinCellTemp / 10).ToString();
            BatteryMaxCTxt.Text = (battery.MaxCellTemp / 10).ToString();
            BatteryBalancePositiveTxt.Text = battery.BalanceVoltageThresholdRising.ToString();
            BatteryBalanceNegativeTxt.Text = battery.BalanceVoltageThresholdFalling.ToString();

            ActualVoltageTxt.Text = String.Format(string.Format("{0:0.00}", BatteryChargeService.Instance.ChargerActualVoltage));
            ActualCurrentTxt.Text = String.Format(string.Format("{0:0.00}", BatteryChargeService.Instance.ChargerActualCurrent));

            if (!BatteryChargeService.Instance.IsCommsOk) Comms_Ok.ForeColor = Color.Red; else Comms_Ok.ForeColor = Color.Green;
            if (!BatteryChargeService.Instance.IsACOk) AC_Ok.ForeColor = Color.Red; else AC_Ok.ForeColor = Color.Green;
            if (!BatteryChargeService.Instance.IsDCOk) DC_Ok.ForeColor = Color.Red; else DC_Ok.ForeColor = Color.Green;
            if (!BatteryChargeService.Instance.IsTempOk) { Temp_Ok.BackColor = Color.Red; BTNTempWarn.ForeColor = Color.Red; } else { Temp_Ok.ForeColor = Color.Green; BTNTempWarn.BackColor= Color.Transparent; }
            if (!BatteryChargeService.Instance.IsHardwareOk) { BTNTempWarn.BackColor = Color.Red;  HW_Ok.ForeColor = Color.Red; } else { HW_Ok.ForeColor = Color.Green; BTNTempWarn.BackColor = Color.Transparent; }

            batteryStatusLabel.Text = "Battery - " + CanReceivingNode.GetStatusText(BatteryChargeService.Instance.BatteryState);
            batteryStatusLabel.ToolTipText = BatteryChargeService.Instance.BatteryStateMessage;
            batteryStatusLabel.BackColor = CanReceivingNode.GetStatusColour(BatteryChargeService.Instance.BatteryState);
            chargerStatusLabel.Text = "Charger - " + CanReceivingNode.GetStatusText(BatteryChargeService.Instance.ChargerState);
            chargerStatusLabel.ToolTipText = BatteryChargeService.Instance.ChargerStateMessage;
            chargerStatusLabel.BackColor = CanReceivingNode.GetStatusColour(BatteryChargeService.Instance.ChargerState);
            dischargerStripStatusLabel.Text = "Discharger - " + CanReceivingNode.GetStatusText(BatteryDischargeService.Instance.DischargerState);
            dischargerStripStatusLabel.BackColor = CanReceivingNode.GetStatusColour(BatteryDischargeService.Instance.DischargerState);
            chargerStatusLabel.ToolTipText = BatteryDischargeService.Instance.DischargerStateMessage;

            if (BatteryChargeService.Instance.IsCharging) preCharge = false;

            UpdateStartStopDetails();
        }

        private void MonitoringDataReceived(ChargeDataReceivedEventArgs e) {

            ChargeData chargeData = e.Message;

          
        }

        private void ChargerLayoutPanel_Paint(object sender, PaintEventArgs e) {

        }

        private void ChargeVoltUp_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(RequestedChargeVoltage.Text) + 1;
            RequestedChargeVoltage.Text = tempInt.ToString();
        }

        private void ChargeVoltDown_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(RequestedChargeVoltage.Text) - 1;
            RequestedChargeVoltage.Text = tempInt.ToString();
        }

        private void ChargeCurrentUp_Click(object sender, EventArgs e) {
            Double tempInt = Convert.ToDouble(RequestedChargeCurrent.Text);
            tempInt += 1;
            RequestedChargeCurrent.Text = tempInt.ToString();
        }

        private void ChargeCurrentDown_Click(object sender, EventArgs e) {
            Double tempInt = Convert.ToDouble(RequestedChargeCurrent.Text);
            tempInt -= 1;
            RequestedChargeCurrent.Text = tempInt.ToString();
        }

        private void ChargeToUp_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(chargeToPercentage.Text) + 5;
            if (tempInt > 100) { tempInt = 100; }
            chargeToPercentage.Text = tempInt.ToString();

        }

        private void ChargeToDown_Click(object sender, EventArgs e) {
            int tempInt = Int32.Parse(chargeToPercentage.Text) - 5;
            if (tempInt < 0) { tempInt = 0; }
            chargeToPercentage.Text = tempInt.ToString();
        }

        private void IpAddressTb_Click(object sender, EventArgs e) {
            ConnectSwitch = true;
            button13.Enabled = true;
        }

        private void PortTb_TextChanged(object sender, EventArgs e) {
            ConnectSwitch = false;
            button13.Enabled = false;
        }

        private void Button11_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 1;
                } else {
                    portTb.Text += 1;
                }
            }
        }

        private void Button10_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 2;
                } else {
                    portTb.Text += 2;
                }
            }
        }

        private void Button9_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 3;
                } else {
                    portTb.Text += 3;
                }
            }
        }

        private void Button8_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 4;
                } else {
                    portTb.Text += 4;
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 5;
                } else {
                    portTb.Text += 5;
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 6;
                } else {
                    portTb.Text += 6;
                }
            }
        }

        private void Ip1_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 7;
                } else {
                    portTb.Text += 7;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 8;
                } else {
                    portTb.Text += 8;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 9;
                } else {
                    portTb.Text += 9;
                }
            }
        }

        private void Button13_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += '.';
                } else {
                    portTb.Text += '.';
                }
            }
        }

        private void Button12_Click(object sender, EventArgs e) {
            try {
                if (InterfaceCheckedListBox.Enabled) {
                    if (ConnectSwitch) {
                        ipAddressTb.Text = ipAddressTb.Text.Remove(ipAddressTb.Text.Length - 1, 1);
                    } else {
                        portTb.Text = portTb.Text.Remove(portTb.Text.Length - 1, 1);
                    }
                }
            } catch { }
        }

        private void Button14_Click(object sender, EventArgs e) {
            if (InterfaceCheckedListBox.Enabled) {
                if (ConnectSwitch) {
                    ipAddressTb.Text += 0;
                } else {
                    portTb.Text += 0;
                }
            }
        }
    }

}


