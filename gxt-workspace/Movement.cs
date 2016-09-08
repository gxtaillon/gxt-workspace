using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gxt_workspace
{
    // http://www.pinvoke.net/default.aspx/user32/EnumWindows.html
    public class Movement
    {
        public delegate bool CallBackPtr(IntPtr hwnd, int lParam);
        private CallBackPtr callBackPtr;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int EnumWindows(CallBackPtr callPtr, int lPar);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

        public static bool Report(IntPtr hWnd, int lParam)
        {
            Rectangle rectangle;
            MovementDirection md = (MovementDirection)lParam;
            var visible = IsWindowVisible(hWnd);
            if (visible)
            {
                RECT rect = new RECT();
                GetWindowRect(hWnd, ref rect);
                rectangle = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

                WINDOWPLACEMENT wndpl = new WINDOWPLACEMENT();
                wndpl.Lenght = (uint)Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                if (!GetWindowPlacement(hWnd, ref wndpl))
                {
                    MessageBox.Show("Bad size!");
                }
                var seeable = wndpl.ShowCmd == (uint)ShowCmd.SW_SHOWMAXIMIZED || wndpl.ShowCmd == (uint)ShowCmd.SW_SHOWNORMAL;
                
                if (seeable && IsOnScreen(rectangle))
                {
                    uint pid;
                    GetWindowThreadProcessId(hWnd, out pid);
                    //MessageBox.Show("Visible Control Window handle is " + hWnd + " at " + rectangle.ToString() + " of pid " + pid);
                    int dLeft = 0, dTop = 0;
                    switch (md)
                    {
                        case MovementDirection.Left:
                            dLeft = -100;
                            break;
                        case MovementDirection.Right:
                            dLeft = 100;
                            break;
                        case MovementDirection.Up:
                            dTop = -100;
                            break;
                        case MovementDirection.Down:
                            dTop = 100;
                            break;
                    }
                    SetWindowPos(hWnd, 0, rectangle.Left + dLeft, rectangle.Top + dTop, 0, 0, (uint) (SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_SHOWWINDOW));
                }
            }
            return true;
        }

        public void Move(MovementDirection md)
        {

            // note in other situations, it is important to keep 
            // callBackPtr as a member variable so it doesnt GC while you're calling EnumWindows

            callBackPtr = new CallBackPtr(Movement.Report);
            Movement.EnumWindows(callBackPtr, (int) md);
        }

        [Flags]
        public enum MovementDirection : int
        {
            Left, Right, Up, Down
        }

        // http://stackoverflow.com/a/11655557
        public static bool IsOnScreen(Rectangle rectangle)
        {
            return Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(rectangle));
        }

        // http://stackoverflow.com/a/1434577
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpWndpl);
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public long X;
            public long Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public uint Lenght;
            public uint Flags;

            public uint ShowCmd;
            public POINT PtMinPosition;
            public POINT PtMaxPosition;
            public RECT RcNormalPosition;
        }

        private enum ShowCmd : uint
        {
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWNORMAL = 1
        }

        [Flags]
        private enum SetWindowPosFlags : uint
        {
            SWP_SHOWWINDOW = 0x0040,
            SWP_NOZORDER = 0x0004,
            SWP_NOSIZE = 0x0001
        }
    }
}

