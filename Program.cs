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

        // Configuration settings
        private static string inputType = "mouse";      // "mouse" or "keyboard"
        private static string mouseButton = "left";     // "left", "right", "middle"
        private static string keyToPress = "SPACE";     // Key for keyboard mode
        private static int clickInterval = 100;         // milliseconds between actions
        private static int randomDelay = 0;             // max random delay to add
        private static bool useRandomDelay = false;
        private static POINT targetPosition = new POINT { X = -1, Y = -1 }; // -1 means follow cursor
        private static int maxClicks = 0;               // 0 = infinite

        private static Random random = new Random();

        public static void Main(string[] args)
        {
            Console.Title = "Windows Auto-Clicker - Configuration Mode";
            
            while (true)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ConfigureInputType();
                        break;
                    case "2":
                        ConfigureMouseButton();
                        break;
                    case "3":
                        ConfigureKeyboard();
                        break;
                    case "4":
                        ConfigureTiming();
                        break;
                    case "5":
                        ConfigurePosition();
                        break;
                    case "6":
                        ConfigureRandomDelay();
                        break;
                    case "7":
                        ConfigureClickLimit();
                        break;
                    case "8":
                        StartAutomation();
                        break;
                    case "9":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║          Windows Auto-Clicker        ║");
            Console.WriteLine("║            Configuration Menu         ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();
            
            Console.WriteLine("┌─ Current Configuration ─────────────┐");
            Console.WriteLine($"│ Input Type: {inputType.ToUpper(),-24}│");
            
            if (inputType == "mouse")
            {
                Console.WriteLine($"│ Mouse Button: {mouseButton.ToUpper(),-22}│");
            }
            else
            {
                Console.WriteLine($"│ Key to Press: {keyToPress,-22}│");
            }
            
            Console.WriteLine($"│ Click Interval: {clickInterval}ms{new string(' ', 18 - clickInterval.ToString().Length)}│");
            Console.WriteLine($"│ Random Delay: {(useRandomDelay ? randomDelay + "ms" : "Disabled"),-23}│");
            Console.WriteLine($"│ Target: {(targetPosition.X == -1 ? "Follow Cursor" : $"({targetPosition.X}, {targetPosition.Y})"),-29}│");
            Console.WriteLine($"│ Click Limit: {(maxClicks == 0 ? "Unlimited" : maxClicks.ToString()),-24}│");
            Console.WriteLine("└──────────────────────────────────────┘");
            Console.WriteLine();
            
            Console.WriteLine("Configuration Options:");
            Console.WriteLine("  1. Set input type (Mouse/Keyboard)");
            Console.WriteLine("  2. Configure mouse button");
            Console.WriteLine("  3. Configure keyboard key");
            Console.WriteLine("  4. Set timing/interval");
            Console.WriteLine("  5. Set target position");
            Console.WriteLine("  6. Configure random delay");
            Console.WriteLine("  7. Set click limit");
            Console.WriteLine("  8. 🚀 START AUTOMATION");
            Console.WriteLine("  9. Exit");
            Console.WriteLine();
            Console.Write("Choose option (1-9): ");
        }

        private static void ConfigureInputType()
        {
            Console.Clear();
            Console.WriteLine("=== Input Type Configuration ===");
            Console.WriteLine("1. Mouse clicking");
            Console.WriteLine("2. Keyboard input");
            Console.Write("Choose (1-2): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    inputType = "mouse";
                    Console.WriteLine("✓ Set to mouse clicking");
                    break;
                case "2":
                    inputType = "keyboard";
                    Console.WriteLine("✓ Set to keyboard input");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            
            Thread.Sleep(1000);
        }

        private static void ConfigureMouseButton()
        {
            if (inputType != "mouse")
            {
                Console.WriteLine("Mouse configuration only available in mouse mode");
                Thread.Sleep(1500);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("=== Mouse Button Configuration ===");
            Console.WriteLine("1. Left click");
            Console.WriteLine("2. Right click");
            Console.WriteLine("3. Middle click");
            Console.Write("Choose (1-3): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    mouseButton = "left";
                    Console.WriteLine("✓ Set to left click");
                    break;
                case "2":
                    mouseButton = "right";
                    Console.WriteLine("✓ Set to right click");
                    break;
                case "3":
                    mouseButton = "middle";
                    Console.WriteLine("✓ Set to middle click");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            
            Thread.Sleep(1000);
        }

        private static void ConfigureKeyboard()
        {
            if (inputType != "keyboard")
            {
                Console.WriteLine("Keyboard configuration only available in keyboard mode");
                Thread.Sleep(1500);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("=== Keyboard Key Configuration ===");
            Console.WriteLine("Common options:");
            Console.WriteLine("  Letters: A, B, C, ... Z");
            Console.WriteLine("  Gaming: W, A, S, D");
            Console.WriteLine("  Special: SPACE, ENTER, TAB, ESC");
            Console.WriteLine("  Function: F1, F2, F3, ... F12");
            Console.WriteLine();
            Console.Write("Enter key to press: ");
            
            string input = Console.ReadLine()?.ToUpper().Trim();
            if (!string.IsNullOrEmpty(input))
            {
                byte virtualKey = GetVirtualKeyCode(input);
                if (virtualKey != 0)
                {
                    keyToPress = input;
                    Console.WriteLine($"✓ Set to press '{keyToPress}' key");
                }
                else
                {
                    Console.WriteLine("Invalid key. Please try again.");
                }
            }
            
            Thread.Sleep(1500);
        }

        private static void ConfigureTiming()
        {
            Console.Clear();
            Console.WriteLine("=== Timing Configuration ===");
            Console.WriteLine($"Current interval: {clickInterval}ms");
            Console.WriteLine();
            Console.WriteLine("Presets:");
            Console.WriteLine("  1. Very Fast (10ms)");
            Console.WriteLine("  2. Fast (50ms)");
            Console.WriteLine("  3. Normal (100ms)");
            Console.WriteLine("  4. Slow (500ms)");
            Console.WriteLine("  5. Very Slow (1000ms)");
            Console.WriteLine("  6. Custom value");
            Console.Write("Choose option (1-6): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    clickInterval = 10;
                    break;
                case "2":
                    clickInterval = 50;
                    break;
                case "3":
                    clickInterval = 100;
                    break;
                case "4":
                    clickInterval = 500;
                    break;
                case "5":
                    clickInterval = 1000;
                    break;
                case "6":
                    Console.Write("Enter custom interval (10-10000ms): ");
                    if (int.TryParse(Console.ReadLine(), out int custom) && custom >= 10 && custom <= 10000)
                    {
                        clickInterval = custom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid interval. Keeping current setting.");
                        Thread.Sleep(1500);
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    Thread.Sleep(1000);
                    return;
            }
            
            Console.WriteLine($"✓ Interval set to {clickInterval}ms");
            Thread.Sleep(1000);
        }

        private static void ConfigurePosition()
        {
            Console.Clear();
            Console.WriteLine("=== Position Configuration ===");
            Console.WriteLine("1. Follow cursor (click wherever cursor is)");
            Console.WriteLine("2. Set specific coordinates");
            Console.WriteLine("3. Use current cursor position");
            Console.Write("Choose (1-3): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    targetPosition = new POINT { X = -1, Y = -1 };
                    Console.WriteLine("✓ Set to follow cursor");
                    break;
                case "2":
                    Console.Write("Enter X coordinate: ");
                    if (int.TryParse(Console.ReadLine(), out int x))
                    {
                        Console.Write("Enter Y coordinate: ");
                        if (int.TryParse(Console.ReadLine(), out int y))
                        {
                            targetPosition = new POINT { X = x, Y = y };
                            Console.WriteLine($"✓ Set to click at ({x}, {y})");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Y coordinate");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid X coordinate");
                    }
                    break;
                case "3":
                    GetCursorPos(out POINT currentPos);
                    targetPosition = currentPos;
                    Console.WriteLine($"✓ Set to current position ({currentPos.X}, {currentPos.Y})");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            
            Thread.Sleep(1500);
        }

        private static void ConfigureRandomDelay()
        {
            Console.Clear();
            Console.WriteLine("=== Random Delay Configuration ===");
            Console.WriteLine($"Current: {(useRandomDelay ? $"0-{randomDelay}ms" : "Disabled")}");
            Console.WriteLine();
            Console.WriteLine("1. Disable random delay");
            Console.WriteLine("2. Enable with custom range");
            Console.Write("Choose (1-2): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    useRandomDelay = false;
                    Console.WriteLine("✓ Random delay disabled");
                    break;
                case "2":
                    Console.Write("Enter maximum random delay (0-1000ms): ");
                    if (int.TryParse(Console.ReadLine(), out int delay) && delay >= 0 && delay <= 1000)
                    {
                        randomDelay = delay;
                        useRandomDelay = true;
                        Console.WriteLine($"✓ Random delay set to 0-{delay}ms");
                    }
                    else
                    {
                        Console.WriteLine("Invalid delay value");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            
            Thread.Sleep(1500);
        }

        private static void ConfigureClickLimit()
        {
            Console.Clear();
            Console.WriteLine("=== Click Limit Configuration ===");
            Console.WriteLine($"Current: {(maxClicks == 0 ? "Unlimited" : maxClicks.ToString())}");
            Console.WriteLine();
            Console.WriteLine("1. Unlimited clicks");
            Console.WriteLine("2. Set specific limit");
            Console.Write("Choose (1-2): ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    maxClicks = 0;
                    Console.WriteLine("✓ Set to unlimited clicks");
                    break;
                case "2":
                    Console.Write("Enter maximum number of clicks (1-99999): ");
                    if (int.TryParse(Console.ReadLine(), out int limit) && limit >= 1 && limit <= 99999)
                    {
                        maxClicks = limit;
                        Console.WriteLine($"✓ Click limit set to {limit}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid limit value");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            
            Thread.Sleep(1500);
        }

        private static void StartAutomation()
        {
            Console.Clear();
            Console.WriteLine("🚀 STARTING AUTOMATION");
            Console.WriteLine("═══════════════════════");
            Console.WriteLine($"Mode: {inputType.ToUpper()}");
            
            if (inputType == "mouse")
            {
                Console.WriteLine($"Button: {mouseButton.ToUpper()}");
                if (targetPosition.X == -1)
                {
                    Console.WriteLine("Target: Following cursor");
                }
                else
                {
                    Console.WriteLine($"Target: ({targetPosition.X}, {targetPosition.Y})");
                }
            }
            else
            {
                Console.WriteLine($"Key: {keyToPress}");
            }
            
            Console.WriteLine($"Interval: {clickInterval}ms");
            if (useRandomDelay)
            {
                Console.WriteLine($"Random delay: 0-{randomDelay}ms");
            }
            if (maxClicks > 0)
            {
                Console.WriteLine($"Limit: {maxClicks} clicks");
            }
            
            Console.WriteLine();
            Console.WriteLine("Starting in 5 seconds...");
            Console.WriteLine("Press ESC to stop (will be monitored during automation)");
            
            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine($"Starting in {i}...");
                Thread.Sleep(1000);
            }
            
            RunAutomation();
        }

        private static void RunAutomation()
        {
            Console.Clear();
            Console.WriteLine("🟢 AUTOMATION RUNNING");
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine();
            
            int clickCount = 0;
            DateTime startTime = DateTime.Now;
            
            while (true)
            {
                // Check for ESC key to stop
                if (GetAsyncKeyState(0x1B) < 0) // ESC key
                {
                    Console.WriteLine("\n🔴 STOPPED by user (ESC pressed)");
                    break;
                }
                
                // Perform the action
                if (inputType == "mouse")
                {
                    PerformMouseAction();
                }
                else
                {
                    PerformKeyboardAction();
                }
                
                clickCount++;
                
                // Update stats every 10 actions
                if (clickCount % 10 == 0)
                {
                    var elapsed = DateTime.Now - startTime;
                    var actionsPerSecond = clickCount / elapsed.TotalSeconds;
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine($"Count: {clickCount}, Rate: {actionsPerSecond:F1}/sec, Time: {elapsed:mm\\:ss}");
                }
                
                // Check if we've reached the click limit
                if (maxClicks > 0 && clickCount >= maxClicks)
                {
                    Console.WriteLine($"\n🎯 COMPLETED - Reached limit of {maxClicks} clicks");
                    break;
                }
                
                // Calculate delay
                int delay = clickInterval;
                if (useRandomDelay)
                {
                    delay += random.Next(0, randomDelay + 1);
                }
                
                Thread.Sleep(delay);
            }
            
            var totalTime = DateTime.Now - startTime;
            var avgRate = clickCount / totalTime.TotalSeconds;
            
            Console.WriteLine();
            Console.WriteLine("📊 AUTOMATION SUMMARY");
            Console.WriteLine($"Total actions: {clickCount}");
            Console.WriteLine($"Total time: {totalTime:mm\\:ss}");
            Console.WriteLine($"Average rate: {avgRate:F1} actions/second");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private static void PerformMouseAction()
        {
            POINT clickPos;
            
            if (targetPosition.X == -1) // Follow cursor
            {
                GetCursorPos(out clickPos);
            }
            else // Use specific position
            {
                clickPos = targetPosition;
                SetCursorPos(clickPos.X, clickPos.Y);
            }
            
            switch (mouseButton)
            {
                case "left":
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    break;
                case "right":
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_RIGHTUP, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    break;
                case "middle":
                    mouse_event(MOUSEEVENTF_MIDDLEDOWN, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    mouse_event(MOUSEEVENTF_MIDDLEUP, (uint)clickPos.X, (uint)clickPos.Y, 0, IntPtr.Zero);
                    break;
            }
        }

        private static void PerformKeyboardAction()
        {
            byte virtualKey = GetVirtualKeyCode(keyToPress);
            if (virtualKey != 0)
            {
                keybd_event(virtualKey, 0, 0, 0);           // Key down
                keybd_event(virtualKey, 0, KEYEVENTF_KEYUP, 0); // Key up
            }
        }

        private static byte GetVirtualKeyCode(string keyName)
        {
            switch (keyName.ToUpper())
            {
                case "SPACE": return 0x20;
                case "ENTER": return 0x0D;
                case "ESC": case "ESCAPE": return 0x1B;
                case "TAB": return 0x09;
                case "SHIFT": return 0x10;
                case "CTRL": case "CONTROL": return 0x11;
                case "ALT": return 0x12;
                case "F1": return 0x70;
                case "F2": return 0x71;
                case "F3": return 0x72;
                case "F4": return 0x73;
                case "F5": return 0x74;
                case "F6": return 0x75;
                case "F7": return 0x76;
                case "F8": return 0x77;
                case "F9": return 0x78;
                case "F10": return 0x79;
                case "F11": return 0x7A;
                case "F12": return 0x7B;
                default:
                    // For single characters (A-Z, 0-9)
                    if (keyName.Length == 1)
                    {
                        char c = keyName[0];
                        if (char.IsLetterOrDigit(c))
                        {
                            return (byte)char.ToUpper(c);
                        }
                    }
                    return 0; // Invalid key
            }
        }
    }
}