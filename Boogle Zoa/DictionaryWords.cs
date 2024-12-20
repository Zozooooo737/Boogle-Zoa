namespace Boogle_Zoa
{
    /// <summary>
    /// Représente un dictionnaire qui regroupe tous les mots d'une langue (<see cref="language">).  
    /// </summary>
    public class DictionaryWords
    {
        /// <summary>
        /// Liste contenant tous les mots du dictionnaire.
        /// </summary>
        private readonly List<string> words;

        /// <summary>
        /// Langue du dictionnaire représentée par un code (par exemple, "FR" pour français, "EN" pour anglais).
        /// </summary>
        private readonly string language;

        /// <summary>
        /// Dictionnaire associant chaque taille de mot (int) à une liste de mots correspondant à cette taille.
        /// </summary>
        private readonly Dictionary<int, List<string>> wordsBySize = new Dictionary<int, List<string>> { };

        /// <summary>
        /// Dictionnaire associant chaque lettre (char) à une liste de mots qui commencent par cette lettre.
        /// </summary>
        private readonly Dictionary<char, List<string>> wordsByLetter = new Dictionary<char, List<string>> { };



        /// <summary>
        /// Crée un dictionnaire d'une langue (<paramref name="language"/>) à partir d'un fichier par son chemin d'accès (<paramref name="filePath"/>)
        /// </summary>
        /// <param name="filePath">Chemin d'accès au fichier de mots.</param>
        /// <param name="language">Langue du dictionnaire.</param>
        /// En un parcours de la liste words, on parvient à remplir les deux dictionnaires.
        /// A la fin du parcours, on trie chaque liste présente dans les 2 dictionnaires afin de faciliter le travail des algorithmes de recherche.
        /// Si le fichier n'existe pas, on catch l'erreur et initialise `words` avec une liste vide.
        /// Optimisation de la Mémoire --> On initialise "int n" et "int c" à l'extérieur de la boucle "foreach" pour réaliser que 2 allocations au lieu de 2*n.
        /// Optimisation de la Mémoire --> Les clés des deux dictionnaire "wordsBySize" et "wordsByLetter" ne sont pas prédéfinies afin de gagner en espace de mémoire. Exemple : si dans mon fichier, aucun mot ne commence par la lettre "F", le couple {'F', List'string'} ne sera pas présent dans wordsByLetter.
        public DictionaryWords(string language)
        {
            this.language = language;
            words = ReadDictionnary(language);

            QuickSort(words, 0, words.Count - 1);

            int n;
            char c;
            
            foreach (string word in words) 
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
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier.</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// On considère que le mot en paramètre `word` n'est pas nul ou vide. Cette condition sera vérifié quand l'utilisateur entrera un mot dans la console.
        /// Avant de récupérer la liste restreinte qui contient potentiellement le mot, on vérifie que la clé correspondant au mot existe dans le dictionnaire, afin de ne pas générer une erreur de type `KeyNotFoundException`.
        /// Tentative 1 : On récupère la liste des mots qui ont la même taille que "word", et on effectue une recherche dichotomique dans cette liste.
        public bool CheckWord1(string word)
        {
            bool exist = false;

            word = word.ToUpper();
            int n = word.Length;

            if (wordsBySize.ContainsKey(n))
            {
                List<string> sameLengthWords = wordsBySize[n];
                exist = RecursiveBinarySearch(word, sameLengthWords, 0, sameLengthWords.Count - 1);
            }
            return exist;
        }


        /// <summary>
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier.</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// On considère que le mot en paramètre `word` n'est pas nul ou vide. Cette condition sera vérifié quand l'utilisateur entrera un mot dans la console.
        /// On vérifie que les clés sont présents dans les dictionnaires  avant de les utiliser pour éviter une erreur de type `KeyNotFoundException`.
        /// En effet, le mot existe dans le dictionnaire si et seulement si ses 2 clés sont présentes dans leur dictionnaire respectif.
        /// Tentative 2 : On récupère la liste des mots qui ont la même taille que "word" et la liste des mots qui commence par la même lettre que "word".
        ///               On effectue une recherche dichotomique sur la liste la plus petite.
        /// Optimisation de la Mémoire : On effectue 2 "return" pour éviter de recréer une liste de "string" qui occupera un espace dans la mémoire inutile.
        public bool CheckWord2(string word)
        {
            bool exist = false;

            word = word.ToUpper();
            int n = word.Length;
            char c = word[0];

            if (wordsBySize.ContainsKey(n) && wordsByLetter.ContainsKey(c))
            {
                List<string> sameLengthWords = wordsBySize[n];
                List<string> sameLetterWords = wordsByLetter[c];

                if (sameLengthWords.Count < sameLetterWords.Count)
                {
                    exist = RecursiveBinarySearch(word, sameLengthWords, 0, sameLengthWords.Count-1);
                }
                else
                {
                    exist = RecursiveBinarySearch(word, sameLetterWords, 0, sameLetterWords.Count - 1);
                }
            }
            
            return exist;
        }


        /// <summary>
        /// Vérifie la présence d'un mot (<paramref name="word"/>) dans le dictionnaire à travers une recherche dichotomique (<see cref="RecursiveBinarySearch"/>) en déterminant une liste de mots restreinte et triée.
        /// </summary>
        /// <param name="word">Mot à vérifier.</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// On considère que le mot en paramètre `word` n'est pas nul ou vide. Cette condition sera vérifié quand l'utilisateur entrera un mot dans la console.
        /// On vérifie que les clés sont présents dans les dictionnaires  avant de les utiliser pour éviter une erreur de type `KeyNotFoundException`.
        /// En effet, le mot existe dans le dictionnaire si et seulement si ses 2 clés sont présentes dans leur dictionnaire respectif.        /// La méthode "Intersect" renvoie une liste de type "IEnumerable" de string, nous devons la convertir en "List" avec la méthode "ToList()".
        /// Tentative 3 : On récupère la liste des mots qui ont la même taille que "word" et la liste des mots qui commence par la même lettre que "word".
        ///               On effectue une recherche dichotomique sur l'intersection de ces 2 listes.
        public bool CheckWord3(string word)
        {
            bool exist = false;
            
            word = word.ToUpper();
            int n = word.Length;
            char c = word[0];

            if (wordsBySize.ContainsKey(n) && wordsByLetter.ContainsKey(c))
            {
                List<string> sameLengthWords = wordsBySize[n];
                List<string> sameLetterWords = wordsByLetter[c];

                List<string> commonWords = sameLengthWords.Intersect(sameLetterWords).ToList();
                exist = RecursiveBinarySearch(word, commonWords, 0, commonWords.Count - 1);
            }
            return exist;
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

            for (int i = 0; i < wordsBySize.Keys.Max(); i++)
            {
                if (wordsBySize.Keys.Contains(i))
                {
                    description += $"{i} : {wordsBySize[i].Count}\n";
                }
            }

            description += "\nNombre de mots par lettre :\n";

            for (int i = 0; i < alpha.Length; i++)
            {
                if (wordsByLetter.Keys.Contains(alpha[i]))
                {
                    description += $"{alpha[i]} : {wordsByLetter[alpha[i]].Count}\n";
                }
            }
            return description;
        }



        /// <summary>
        /// Trie une liste de mots (<paramref name="list"/>) par ordre alphabétique, en utilisant la méthode du tri rapide.
        /// </summary>
        /// <param name="list">Liste de mots à trier.</param>
        /// <param name="start">Index de début.</param>
        /// <param name="end">INdex de fin.</param>
        /// Optimisation de la Complexité --> Nous avons choisi cette méthode de tri car elle permet d'avoir un temps d'execution incroyablement réduit.
        /// Optimisation de la Mémoire --> Nous avons intialisé la variable `temp` en dehors de la boucle, pour réduire l'allocation de la mémoire à 1 case. 
        public static void QuickSort(List<string> list, int start, int end)
        {
            if (end - start < 1) return;

            string pivot = list[end];
            int wall = start;
            int current = start;
            string temp;

            while (current < end)
            {
                if (string.Compare(list[current], pivot) < 0)
                {
                    if (wall != current)
                    {
                        temp = list[current];
                        list[current] = list[wall];
                        list[wall] = temp;
                    }
                    wall++;
                }
                current++;
            }

            string tmpPivot = list[wall];
            list[wall] = list[end];
            list[end] = tmpPivot;

            QuickSort(list, start, wall - 1);
            QuickSort(list, wall + 1, end);
        }


        /// <summary>
        /// Réalise une recherche dichotomique pour trouver un mot (<paramref name="mot"/>) dans une liste de mot (<paramref name="list"/>).
        /// </summary>
        /// <param name="mot">Mot à trouver.</param>
        /// <param name="list">Liste dans laquelle on cherche le mot.</param>
        /// <param name="min">Indice délimitant la borne inférieure.</param>
        /// <param name="max">Indice délimitant la borne supérieure.</param>
        /// <returns><c>true</c> si le mot est présent dans la liste ; sinon, <c>false</c>.</returns>
        /// On vérifie que les paramètres sont bons avant de lancer la recherche récursive. 
        /// Recursive Binary Search = Recherche Dichotomique Recursive : Méthode de classe car elle ne dépends pas d'une instance particulière.
        public static bool RecursiveBinarySearch(string word, List<string> list, int min, int max)
        {
            if (min > max || list == null || list.Count == 0)
            {
                return false;
            }

            int mid = (min + max) / 2;

            int comparaison = string.Compare(word, list[mid]);

            if (comparaison == 0)
            {
                return true;
            }
            else if (comparaison < 0)
            {
                return RecursiveBinarySearch(word, list, min, mid - 1);
            }
            else
            {
                return RecursiveBinarySearch(word, list, mid + 1, max);
            }
        }


        /// <summary>
        /// Lit un fichier dictionnaire contenant une liste de mots en fonction de la langue spécifiée (<paramref name="language"/>). <br/>
        /// Si la langue n'est ni "FR" ni "EN", un fichier de test est utilisé par défaut.
        /// </summary>
        /// <returns>
        /// Une liste de mots extraits du fichier dictionnaire.
        /// </returns>
        private static List<string> ReadDictionnary(string language)
        {
            List<string> listOfWords;
            string filePath;

            if (language == "FR")
            {
                filePath = "../../../data/MotsPossiblesFR.txt";
            }
            else if (language == "EN")
            {
                filePath = "../../../data/MotsPossiblesEN.txt";
            }
            else
            {
                filePath = "../../../../Boogle Zoa/data/data test/TestWords.txt";
            }

            try
            {
                string content = File.ReadAllText(filePath);
                listOfWords = new List<string>(content.Split(' '));
            }
            catch (FileNotFoundException f)
            {
                listOfWords = new List<string> { };
                Console.WriteLine("Le fichier n'existe pas " + f.Message);
            }
            return listOfWords;
        }
    }
}
