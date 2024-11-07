namespace Boogle_Zoa
{
    internal class Player
    {
        #region Attributes

        private string name;
        private int score;
        private List<string> wordsFound;

        #endregion


        #region Constructor

        public Player(string n)
        {
            this.name = n;
            score = 0;
            wordsFound = new List<string>();
        }

        #endregion


        #region Properties

        public int Score
        {
            get { return score; }
        }

        #endregion


        #region Methods

        public bool Contain(string w)
        {
            return wordsFound.Contains(w);
        }


        public void AddWord(string w)
        {
            wordsFound.Add(w); // Verifier dans le main que le mot n'est pas deja dans la liste de mots trouvée par le joueur
            score += w.Length;
        }


        public string toString()
        {
            string description = $"Joueur : {name} \nScore : {score} \nMots trouvés : ";
            foreach (string mot in wordsFound)
            {
                description += mot + " ";
            }
            return description;
        }

        #endregion
    }
}
