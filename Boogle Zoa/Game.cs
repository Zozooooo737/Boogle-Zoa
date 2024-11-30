using System.Diagnostics;
using System.Runtime.InteropServices;


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


        public Game(int numberOfPlayers, TimeSpan timePerRound, int numberOfRound, int sizeOfBoard, string language)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.timePerRound = timePerRound;
            this.numberOfRound = numberOfRound;
            this.sizeOfBoard = sizeOfBoard;
            this.language = language;

            dictionaryWords = new DictionaryWords(language);

            players = new Player[numberOfPlayers];

            random = new Random();
        }



        // Ecris message de validité
        public void InitializePlayerName()
        {
            for(int i = 0; i < numberOfPlayers; i++)
            {
                string name = "";
                do
                {
                    Console.WriteLine($"Player {i + 1} - Enter your name :");
                    name = Console.ReadLine();
                }
                while ( name == "" || name == null );

                players[i] = new Player(name);
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
                Program.DisplayCentered(($"ROUND {r + 1}\n"));

                for (int p = 0; p < numberOfPlayers; p++)
                {
                    duration = timePerRound;
                    startTime = DateTime.Now;
                    endTime = startTime + duration;

                    // Size doit faire 4 sinon probleme sur Board
                    Dice[] dices = new Dice[sizeOfBoard * sizeOfBoard];
                    CreateDices(dices);

                    Program.DisplayCentered(($"Your turn {players[p].Name} !\n"));

                    // Création de la Board
                    Board board = new Board(dices);
                    Program.DisplayCentered((board.toString()));

                    Console.WriteLine($"Which words can you see ?\n");

                    while (DateTime.Now < endTime)
                    {
                        // Boucle Entrer mot / Verif le mot / ajoute le mot au joueur
                        string word = Console.ReadLine();
                        Console.Write(word);
                        if (board.GameBoardTest(word, dictionaryWords))
                        {
                            players[p].AddWord(word, lettersInformation);
                            Console.Write($" The word is valid! Your score is now at {players[p].Score} points!");
                        }
                        else
                        {
                            Console.Write(" The word is unvalid... Try again !");
                        }

                        Thread.Sleep(1000);

                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.BufferWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                    }

                    // Améliorer le fait que tant qu'il n'y a pas de mots entré alors le end time n'est pas atteint

                }
                // Fin de round
            }
            int scoreWinner = players[0].Score;
            string winner = players[0].Name;
            for (int p = 1; p < numberOfPlayers; p++)
            {
                if (players[p].Score > scoreWinner)
                {
                    scoreWinner = players[p].Score;
                    winner = players[p].Name;
                }
            }
            Console.WriteLine($"Congrats {winner}, you won with a score of {scoreWinner} points !");
            Console.WriteLine(" ----");
            Console.WriteLine("|    |");
            Console.WriteLine("|  * |    <- téton de enzo sur Minecraft");
            Console.WriteLine("|    |");
            Console.WriteLine(" ----");
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
