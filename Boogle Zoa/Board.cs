﻿namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un plateau de jeu 4x4 cases, composé de lettres générées à partir de dés (<see cref="dices"/>). <br/>
    /// Chaque case contient une lettre visible (<see cref="visibleLetters"/>) organisée dans une grille (<see cref="boardOfLetters"/>). <br/>
    /// La taille totale est fixée par <see cref="size"/> et la longueur d'un côté par <see cref="side"/>.
    /// </summary>
    public class Board
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



        /// <summary>
        /// Crée un plateau de jeu à partir d'une liste de dés (<paramref name="dices"/>).
        /// </summary>
        /// <param name="d">Liste de dés (<see cref="Dice"/>)</param>
        /// Update : Réalise une matrice à partir de n'importe quelle taille de `dices` (entre 4 et 32 par exemple)
        public Board(Dice[] dices)
        {
            this.dices = dices;
            visibleLetters = new char[size];

            for (int i = 0; i < size; i++)
            {
                visibleLetters[i] = this.dices[i].VisibleLetter;
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



        /// <summary>
        /// Renvoie la liste des lettres visibles (<see cref="visibleLetters"/>) sur le plateau.
        /// </summary>
        public char[] VisibleLetter
        {
            get { return visibleLetters; }
        }


        /// <summary>
        /// Renvoie le plateau de lettres (<see cref="BoardOfLetters"/>).
        /// </summary>
        public char[,] BoardOfLetters
        {
            get { return boardOfLetters; }
        }


        /// <summary>
        /// Vérifie si un mot donné (<paramref name="word"/>) est présent sur le plateau de jeu (<see cref="boardOfLetters"/>) et dans le dictionnaire (<paramref name="dico"/>).
        /// </summary>
        /// <param name="word">Mot à vérifier</param>
        /// <param name="dico">Dictionnaire utilisé</param>
        /// <returns>
        /// <c>true</c> si le mot est présent sur le plateau de jeu ; sinon, <c>false</c>.
        /// </returns>
        /// Update : Renvoie la liste `allPaths` au lieu d'un bool / ou utiliser directement `FindAllWordPath`.
        public bool GameBoardTest(string word, DictionaryWords dico)
        {
            bool wordFound = false;

            if (dico.CheckWord3(word))
            {
                List<List<(int, int)>> allPaths = new List<List<(int, int)>>();
                allPaths = FindAllWordPaths(word);

                if (allPaths.Count > 0)
                {
                    wordFound = true;
                }
            }
            
            return wordFound;
        }


        /// <summary>
        /// Renvoie tous les chemins possibles pour former un mot donné (<paramref name="word"/>) sur le plateau de lettres (<see cref="boardOfLetters"/>). <br/>
        /// Chaque chemin est une liste de coordonnées (x, y) représentant les cases successives utilisées pour former le mot.
        /// </summary>
        /// <param name="word">Le mot à rechercher sur le plateau.</param>
        /// <returns>Une liste de chemins, chaque chemin étant une liste de coordonnées (x, y) des cases parcourues.</returns>
        /// Nous lançons une recherche récursive sur chacune des cases du plateau afin de lister tous les chemins possibles du mot.
        /// A termes, le joueur pourra choisir laquelle de ces possibilités il souhaite compter, afin de gérer le cas où un mot serait présent plusieurs fois sur le plateau.
        public List<List<(int, int)>> FindAllWordPaths(string word)
        {
            List<List<(int, int)>> allPaths = new List<List<(int, int)>> { };
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


        /// <summary>
        /// Recherche récursive des chemins possibles pour former un mot donné sur le plateau de lettres.
        /// La méthode explore les cases adjacentes pour correspondre aux lettres du mot, ajoutant chaque chemin valide à <paramref name="allPaths"/>.
        /// </summary>
        /// <param name="word">Mot à rechercher</param>
        /// <param name="board">Plateau de lettres où la recherche est effectuée.</param>
        /// <param name="x">Coordonnée x de la case actuelle sur le plateau</param>
        /// <param name="y">Coordonnée y de la case actuelle sur le plateau</param>
        /// <param name="index">Index actuel dans le mot, représentant la lettre cible à rechercher sur cette case</param>
        /// <param name="path">Liste des coordonnées (x, y) représentant le chemin actuel pour former le mot</param>
        /// <param name="allPaths">Collection de tous les chemins trouvés, chaque chemin étant une liste de coordonnées formant le mot</param>
        /// Version 2 : Nous avons fait une nouvelle fonction récursive pour gérer le cas où plusieurs chemins seraient possibles à partir d'une même lettre.
        /// Optimisation de la Fonction : Initialement nous faisions une disjonction de cas pour trouver les cases adjacentes, désormais nous réalisons toutes les adjacences possibles et en début de fonction, nous éliminons toutes les cases qui ne respectent pas les conditions.
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


        /// <summary>
        /// Renvoie une chaîne de caractères <c>string</c> qui affiche le plateau de jeu (<see cref="boardOfLetters"/>).
        /// </summary>
        /// <returns>Renvoie une chaîne de caractères <c>string</c> structurée.</returns>
        public string toString()
        {
            string description = "";

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    description += boardOfLetters[i, j] + " ";
                }
                description += "\n";
            }
            return description;
        }
    }
}