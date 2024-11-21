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

        private Player[] players;

        private Random random;


        // A REVOIR, comment creer ca a partir du fichier

        Dictionary<char, (int, int)> lettersInformation = new Dictionary<char, (int, int)>
            {
                {'A', (1, 9)},
                {'B', (3, 2)},
                {'C', (3, 2)},
                {'D', (2, 3)},
                {'E', (1, 15)},
                {'F', (4, 2)},
                {'G', (2, 2)},
                {'H', (4, 2)},
                {'I', (1, 8)},
                {'J', (8, 1)},
                {'K', (10, 1)},
                {'L', (1, 5)},
                {'M', (2, 3)},
                {'N', (1, 6)},
                {'O', (1, 6)},
                {'P', (3, 2)},
                {'Q', (8, 1)},
                {'R', (1, 6)},
                {'S', (1, 6)},
                {'T', (1, 6)},
                {'U', (1, 6)},
                {'V', (4, 2)},
                {'W', (10, 1)},
                {'X', (10, 1)},
                {'Y', (10, 1)},
                {'Z', (10, 1)}
            };

        public Game(int numberOfPlayers, TimeSpan timePerRound, int numberOfRound, int sizeOfBoard, string language)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.timePerRound = timePerRound;
            this.numberOfRound = numberOfRound;
            this.sizeOfBoard = sizeOfBoard;
            this.language = language;

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
                    board.toString();

                    // Boucle Entrer mot / Verif le mot / ajoute le mot au joueur

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
