using System;
using System.Windows.Forms;

namespace DCSGlobal.Medical.VoiceRecoder
{
    public interface INAudioDemoPlugin
    {
        string Name { get; }
        Control CreatePanel();
    }
}
