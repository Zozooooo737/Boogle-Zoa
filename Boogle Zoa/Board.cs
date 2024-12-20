namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un plateau de jeu 4x4 cases, composé de lettres générées à partir de dés (<see cref="dices"/>). <br/>
    /// Chaque case contient une lettre visible (<see cref="visibleLetters"/>) organisée dans une grille (<see cref="boardOfLetters"/>). <br/>
    /// La taille totale est fixée par <see cref="size"/> et la longueur d'un côté par <see cref="side"/>.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Tableau de dés utilisés pour générer les lettres du plateau.
        /// </summary>
        private readonly Dice[] dices;

        /// <summary>
        /// Tableau des lettres visibles générées par les dés.
        /// Ces lettres forment le contenu du plateau de jeu.
        /// </summary>
        private readonly char[] visibleLetters;

        /// <summary>
        /// Grille 2D représentant le plateau de jeu avec les lettres visibles.
        /// Chaque case de la grille contient une lettre.
        /// </summary>
        private readonly char[,] boardOfLetters;

        /// <summary>
        /// Nombre total de cases sur le plateau.
        /// Cette valeur correspond à la taille totale du plateau de jeu<see cref="boardOfLetters"/>. 
        /// </summary>
        private readonly int size;

        /// <summary>
        /// Longueur d'un côté de la grille du plateau de jeu.
        /// La grille est de forme carrée, et cette valeur est obtenue en prenant la racine carrée de <see cref="size"/>.
        /// </summary>
        private readonly int side;

        /// <summary>
        /// Tableau statique représentant les 8 directions possibles pour naviguer dans la grille.
        /// Chaque direction est définie par un déplacement en x et en y, permettant d'explorer les cases adjacentes.
        /// </summary>
        private static readonly (int, int)[] directions = new (int, int)[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };



        /// <summary>
        /// Crée un plateau de jeu à partir d'une liste de dés (<paramref name="dices"/>).
        /// </summary>
        /// <param name="d">Liste de dés (<see cref="Dice"/>).</param>
        /// Update : Réalise une matrice à partir de n'importe quelle taille de `dices` (entre 4 et 32 par exemple)
        public Board(Dice[] dices)
        {
            this.dices = dices;

            size = dices.Length;
            side = Convert.ToInt32(Math.Sqrt(dices.Length));

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
        /// Renvoie la hauteur du plateau de lettres (<see cref="side"/>).
        /// </summary>
        public int Side
        {
            get { return side;  }
        }



        /// <summary>
        /// Vérifie si un mot donné (<paramref name="word"/>) est présent sur le plateau de jeu (<see cref="FindAllWordPaths"/>) et dans le dictionnaire (<paramref name="dictionary"/>).
        /// </summary>
        /// <param name="word">Mot à vérifier.</param>
        /// <param name="dictionary">Dictionnaire utilisé pour la vérification.</param>
        /// <returns>
        /// Un entier indiquant le résultat de la vérification :
        /// <list type="bullet">
        ///   <item><description><c>0</c> : Le mot est présent à la fois dans le dictionnaire et sur le plateau de jeu.</description></item>
        ///   <item><description><c>1</c> : Le mot est présent dans le dictionnaire mais pas sur le plateau de jeu.</description></item>
        ///   <item><description><c>2</c> : Le mot n'est ni dans le dictionnaire ni sur le plateau de jeu.</description></item>
        /// </list>
        /// </returns>
        public int GameBoardTest(string word, DictionaryWords dictionary)
        {
            int wordFound = 2;

            if (dictionary.CheckWord3(word))
            {
                List<List<(int, int)>> allPaths = new List<List<(int, int)>>();
                allPaths = FindAllWordPaths(word);

                if (allPaths.Count > 0)
                {
                    wordFound = 0;
                }
                else
                {
                    wordFound = 1;
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
        /// <param name="word">Mot à rechercher.</param>
        /// <param name="board">Plateau de lettres où la recherche est effectuée.</param>
        /// <param name="x">Coordonnée x de la case actuelle sur le plateau.</param>
        /// <param name="y">Coordonnée y de la case actuelle sur le plateau.</param>
        /// <param name="index">Index actuel dans le mot, représentant la lettre cible à rechercher sur cette case.</param>
        /// <param name="path">Liste des coordonnées (x, y) représentant le chemin actuel pour former le mot.</param>
        /// <param name="allPaths">Collection de tous les chemins trouvés, chaque chemin étant une liste de coordonnées formant le mot.</param>
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
                foreach (var (dx, dy) in directions)
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