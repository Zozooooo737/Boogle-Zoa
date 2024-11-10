namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un dé à 6 faces avec une lettre sur chacune d'elles (<see cref="letters"/>), dont une face visible (<see cref="visibleLetter"/>).
    /// </summary>
    public class Dice
    {
        private char[] letters;                         // Tableau des lettres présentes sur les 6 faces du dé.
        private char visibleLetter;                     // Lettre qui sera sur la face supérieure du dé quand il sera lancé.
        /// <summary>
        /// Cet attribut de classe de type dictionnaire stock le nombre d'occurence de lettres visibles (<see cref="visibleLetter"/>) sur le plateau de jeu.
        /// </summary>
        private static Dictionary<char, int> usedLetters = new Dictionary<char, int>            // Ce dictionnaire associe à chaque lettre de l'alphabet un int. Ce int
        {                                                                                       // Ce int corresponds au nombre de fois que la lettre est apparu lors du
            { 'A', 0 }, { 'B', 0 }, { 'C', 0 }, { 'D', 0 }, { 'E', 0 },                         // lancement des dés, i.e. apparu en jeu sur le plateau.
            { 'F', 0 }, { 'G', 0 }, { 'H', 0 }, { 'I', 0 }, { 'J', 0 },
            { 'K', 0 }, { 'L', 0 }, { 'M', 0 }, { 'N', 0 }, { 'O', 0 },
            { 'P', 0 }, { 'Q', 0 }, { 'R', 0 }, { 'S', 0 }, { 'T', 0 },
            { 'U', 0 }, { 'V', 0 }, { 'W', 0 }, { 'X', 0 }, { 'Y', 0 },
            { 'Z', 0 }
        };

    
        /// <summary>
        /// Crée un dé aléatoirement à partir d'un alphabet donné.
        /// </summary>
        /// <param name="r">Une instance de la classe <see cref="Random"./></param>
        /// <param name="lettersInformation">Un dictionnaire où chaque clé est une lettre et chaque valeur est un tableau de taille 2 représentant le poids et le nombre de cette lettre.</param>
        public Dice(Random r, Dictionary<char, int[]> lettersInformation)             // Paramètres : Objet Random pour les tirages au hasard, Dictionnaire pour connaitre les informations sur les lettres.
        {
            letters = new char[6];                                                    // On crée la liste de char de taille 6 pour les 6 faces du dé.

            for (int i = 0; i < 6; i++)                                               // On parcours chaque face pour lui assigner une lettre.
            {
                int n;                                                                // On itialise le int n avant la boucle while pour optimiser la mémoire.

                do
                {
                    n = r.Next(lettersInformation.Count);                             // On affecte à n un indice aléatoire entre 0 et 25.
                    letters[i] = lettersInformation.ElementAt(n).Key;                 // On ajoute la lettre à la face i du dé.
                }
                while (usedLetters[letters[i]] >= lettersInformation[letters[i]][1]); // On répète cette information tant que la lettre choisi est une lettre n'ayant pas encore atteint sa limite, d'après l'historique usedLetters.
            }

            Roll(r);                                                                  // On lance la méthode de lancement du dé.
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
        public char VisibleLetter                                                     // Propriété utilisé pour connaitre la face visible du dé lors de la création du tableau.
        {
            get { return visibleLetter; }
        }

        /// <summary>
        /// Renvoie le dictionnaire des apparitions de chaques lettres visibles (<see cref="usedLetters"/>).
        /// </summary>
        public static Dictionary<char, int> UsedLetters                                                    // Propriété utilisé pour connaitre la face visible du dé lors de la création du tableau.
        {
            get { return usedLetters; }
        }


        /// <summary>
        /// Lance le dé pour déterminer quelle lettre du dé sera visible (<see cref="visibleLetter"/>), et actualise le dictionnaire d'apparition (<see cref="usedLetters"/>).
        /// </summary>
        /// <param name="r">Une instance de la classe <see cref="Random".</param>
        public void Roll(Random r)                                                    // Méthode permettant d'obtenir le résultat d'un lancement de dé avec en paramètre : Objet Random pour lancer le dé au hasard.
        {
            int n = r.Next(6);                                                        // On choisit un int n au hasard entre 0 et 5.
            visibleLetter = letters[n];                                               // On associe la lettre supérieur de la face à visibleLetter.

            usedLetters[visibleLetter] += 1;                                          // On incrément le nombre d'utilisations de la lettre visible.
        }
        

        /// <summary>
        /// Renvoie une chaîne de caractère <c>string</c> qui décrit le dé (<see cref="Dice"/>) avec l'ensemble de ses faces (<see cref="letters"/>) et sa face visible (<see cref="visibleLetter"/>).
        /// </summary>
        /// <returns>Renvoie une chaîne de caractères structurée.</returns>
        public string toString()                                                                // Méthode qui renvoie la descriptiion du dé : Faces + Lettre Visible.
        {
            string description = $"Lettre visible : {visibleLetter} \nEnsemble des lettres : ";       // On utilise l'interpolation de chaînes pour faciliter l'insertion des attributs dans le string
            foreach (char Lettre in letters)
            {
                description += Lettre + " ; ";
            }
            return description;
        }
    }
}