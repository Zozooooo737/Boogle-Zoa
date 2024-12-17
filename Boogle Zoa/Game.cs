namespace Boogle_Zoa
{
    public class Game
    {
        private int numberOfPlayers;
        private TimeSpan timePerRound;
        private int numberOfRounds;
        private int sizeOfBoard;
        private string language;
        private DictionaryWords dictionaryWords;

        private Player[] players;

        private static Dictionary<char, (int, int)> lettersInformation;

        private Random random;

        private IDisplay display;


        public Game(int numberOfPlayers, TimeSpan timePerRound, int numberOfRounds, int sizeOfBoard, string language, IDisplay display)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.timePerRound = timePerRound;
            this.numberOfRounds = numberOfRounds;
            this.sizeOfBoard = sizeOfBoard;
            this.language = language;

            dictionaryWords = new DictionaryWords(language);
            ReadLettersInformation(language);

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

                        while (true)
                        {
                            word = display.GetWord();
                            if (word != "")
                            {
                                break;
                            }
                        }

                        if (board.GameBoardTest(word, dictionaryWords))
                        {
                            players[p].AddWord(word, lettersInformation);
                            display.DisplayMessage($" The word is valid! Your score is now at {players[p].Score} points!");
                        }
                        else
                        {
                            display.DisplayMessage(" The word is unvalid... Try again !");
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


        public void AdjustLetterStocks()
        {
            // Calcule le stock total nécessaire
            int totalStock = sizeOfBoard * sizeOfBoard * numberOfPlayers * numberOfRounds;

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
                (int, int) info = (lettersInformation[letter].Item1, (int)Math.Round(lettersInformation[letter].Item2 * adjustmentFactor)+1);
                lettersInformation[letter] = info;
            }
        }


        private Board CreateBoard()
        {
            Dice[] dices = new Dice[sizeOfBoard * sizeOfBoard];

            for (int i = 0; i < dices.Length; i++)
            {
                dices[i] = new Dice(random, lettersInformation);
            }

            Board board = new Board(dices);

            return board;
        }


        private void GenerateWordCloud(Player player)
        {
            WordCloud wordCloud = new WordCloud(player);
            wordCloud.SaveAndOpenImage();
        }


        private void ReadLettersInformation(string language)
        {
            lettersInformation = new Dictionary<char, (int, int)> { };
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
                    lettersInformation.Add(char.Parse(information[0]), (Convert.ToInt32(information[1]), Convert.ToInt32(information[2])));
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
        }
    }
}
