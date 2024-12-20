namespace Boogle_Zoa
{
    public class Program
    {
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
        /// Instance utilisé pour réaliser l'affichage du Jeu.
        /// </summary>
        private readonly static ConsoleDisplay display = new ConsoleDisplay();



        /// <summary>
        /// Point d'entrée principal de l'application. 
        /// Gère l'affichage de bienvenue, la configuration de la console, 
        /// le menu principal, les paramètres du jeu et l'exécution de la partie.
        /// </summary>
        /// <param name="args">Arguments passés en ligne de commande (non utilisés).</param>
        private static void Main(string[] args)
        {
            display.PlayWelcomeSound();

            display.SetupDisplay();

            display.DisplayWelcome();
            
            int type;

            do
            {
                type = DisplayMenu();

                if (type == 0)
                {
                    LaunchGame(true);
                }
                else if (type == 1)
                {
                    int selected = 0;

                    do
                    {
                        selected = DisplayCustom();

                        SetOptions(selected);
                    }
                    while (selected != 5);

                    LaunchGame();
                }
                else if (type == 2)
                {
                    DisplayThemes();
                }

            } while (type != 3);

            display.DisplayGoodbye();
        }


        /// <summary>
        /// Initialise et lance une nouvelle partie du jeu avec les paramètres configurés. <br/>
        /// Si <paramref name="normal"/> est défini sur <c>true</c>, une partie par défaut est lancée avec les paramètres prédéfinis.
        /// </summary>
        private static void LaunchGame(bool normal = false)
        {
            if (normal)
            {
                numberOfPlayer = 2;
                timePerRound = TimeSpan.FromSeconds(30);
                numberOfRound = 3;
                sizeOfBoard = 4;
                language = "FR";
            }

            Game g = new Game(numberOfPlayer, timePerRound, numberOfRound, sizeOfBoard, language, display);

            g.SetNameOfPlayers(display.InitializePlayerName(numberOfPlayer));
            g.Process();
        }


        /// <summary>
        /// Affiche la fenêtre du menu principal avec plusieurs options et permet à l'utilisateur de faire un choix.
        /// </summary>
        /// <returns>
        /// Un entier représentant l'option sélectionnée par l'utilisateur :
        /// <list type="bullet">
        /// <item><description>0 : Mode NORMAL</description></item>
        /// <item><description>1 : Mode CUSTOM</description></item>
        /// <item><description>2 : Accès au THEMES</description></item>
        /// <item><description>3 : Quitter le programme</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Cette méthode utilise la fonction <c>Menu</c> pour afficher et gérer les choix.
        /// </remarks>
        private static int DisplayMenu()
        {
            string[] options = { "NORMAL", "CUSTOM", "THEMES", "QUIT" };

            int selected = display.Menu(options);

            return selected;
        }


        /// <summary>
        /// Affiche la fenêtre du menu custom avec plusieurs options et permet à l'utilisateur de choisir ce qu'il veut modifier.
        /// </summary>
        /// <returns>
        /// Un entier représentant l'option sélectionnée par l'utilisateur :
        /// <list type="bullet">
        /// <item><description>0 : Nombre de joueurs</description></item>
        /// <item><description>1 : Temps par manche</description></item>
        /// <item><description>2 : Nombre de manches</description></item>
        /// <item><description>3 : Taille du plateau</description></item>
        /// <item><description>4 : Langue</description></item>
        /// <item><description>5 : Lancer le jeu</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Cette méthode utilise la fonction <c>Menu</c> pour afficher et gérer les choix.
        /// </remarks>
        private static int DisplayCustom()
        {
            string[] options = {
                                $"NUMBER OF PLAYERS : {numberOfPlayer}",
                                $"TIME PER ROUND : {timePerRound.ToString(@"mm\:ss")}",
                                $"NUMBER OF ROUNDS : {numberOfRound}",
                                $"SIZE OF BOARD : {sizeOfBoard}",
                                $"LANGUAGE : {language}",
                                "GO"
                               };

            int selected = display.Menu(options);

            return selected;
        }


        /// <summary>
        /// Affiche la fenêtre pour modifier une option spécifique du jeu en fonction de l'entrée donnée.
        /// </summary>
        /// <param name="option">Option à modifier.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description><c>0</c> : Modifier le nombre de joueurs.</description></item>
        /// <item><description><c>1</c> : Modifier le temps par manche.</description></item>
        /// <item><description><c>2</c> : Modifier le nombre de manches.</description></item>
        /// <item><description><c>3</c> : Modifier la taille du plateau.</description></item>
        /// <item><description><c>4</c> : Modifier la langue (FR ou EN).</description></item>
        /// <item><description><c>5</c> : Terminer la configuration et lancer le jeu.</description></item>
        /// </list>
        /// </remarks>
        private static void SetOptions(int option)
        {
            switch (option)
            {
                case 0:
                    numberOfPlayer = display.GetValideNumber("Entrez un nombre de joueurs entre 2 et 5 :", 2, 5);
                    break;

                case 1:
                    timePerRound = display.GetValideTime("Entrez un temps par manche entre 00:10 et 02:00 :", "00:00:10", "00:02:00");
                    break;

                case 2:
                    numberOfRound = display.GetValideNumber("Entrez un nombre de manches entre 1 et 6 :", 1, 6);
                    break;

                case 3:
                    sizeOfBoard = display.GetValideNumber("Entrez une taille de matrice entre 3 et 7 :", 3, 7);
                    break;

                case 4:
                    language = display.GetValideWord("Entrez une langue entre FR et EN :", ["FR", "EN"]);
                    break;

                case 5:
                    break;
            }
        }


        /// <summary>
        /// Affiche la fenêtre de personnalisation du thème du jeu avec plusieurs options et permet à l'utilisateur de choisir ce qu'il veut modifier.
        /// </summary>
        /// <returns>
        /// Un entier représentant le thème sélectionné par l'utilisateur :
        /// <list type="bullet">
        /// <item><description>0 : Thème EGYPT</description></item>
        /// <item><description>1 : Thème DARK</description></item>
        /// <item><description>2 : Thème LIGHT</description></item>
        /// <item><description>3 : Thème ARCANE</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Cette méthode utilise la fonction <c>Menu</c> pour afficher et gérer les choix.
        /// </remarks>
        /// Problème : Bug Graphique
        private static void DisplayThemes()
        {
            string[] options = ConsoleDisplay.Themes.Keys.ToArray();

            int selected = display.Menu(options);

            ConsoleDisplay.Theme = options[selected];
        }
    }
}
