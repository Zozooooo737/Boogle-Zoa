namespace Boogle_Zoa
{
    
    internal class Dice
    {
        #region Attributes 

        private char[] letters;
        private char visibleLetter;

        private static Dictionary<char, int> usedLetters = new Dictionary<char, int>
        {
            { 'A', 0 }, { 'B', 0 }, { 'C', 0 }, { 'D', 0 }, { 'E', 0 },
            { 'F', 0 }, { 'G', 0 }, { 'H', 0 }, { 'I', 0 }, { 'J', 0 },
            { 'K', 0 }, { 'L', 0 }, { 'M', 0 }, { 'N', 0 }, { 'O', 0 },
            { 'P', 0 }, { 'Q', 0 }, { 'R', 0 }, { 'S', 0 }, { 'T', 0 },
            { 'U', 0 }, { 'V', 0 }, { 'W', 0 }, { 'X', 0 }, { 'Y', 0 },
            { 'Z', 0 }
        };

        #endregion


        #region Constructor

        public Dice(Random r, Dictionary<char, int[]> alphabet)
        {
            letters = new char[6];
            

            for (int i = 0; i < 6; i++)
            {
                int n;

                do
                {
                    n = r.Next(alphabet.Count);  // n est un indice aléatoire entre 0 et 25
                    letters[i] = alphabet.ElementAt(n).Key;  // On ajoute la lettre à la face i du dé
                }
                while (usedLetters[letters[i]] >= alphabet[letters[i]][1]);  // On vérifie que la lettre ajouté sur la face a le droit d'être utiliser en comparant le fichier et son historique
            }

            Roll(r);
        }

        #endregion


        #region Properties

        public char VisibleLetter
        {
            get { return visibleLetter; }
        }

        #endregion


        #region Methods

        public void Roll(Random r)
        {
            int n = r.Next(6);
            visibleLetter = letters[n];

            usedLetters[letters[n]] += 1;

        }


        public string toString()
        {
            string description = $"Lettre visible : {letters} \nEnsemble des lettres : ";
            foreach (char Lettre in letters)
            {
                description += Lettre + " ";
            }
            return description;
        }

        #endregion
    }
}