﻿using ArrowPointCANBusTool.Charger;
using ArrowPointCANBusTool.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrowPointCANBusTool.Forms
{
    public partial class ChargerControlForm : Form
    {

        private UdpService udpService;
        private ChargeService chargeService;


        public ChargerControlForm(UdpService udpService)
        {
            InitializeComponent();
            this.udpService = udpService;

            this.chargeService = new ChargeService(udpService);

        }

        private void startCharge_Click(object sender, EventArgs e)
        {
            if (chargeService.IsCharging())
            {
                chargeService.StopCharge();
                startCharge.Text = "Start Charge";
                chargeBar.Visible = false;
            }
            else
            {
                chargeService.RequestedCurrent = float.Parse(maxChargeCurrent.Value.ToString());
                chargeService.RequestedVoltage = float.Parse(maxChargeVoltage.Value.ToString());
                chargeService.SupplyCurrentLimit = float.Parse(maxSocketCurrent.SelectedItem.ToString());
                chargeService.StartCharge();
                startCharge.Text = "Stop Charge";
                chargeBar.Visible = true;
            }
        }

        private void ChargerControlForm_Load(object sender, EventArgs e)
        {
            // Move this logic to the receiver
            Timer timer = new Timer
            {
                Interval = (100)
            };
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        
        private void TimerTick(object sender, EventArgs e)
        {       
            BatterySOC.Value = (int)(chargeService.Battery.GetBMU(0).SOCPercentage * 100);
            BatteryPackMaTxt.Text = chargeService.Battery.GetBMU(0).BatteryCurrent.ToString();
            BatteryPackMvTxt.Text = chargeService.Battery.GetBMU(0).BatteryVoltage.ToString();
            BatteryCellMinMvTxt.Text = chargeService.Battery.GetBMU(0).MinCellVoltage.ToString();
            BatteryCellMaxMvTxt.Text = chargeService.Battery.GetBMU(0).MaxCellVoltage.ToString();
            BatteryMinCTxt.Text = chargeService.Battery.GetBMU(0).MinCellTemp.ToString();
            BatteryMaxCTxt.Text = chargeService.Battery.GetBMU(0).MaxCellTemp.ToString();
            BatteryBalancePositiveTxt.Text = chargeService.Battery.GetBMU(0).BalanceVoltageThresholdRising.ToString();
            BatteryBalanceNegativeTxt.Text = chargeService.Battery.GetBMU(0).BalanceVoltageThresholdFalling.ToString();
        }

        private void startDischarge_Click(object sender, EventArgs e)
        {

        }
    }
}
