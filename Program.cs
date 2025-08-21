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

        // Keyboard event constants
        private const uint KEYEVENTF_KEYUP = 0x02;

        // Common virtual key codes
        private const byte VK_SPACE = 0x20;
        private const byte VK_ENTER = 0x0D;
        private const byte VK_ESCAPE = 0x1B;
        private const byte VK_TAB = 0x09;
        private const byte VK_SHIFT = 0x10;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_ALT = 0x12;

        public static void Main(string[] args)
        {
            Console.WriteLine("Windows Auto-Clicker");
            Console.WriteLine("Mouse and Keyboard input support\n");

            while (true)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TestMouseClicks();
                        break;
                    case "2":
                        TestKeyboardInput();
                        break;
                    case "3":
                        TestSpecificKey();
                        break;
                    case "4":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.WriteLine("=== Auto-Clicker Test Menu ===");
            Console.WriteLine("1. Test mouse clicks");
            Console.WriteLine("2. Test keyboard input");
            Console.WriteLine("3. Test specific key");
            Console.WriteLine("4. Exit");
            Console.Write("\nChoose option (1-4): ");
        }

        // Mouse clicking methods
        private static void PerformLeftClick()
        {
            GetCursorPos(out POINT pos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
        }

        private static void PerformRightClick()
        {
            GetCursorPos(out POINT pos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_RIGHTUP, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
        }

        private static void PerformMiddleClick()
        {
            GetCursorPos(out POINT pos);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_MIDDLEUP, (uint)pos.X, (uint)pos.Y, 0, IntPtr.Zero);
        }

        // Keyboard input methods
        private static void PerformKeyPress(byte virtualKey)
        {
            keybd_event(virtualKey, 0, 0, 0);           // Key down
            keybd_event(virtualKey, 0, KEYEVENTF_KEYUP, 0); // Key up
        }

        private static void PerformKeyPress(char key)
        {
            byte virtualKey = (byte)char.ToUpper(key);
            PerformKeyPress(virtualKey);
        }

        private static void PerformKeyHold(byte virtualKey, int holdTimeMs)
        {
            keybd_event(virtualKey, 0, 0, 0);           // Key down
            Thread.Sleep(holdTimeMs);
            keybd_event(virtualKey, 0, KEYEVENTF_KEYUP, 0); // Key up
        }

        private static byte GetVirtualKeyCode(string keyName)
        {
            switch (keyName.ToUpper())
            {
                case "SPACE": return VK_SPACE;
                case "ENTER": return VK_ENTER;
                case "ESC": case "ESCAPE": return VK_ESCAPE;
                case "TAB": return VK_TAB;
                case "SHIFT": return VK_SHIFT;
                case "CTRL": case "CONTROL": return VK_CONTROL;
                case "ALT": return VK_ALT;
                case "F1": return 0x70;
                case "F2": return 0x71;
                case "F3": return 0x72;
                case "F4": return 0x73;
                case "F5": return 0x74;
                case "W": return 0x57;
                case "A": return 0x41;
                case "S": return 0x53;
                case "D": return 0x44;
                default:
                    // For single characters
                    if (keyName.Length == 1)
                    {
                        return (byte)char.ToUpper(keyName[0]);
                    }
                    return 0; // Invalid key
            }
        }

        private static void TestMouseClicks()
        {
            Console.WriteLine("\n=== Mouse Click Test ===");
            Console.WriteLine("1. Left click");
            Console.WriteLine("2. Right click");
            Console.WriteLine("3. Middle click");
            Console.WriteLine("4. Multiple left clicks");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine();
            Console.WriteLine("\nStarting in 3 seconds - position your cursor!");

            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"{i}...");
                Thread.Sleep(1000);
            }

            switch (choice)
            {
                case "1":
                    PerformLeftClick();
                    Console.WriteLine("Left click performed!");
                    break;
                case "2":
                    PerformRightClick();
                    Console.WriteLine("Right click performed!");
                    break;
                case "3":
                    PerformMiddleClick();
                    Console.WriteLine("Middle click performed!");
                    break;
                case "4":
                    for (int i = 0; i < 5; i++)
                    {
                        PerformLeftClick();
                        Console.WriteLine($"Click {i + 1} performed");
                        Thread.Sleep(300);
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }

            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void TestKeyboardInput()
        {
            Console.WriteLine("\n=== Keyboard Input Test ===");
            Console.WriteLine("Testing common keys in 3 seconds...");
            Console.WriteLine("Make sure a text editor is focused!");

            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"{i}...");
                Thread.Sleep(1000);
            }

            // Type "Hello World"
            string message = "Hello World";
            foreach (char c in message)
            {
                if (c == ' ')
                {
                    PerformKeyPress(VK_SPACE);
                }
                else
                {
                    PerformKeyPress(c);
                }
                Thread.Sleep(100);
            }

            Thread.Sleep(500);
            PerformKeyPress(VK_ENTER); // Press Enter

            Console.WriteLine("Keyboard test completed!");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void TestSpecificKey()
        {
            Console.WriteLine("\n=== Specific Key Test ===");
            Console.Write("Enter key to press (e.g., W, A, S, D, SPACE, ENTER, F1): ");
            string keyInput = Console.ReadLine();

            byte virtualKey = GetVirtualKeyCode(keyInput);
            if (virtualKey == 0)
            {
                Console.WriteLine("Invalid key. Please try again.");
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine($"\nPressing '{keyInput.ToUpper()}' in 3 seconds...");
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"{i}...");
                Thread.Sleep(1000);
            }

            PerformKeyPress(virtualKey);
            Console.WriteLine($"Key '{keyInput.ToUpper()}' pressed!");

            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}