using System.Collections.Generic;

namespace Boogle_Zoa
{
    internal class Board
    {
        private Dice[] dices;
        private char[] visibleLetters;
        private char[,] boardOfLetters;
        private const int size = 16;
        private const int side = 4;

        private static readonly (int, int)[] Directions = new (int, int)[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };


        public Board(Dice[] d)
        {
            dices = d;
            visibleLetters = new char[size];

            for (int i = 0; i < size; i++)
            {
                visibleLetters[i] = dices[i].VisibleLetter;
            }

            boardOfLetters = new char[side, side];

            int n = 0;

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    boardOfLetters[i, j] = visibleLetters[n];
                    n++;
                }
            }
        }



        public string toString()
        {
            string description = "";

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    description += boardOfLetters[i,j] + " ";
                }
                description += "\n";
            }
            return description;
        }


        public bool GameBoard_Test(string word, DictionaryWords dico)
        {
            bool wordFound = false;

            List<List<(int, int)>> allPaths = new List<List<(int, int)>>();
            allPaths = FindAllWordPaths(word);

            if (allPaths.Count > 0)
            {
                wordFound = true;
            }
            return wordFound;
        }


        //
        // VOIR CAS PARTICULIER TOT (TOTO) --> Ajouter en paramêtre une liste dynamique qui stoquera les positions des lettres trouvés, de plus a chaque vérif --> on regarde si la case n'est pas déjà présente dans la liste;
        //

        //
        // VOIR CAS PARTICULIER TOUT.T --> Choisir le mots a retirer quand plusieurs poossibilité
        // 

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



        public List<List<(int, int)>> FindAllWordPaths(string word)
        {
            List<List<(int, int)>> allPaths = new List<List<(int, int)>>(); 
            word = word.ToUpper();

            for (int x = 0; x < boardOfLetters.GetLength(0); x++)
            {
                for (int y = 0; y < boardOfLetters.GetLength(1); y++)
                {
                    FindWordRecursive(word, boardOfLetters, x, y, 0, new List<(int, int)>(), allPaths);
                }
            }

            return allPaths;
        }


        private void FindWordRecursive(string word, char[,] board, int x, int y, int index, List<(int, int)> path, List<List<(int, int)>> allPaths)
        {
            if (x < 0 || x >= board.GetLength(0) || y < 0 || y >= board.GetLength(1) || board[x, y] != word[index] || path.Contains((x, y)))
            {
                return;
            }

            path.Add((x, y));

            if (index == word.Length - 1)
            {
                allPaths.Add(new List<(int, int)>(path));
            }
            else
            {
                foreach (var (dx, dy) in Directions)
                {
                    int newX = x + dx;
                    int newY = y + dy;
                    FindWordRecursive(word, board, newX, newY, index + 1, path, allPaths);
                }
            }
            path.RemoveAt(path.Count - 1);
        }
    }
}