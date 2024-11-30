using System.Runtime.InteropServices;
using System.Media;


namespace Boogle_Zoa
{
    public class Program
    {
        /// <summary>
        /// Supprime une option spécifique du menu système d'une fenêtre donnée.
        /// </summary>
        /// <param name="hMenu">Handle du menu système de la fenêtre.</param>
        /// <param name="nPosition">Position de l'option à supprimer dans le menu.</param>
        /// <param name="wFlags">Indicateurs qui définissent le type de suppression.</param>
        /// <returns>Retourne un entier indiquant le succès ou l'échec de l'opération.</returns>
        [DllImport("user32.dll")] private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        /// <summary>
        /// Récupère un handle vers le menu système d'une fenêtre console.
        /// </summary>
        /// <param name="hWnd">Handle de la fenêtre console.</param>
        /// <param name="bRevert">Indique si le menu système doit être restauré à son état initial.</param>
        /// <returns>Retourne un handle vers le menu système de la fenêtre.</returns>
        [DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Récupère un handle vers la fenêtre console actuelle.
        /// </summary>
        /// <returns>Retourne un handle vers la fenêtre console.</returns>
        [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();


        /// <summary>
        /// Liste des thèmes disponibles. Chaque thème est défini par un couple de couleurs 
        /// (couleur de fond, couleur du texte) pour personnaliser l'affichage dans la console.
        /// </summary>
        private static Dictionary<string, (ConsoleColor Background, ConsoleColor Foreground)> themes = new Dictionary<string, (ConsoleColor, ConsoleColor)>
        {
            { "EGYPT", (ConsoleColor.Yellow, ConsoleColor.DarkYellow) },
            { "DARK", (ConsoleColor.DarkCyan, ConsoleColor.DarkBlue) },
            { "LIGHT", (ConsoleColor.Gray, ConsoleColor.DarkRed) }
        };

        /// <summary>
        /// Thème actuellement sélectionné, représenté par son nom.
        /// </summary>
        private static string theme = "EGYPT";


        /// <summary>
        /// Largeur de la console en caractères.
        /// </summary>
        private static int width = 80;

        /// <summary>
        /// Hauteur de la console en caractères.
        /// </summary>
        private static int height = 20;

        /// <summary>
        /// Caractère utilisé pour dessiner la bordure de la console.
        /// </summary>
        private static char border = '▓';

        /// <summary>
        /// Nombre de joueurs participant au jeu.
        /// </summary>
        private static int numberOfPlayer = 2;

        /// <summary>
        /// Temps accordé à chaque manche.
        /// </summary>
        private static TimeSpan timePerRound = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Nombre total de manches dans une partie.
        /// </summary>
        private static int numberOfRound = 3;

        /// <summary>
        /// Taille du plateau de jeu en termes de nombre de cases par côté.
        /// Par exemple, une valeur de 4 indique un plateau de 4x4 cases.
        /// </summary>
        private static int sizeOfBoard = 4;

        /// <summary>
        /// Langue des mots à trouver dans le jeu.
        /// </summary>
        private static string language = "FR";



        /// <summary>
        /// Point d'entrée principal de l'application. 
        /// Gère l'affichage de bienvenue, la configuration de la console, 
        /// le menu principal, les paramètres du jeu et l'exécution de la partie.
        /// </summary>
        /// <param name="args">Arguments passés en ligne de commande (non utilisés).</param>
        static void Main(string[] args)
        {
            // Joue le son de bienvenue.
            PlaySoundWelcome();

            // Configure la console (blocage du redimensionnement, dimensions, couleurs).
            SetupConsole();

            // Affiche la fenêtre de bienvenue.
            DisplayWelcome();

            // Affiche le menu principal et retourne le choix de l'utilisateur.
            int type = DisplayMenu();

            if (type == 0)
            {
                // Mode de jeu avec paramètres par défaut.
                Game g = new Game(numberOfPlayer, timePerRound, numberOfRound, sizeOfBoard, language);

                // Initialise les noms des joueurs.
                g.InitializePlayerName();

                // Lance le déroulement du jeu.
                g.Process();
            }
            else if (type == 1)
            {
                // Mode de jeu personnalisé, permet de configurer les paramètres avant de lancer le jeu.
                int selected = 0;

                do
                {
                    // Liste des options de configuration affichées dans le menu.
                    string[] options = {
                                        $"NUMBER OF PLAYERS : {numberOfPlayer}",
                                        $"TIME PER ROUND : {timePerRound.ToString(@"hh\:mm")}",
                                        $"NUMBER OF ROUNDS : {numberOfRound}",
                                        $"SIZE OF BOARD : {sizeOfBoard}",
                                        $"LANGUAGE : {language}",
                                        "GO"
                                       };

                    // Affiche le menu et récupère l'option sélectionnée par l'utilisateur.
                    selected = Menu(options);

                    switch (selected)
                    {
                        case 0:
                            // Modifie le nombre de joueurs.
                            numberOfPlayer = Convert.ToInt32(Console.ReadLine());
                            break;

                        case 1:
                            // Modifie le temps par manche.
                            string timeString = Console.ReadLine();
                            timePerRound = TimeSpan.Parse(timeString);
                            break;

                        case 2:
                            // Modifie le nombre de manches.
                            numberOfRound = Convert.ToInt32(Console.ReadLine());
                            break;

                        case 3:
                            // Modifie la taille du plateau.
                            sizeOfBoard = Convert.ToInt32(Console.ReadLine());
                            break;

                        case 4:
                            // Modifie la langue.
                            language = Console.ReadLine().ToUpper();
                            break;

                        case 5:
                            // Termine la configuration et lance le jeu.
                            break;
                    }
                }
                while (selected != 5);

                // Crée et lance une nouvelle partie avec les paramètres configurés.
                Game g = new Game(numberOfPlayer, timePerRound, numberOfRound, sizeOfBoard, language);
                g.InitializePlayerName();
                g.Process();
            }
            else
            {
                // Quitte l'application avec un message d'au revoir.
                DisplayGoodbye();
            }
        }



        /// <summary>
        /// Configure la console en ajustant ses dimensions, son apparence et en empêchant le redimensionnement de la fenêtre.
        /// </summary>
        public static void SetupConsole()
        {
            // Ce code récupère le menu système de la console et supprime l'option "Redimensionner" pour empêcher le redimensionnement de la fenêtre.
            nint systemMenu = GetSystemMenu(GetConsoleWindow(), false);
            int sizeCommand = 0xF000;
            DeleteMenu(systemMenu, sizeCommand, 0x00000000);

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.BackgroundColor = themes["EGYPT"].Item1;
            Console.ForegroundColor = themes["EGYPT"].Item2;
        }


        /// <summary>
        /// Affiche l'écran d'accueil avec un message de bienvenue et un design personnalisé.<br/>
        /// L'utilisateur peut démarrer en appuyant sur la touche Entrée.
        /// </summary>
        public static void DisplayWelcome()
        {
            ConsoleKey key;

            Console.WriteLine(new string(border, width));
            DisplayCentered("", 4);
            DisplayCentered("  ____     ___     ___     ____   _       _____     _____   ___      _    ");
            DisplayCentered(" | __ )   / _ \\   / _ \\   / ___| | |     | ____|   |__  /  / _ \\    / \\   ");
            DisplayCentered(" |  _ \\  | | | | | | | | | |  _  | |     |  _|       / /  | | | |  / _ \\  ");
            DisplayCentered(" | |_) | | |_| | | |_| | | |_| | | |___  | |___     / /_  | |_| | / ___ \\ ");
            DisplayCentered(" |____/   \\___/   \\___/   \\____| |_____| |_____|   /____|  \\___/ /_/   \\_\\"); //74
            DisplayCentered("", 3);
            DisplayCentered("Press Enter to start !");
            DisplayCentered("", 3);
            DisplayCentered("By Noa & Enzo");
            DisplayCentered("", 1);
            Console.Write(new string(border, width));

            do
            {
                key = Console.ReadKey(true).Key;
            }
            while(key != ConsoleKey.Enter);

            Console.Clear();
        }


        /// <summary>
        /// Affiche le menu principal avec plusieurs options et permet à l'utilisateur de faire un choix.
        /// </summary>
        /// <returns>
        /// Un entier représentant l'option sélectionnée par l'utilisateur :
        /// <list type="bullet">
        /// <item><description>0 : Mode NORMAL</description></item>
        /// <item><description>1 : Mode CUSTOM</description></item>
        /// <item><description>2 : Quitter le programme</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Cette méthode utilise la fonction <c>Menu</c> pour afficher et gérer les choix.
        /// </remarks>
        public static int DisplayMenu()
        {
            string[] options = { "NORMAL", "CUSTOM", "QUIT"};

            int selected = Menu(options);

            return selected;
        }


        /// <summary>
        /// Affiche un message d'au revoir à l'utilisateur.
        /// </summary>
        public static void DisplayGoodbye()
        {
            Console.WriteLine("A FAIRE");
        }


        /// <summary>
        /// Joue un son d'introduction pour l'écran de bienvenue.
        /// </summary>
        public static void PlaySoundWelcome()
        {
            SoundPlayer sound = new SoundPlayer("../../../../Boogle Zoa/ressource/music/Music_Intro.wav");
            sound.Play();
        }


        /// <summary>
        /// Affiche un menu interactif dans la console à partir d'une liste d'options, 
        /// permettant à l'utilisateur de naviguer avec les flèches du clavier et de sélectionner une option.
        /// </summary>
        /// <param name="options">Tableau de chaînes de caractères représentant les options du menu.</param>
        /// <returns>
        /// Un entier représentant l'index de l'option sélectionnée par l'utilisateur.
        /// </returns>
        /// <remarks>
        /// L'utilisateur peut naviguer dans le menu à l'aide des touches fléchées :
        /// <list type="bullet">
        /// <item><description><c>Flèche haut</c> : se déplacer vers l'option précédente.</description></item>
        /// <item><description><c>Flèche bas</c> : se déplacer vers l'option suivante.</description></item>
        /// <item><description><c>Entrée</c> : valider la sélection.</description></item>
        /// </list>
        /// </remarks>
        public static int Menu(string[] options)
        {

            // Réaliser une méthode qui permet d'afficher un menu à partir d'une liste d'options.
            // Renvoie un int qui corresponds à l'option choisi par l'utilisateur

            ConsoleKey key;
            int selected = 0;
            int size = options.Length;

            do
            {
                Console.Clear();
                Console.WriteLine(new string(border, width));

                DisplayCentered("", (height-1-(size*2))/2); //////////////////////////////////

                for (int i = 0; i < size; i++)
                {
                    if (i == selected)
                    {
                        DisplayCentered(options[i], 1, true);
                    }
                    else
                    {
                        DisplayCentered(options[i]);
                    }
                    DisplayCentered("", 1);
                }

                DisplayCentered("", (height - 1 - (size * 2)) / 2); /////////////////////////////////
                Console.Write(new string(border, width));

                key = Console.ReadKey().Key;

                if (key == ConsoleKey.DownArrow) selected = (selected + 1) % size;
                else if (key == ConsoleKey.UpArrow) selected = Math.Abs((selected - 1) % size);
            }
            while (key != ConsoleKey.Enter);

            Console.Clear();

            return selected;
        }


        /// <summary>
        /// Affiche un texte
        /// </summary>
        /// <param name="text"></param>
        /// <param name="n"></param>
        /// <param name="selected"></param>
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
