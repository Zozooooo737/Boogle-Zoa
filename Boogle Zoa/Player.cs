namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un joueur qui dispose d'un nom (<see cref="name"/>) ainsi que d'un score (<see cref="score"/>) et d'une liste des mots qui l'a trouvés (<see cref="wordsFound"/>), respectivement initialisé à 0 et par une liste vide.
    /// </summary>
    internal class Player
    {
        private string name;
        private int score;
        private List<string> wordsFound;



        /// <summary>
        /// Crée un joueur à partir d'un nom (<see cref="name"/>).
        /// </summary>
        /// <param name="n">Nom du Joueur</param>
        public Player(string name)
        {
            this.name = name;
            score = 0;
            wordsFound = new List<string>() { };   
        }



        /// <summary>
        /// Renvoie le (<see cref="score"/>) du Joueur.
        /// </summary>
        public int Score
        {
            get { return score; }
        }


        /// <summary>
        /// Vérifie si un mot donné (<paramref name="word"/>) est présent dans la liste des mots trouvés par le joueur (<see cref="wordsFound"/>).
        /// </summary>
        /// <param name="word">Le mot à rechercher dans la liste des mots trouvés par le joueur.</param>
        /// <returns>
        /// <c>true</c> si le mot est présent dans la liste des mots trouvés par le joueur ; sinon, <c>false</c>.
        /// </returns>
        public bool Contain(string word)
        {
            return wordsFound.Contains(word);
        }


        /// <summary>
        /// Ajoute un mot <paramref name="word"/> à la liste des mots trouvés (<see cref="wordsFound"/>) et met à jour le score du joueur (<see cref="score"/>). Le score est calculé en additionnant
        /// le poids de chaque lettre du mot, selon les valeurs présentes dans le dictionnaire <paramref name="lettersInformation"/>.
        /// </summary>
        /// <param name="word">Le mot à ajouter à la liste des mots trouvés</param>
        /// <param name="lettersInformation">Un dictionnaire où chaque clé est une lettre et chaque valeur est un tableau de taille 2 représentant le poids et le nombre de cette lettre.</param>
        /// <remarks>
        /// Avant d'ajouter le mot <paramref name="word"/>, on vérifie qu'il n'est pas déjà présent dans la liste <see cref="wordsFound"/>.
        /// Il est supposé que <paramref name="word"/> existe dans le dictionnaire de la langue concernée
        /// </remarks>
        public void AddWord(string word, Dictionary<char, int[]> lettersInformation)
        {
            if (!Contain(word))
            {
                wordsFound.Add(word);
                   
                foreach(char c in word)
                {
                    score += lettersInformation[word[c]][0];
                }
            }
        }


        /// <summary>
        /// Renvoie une chaîne de caractère <c>string</c> qui décrit le joueur (<see cref="Player"/>) avec son nom (<see cref="name"/>), son score (<see cref="score"/>) et sa liste des mots qu'il a trouvé (<see cref="wordsFound"/>).
        /// </summary>
        /// <returns>Renvoie une chaîne de caractère <c>string</c> déjà structurée.</returns>
        public string toString()
        {
            string description = $"Joueur : {name} \n" +
                                 $"Score : {score} \n" +
                                  "Mots trouvés : ";

            foreach (string mot in wordsFound)
            {
                description += mot + " ; ";
            }
            return description;
        }
    }
}