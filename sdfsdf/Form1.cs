using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sdfsdf
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dsFlags, int dx, int dy, int cButtons, int dsExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        private void DoMouseLeftClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
        private void DoMouseRightClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPOINT);

        public Hook _hook_f1;
        public bool act = false;
        public POINT p = new POINT();

        public Form1()
        {
            InitializeComponent();

            _hook_f1 = new Hook(0x57);
            _hook_f1.KeyPressed += new KeyPressEventHandler(trig);
            _hook_f1.SetHook();
        }

        void trig(object sender, KeyPressEventArgs e)
        {
            if (act == false)
            {
                act = true;
                click_ms();
            }
            else
            {
                act = false;            
            }
        }

        void click_ms()
        {
            while(act == true)
            {
                GetCursorPos(ref p);
                ClientToScreen(Handle, ref p);
                DoMouseLeftClick(p.x, p.y);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetCursorPos(ref p);
            ClientToScreen(Handle, ref p);
            DoMouseLeftClick(p.x, p.y);
        }
    }
}
