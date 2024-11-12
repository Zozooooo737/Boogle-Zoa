﻿namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un dictionnaire qui regroupe tous les mots d'une langue (<see cref="language">).  
    /// </summary>
    public class DictionaryWords
    {
        private List<string> words;
        private string language;

        private Dictionary<int, List<string>> wordsBySize = new Dictionary<int, List<string>> { };
        private Dictionary<char, List<string>> wordsByLetter = new Dictionary<char, List<string>> { };



        /// <summary>
        /// Crée un dictionnaire d'une langue (<paramref name="language"/>) à partir d'un fichier par son chemin d'accès (<paramref name="filePath"/>)
        /// </summary>
        /// <param name="filePath">Chemin d'accès au fichier de mots</param>
        /// <param name="language">Langue du dictionnaire</param>
        /// En un parcours de la liste words, on parvient à remplir les deux dictionnaires.
        /// A la fin du parcours, on trie chaque liste présente dans les 2 dictionnaires afin de faciliter le travail des algorithmes de recherche.
        /// Optimisation de la Mémoire --> On initialise "int n" et "int c" à l'extérieur de la boucle "foreach" pour réaliser que 2 allocations au lieu de 2*n.
        /// Optimisation de la Mémoire --> Les clés des deux dictionnaire "wordsBySize" et "wordsByLetter" ne sont pas prédéfinies afin de gagner en espace de mémoire. Exemple : si dans mon fichier, aucun mot ne commence par la lettre "F", le couple {'F', List'string'} ne sera pas présent dans wordsByLetter.
        public DictionaryWords(string filePath, string language)
        {
            string content = File.ReadAllText(filePath);
            words = new List<string>(content.Split(' '));
            
            this.language = language;

            int n;
            char c;

            foreach (string word in words) // 
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

            foreach(List<string> list in wordsBySize.Values)
            {
                list.Sort();
            }

            foreach (List<string> list in wordsByLetter.Values)
            {
                list.Sort();
            }
        }

        /// <summary>
        /// Renvoie la liste de mots (<see cref="words"/>) du dictionnaire.
        /// </summary>
        public List<string> Words
        {
            get { return words; }
        }


        /// <summary>
        /// Renvoie le dictionnaire qui associe à chaque caractère, la liste des mots commencant par ce caractère (<see cref="wordsByLetter"/>).
        /// </summary>
        public Dictionary<char, List<string>> WordsByLetter
        {
            get { return wordsByLetter; }
        }

        /// <summary>
        /// Renvoie le dictionnaire qui associe à chaque taille, la liste des mots faisant cette taille (<see cref="wordsBySize"/>).
        /// </summary>
        public Dictionary<int, List<string>> WordsBySize
        {
            get { return wordsBySize; }
        }


        /// <summary>
        /// Renvoie une chaîne de caractère <c>string</c> qui décrit un dictionnaire par sa langue (<see cref="language"/>), 
        /// et par le nombre de mots par longueur (<see cref="wordsBySize"/>) et par le nombre de mots par sa première lettre (<see cref="wordsByLetter"/>).
        /// </summary>
        /// <returns>Renvoie une chaîne de caractères <c>string</c> structurée.</returns>
        /// On parcourt chaque caractère de l'alphabet pour afficher le contenu de "wordsByLetter" dans un ordre alphabétique, 
        /// garantissant une présentation claire et cohérente, indépendamment de l’ordre de stockage dans le dictionnaire.
        /// Avant chaque affichage, il vérifie l'existence de chaque clé dans "wordsBySize" et "wordsByLetter". 
        /// Cette vérification permet d’éviter les erreurs d’accès, surtout si certaines longueurs ou lettres n’ont aucun mot associé.
        public string toString()
        {
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string description;
            
            description = "Description du Dictionnaire \n\n" +
                         $"Langue : {language} \n\n" +
                          "Nombre de mots par longueur :\n";

            for(int i=0; i< wordsBySize.Keys.Max(); i++)
            {
                if(wordsBySize.Keys.Contains(i))
                {
                    description += $"{i} : {wordsBySize[i].Count}\n";
                }
            }

            description += "\nNombre de mots par lettre :\n";

            for (int i = 0; i < alpha.Length ; i++)
            {
                if (wordsByLetter.Keys.Contains(alpha[i]))
                {
                    description += $"{alpha[i]} : {wordsByLetter[alpha[i]].Count}\n";
                }
            }

            return description;
        }


        /// <summary>
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// Tentative 1 : On récupère la liste des mots qui ont la même taille que "word", et on effectue une recherche dichotomique dans cette liste.
        public bool CheckWord1(string word)
        {
            int n = word.Length;
            List<string> sameLengthWords = wordsBySize[n];

            return RecursiveBinarySearch(word.ToUpper(), sameLengthWords, 0, sameLengthWords.Count - 1);
        }


        /// <summary>
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// Tentative 2 : On récupère la liste des mots qui ont la même taille que "word" et la liste des mots qui commence par la même lettre que "word".
        ///               On effectue une recherche dichotomique sur la liste la plus petite.
        /// Optimisation de la Mémoire : On effectue 2 "return" pour éviter de recréer une liste de "string" qui occupera un espace dans la mémoire inutile.
        public bool CheckWord2(string word)
        {
            word = word.ToUpper();

            int n = word.Length;
            char c = word[0];
            List<string> sameLengthWords = wordsBySize[n];
            List<string> sameLetterhWords = wordsByLetter[c];

            if(sameLengthWords.Count < sameLetterhWords.Count)
            {
                return RecursiveBinarySearch(word, sameLengthWords, 0, sameLengthWords.Count - 1);
            }
            else
            {
                return RecursiveBinarySearch(word, sameLetterhWords, 0, sameLetterhWords.Count - 1);
            }
        }


        /// <summary>
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// Tentative 3 : On récupère la liste des mots qui ont la même taille que "word" et la liste des mots qui commence par la même lettre que "word".
        ///               On effectue une recherche dichotomique sur l'intersection de ces 2 listes.
        /// La méthode "Intersect" renvoie une liste de type "IEnumerable" de string, nous devons la convertir en "List" avec la méthode "ToList()".
        public bool CheckWord3(string word)
        {
            word = word.ToUpper();

            int n = word.Length;
            char c = word[0];
            List<string> sameLengthWords = wordsBySize[n];
            List<string> sameLetterhWords = wordsByLetter[c];

            List<string>  commonWords = sameLengthWords.Intersect(sameLetterhWords).ToList();

            return RecursiveBinarySearch(word, commonWords, 0, commonWords.Count - 1);
        }


        /// <summary>
        /// Réalise une recherche dichotomique pour trouver un mot (<paramref name="mot"/>) dans une liste de mot (<paramref name="list"/>).
        /// </summary>
        /// <param name="mot">Mot à trouver</param>
        /// <param name="list">Liste dans laquelle on cherche le mot</param>
        /// <param name="min">Indice délimitant la borne inférieure</param>
        /// <param name="max">Indice délimitant la borne supérieure</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// Recursive Binary Search = Recherche Dichotomique Recursive
        public bool RecursiveBinarySearch(string word, List<string> list, int min, int max)
        {
            if (min > max)
            {
                return false;
            }

            int mid = (min + max) / 2;

            int comparison = string.Compare(word, list[mid]);

            if (comparison == 0)
            {
                return true;
            }
            else if (comparison < 0)
            {
                return RecursiveBinarySearch(word, list, min, mid - 1);
            }
            else
            {
                return RecursiveBinarySearch(word, list, mid + 1, max);
            }
        }
    }
}
