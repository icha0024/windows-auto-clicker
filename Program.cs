using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoClicker
{
    public class Program
    {
        // Windows API imports for mouse operations
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cData, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        // Windows API imports for keyboard operations
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        // Point structure for cursor position
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        // Mouse event constants
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x40;

        public static void Main(string[] args)
        {
            Console.WriteLine("Windows Auto-Clicker");
            Console.WriteLine("Basic clicking functionality test\n");

            // Get current cursor position
            GetCursorPos(out POINT currentPos);
            Console.WriteLine($"Current cursor position: ({currentPos.X}, {currentPos.Y})");

            Console.WriteLine("\nTesting click functions:");
            Console.WriteLine("1. Test single left click at current position");
            Console.WriteLine("2. Test 5 clicks with delay");
            Console.WriteLine("3. Test right click");
            Console.WriteLine("4. Exit");
            Console.Write("\nChoose option (1-4): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    TestSingleClick();
                    break;
                case "2":
                    TestMultipleClicks();
                    break;
                case "3":
                    TestRightClick();
                    break;
                case "4":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Exiting...");
                    return;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Performs a single left mouse click at the current cursor position
        /// </summary>
        private static void PerformLeftClick()
        {
            GetCursorPos(out POINT pos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Performs a single right mouse click at the current cursor position
        /// </summary>
        private static void PerformRightClick()
        {
            GetCursorPos(out POINT pos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_RIGHTUP, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Performs a click at a specific position
        /// </summary>
        private static void PerformClickAt(int x, int y, string clickType = "left")
        {
            SetCursorPos(x, y);
            Thread.Sleep(10); // Small delay to ensure cursor is positioned

            switch (clickType.ToLower())
            {
                case "left":
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, IntPtr.Zero);
                    break;
                case "right":
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)x, (uint)y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, IntPtr.Zero);
                    break;
                case "middle":
                    mouse_event(MOUSEEVENTF_MIDDLEDOWN, (uint)x, (uint)y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_MIDDLEUP, (uint)x, (uint)y, 0, IntPtr.Zero);
                    break;
            }
        }

        private static void TestSingleClick()
        {
            Console.WriteLine("\nPerforming single left click in 3 seconds...");
            Console.WriteLine("Position your cursor where you want to click!");
            
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"Clicking in {i}...");
                Thread.Sleep(1000);
            }

            PerformLeftClick();
            Console.WriteLine("Click performed!");
        }

        private static void TestMultipleClicks()
        {
            Console.WriteLine("\nPerforming 5 clicks with 500ms delay in 3 seconds...");
            Console.WriteLine("Position your cursor where you want to click!");
            
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"Starting in {i}...");
                Thread.Sleep(1000);
            }

            for (int i = 1; i <= 5; i++)
            {
                PerformLeftClick();
                Console.WriteLine($"Click {i} performed");
                Thread.Sleep(500);
            }
            Console.WriteLine("All clicks completed!");
        }

        private static void TestRightClick()
        {
            Console.WriteLine("\nPerforming single right click in 3 seconds...");
            Console.WriteLine("Position your cursor where you want to right-click!");
            
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"Right-clicking in {i}...");
                Thread.Sleep(1000);
            }

            PerformRightClick();
            Console.WriteLine("Right-click performed!");
        }
    }
}