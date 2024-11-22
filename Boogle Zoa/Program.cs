using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Boogle_Zoa
{
    public class Program
    {
        // Modifications nécessaires pour afficher une bonne console : Paramètre > Système > Espace développeurs > Terminal : Hôte de la console Windows.
        // Préciser : NE PAS METTRE EN PLEINE ECRAN SINON ON TE HAGRA
        [DllImport("user32.dll")] private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags); // Supprime une commande d'un menu.
        [DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert); // Récupère le menu système d'une fenêtre.
        [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow(); // Récupère le handle de la console.

        private static int width = 80;
        private static int height = 20;
        static void Main(string[] args)
        {
            nint systemMenu = GetSystemMenu(GetConsoleWindow(), false); // Récupère le menu système de la console.
            int sizeCommand = 0xF000; // Code associé à l'option "Redimensionner".
            DeleteMenu(systemMenu, sizeCommand, 0x00000000); // Supprime l'option "Redimensionner" du menu.

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            TimeSpan secSpan = TimeSpan.FromSeconds(30);

            Game g = new Game(2, secSpan, 3, 4, "FR");
            DisplayCentered("Boogle ZOA");
            DisplayCentered("Press any key to start !");

            Console.ReadKey();
            Console.Clear();
            g.InitializePlayerName();
            g.Process();

        }

        #region Interface Graphique
        public static void DisplayCentered(string text)
        {
            List<string> lines = new List<string>(text.Split("\n"));
            int l = lines[0].Length/2;
            int space = width/2 - l;

            foreach( string line in lines ) 
            {
                Console.Write(new string(' ', space));
                Console.WriteLine(line);
            }
        }

        #endregion

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
