﻿using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class WaveOutSettingsPanel : UserControl
    {
        public WaveOutSettingsPanel()
        {
            InitializeComponent();
            InitialiseDeviceCombo();
            InitialiseStrategyCombo();
        }

        internal class CallbackComboItem
        {
            public CallbackComboItem(string text, WaveCallbackStrategy strategy)
            {
                Text = text;
                Strategy = strategy;
            }
            public string Text { get; private set; }
            public WaveCallbackStrategy Strategy { get; private set; }
        }

        private void InitialiseDeviceCombo()
        {
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                var capabilities = WaveOut.GetCapabilities(deviceId);
                comboBoxWaveOutDevice.Items.Add(String.Format("Device {0} ({1})", deviceId, capabilities.ProductName));
            }
            if (comboBoxWaveOutDevice.Items.Count > 0)
            {
                comboBoxWaveOutDevice.SelectedIndex = 0;
            }
        }

        private void InitialiseStrategyCombo()
        {
            comboBoxCallback.DisplayMember = "Text";
            comboBoxCallback.ValueMember = "Strategy";
            comboBoxCallback.Items.Add(new CallbackComboItem("Window", WaveCallbackStrategy.NewWindow));
            comboBoxCallback.Items.Add(new CallbackComboItem("Function", WaveCallbackStrategy.FunctionCallback));
            comboBoxCallback.Items.Add(new CallbackComboItem("Event", WaveCallbackStrategy.Event));
            comboBoxCallback.SelectedIndex = 0;
        }

        public int SelectedDeviceNumber { get { return comboBoxWaveOutDevice.SelectedIndex; } }

        public WaveCallbackStrategy CallbackStrategy { get { return ((CallbackComboItem)comboBoxCallback.SelectedItem).Strategy; } }
    }
}
