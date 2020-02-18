using SharpDX;
using SharpDX.Animation;
using SharpDX.D3DCompiler;
using SharpDX.Diagnostics;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DirectComposition;
using SharpDX.DirectInput;
using SharpDX.DirectManipulation;
using SharpDX.DirectSound;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Mathematics;
using SharpDX.Multimedia;
using SharpDX.RawInput;
using SharpDX.Text;
using SharpDX.WIC;
using SharpDX.Win32;
using SharpDX.Windows;
using SharpDX.X3DAudio;
using SharpDX.XAPO;
using SharpDX.XAudio2;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXE_Start
{
    class Program
    {
        private static bool exitRequest = false;
        private static bool isStopedThread = false;

        private static Thread windowThread = new Thread(WindowThread);
        private static Form mainForm;

        private static void WindowThread()
        {
            mainForm = new Form();
            mainForm.Text = "DX12";
            mainForm.Width = 640;
            mainForm.Height = 480;
            mainForm.Show();

            while (!exitRequest)
            {
                mainForm.Update();
                mainForm.Invalidate();
                mainForm.Refresh();
                Application.DoEvents();
            }

            mainForm.Close();
            mainForm.Dispose();

            isStopedThread = true;
        }

        static void Main(string[] args)
        {
            windowThread.Start();
            Console.Read();
            exitRequest = true;
            while (!isStopedThread) { }
            try { windowThread.Abort(); } catch { }
        }
    }
}
