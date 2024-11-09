namespace Boogle_Zoa
{
    internal class Dice
    {
        #region Attributes

        private char[] letters;                         // Tableau des lettres présentes sur les 6 faces du dé.
        private char visibleLetter;                     // Lettre qui sera sur la face supérieure du dé quand il sera lancé.

        private static Dictionary<char, int> usedLetters = new Dictionary<char, int>            // Ce dictionnaire associe à chaque lettre de l'alphabet un int. Ce int
        {                                                                                       // Ce int corresponds au nombre de fois que la lettre est apparu lors du
            { 'A', 0 }, { 'B', 0 }, { 'C', 0 }, { 'D', 0 }, { 'E', 0 },                         // lancement des dés, i.e. apparu en jeu sur le plateau.
            { 'F', 0 }, { 'G', 0 }, { 'H', 0 }, { 'I', 0 }, { 'J', 0 },
            { 'K', 0 }, { 'L', 0 }, { 'M', 0 }, { 'N', 0 }, { 'O', 0 },
            { 'P', 0 }, { 'Q', 0 }, { 'R', 0 }, { 'S', 0 }, { 'T', 0 },
            { 'U', 0 }, { 'V', 0 }, { 'W', 0 }, { 'X', 0 }, { 'Y', 0 },
            { 'Z', 0 }
        };

        #endregion Attributes


        #region Constructor

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

        #endregion Constructor


        #region Properties

        public char VisibleLetter                                                     // Propriété utilisé pour connaitre la face visible du dé lors de la création du tableau.
        {
            get { return visibleLetter; }
        }

        #endregion Properties


        #region Methods

        public void Roll(Random r)                                                    // Méthode permettant d'obtenir le résultat d'un lancement de dé avec en paramètre : Objet Random pour lancer le dé au hasard.
        {
            int n = r.Next(6);                                                        // On choisit un int n au hasard entre 0 et 5.
            visibleLetter = letters[n];                                               // On associe la lettre supérieur de la face à visibleLetter.

            usedLetters[visibleLetter] += 1;                                          // On incrément le nombre d'utilisations de la lettre visible.
        }
        

        public string toString()                                                                // Méthode qui renvoie la descriptiion du dé : Faces + Lettre Visible.
        {
            string description = $"Lettre visible : {letters} \nEnsemble des lettres : ";       // On utilise l'interpolation de chaînes pour faciliter l'insertion des attributs dans le string
            foreach (char Lettre in letters)
            {
                description += Lettre + " ; ";
            }
            return description;
        }

        #endregion Methods
    }
}