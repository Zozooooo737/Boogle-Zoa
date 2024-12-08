using System.Runtime.InteropServices;
using System.Media;


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
        private static IDisplay display = new ConsoleDisplay();



        /// <summary>
        /// Point d'entrée principal de l'application. 
        /// Gère l'affichage de bienvenue, la configuration de la console, 
        /// le menu principal, les paramètres du jeu et l'exécution de la partie.
        /// </summary>
        /// <param name="args">Arguments passés en ligne de commande (non utilisés).</param>
        static void Main(string[] args)
        {
            // Joue le son de bienvenue.
            display.PlaySoundWelcome();

            // Configure la console (blocage du redimensionnement, dimensions, couleurs).
            display.SetupDisplay();

            // Affiche la fenêtre de bienvenue.
            display.DisplayWelcome();
            
            int type;

            do
            {
                // Affiche le menu principal et retourne le choix de l'utilisateur.
                type = DisplayMenu();

                if (type == 0)
                {
                    // Mode de jeu avec paramètres par défaut.
                    Game g = new Game(numberOfPlayer, timePerRound, numberOfRound, sizeOfBoard, language, display);

                    // Initialise les noms des joueurs.
                    g.SetNameOfPlayers(display.InitializePlayerName(numberOfPlayer));

                    // Lance le déroulement du jeu.
                    g.Process();
                }
                else if (type == 1)
                {
                    // Mode de jeu personnalisé, permet de configurer les paramètres avant de lancer le jeu.
                    int selected = 0;

                    do
                    {
                        selected = DisplayCustom();

                        switch (selected)
                        {
                            case 0:
                                // Modifie le nombre de joueurs.
                                numberOfPlayer = display.GetValideNumber("Entrez un nombre de joueurs entre 2 et 5 :", 2, 5);
                                break;

                            case 1:
                                // Modifie le temps par manche.
                                timePerRound = display.GetValideTime("Entrez un temps par manche entre 00:10 et 02:00 :", "00:00:10", "00:02:00");
                                break;

                            case 2:
                                // Modifie le nombre de manches.
                                numberOfRound = display.GetValideNumber("Entrez un nombre de manches entre 1 et 6 :", 1, 6);
                                break;

                            case 3:
                                // Modifie la taille du plateau.
                                sizeOfBoard = display.GetValideNumber("Entrez une taille de matrice entre 3 et 7 :", 3, 7);
                                break;

                            case 4:
                                // Modifie la langue.
                                language = display.GetValideWord("Entrez une langue entre FR et EN :", ["FR", "EN"]);
                                break;

                            case 5:
                                // Termine la configuration et lance le jeu.
                                break;
                        }
                    }
                    while (selected != 5);

                    // Crée et lance une nouvelle partie avec les paramètres configurés.
                    Game g = new Game(numberOfPlayer, timePerRound, numberOfRound, sizeOfBoard, language, display);

                    g.SetNameOfPlayers(display.InitializePlayerName(numberOfPlayer));
                    g.Process();
                }
                else if (type == 2)
                {
                    string[] options = ConsoleDisplay.Themes.Keys.ToArray();

                    int selected = display.Menu(options);

                    ConsoleDisplay.Theme = options[selected];
                    // Probleme graphique : ???
                }

            } while (type != 3);

            // Quitte l'application avec un message d'au revoir.
            display.DisplayGoodbye();       
        }


        /// <summary>
        /// Affiche la fenêtre du menu principal avec plusieurs options et permet à l'utilisateur de faire un choix.
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
        public static int DisplayCustom()
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
    }
}
