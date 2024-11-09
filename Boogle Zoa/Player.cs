namespace Boogle_Zoa
{
    internal class Player
    {
        #region Attributes

        private string name;                            // Nom du Joueur.
        private int score;                              // Score du Joueur.
        private List<string> wordsFound;                // Liste des mots trouvées par le Joueur.

        #endregion Attributes


        #region Constructor

        public Player(string n)                         // Paramètre : Nom du Joueur.
        {
            this.name = n;
            score = 0;                                  // On initialise son score à 0.
            wordsFound = new List<string>() { };        // On initialise sa liste de mot avec une List vide.
        }

        #endregion Constructor


        #region Properties

        public int Score                                // Propriété à revoir, pas encore utilisé.
        {
            get { return score; }
        }

        #endregion Properties


        #region Methods

        public bool Contain(string w)                   // Méthode qui regarde si le mot w est présent dans la liste des mots trouvés par le Joueur.
        {
            return wordsFound.Contains(w);
        }


        public void AddWord(string w, Dictionary<char, int[]> lettersInformation)          // Méthode qui prends en paramètres un mot et les informations concernant les lettres (nombre, poids).
        {
            if (!Contain(w))                            // On vérifie que le mot n'est pas déjà présent dans la liste des mots trouvés par le Joueur
            {
                wordsFound.Add(w);                      // On ajoute ce mot dans sa liste /!\ On considère que le mot est présent dans le dictionnaire, à revoir
                   
                foreach(char c in w)
                {
                    score += lettersInformation[w[c]][0];                                  // Grâce à lettersInformations, on obtient le poids de chaque lettre, qu'on ajoute au score du Joueur.
                }
            }
        }


        public string toString()                        // Méthode qui renvoie la description du Joueur.
        {
            string description = $"Joueur : {name} \n" +                                   // On utilise l'interpolation de chaînes pour faciliter l'insertion des attributs dans le string
                                 $"Score : {score} \n" +
                                  "Mots trouvés : ";

            foreach (string mot in wordsFound)
            {
                description += mot + " ; ";
            }
            return description;
        }

        #endregion Methods
    }
}