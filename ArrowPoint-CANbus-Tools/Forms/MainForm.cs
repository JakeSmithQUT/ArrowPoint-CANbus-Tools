﻿using ArrowPointCANBusTool.CanBus;
using ArrowPointCANBusTool.Forms;
using ArrowPointCANBusTool.Model;
using ArrowPointCANBusTool.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArrowPointCANBusTool
{
    public partial class FormMain : Form
    {
        private CanService udpService;        
        private CarData carData;

        public FormMain()
        {
            InitializeComponent();
        }

        private void RawDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceivePacketForm ReceivePacketForm = new ReceivePacketForm(this.udpService)
            {
                MdiParent = this
            };
            ReceivePacketForm.Show();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.udpService.Close();
            Application.Exit();
        }

        private void ConnectionSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.udpService = new CanService();
            udpService.RequestConnectionStatusChange += FormMain_RequestConnectionStatusChange;

            // Setup as initially not connected
            FormMain_RequestConnectionStatusChange(false);

            this.carData = new CarData(this.udpService);

            ShowSettingsForm();
        }

        private void FormMain_RequestConnectionStatusChange(bool connected)
        {
            if (connected)
            {
                connectedStatusLabel.Text = "Connected";
                connectedStatusLabel.BackColor = Color.DarkGreen;
            } else
            {
                connectedStatusLabel.Text = "Not Connected";
                connectedStatusLabel.BackColor = Color.Red;
            }
        }

        private void ShowSettingsForm()
        {
             foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(SettingsForm))
                {
                    form.Activate();
                    return;
                }
            }

            SettingsForm settingsForm = new SettingsForm(this.udpService)
            {
                MdiParent = this
            };
            settingsForm.Show();
        }

        private void SendPacketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendPacketForm endPacketForm = new SendPacketForm(this.udpService)
            {
                MdiParent = this
            };
            endPacketForm.Show();
        }

        private void DashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SendCanPacketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendPacketForm sendPacketForm = new SendPacketForm(this.udpService)
            {
                MdiParent = this
            };
            sendPacketForm.Show();
        }

        private void MotorControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MotorControllerSimulatorForm motorControllerSimulatorForm = new MotorControllerSimulatorForm(this.udpService)
            {
                MdiParent = this
            };
            motorControllerSimulatorForm.Show();
        }

        private void CanbusOverviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanbusDashboardForm canbusDashboardForm = new CanbusDashboardForm(this.carData)
            {
                MdiParent = this
            };
            canbusDashboardForm.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox
            {
                MdiParent = this
            };
            aboutBox.Show();
        }

        private void DriverControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DriverControllerSimulatorForm driverControllerSimulatorForm = new DriverControllerSimulatorForm(this.udpService)
            {
                MdiParent = this
            };
            driverControllerSimulatorForm.Show();
        }

        private void DataLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataLoggerForm dataLoggerForm = new DataLoggerForm(this.udpService)
            {
                MdiParent = this
            };
            dataLoggerForm.Show();
        }

        private void LogReplayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataLogReplayerForm dataLogReplayerForm = new DataLogReplayerForm(this.udpService)
            {
                MdiParent = this
            };
            dataLogReplayerForm.Show();
        }

        private void ConnectedStatusLabel_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void BatteryChargerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChargerControlForm chargerControlForm = new ChargerControlForm(this.udpService)
            {
                MdiParent = this
            };
            chargerControlForm.Show();
        }

        private void BatteryViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatteryViewerForm batteryViewerForm = new BatteryViewerForm(this.udpService)
            {
                MdiParent = this
            };
            batteryViewerForm.Show();
        }
    }
}
