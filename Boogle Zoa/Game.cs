namespace Boogle_Zoa
{
    /// <summary>
    /// Représente une partie du jeu avec plusieurs joueurs (<see cref="Player"/>), un plateau (<see cref="Board"/>), et des règles spécifiques 
    /// comme le nombre de tours (<see cref="numberOfRounds"/>) et le temps par tour (<see cref="timePerRound"/>).
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Nombre de joueurs dans la partie.
        /// </summary>
        private readonly int numberOfPlayers;

        /// <summary>
        /// Durée de chaque tour.
        /// </summary>
        private readonly TimeSpan timePerRound;

        /// <summary>
        /// Nombre de tours dans la partie.
        /// </summary>
        private readonly int numberOfRounds;

        /// <summary>
        /// Taille d'un côté du plateau de jeu.
        /// </summary>
        private readonly int sideOfBoard;

        /// <summary>
        /// Langue utilisée pour le jeu.
        /// </summary>
        private readonly string language;

        /// <summary>
        /// Dictionnaire des mots de la langue spécifiée.
        /// </summary>
        private readonly DictionaryWords dictionaryWords;

        /// <summary>
        /// Tableau des joueurs de la partie.
        /// </summary>
        private readonly Player[] players;

        /// <summary>
        /// Informations sur les lettres (poids et quantité disponible).
        /// </summary>
        /// <remarks>Un dictionnaire où chaque clé est une lettre et chaque valeur est un tuple représentant le poids et le nombre d'apparition de cette lettre.</remarks>
        private readonly Dictionary<char, (int, int)> lettersInformation;

        /// <summary>
        /// Générateur aléatoire.
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// Interface utilisée pour afficher les éléments de la partie.
        /// </summary>
        private readonly IDisplay display;



        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Game"/>.
        /// </summary>
        /// <param name="numberOfPlayers">Nombre de joueurs dans la partie.</param>
        /// <param name="timePerRound">Durée de chaque tour.</param>
        /// <param name="numberOfRounds">Nombre de tours dans la partie.</param>
        /// <param name="sideOfBoard">Taille d'un côté du plateau de jeu.</param>
        /// <param name="language">Langue utilisée pour le jeu.</param>
        /// <param name="display">Interface utilisée pour afficher les éléments de la partie.</param>
        /// <remarks>
        /// Les quantités de lettres sont ajustées dynamiquement en fonction des paramètres de la partie
        /// (nombre de joueurs, nombre de manches, et taille du plateau de jeu) via la méthode <see cref="AdjustLetterStocks"/>.
        /// </remarks>
        public Game(int numberOfPlayers, TimeSpan timePerRound, int numberOfRounds, int sideOfBoard, string language, IDisplay display)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.timePerRound = timePerRound;
            this.numberOfRounds = numberOfRounds;
            this.sideOfBoard = sideOfBoard;
            this.language = language;

            dictionaryWords = new DictionaryWords(language);
            lettersInformation = ReadLettersInformation(language);

            AdjustLetterStocks();
            Dice.ResetUsedLetters();

            players = new Player[numberOfPlayers];

            random = new Random();
            this.display = display;
        }



        /// <summary>
        /// Initialise les joueurs de la partie avec leurs noms respectifs.
        /// </summary>
        /// <param name="names">Tableau contenant les noms des joueurs.</param>
        /// <remarks>
        /// Cette méthode associe chaque nom du tableau <paramref name="names"/> à un joueur et les ajoute à la liste des joueurs de la partie. Le nombre de noms 
        /// dans <paramref name="names"/> doit correspondre au nombre de joueurs initialisé dans <see cref="Game.numberOfPlayers"/>.
        /// </remarks>
        public void SetNameOfPlayers(string[] names)
        {
            for(int i = 0; i < numberOfPlayers; i++)
            {
                Player p = new Player(names[i]);
                players[i] = p;
            }
            Console.Clear();
        }


        /// <summary>
        /// Exécute le processus principal du jeu, en gérant les tours et les actions des joueurs.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Le processus suit ces étapes pour chaque tour de jeu :
        /// </para>
        /// <list type="number">
        ///     <item><description>Un joueur est sélectionné pour jouer son tour, avec un plateau de jeu généré.</description></item>
        ///     <item><description>Le joueur saisit un mot et celui-ci est vérifié pour sa validité sur le plateau et dans le dictionnaire.</description></item>
        ///     <item><description>Les scores sont mis à jour si le mot est valide et non déjà trouvé par ce joueur.</description></item>
        ///     <item><description>Des messages appropriés sont affichés pour guider le joueur selon la validité de son mot.</description></item>
        /// </list>
        /// <para>
        /// À la fin de tous les tours, le score final des joueurs est comparé et le gagnant est annoncé. Les nuages de mots de tous les joueurs sont générés et affichés.
        /// </para>
        /// </remarks>
        public void Process()
        {
            TimeSpan duration;
            DateTime startTime;
            DateTime endTime;

            for (int r = 0; r < numberOfRounds; r++)
            {

                for (int p = 0; p < numberOfPlayers; p++)
                {
                    duration = timePerRound;
                    startTime = DateTime.Now;
                    endTime = startTime + duration;

                    Board board = CreateBoard();

                    display.DisplayCountDown();
                    display.DisplayGame(r, players[p].Name, board);

                    while (DateTime.Now < endTime)
                    {
                        string word = "";

                        if (display.IsWordAvailable())
                        {
                            word = display.GetWord();

                        } 

                        if (string.IsNullOrWhiteSpace(word))
                        { 
                            continue;
                        }
                        else if (word.Length < 2)
                        {
                            display.DisplayMessage($"The word must be at least 2 letters long.. Try again !");
                            continue;
                        }


                        int isFound = board.GameBoardTest(word, dictionaryWords);

                        if (isFound == 0)
                        {
                            bool isAdded = players[p].AddWord(word, lettersInformation);
                            
                            if (isAdded)
                            {
                                display.DisplayMessage($"The word is valid! Your score is now at {players[p].Score} points!");
                            }
                            else if (!isAdded)
                            {
                                display.DisplayMessage($"The word is valid! But you have already found the word... Try again !");
                            }
                        }
                        else if (isFound == 1)
                        {
                            display.DisplayMessage("The word is not present on the board... Try again !");
                        }
                        else if (isFound == 2)
                        {
                            display.DisplayMessage("The word is not present in the dictionary... Try again !");
                        }
                    }
                }
            }

            int scoreWinner = 0;
            Player winner = players[0];

            foreach(Player player in players)
            {
                GenerateWordCloud(player);

                if (player.Score > scoreWinner)
                {
                    scoreWinner = player.Score;
                    winner = player;
                }
            }

            display.PlayEndingSound();
            display.DisplayWinner(winner);
        }


        /// <summary>
        /// Crée un nouveau plateau de jeu à partir de dés.
        /// </summary>
        /// <returns>Un objet <see cref="Board"/> représentant le plateau de jeu créé.</returns>
        /// <remarks>
        /// La méthode génère un tableau de dés (<see cref="Dice"/>) de taille égale au carré de <see cref="sideOfBoard"/>.
        /// Chaque dé est lancé aléatoirement à l'aide du générateur <see cref="random"/> avant d'être ajouté au plateau.
        /// </remarks>
        private Board CreateBoard()
        {
            Dice[] dices = new Dice[sideOfBoard * sideOfBoard];

            for (int i = 0; i < dices.Length; i++)
            {
                dices[i] = new Dice(random, lettersInformation);
                dices[i].Roll(random);
            }

            Board board = new Board(dices);

            return board;
        }


        /// <summary>
        /// Ajuste les quantités de lettres disponibles en fonction des paramètres de la partie.
        /// </summary>
        /// <remarks>
        /// Le calcul est basé sur le nombre total de lettres nécessaires pour couvrir le plateau de jeu pour tous les joueurs, toutes les manches et toutes les tailles de plateau. Les quantités existantes sont ajustées
        /// proportionnellement, avec un facteur d'ajustement calculé en fonction de la différence entre le stock actuel et le stock nécessaire.
        /// </remarks>
        private void AdjustLetterStocks()
        {
            int totalStock = sideOfBoard * sideOfBoard * numberOfPlayers * numberOfRounds;

            int currentStock = 0;
            foreach ((int, int) info in lettersInformation.Values)
            {
                currentStock += info.Item2;
            }

            double adjustmentFactor = (double)totalStock / currentStock;

            foreach (char letter in lettersInformation.Keys)
            {
                (int, int) info = (lettersInformation[letter].Item1, (int)Math.Round(lettersInformation[letter].Item2 * adjustmentFactor) + 1);
                lettersInformation[letter] = info;
            }
        }


        /// <summary>
        /// Génère un nuage de mots pour un joueur à partir des mots qu'il a trouvés durant la partie.
        /// </summary>
        /// <param name="player">Joueur.</param>
        private void GenerateWordCloud(Player player)
        {
            WordCloud wordCloud = new WordCloud(player);
            wordCloud.SaveAndOpenImage();
        }



        /// <summary>
        /// Lit les informations des lettres depuis un fichier en fonction de la langue spécifiée.
        /// </summary>
        /// <param name="language">Langue du jeu.</param>
        /// <returns>
        /// Un dictionnaire où chaque clé est une lettre et chaque valeur est un tuple représentant le poids et le nombre d'apparitions de cette lettre dans le jeu.
        /// </returns>
        private static Dictionary<char, (int, int)> ReadLettersInformation(string language)
        {
            Dictionary<char, (int, int)>  dico = new Dictionary<char, (int, int)> { };
            string filepath;

            if (language == "FR")
            {
                filepath = "../../../data/LettresFR.txt";
            }
            else
            {
                filepath = "../../../data/LettresEN.txt";
            }

            StreamReader sReader = null;

            try
            {
                sReader = new StreamReader(filepath);

                string line;

                while ((line = sReader.ReadLine()) != null) // Lire chaque ligne jusqu'à la fin du fichier
                {
                    string[] information = line.Split(';');
                    dico.Add(char.Parse(information[0]), (Convert.ToInt32(information[1]), Convert.ToInt32(information[2])));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sReader != null) { sReader.Close(); }
            }
            return dico;
        }
    }
}
