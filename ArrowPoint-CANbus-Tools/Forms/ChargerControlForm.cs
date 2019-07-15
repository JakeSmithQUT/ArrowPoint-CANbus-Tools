﻿using ArrowPointCANBusTool.Canbus;
using ArrowPointCANBusTool.Services;
using ArrowPointCANBusTool.Model;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ArrowPointCANBusTool.Forms
{
    public partial class ChargerControlForm : Form
    {        
        private BatteryChargeService chargeService;
        private BatteryDischargeService dischargeService;
        private BatteryMonitoringService monitoringService;

        private Timer timer;

        public ChargerControlForm()
        {
            InitializeComponent();            

            chargeService = new BatteryChargeService();
            dischargeService = new BatteryDischargeService();
            monitoringService = new BatteryMonitoringService(chargeService, dischargeService, 5000);
            monitoringService.BatteryMonitorUpdateEventHandler += new BatteryMonitorUpdateEventHandler(MonitoringDataReceived);

            RequestedChargeCurrent.Maximum = decimal.Parse(maxSocketCurrent.SelectedItem.ToString());
        }

        private void StartCharge_Click(object sender, EventArgs e)
        {
            // This should never happen.  It is a safety just in case
            if (dischargeService.IsDischarging)
            {
                chargeService.StopCharge();
                return;
            }

            if (chargeService.IsCharging)
                chargeService.StopCharge();
            else
            {
                startDischarge.Enabled = false;

                if ((chargeService.BatteryState == CanReceivingNode.STATE_WARNING || chargeService.BatteryState == CanReceivingNode.STATE_ON || chargeService.BatteryState == CanReceivingNode.STATE_IDLE) &&
                    (chargeService.ChargerState == CanReceivingNode.STATE_WARNING || chargeService.ChargerState == CanReceivingNode.STATE_ON || chargeService.ChargerState == CanReceivingNode.STATE_IDLE))
                { 
                    chargeService.RequestedCurrent = float.Parse(RequestedChargeCurrent.Value.ToString());
                    chargeService.RequestedVoltage = float.Parse(RequestedChargeVoltage.Value.ToString());
                    chargeService.SupplyCurrentLimit = float.Parse(maxSocketCurrent.SelectedItem.ToString());
                    chargeService.ChargeToPercentage = float.Parse(chargeToPercentage.Value.ToString());
                    chargeService.StartCharge();
                }
                else
                    MessageBox.Show("Charger of battery is currently in an invalid state to start charging",
                     "Check Battery and Charger",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);

            }
                
            UpdateStartStopDetails();
        }

        private void StartDischarge_Click(object sender, EventArgs e)
        {

            // This should never happen.  It is a safety just in case
            if (chargeService.IsCharging)
            {
                dischargeService.StopDischarge();
                return;
            }

            if (dischargeService.IsDischarging)
            {
                dischargeService.StopDischarge();
                startDischarge.Text = "Start Discharge";
            }
            else
            {
                startCharge.Enabled = false;
                dischargeService.StartDischarge();
                startDischarge.Text = "Stop Discharge";
            }
        }


        private void ChargerControlForm_Load(object sender, EventArgs e)
        {
            ChargeChart.DataSource = monitoringService.ChargeDataSet;
            ChargeChart.DataBind();
            
            timer = new Timer
            {
                Interval = (100)
            };
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        private void ChargerControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            chargeService.ShutdownCharge();
            chargeService.BatteryService.ShutdownService();
        }

        private void UpdateStartStopDetails()
        {
            if (chargeService.IsCharging)
            {
                startDischarge.Enabled = false;
                startCharge.Text = "Stop Charge";
                maxSocketCurrent.Enabled = false;
            }
            else
            {
                ActualVoltageTxt.Text = "";
                ActualCurrentTxt.Text = "";

                startDischarge.Enabled = true;
                startCharge.Text = "Start Charge";
                maxSocketCurrent.Enabled = true;
            }

            if (dischargeService.IsDischarging)
            {
                startCharge.Enabled = false;
                startDischarge.Text = "Stop Discharge";
            } else
            {
                startCharge.Enabled = true;
                startDischarge.Text = "Start Discharge";
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {

            Battery battery = chargeService.BatteryService.BatteryData;

            SOCText.Text = (battery.SOCPercentage * 100).ToString() + "%";
            BatteryPackMaTxt.Text = battery.BatteryCurrent.ToString();
            BatteryPackMvTxt.Text = battery.BatteryVoltage.ToString();
            BatteryCellMinMvTxt.Text = battery.MinCellVoltage.ToString();
            BatteryCellMaxMvTxt.Text = battery.MaxCellVoltage.ToString();
            BatteryMinCTxt.Text = (battery.MinCellTemp / 10).ToString();
            BatteryMaxCTxt.Text = (battery.MaxCellTemp / 10).ToString();
            BatteryBalancePositiveTxt.Text = battery.BalanceVoltageThresholdRising.ToString();
            BatteryBalanceNegativeTxt.Text = battery.BalanceVoltageThresholdFalling.ToString();

            ActualVoltageTxt.Text = String.Format(string.Format("{0:0.00}", chargeService.ChargerVoltage));
            ActualCurrentTxt.Text = String.Format(string.Format("{0:0.00}", chargeService.ChargerCurrent)); 

            if (!chargeService.IsCommsOk) Comms_Ok.ForeColor = Color.Red; else Comms_Ok.ForeColor = Color.Green;
            if (!chargeService.IsACOk) AC_Ok.ForeColor = Color.Red; else AC_Ok.ForeColor = Color.Green;
            if (!chargeService.IsDCOk) DC_Ok.ForeColor = Color.Red; else DC_Ok.ForeColor = Color.Green;
            if (!chargeService.IsTempOk) Temp_Ok.ForeColor = Color.Red; else Temp_Ok.ForeColor = Color.Green;
            if (!chargeService.IsHardwareOk) HW_Ok.ForeColor = Color.Red; else HW_Ok.ForeColor = Color.Green;

            batteryStatusLabel.Text = "Battery - " + CanReceivingNode.GetStatusText(chargeService.BatteryState);
            batteryStatusLabel.ToolTipText = chargeService.BatteryStateMessage;
            batteryStatusLabel.BackColor = CanReceivingNode.GetStatusColour(chargeService.BatteryState);
            chargerStatusLabel.Text = "Charger - " + CanReceivingNode.GetStatusText(chargeService.ChargerState);
            chargerStatusLabel.ToolTipText = chargeService.ChargerStateMessage;
            chargerStatusLabel.BackColor = CanReceivingNode.GetStatusColour(chargeService.ChargerState);
            dischargerStripStatusLabel.Text = "Discharger - " + CanReceivingNode.GetStatusText(dischargeService.DischargerState);
            dischargerStripStatusLabel.BackColor = CanReceivingNode.GetStatusColour(dischargeService.DischargerState);
            chargerStatusLabel.ToolTipText = dischargeService.DischargerStateMessage;

            UpdateStartStopDetails();
        }

        private void MonitoringDataReceived(ChargeDataReceivedEventArgs e)
        {

            ChargeData chargeData = e.Message;

            if (ChargeChart.InvokeRequired)
            {
                ChargeChart.Invoke(new Action(() =>
                {
                    ChargeChart.DataSource = monitoringService.ChargeDataSet;
                    ChargeChart.DataBind();
                }
                ));
            }
        }


        private void RequestedChargeCurrent_ValueChanged(object sender, EventArgs e)
        {
            chargeService.RequestedCurrent = float.Parse(RequestedChargeCurrent.Value.ToString());
        }

        private void MaxSocketCurrent_SelectedIndexChanged(object sender, EventArgs e)
        {
            RequestedChargeCurrent.Maximum = decimal.Parse(maxSocketCurrent.SelectedItem.ToString());
        }

        private void ChargeToPercentage_ValueChanged(object sender, EventArgs e)
        {
            chargeService.ChargeToPercentage = float.Parse(chargeToPercentage.Value.ToString());
        }

        private void RequestedChargeVoltage_ValueChanged(object sender, EventArgs e)
        {
            chargeService.RequestedVoltage = float.Parse(RequestedChargeVoltage.Value.ToString());
        }

        private void ClearData_Click(object sender, EventArgs e)
        {
            monitoringService.ClearChargeData();
            ChargeChart.DataBind();
        }

        private void SaveData_Click(object sender, EventArgs e)
        {
            Stream ioStream;
            StreamWriter ioWriterStream;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                FileName = "BatteryLog-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((ioStream = saveFileDialog.OpenFile()) != null)
                {
                    ioWriterStream = new StreamWriter(ioStream);
                    monitoringService.SaveChargeData(ioWriterStream);                    
                }
            }            
        }


    }
}
