using System;
using System.Runtime.InteropServices.JavaScript;

namespace Boogle_Zoa
{
    public class Program
    {
        static void Main(string[] args)
        {
            TimeSpan secSpan = TimeSpan.FromSeconds(30);

            Game g = new Game(2, secSpan, 3, 4, "FR");


            g.InitializePlayerName();
            g.Process();
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
