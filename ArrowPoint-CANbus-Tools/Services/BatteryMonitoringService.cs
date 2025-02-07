﻿using ArrowPointCANBusTool.Canbus;
using ArrowPointCANBusTool.Services;
using ArrowPointCANBusTool.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArrowPointCANBusTool.Services
{

    public delegate void BatteryMonitorUpdateEventHandler(ChargeDataReceivedEventArgs e);

    public class BatteryMonitoringService
    {

        private static readonly BatteryMonitoringService instance = new BatteryMonitoringService();

        private List<ChargeData> chargeDataSet = new List<ChargeData>();
        public event BatteryMonitorUpdateEventHandler BatteryMonitorUpdateEventHandler;
        public List<ChargeData> ChargeDataSet { get { return chargeDataSet; } }

        static BatteryMonitoringService()
        {
        }

        public static BatteryMonitoringService Instance
        {
            get
            {
                return instance;
            }
        }

        private BatteryMonitoringService()
        {
            Timer updateChargeDataTimer = new System.Timers.Timer
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            updateChargeDataTimer.Elapsed += UpdateChargeData;
        }


        private void UpdateChargeData(object sender, EventArgs e)
        {
            Battery battery = BatteryChargeService.Instance.BatteryService.BatteryData;

            ChargeData chargeData = new ChargeData
            {
                    DateTime = DateTime.Now,
                    SOC = battery.SOCPercentage,
                    ChargeCurrentA = BatteryChargeService.Instance.ChargerActualCurrent,
                    ChargeVoltagemV = BatteryChargeService.Instance.ChargerActualVoltage,
                    PackmA = battery.BatteryCurrent,
                    PackmV = battery.BatteryVoltage,
                    MinCellmV = battery.MinCellVoltage,
                    MaxCellmV = battery.MaxCellVoltage,
                    MinCellTemp = battery.MinCellTemp,
                    MaxCellTemp = battery.MaxCellTemp,
                    BalanceVoltageThresholdFalling = battery.BalanceVoltageThresholdFalling,
                    BalanceVoltageThresholdRising = battery.BalanceVoltageThresholdRising,
                    ChargeCellVoltageError = battery.MinChargeCellVoltageError,
                    DischargeCellVoltageError = battery.MinDischargeCellVoltageError
            };

            if (BatteryChargeService.Instance.IsCharging && BatteryDischargeService.Instance.IsDischarging) chargeData.State = ChargeData.STATE_ERROR;
            else if (BatteryChargeService.Instance.IsCharging) chargeData.State = ChargeData.STATE_CHARGE;
            else if (BatteryDischargeService.Instance.IsDischarging) chargeData.State = ChargeData.STATE_DISCHARGE;
            else chargeData.State = ChargeData.STATE_IDLE;
            chargeDataSet.Add(chargeData);

            BatteryMonitorUpdateEventHandler?.Invoke(new ChargeDataReceivedEventArgs(chargeData));
        }

        public void ClearChargeData()
        {
            chargeDataSet.Clear();
        }

        public void SaveChargeData(string fileName)
        {
            StreamWriter fileStream = new System.IO.StreamWriter(fileName);
            if (fileStream != null)
                SaveChargeData(fileStream);
            else
                throw (new FileNotFoundException());
        }

        public void SaveChargeData(StreamWriter ioStream)
        {
            StreamWriter recordStream = ioStream;
            recordStream.WriteLine("Date time     , SOC %, Charge Current , Charge Voltage, Pack mA, Pack mV, Min Cell mV, Max Cell mV, Min Cell Temp, Max Cell Temp, Balance +, Balance -, Charge Voltage Error, Discharge Voltage Error");

            foreach (ChargeData chargeData in chargeDataSet)
            {
                string newLine = "";

                newLine = newLine + MyExtensions.AlignLeft(chargeData.DateTime.ToString("HH:mm:ss"), 14, false);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.SOCAsInt.ToString(), 7, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.ChargeCurrentmA.ToString(), 17, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.ChargeVoltagemV.ToString(), 16, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.PackmA.ToString(), 9, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.PackmV.ToString(), 9, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.MinCellmV.ToString(), 13, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.MaxCellmV.ToString(), 13, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.MinCellTemp.ToString(), 15, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.MaxCellTemp.ToString(), 15, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.BalanceVoltageThresholdRising.ToString(), 11, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.BalanceVoltageThresholdFalling.ToString(), 11, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.ChargeCellVoltageError.ToString(), 22, true);
                newLine = newLine + MyExtensions.AlignLeft(chargeData.DischargeCellVoltageError.ToString(), 12, true);

                recordStream.WriteLine(newLine);
            }

            ioStream.Close();
            recordStream.Close();
        }

    }
}
