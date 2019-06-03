﻿using System;
using ArrowPointCANBusTool.CanBus;
using ArrowPointCANBusTool.Charger;
using ArrowPointCANBusTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArrowPointCANBusTest.Services
{
    [TestClass]
    public class ElconServiceTest
    {
        [TestMethod]
        public void StartStopChargeTest()
        {
            CanService canService = new CanService();

            canService.ConnectViaLoopBack();
            Assert.IsTrue(canService.IsConnected());

            ElconService elconService = new ElconService(canService, 230, 10);
            elconService.StartCharge();
            Assert.IsTrue(elconService.IsCharging);

            elconService.StopCharge();
            Assert.IsFalse(elconService.IsCharging);

            CanPacket canPacket = canService.LastestCanPacket(ElconService.ELCON_CAN_COMMAND);
            // Update voltage requested to 0
            Assert.AreEqual(canPacket.Int3, 0);
            Assert.AreEqual(canPacket.Int2, 0);

            canService.Disconnect();
            Assert.IsFalse(canService.IsConnected());
        }

        [TestMethod]
        public void OverVoltageTest()
        {
            CanService canService = new CanService();
            ElconService elconService = new ElconService(canService, 230, 10)
            {
                // 198V is the max for the charger
                VoltageRequested = 500
            };
            Assert.AreEqual(elconService.ChargerVoltage, elconService.ChargerVoltageLimit);

            elconService = new ElconService(canService, 120, 10);

            // Power supply voltage is now lower than the max for the charger
            // so the charger can only supply at that voltage
            elconService.VoltageRequested = elconService.ChargerVoltageLimit;
            Assert.AreEqual(elconService.ChargerVoltage, 120);
        }

        [TestMethod]
        public void OverCurrentTest()
        {
            CanService canService = new CanService();

            // Request more current that the charger provides, make sure it steps us down
            ElconService elconService = new ElconService(canService, 230, 100)
            {
                CurrentRequested = 80
            };
            Assert.AreEqual(elconService.ChargerCurrent, elconService.ChargerCurrentLimit);

            // Request more current that the mains provides, make sure it steps us down
            elconService = new ElconService(canService, 230, 10)
            {
                CurrentRequested = 20
            };
            Assert.AreEqual(elconService.ChargerCurrent, 10);
        }

        [TestMethod]
        public void AdjustVoltageTest()
        {
            CanService canService = new CanService();
            ElconService elconService = new ElconService(canService, 230, 10)
            {
                VoltageRequested = 160
            };
            Assert.AreEqual(elconService.ChargerVoltage, 160);

            elconService.VoltageRequested = 150;
            Assert.AreEqual(elconService.ChargerVoltage, 150);

            elconService.VoltageRequested = 170;
            Assert.AreEqual(elconService.ChargerVoltage, 170);

            elconService.VoltageRequested = 0;
            Assert.AreEqual(elconService.ChargerVoltage, 0);

            elconService.VoltageRequested = 120;
            Assert.AreEqual(elconService.ChargerVoltage, 120);

            // Over the max of the charger, so should bring us back down
            elconService.VoltageRequested = 240;
            Assert.AreEqual(elconService.ChargerVoltage, elconService.ChargerVoltageLimit);

            elconService.VoltageRequested = 0;
            Assert.AreEqual(elconService.ChargerVoltage, 0);
        }

        [TestMethod]
        public void AdjustCurrentTest()
        {
            CanService canService = new CanService();
            ElconService elconService = new ElconService(canService, 230, 10)
            {
                CurrentRequested = 8
            };
            Assert.AreEqual(elconService.ChargerCurrent, 8);

            elconService.CurrentRequested = 7;
            Assert.AreEqual(elconService.ChargerCurrent, 7);

            elconService.CurrentRequested = 9;
            Assert.AreEqual(elconService.ChargerCurrent, 9);

            elconService.CurrentRequested = 0;
            Assert.AreEqual(elconService.ChargerCurrent, 0);

            elconService.CurrentRequested = 9;
            Assert.AreEqual(elconService.ChargerCurrent, 9);

            // Over the max of the supply, so should bring us back down
            elconService.CurrentRequested = 11;
            Assert.AreEqual(elconService.ChargerCurrent, 10);

            elconService.CurrentRequested = 0;
            Assert.AreEqual(elconService.ChargerCurrent, 0);
        }


    }
}
