namespace Boogle_Zoa
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test \n");

            Random random = new Random();

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

            Dice A1 = new Dice(random, lettersInformation);
            Dice A2 = new Dice(random, lettersInformation);
            Dice A3 = new Dice(random, lettersInformation);
            Dice A4 = new Dice(random, lettersInformation);
            Dice B1 = new Dice(random, lettersInformation);
            Dice B2 = new Dice(random, lettersInformation);
            Dice B3 = new Dice(random, lettersInformation);
            Dice B4 = new Dice(random, lettersInformation);
            Dice C1 = new Dice(random, lettersInformation);
            Dice C2 = new Dice(random, lettersInformation);
            Dice C3 = new Dice(random, lettersInformation);
            Dice C4 = new Dice(random, lettersInformation);
            Dice D1 = new Dice(random, lettersInformation);
            Dice D2 = new Dice(random, lettersInformation);
            Dice D3 = new Dice(random, lettersInformation);
            Dice D4 = new Dice(random, lettersInformation);

            Dice[] listOfDice = new Dice[16] { A1, A2, A3, A4, B1, B2, B3, B4, C1, C2, C3, C4, D1, D2, D3, D4 };

            Board B = new Board(listOfDice);

            Console.WriteLine(B.toString());

            //DictionaryWords Dico = new DictionaryWords("../../../data/MotsPossiblesFR.txt", "FR");
            
            

            List<string> sameLengthWords = [];
            List<string> sameLetterWords = ["cc", "aa", "bb"];

            foreach (string d in sameLengthWords)
            {
                Console.WriteLine(d);
            }

            TimeSpan secSpan = TimeSpan.FromSeconds(30);

            Game g = new Game(2, secSpan, 3, 4, "FR");

            g.InitializePlayerName();
            g.Start();
            

            
        }

        #region SauvegardeDatajensaisrien
        public int FindWord(string word, char[,] board, int x, int y, int index = 0)
        {
            // 
            if (index == word.Length - 1 && board[x, y] == word[index])
            {
                return 1;
            }

            // Vérifie les limites et la correspondance des caractères
            if (x < 0 || x >= board.GetLength(0) || y < 0 || y >= board.GetLength(1) || board[x, y] != word[index])
            {
                return 0;
            }

            // Cas 1 : Les cases dans les coins ont 3 voisins adjacents
            if (x == 0 && y == 0)
            {
                return FindWord(word, board, x, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y, index + 1);
            }
            else if (x == 0 && y == board.GetLength(1) - 1)
            {
                return FindWord(word, board, x + 1, y, index + 1) +
                        FindWord(word, board, x + 1, y - 1, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1);
            }
            else if (x == board.GetLength(0) - 1 && y == board.GetLength(1) - 1)
            {
                return FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1) +
                        FindWord(word, board, x - 1, y - 1, index + 1);
            }
            else if (x == board.GetLength(0) - 1 && y == 0)
            {
                return FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x - 1, y + 1, index + 1) +
                        FindWord(word, board, x, y + 1, index + 1);
            }
            // Cas 2 : Les cases sur les bords (coins exclus) ont 5 voisins adjacents
            else if (x == 0) // Bord supérieur
            {
                return FindWord(word, board, x, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y, index + 1) +
                        FindWord(word, board, x + 1, y - 1, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1);
            }
            else if (y == board.GetLength(1) - 1) // Bord droit
            {
                return FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x - 1, y - 1, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1) +
                        FindWord(word, board, x + 1, y - 1, index + 1) +
                        FindWord(word, board, x + 1, y, index + 1);
            }
            else if (x == board.GetLength(0) - 1) // Bord inférieur
            {
                return FindWord(word, board, x, y + 1, index + 1) +
                        FindWord(word, board, x - 1, y + 1, index + 1) +
                        FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x - 1, y - 1, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1);
            }
            else if (y == 0) // Bord gauche
            {
                return FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x - 1, y + 1, index + 1) +
                        FindWord(word, board, x, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y, index + 1);
            }
            else // Dernier Cas : La case se trouve à l'intérieur
            {
                return FindWord(word, board, x - 1, y - 1, index + 1) +
                        FindWord(word, board, x - 1, y, index + 1) +
                        FindWord(word, board, x - 1, y + 1, index + 1) +
                        FindWord(word, board, x, y - 1, index + 1) +
                        FindWord(word, board, x, y + 1, index + 1) +
                        FindWord(word, board, x + 1, y - 1, index + 1) +
                        FindWord(word, board, x + 1, y, index + 1) +
                        FindWord(word, board, x + 1, y + 1, index + 1);
            }
        }

        #endregion
    }
}
