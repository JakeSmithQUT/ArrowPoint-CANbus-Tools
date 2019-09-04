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


        public MainFormPi() {
            batteryService = BatteryService.Instance;
            InitializeComponent();
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
        }



        /*
        private void FormMain_RequestConnectionStatusChange(bool connected) {
            if (connected) {
                connectedStatusLabel.Text = "Connected";
                connectedStatusLabel.BackColor = Color.DarkGreen;
                MenuStrip.Items.Find("dashboardToolStripMenuItem", true)[0].Enabled = true;
                MenuStrip.Items.Find("monitoringToolStripMenuItem", true)[0].Enabled = true;
                MenuStrip.Items.Find("simulatorsToolStripMenuItem", true)[0].Enabled = true;
                MenuStrip.Items.Find("batteryToolStripMenuItem", true)[0].Enabled = true;
                MenuStrip.Items.Find("LoadConfigurationToolStripMenuItem", true)[0].Enabled = true;
                MenuStrip.Items.Find("SaveConfigurationToolStripMenuItem", true)[0].Enabled = true;
            } else {
                connectedStatusLabel.Text = "Not Connected";
                connectedStatusLabel.BackColor = Color.Red;
                MenuStrip.Items.Find("dashboardToolStripMenuItem", true)[0].Enabled = false;
                MenuStrip.Items.Find("monitoringToolStripMenuItem", true)[0].Enabled = false;
                MenuStrip.Items.Find("simulatorsToolStripMenuItem", true)[0].Enabled = false;
                MenuStrip.Items.Find("batteryToolStripMenuItem", true)[0].Enabled = false;
                MenuStrip.Items.Find("LoadConfigurationToolStripMenuItem", true)[0].Enabled = false;
                MenuStrip.Items.Find("SaveConfigurationToolStripMenuItem", true)[0].Enabled = false;
            }
        }
        */
        /*
        private void ShowConnectionForm() {
            foreach (Form form in Application.OpenForms) {
                if (form.GetType() == typeof(ConnectForm)) {
                    form.Activate();
                    return;
                }
            }

            ConnectForm settingsForm = new ConnectForm() {
                MdiParent = this
            };
            settingsForm.Show();
        }
        */


        /*
        private void ConnectDisconnectToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowConnectionForm();
        }
        */
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

            string samplePacket = "005472697469756d00be61fea90031010000050800080000000000000000";
            CanPacket canPacket = new CanPacket();
           // CanService.Instance.Connect("10.11.12.13",29536);//29536

            int sent = CanService.Instance.SendMessage(canPacket);

            if (CanService.Instance.IsConnected() == false) {
                connectionLBL.Text = "Not Connected";
            } else {
                connectionLBL.Text = "Connected";
            }

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
    }

}


