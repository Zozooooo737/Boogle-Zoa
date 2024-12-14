namespace Boogle_Zoa
{
    public class Game
    {
        private int numberOfPlayers;
        private TimeSpan timePerRound;
        private int numberOfRound;
        private int sizeOfBoard;
        private string language;
        private DictionaryWords dictionaryWords;

        private Player[] players;

        private static readonly Dictionary<char, (int, int)> lettersInformation;

        private Random random;

        private IDisplay _display;



        static Game()
        {
            lettersInformation = new Dictionary<char, (int, int)> { };
            string[] lines = File.ReadAllLines( "../../../data/Lettres.txt");
            for (int l = 0; l < lines.Length; l++)
            {
                string[] information = lines[l].Split(';');
                lettersInformation.Add(char.Parse(information[0]), (Convert.ToInt32(information[1]), Convert.ToInt32(information[2])));
            }
        }


        public Game(int numberOfPlayers, TimeSpan timePerRound, int numberOfRound, int sizeOfBoard, string language, IDisplay display)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.timePerRound = timePerRound;
            this.numberOfRound = numberOfRound;
            this.sizeOfBoard = sizeOfBoard;
            this.language = language;

            dictionaryWords = new DictionaryWords(language);

            players = new Player[numberOfPlayers];

            random = new Random();

            _display = display;
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

            for (int r = 0; r < numberOfRound; r++)
            {

                for (int p = 0; p < numberOfPlayers; p++)
                {
                    duration =timePerRound;
                    startTime = DateTime.Now;
                    endTime = startTime + duration;

                    Board board = CreateBoard();
                    _display.DisplayGame(r, players[p].Name, board);

                    while (DateTime.Now < endTime)
                    {
                        string word;

                        while (true)
                        {
                            word = _display.GetWord();
                            if(word != "")
                            {
                                break;
                            }
                        }

                        if (board.GameBoardTest(word, dictionaryWords))
                        {
                            players[p].AddWord(word, lettersInformation);
                            _display.DisplayMessage($" The word is valid! Your score is now at {players[p].Score} points!");
                        }
                        else
                        {
                            _display.DisplayMessage(" The word is unvalid... Try again !");
                        }
                    }
                }
            }

            int scoreWinner = players[0].Score;
            Player winner = players[0];

            for (int p = 1; p < numberOfPlayers; p++)
            {
                if (players[p].Score > scoreWinner)
                {
                    scoreWinner = players[p].Score;
                    winner = players[p];
                }
            }

            GenerateWordCloud(winner);

            _display.DisplayWinner(winner);
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
            WordCloud wordCloud = new WordCloud(player.WordsFound.ToArray());
            wordCloud.SaveAndOpenImage(player.Name + ".png");
        }
    }
}
