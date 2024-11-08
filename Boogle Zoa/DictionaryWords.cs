namespace Boogle_Zoa
{
    internal class DictionaryWords
    {
        #region Attributes

        private List<string> words;  // List<string> car on ne connait pas a l'avance le nombre de mots qui sera présent dans le fichier txt
        private string language;

        private Dictionary<int, List<string>> wordsBySize = new Dictionary<int, List<string>> {  };  // List<string> car on ne connait pas a l'avance le nombre de mots de chaque clé
        
        private Dictionary<char, List<string>> wordsByLetter = new Dictionary<char, List<string>> {  };    // ' ' ' 

        #endregion


        #region Constructor

        public DictionaryWords(string filePath, string l)
        {
            string content = File.ReadAllText(filePath);
            words = new List<string>(content.Split(' '));

            language = l;

            int n ; 
            char c;

            foreach (string word in words)
            {
                n = word.Length;
                c = word[0];

                if(wordsBySize.ContainsKey(n))
                {
                    wordsBySize[n].Add(word);
                }
                else
                {
                    wordsBySize.Add(n, new List<string> {word });
                }

                if (wordsByLetter.Keys.Contains(c))
                {
                    wordsByLetter[c].Add(word);
                }
                else
                {
                    wordsByLetter.Add(c, new List<string> { word });
                }
            }
        }

        #endregion
    }
}
