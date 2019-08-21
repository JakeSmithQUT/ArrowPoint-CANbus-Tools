﻿using ArrowPointCANBusTool.Canbus;
using ArrowPointCANBusTool.Configuration;
using ArrowPointCANBusTool.Forms;
using ArrowPointCANBusTool.Model;
using ArrowPointCANBusTool.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArrowPointCANBusTool.Forms {
    public partial class MainFormPi : Form {

        private CarData carData;
        private NetworkDefinitionForm networkDefinitionForm;
        private BatteryService batteryService;
        private Timer timer;
        private int activeBMUId = 0;


        public MainFormPi() {
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

            // Setup BMU Data
            DataGridViewRow sysStatus = new DataGridViewRow();

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
        private void SendPacketToolStripMenuItem_Click(object sender, EventArgs e) {
            SendPacketForm endPacketForm = new SendPacketForm() {
                MdiParent = this
            };
            endPacketForm.Show();
        }

        private void SendCanPacketsToolStripMenuItem_Click(object sender, EventArgs e) {
            SendPacketForm sendPacketForm = new SendPacketForm() {
                MdiParent = this
            };
            sendPacketForm.Show();
        }

        private void MotorControllerToolStripMenuItem_Click(object sender, EventArgs e) {
            MotorControllerSimulatorForm motorControllerSimulatorForm = new MotorControllerSimulatorForm() {
                MdiParent = this
            };
            motorControllerSimulatorForm.Show();
        }

        private void CanbusOverviewToolStripMenuItem_Click(object sender, EventArgs e) {
            CanbusDashboardForm canbusDashboardForm = new CanbusDashboardForm(this.carData) {
                MdiParent = this
            };
            canbusDashboardForm.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox aboutBox = new AboutBox {
                MdiParent = this
            };
            aboutBox.Show();
        }

        private void DriverControllerToolStripMenuItem_Click(object sender, EventArgs e) {
            DriverControllerSimulatorForm driverControllerSimulatorForm = new DriverControllerSimulatorForm() {
                MdiParent = this
            };
            driverControllerSimulatorForm.Show();
        }

        private void DataLoggerToolStripMenuItem_Click(object sender, EventArgs e) {
            DataLoggerForm dataLoggerForm = new DataLoggerForm() {
                MdiParent = this
            };
            dataLoggerForm.Show();
        }

        private void LogReplayerToolStripMenuItem_Click(object sender, EventArgs e) {
            DataLogReplayerForm dataLogReplayerForm = new DataLogReplayerForm() {
                MdiParent = this
            };
            dataLogReplayerForm.Show();
        }

        /*
        private void ConnectedStatusLabel_Click(object sender, EventArgs e) {
            ShowConnectionForm();
        }
        */
        private void BatteryChargerToolStripMenuItem_Click(object sender, EventArgs e) {
            ChargerControlForm chargerControlForm = new ChargerControlForm() {
                MdiParent = this
            };
            chargerControlForm.Show();
        }

        private void BatteryViewerToolStripMenuItem_Click(object sender, EventArgs e) {
            BatteryViewerForm batteryViewerForm = new BatteryViewerForm() {
                MdiParent = this
            };
            batteryViewerForm.Show();
        }

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
                    TXTMinCellVTemp.Text= ((double)activeBMU.MinCellTemp / 10).ToString();
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
        }

        private void TestBTN_Click(object sender, EventArgs e) {
            DriverControllerSimulatorForm driverControllerSimulatorForm = new DriverControllerSimulatorForm() {
                MdiParent = this
            };
            driverControllerSimulatorForm.Show();
            Console.WriteLine("BRUH");
        }
    }
}
