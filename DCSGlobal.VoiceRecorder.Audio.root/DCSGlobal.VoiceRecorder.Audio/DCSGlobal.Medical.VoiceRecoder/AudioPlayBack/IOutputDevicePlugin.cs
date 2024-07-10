using System;
using System.Linq;
using NAudio.Wave;
using System.Windows.Forms;

namespace DCSGlobal.Medical.VoiceRecoder
{
    public interface IOutputDevicePlugin
    {
        IWavePlayer CreateDevice(int latency);
        UserControl CreateSettingsPanel();
        string Name { get; }
        bool IsAvailable { get; }
        int Priority { get; }
    }
}
