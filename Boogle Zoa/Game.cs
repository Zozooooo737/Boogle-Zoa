namespace Boogle_Zoa
{
    public class Game
    {
        private int numberOfPlayers;
        private TimeSpan timePerRound;
        private int numberOfRounds;
        private int sideOfBoard;
        private string language;
        private DictionaryWords dictionaryWords;

        private Player[] players;

        private Dictionary<char, (int, int)> lettersInformation;

        private Random random;

        private IDisplay display;



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



        public void SetNameOfPlayers(string[] names)
        {
            for(int i = 0; i < numberOfPlayers; i++)
            {
                Player p = new Player(names[i]);
                players[i] = p;
            }
            Console.Clear();
        }


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

                    display.DisplayGame(r, players[p].Name, board);

                    while (DateTime.Now < endTime)
                    {
                        string word;

                        word = display.GetWord();
                        
                        // Ignore si aucun mot n'est saisi.
                        if (string.IsNullOrWhiteSpace(word))
                        {
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

            display.DisplayWinner(winner);
        }


        public Board CreateBoard()
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


        private void AdjustLetterStocks()
        {
            // Calcule le stock total nécessaire
            int totalStock = sideOfBoard * sideOfBoard * numberOfPlayers * numberOfRounds;

            // Calcule le stock actuel
            int currentStock = 0;
            foreach ((int, int) info in lettersInformation.Values)
            {
                currentStock += info.Item2;
            }

            // Calcule le facteur d'ajustement
            double adjustmentFactor = (double)totalStock / currentStock;

            // Ajuste les stocks proportionnellement
            foreach (char letter in lettersInformation.Keys)
            {
                (int, int) info = (lettersInformation[letter].Item1, (int)Math.Round(lettersInformation[letter].Item2 * adjustmentFactor) + 1);
                lettersInformation[letter] = info;
            }
        }


        private void GenerateWordCloud(Player player)
        {
            WordCloud wordCloud = new WordCloud(player);
            wordCloud.SaveAndOpenImage();
        }



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
