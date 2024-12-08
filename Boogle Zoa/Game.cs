using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;


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


        // Optimisation : nous effectuons l'initialisation de lettersInformation qu'une seule fois
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



        // Ecris message de validité
        public void SetNameOfPlayers(string[] names)
        {
            for(int i = 0; i < numberOfPlayers; i++)
            {
                Player p = new Player(names[i]);
                players[i] = p;
            }
            Console.Clear();
        }


        // 3, 2, 1
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

                    // Size doit faire 4 sinon probleme sur Board
                    Dice[] dices = new Dice[sizeOfBoard * sizeOfBoard];
                    CreateDices(dices);

                   

                    // Création de la Board
                    Board board = new Board(dices);
                    _display.DisplayGame(r, players[p].Name, board);

                    while (DateTime.Now < endTime)
                    {
                        // Boucle Entrer mot / Verif le mot / ajoute le mot au joueur
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

                    // Améliorer le fait que tant qu'il n'y a pas de mots entré alors le end time n'est pas atteint

                }
                // Fin de round
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
            _display.DisplayWinner(winner);
        }


        private void CreateDices(Dice[] dices)
        {
            for(int i=0; i<dices.Length; i++)
            {
                dices[i] = new Dice(random, lettersInformation);
            }
        }
    }
}
