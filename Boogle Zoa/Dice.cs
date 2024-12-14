namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un dé à 6 faces avec une lettre sur chacune d'elles (<see cref="letters"/>), dont une face visible (<see cref="visibleLetter"/>).
    /// </summary>
    public class Dice
    {
        private char[] letters;
        private char visibleLetter;

        /// <summary>
        /// Dictionnaire statique qui enregistre le nombre d'apparition de chaque lettre visible (<see cref="visibleLetter"/>) sur le plateau de jeu.
        /// </summary>
        private static Dictionary<char, int> usedLetters = new Dictionary<char, int>
        {
            { 'A', 0 }, { 'B', 0 }, { 'C', 0 }, { 'D', 0 }, { 'E', 0 },
            { 'F', 0 }, { 'G', 0 }, { 'H', 0 }, { 'I', 0 }, { 'J', 0 },
            { 'K', 0 }, { 'L', 0 }, { 'M', 0 }, { 'N', 0 }, { 'O', 0 },
            { 'P', 0 }, { 'Q', 0 }, { 'R', 0 }, { 'S', 0 }, { 'T', 0 },
            { 'U', 0 }, { 'V', 0 }, { 'W', 0 }, { 'X', 0 }, { 'Y', 0 },
            { 'Z', 0 }
        };



        /// <summary>
        /// Crée un dé aléatoirement à partir d'un alphabet donné (<paramref name="lettersInformation"/>).
        /// </summary>
        /// <param name="r">Une instance de la classe Random.</param>
        /// <param name="lettersInformation">Un dictionnaire où chaque clé est une lettre et chaque valeur est un tableau de taille 2 représentant le poids et le nombre de cette lettre.</param>
        /// En utilisant l'attribut de classe <see cref="usedLetters"/>, on prends en compte le nombre d'apparition des lettres afin de respecter le jeu du Boogle. 
        /// Optimisation de la Mémoire --> On a déclare "int c" à l'exterieur de la boucle "for" pour réalisquer qu'une seule allocation mémoire au lieu de 6.
        /// Gérer le cas où plus de lettre dispos, pour finir le jeu à 100%
        public Dice(Random r, Dictionary<char, (int,int)> lettersInformation)
        {
            letters = new char[6];
            
            for (int i = 0; i < 6; i++)
            {
                int n;
                do
                {
                    n = r.Next(lettersInformation.Count);
                    letters[i] = lettersInformation.ElementAt(n).Key;
                }
                while (usedLetters[letters[i]] >= lettersInformation[letters[i]].Item2);
            }
            Roll(r);
        }


        /// <summary>
        /// Crée un dé en fixant comme lettre visible, le caractère (<paramref name="lettersInformation"/>).
        /// </summary>
        /// <param name="visibleLetter">Une lettre.</param>
        /// Ce constructeur permet de crée un dé en prédifinissant sa lettre visible afin de pouvoir crée des plateaux `Board` pertinent pour ses tests unitaires.
        public Dice(char letter)
        {
            letters = new char[6] { 'A', 'B', 'C', 'D', 'E', letter };
            visibleLetter = letter;
        }



        /// <summary>
        /// Renvoie les 6 lettres  (<see cref="letters"/>) du dé.
        /// </summary>
        public char[] Letters 
        { 
            get { return letters; } 
        }

        /// <summary>
        /// Renvoie la lettre visible (<see cref="visibleLetter"/>) du dé.
        /// </summary>
        public char VisibleLetter
        {
            get { return visibleLetter; }
        }

        /// <summary>
        /// Renvoie le dictionnaire des apparitions de chaques lettres visibles (<see cref="usedLetters"/>).
        /// </summary>
        public static Dictionary<char, int> UsedLetters
        {
            get { return usedLetters; }
        }



        /// <summary>
        /// Lance le dé pour déterminer quelle lettre du dé sera visible (<see cref="visibleLetter"/>), et actualise le dictionnaire d'apparition (<see cref="usedLetters"/>).
        /// </summary>
        /// <param name="r">Une instance de la classe Random.</param>
        public void Roll(Random r)
        {
            int n = r.Next(6);                                                     
            visibleLetter = letters[n];

            usedLetters[visibleLetter] += 1;
        }


        /// <summary>
        /// Renvoie une chaîne de caractère <c>string</c> qui décrit le dé (<see cref="Dice"/>) avec l'ensemble de ses faces (<see cref="letters"/>) et sa face visible (<see cref="visibleLetter"/>).
        /// </summary>
        /// <returns>Renvoie une chaîne de caractères <c>string</c> structurée.</returns>
        public string toString()
        {
            string description = $"Lettre visible : {visibleLetter} \nEnsemble des lettres : ";
            foreach (char Lettre in letters)
            {
                description += Lettre + " ; ";
            }
            return description;
        }
    }
}