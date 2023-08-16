using System.Runtime.InteropServices;
using System.Windows;

namespace BCEdit180.Utils {
    public static class MouseLocationHelper {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                this.X = x;
                this.Y = y;
            }
        }

        public static Point GetLocation() {
            POINT mPos;
            if (GetCursorPos(out mPos))
                return new Point(mPos.X, mPos.Y);

            return new Point(0, 0);
        }

        public static void SetLocation(int x, int y) {
            SetCursorPos(x, y);
        }
    }
}