﻿using ArrowPointCANBusTool.Canbus;
using ArrowPointCANBusTool.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrowPointCANBusTool.Forms
{
    public partial class DataLogReplayerForm : Form
    {       
        OpenFileDialog openFileDialog;
        Stream ioStream;       
        Timer timer;

        public DataLogReplayerForm()
        {
            InitializeComponent();

            this.tbFilterFrom.Text = "1";
            this.tbFilterFrom.Enabled = false;
            this.tbFilterTo.Text = "1000";
            this.tbFilterTo.Enabled = false;

            this.btnStop.Enabled = false;
            
            UpdateStatus();

        }

        private void DataLogReplayerForm_Load(object sender, EventArgs e)
        {
            timer = new Timer
            {
                Interval = (100)
            };
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        private void DataLogReplayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }

        private void TimerTick(object sender, EventArgs e) {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            btnStart.Enabled = !CanRecordReplayService.Instance.IsReplaying;
            btnStop.Enabled = CanRecordReplayService.Instance.IsReplaying;
            rbIdInclude.Enabled = !CanRecordReplayService.Instance.IsReplaying;
            rbIdExclude.Enabled = !CanRecordReplayService.Instance.IsReplaying;
            rbIdNone.Enabled = !CanRecordReplayService.Instance.IsReplaying;            
            toolStripStatusText.Text = CanRecordReplayService.Instance.ReplayStatus;
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((ioStream = openFileDialog.OpenFile()) != null)
                    {
                        int filterStatus = CanRecordReplayService.FILTER_NONE;

                        if (rbIdInclude.Checked) filterStatus = CanRecordReplayService.FILTER_INCLUDE;
                        if (rbIdExclude.Checked) filterStatus = CanRecordReplayService.FILTER_EXCLUDE;

                        int filterFrom = int.MinValue;
                        int filterTo = int.MaxValue;

                        Boolean check = Int32.TryParse(tbFilterFrom.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out filterFrom);
                        if (!check)
                        {
                            MessageBox.Show("Failed to parse lower filter value");
                            ioStream.Close();
                            return;
                        }

                        check = Int32.TryParse(tbFilterTo.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out filterTo);
                        if (!check)
                        {
                            MessageBox.Show("Failed to parse upper filter value");
                            ioStream.Close();
                            return;
                        }

                        CanRecordReplayService.Instance.FilterFrom = filterFrom;
                        CanRecordReplayService.Instance.FilterTo = filterTo;
                        CanRecordReplayService.Instance.LoopReplay = checkBoxLoop.Checked;
                        CanRecordReplayService.Instance.FilterType = filterStatus;
                        await CanRecordReplayService.Instance.StartReplaying(ioStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            UpdateStatus();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            CanRecordReplayService.Instance.StopReplaying();
            UpdateStatus();
        }

        private void RbIdInclude_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIdInclude.Checked)
            {
                tbFilterFrom.Enabled = true;
                tbFilterTo.Enabled = true;
            }
        }

        private void RbIdExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIdExclude.Checked)
            {
                tbFilterFrom.Enabled = true;
                tbFilterTo.Enabled = true;
            }
        }

        private void RbIdNone_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIdNone.Checked)
            {
                tbFilterFrom.Enabled = false;
                tbFilterTo.Enabled = false;
            }
        }

        private void CheckBoxLoop_CheckedChanged(object sender, EventArgs e)
        {
            CanRecordReplayService.Instance.LoopReplay = checkBoxLoop.Checked;
        }
    }
}
