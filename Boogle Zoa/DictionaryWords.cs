using System.Collections.Immutable;

namespace Boogle_Zoa
{
    internal class DictionaryWords
    {
        #region Attributes

        private List<string> words;  // List<string> car on ne connait pas a l'avance le nombre de mots qui sera présent dans le fichier txt
        private string language;

        private Dictionary<int, List<string>> wordsBySize = new Dictionary<int, List<string>> { };  // List<string> car on ne connait pas a l'avance le nombre de mots de chaque clé

        private Dictionary<char, List<string>> wordsByLetter = new Dictionary<char, List<string>> { };    // ' ' ' 

        #endregion


        #region Constructor

        public DictionaryWords(string filePath, string l)
        {
            string content = File.ReadAllText(filePath);
            words = new List<string>(content.Split(' '));

            language = l;

            int n;
            char c;

            foreach (string word in words) // En un parcours de la liste words, on parvient à remplir les deux dictionnaires (de plus les keys ne sont pas prédéfini)
            {
                n = word.Length;
                c = word[0];

                if (wordsBySize.ContainsKey(n))
                {
                    wordsBySize[n].Add(word);
                }
                else
                {
                    wordsBySize.Add(n, new List<string> { word });
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

            // On trie les mots par ordre alphabétique pour améliorer les recherches de mots dans le dictionnaire ;
            foreach(List<string> list in wordsBySize.Values)
            {
                list.Sort();
            }

            foreach (List<string> list in wordsByLetter.Values)
            {
                list.Sort();
            }
        }

        #endregion
        

        #region Methods

        public string toString()
        {
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string description;
            
            description = "Description du Dictionnaire \n\n" +
                         $"Langue : {language} \n\n" +
                          "Nombre de mots par longueur :\n";

            for(int i=0; i< wordsBySize.Keys.Count; i++)  // Permet d'afficher par ordre croissant
            {
                if(wordsBySize.Keys.Contains(i)) // On vérifie que la clé est présente dans le dictionnaire, pour ne pas provoquer d'erreur (i=0; i=1)
                {
                    description += $"{i} : {wordsBySize[i].Count}\n";
                }
            }

            description += "\nNombre de mots par lettre :\n";

            for (int i = 0; i < alpha.Length ; i++)  // Permet d'afficher par ordre croissant
            {
                if (wordsByLetter.Keys.Contains(alpha[i])) // On vérifie que la clé est présente dans le dictionnaire, pour ne pas provoquer d'erreur (i=0; i=1)
                {
                    description += $"{alpha[i]} : {wordsByLetter[alpha[i]].Count}\n";
                }
            }

            return description;
        }

        // 1er tentative
        public bool RechDichoRecursif(string mot, List<string> list = null, int min=0, int max=0)
        {
            if(min==0 && max==0)
            {
                int n = mot.Length;
                List<string> shortListOfWords = wordsBySize[n];

                return RechDichoRecursif(mot.ToUpper(), shortListOfWords, 0, shortListOfWords.Count-1);
            }

            if (min > max)
            {
                return false;
            }

            int mid = (min + max) / 2;

            int comparison = string.Compare(mot, list[mid]);

            if (comparison == 0)
            {
                return true;
            }
            else if (comparison < 0)
            {
                return RechDichoRecursif(mot, list, min, mid - 1);
            }
            else
            {
                return RechDichoRecursif(mot, list, mid + 1, max);
            }
        }

        // 2e tentative ...

        // 3e tentative ...

        #endregion
    }


}
