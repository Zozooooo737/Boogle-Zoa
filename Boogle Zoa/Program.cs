using System.Runtime.InteropServices;

namespace Boogle_Zoa
{
    public class Program
    {
        // Ces trois importations permettent d'interagir avec les menus système de la fenêtre console via les bibliothèques Windows,
        // en récupérant le handle de la console, son menu système, et en supprimant des options de ce menu.
        [DllImport("user32.dll")] private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();


        private static (ConsoleColor, ConsoleColor)[] themes =  { ( ConsoleColor.Yellow, ConsoleColor.DarkYellow ),
                                                                  ( ConsoleColor.DarkCyan, ConsoleColor.DarkBlue ),
                                                                  ( ConsoleColor.Gray, ConsoleColor.DarkRed ) };

        private static int numOfTheme = 0;

        private static int width = 80;
        private static int height = 20;

        private static char border = '▓';



        static void Main(string[] args)
        {
            SetupConsole();

            DisplayWelcome();

            int type = DisplayMenu();

            if (type == 0)
            {
                Game g = new Game(2, TimeSpan.FromSeconds(30), 3, 4, "FR");

                g.InitializePlayerName();
                g.Process();
            }
            else
            {
                Console.WriteLine("A FAIRE");
            }

            Console.ReadLine();
        }



        public static void SetupConsole()
        {
            // Ce code récupère le menu système de la console et supprime l'option "Redimensionner" pour empêcher le redimensionnement de la fenêtre.
            nint systemMenu = GetSystemMenu(GetConsoleWindow(), false);
            int sizeCommand = 0xF000;
            DeleteMenu(systemMenu, sizeCommand, 0x00000000);

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.BackgroundColor = themes[numOfTheme].Item1;
            Console.ForegroundColor = themes[numOfTheme].Item2;
        }


        public static void DisplayWelcome()
        {
            Console.WriteLine(new string(border, width));
            DisplayCentered("", 4);
            DisplayCentered("  ____     ___     ___     ____   _       _____     _____   ___      _    ");
            DisplayCentered(" | __ )   / _ \\   / _ \\   / ___| | |     | ____|   |__  /  / _ \\    / \\   ");
            DisplayCentered(" |  _ \\  | | | | | | | | | |  _  | |     |  _|       / /  | | | |  / _ \\  ");
            DisplayCentered(" | |_) | | |_| | | |_| | | |_| | | |___  | |___     / /_  | |_| | / ___ \\ ");
            DisplayCentered(" |____/   \\___/   \\___/   \\____| |_____| |_____|   /____|  \\___/ /_/   \\_\\"); //74
            DisplayCentered("", 3);
            DisplayCentered("Press any key to start !");
            DisplayCentered("", 3);
            DisplayCentered("By Noa & Enzo");
            DisplayCentered("", 1);
            Console.Write(new string(border, width));

            Console.ReadKey();
            Console.Clear();
        }


        public static int DisplayMenu()
        {
            string[] options = { "NORMAL", "CUSTOM" };

            ConsoleKey key;
            int selected = 0;

            do
            {
                Console.Clear();
                Console.WriteLine(new string(border, width));
                DisplayCentered("", 7);
                
                for(int i  = 0; i < options.Length; i++)
                {
                    if (i==selected)
                    {
                        DisplayCentered(options[i], 1, true);
                    }
                    else
                    {
                        DisplayCentered(options[i]);
                    }
                    DisplayCentered("", 2);
                }

                DisplayCentered("", 5);
                Console.Write(new string(border, width));

                key = Console.ReadKey().Key;

                if ( key == ConsoleKey.DownArrow) selected = (selected + 1)%2;
                else if(key == ConsoleKey.UpArrow) selected = Math.Abs((selected - 1) % 2);
            }
            while (key != ConsoleKey.Enter);

            Console.ReadKey();
            Console.Clear();

            return selected;
        }



        public static void DisplayCentered(string text = "", int n = 1, bool selected=false)
        {
            List<string> lines = new List<string>(text.Split("\n"));
            int l ;
            int space;

            for (int i = 0; i < n; i++)
            {
                foreach (string line in lines)
                {
                    string sentence = line;

                    if (line.Length%2 == 1)
                    {
                        sentence +=  " ";
                    }

                    l = sentence.Length / 2;
                    space = width / 2 - l;

                    Console.Write(new string(border, 2));
                    Console.Write(new string(' ', space - 2));

                    if (selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }
                    Console.Write(sentence);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Yellow;

                    Console.Write(new string(' ', space - 2));
                    Console.WriteLine(new string(border, 2));
                }
            }
        }
    }
}
