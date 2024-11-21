using System.Diagnostics;


namespace Boogle_Zoa
{
    public class Game
    {
        // Le main donne la main à tour de rôle à un des joueurs (par défaut 2 joueurs)
        // Laps de temps à définir dans le Main (ex: 6min)
        // Chaque joueur a une minute (voir la classe DateTime et TimeSpan) pour trouver les mots sur une nouvelle instance de plateau.
        // Le joueur saisit un mot après l'autre, à chaque fois nous devons tester l'éligibilité du mot
        // A la fin du temps imparti, changement de joueur.
        // A la fin du temps de jeu on affiche les scores des joueurs et le gagnant






        // D E B U T

        // Initialiser les données : NbrJoueur, TpsRound, NbrRound (3) , FR/EN, etc



        // Boucle de jeu : 3 * ( 2 * ("Affiche plateau", Boucle ( "Propose Mot", "Verif Mot" ) - 30 sec ) )


        // Affichage du Résultat 

        // F I N

        private int numberOfPlayers;
        private TimeSpan timePerRound;
        private int numberOfRound;
        private int sizeOfBoard;
        private string language;
        private DictionaryWords dictionaryWords;

        private Player[] players;

        private static readonly Dictionary<char, (int, int)> lettersInformation;

        private Random random;


        // A REVOIR, comment creer ca a partir du fichier

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
                    Console.WriteLine($"Player {i + 1} - Enter youtr name :");
                    name = Console.ReadLine();
                }
                while ( name == "" || name == null );

                players[i] = new Player(name);
            }
        }


        // 3, 2, 1
        // 
        public void Process()
        {
            for (int r = 0; r < numberOfRound; r++)
            {
                Console.WriteLine($"ROUND {r + 1}\n");

                for (int p = 0; p < numberOfPlayers; p++)
                {
                    Console.WriteLine($"Your turn {players[p].Name} !\n");

                    // Size doit faire 4 sinon probleme sur Board
                    Dice[] dices = new Dice[sizeOfBoard*sizeOfBoard];
                    CreateDices(dices);

                    // Création de la Board
                    Board board = new Board(dices);
                    Console.WriteLine(board.toString());

                    // Boucle Entrer mot / Verif le mot / ajoute le mot au joueur
                    Console.WriteLine($"Which words can you see ?\n");
                    string word = Console.ReadLine();
                    Console.Write(word);
                    if(board.GameBoardTest(word, dictionaryWords))
                    {
                        players[p].AddWord(word, lettersInformation); 
                        Console.Write($" The word is valid! Your score is now at {players[p].Score} points!");
                    }
                    else
                    {
                        Console.Write(" The word is unvalid... Try again !");
                    }

                    Thread.Sleep(2000);

                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);

                    // afficher gagnants et score et nuage de mots et les tétons de Enzo

                    // Temps par round
                    // Fini 

                }
                // Fin de round
            }
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
