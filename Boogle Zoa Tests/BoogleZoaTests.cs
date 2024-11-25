using Boogle_Zoa;

namespace Boogle_Zoa_Tests
{
    [TestClass]
    public class BoogleZoaTests
    {
        readonly Dictionary<char, (int, int)> lettersInformation = new Dictionary<char, (int, int)>
        {
            {'A', (1, 9)},
            {'B', (3, 2)},
            {'C', (3, 2)},
            {'D', (2, 3)},
            {'E', (1, 15)},
            {'F', (4, 2)},
            {'G', (2, 2)},
            {'H', (4, 2)},
            {'I', (1, 8)},
            {'J', (8, 1)},
            {'K', (10, 1)},
            {'L', (1, 5)},
            {'M', (2, 3)},
            {'N', (1, 6)},
            {'O', (1, 6)},
            {'P', (3, 2)},
            {'Q', (8, 1)},
            {'R', (1, 6)},
            {'S', (1, 6)},
            {'T', (1, 6)},
            {'U', (1, 6)},
            {'V', (4, 2)},
            {'W', (10, 1)},
            {'X', (10, 1)},
            {'Y', (10, 1)},
            {'Z', (10, 1)}
        };

        readonly Random random = new Random();

        readonly List<string> testWords = new List<string>
        {
            "APPLE", "APRICOT", "AVOCADO", "BANANA", "BERRY", "BLACKBERRY", "BLUEBERRY", 
            "CHERRY", "COCONUT", "DATE", "FIG", "GRAPE", "KIWI", "LEMON", "LIME", 
            "MANDARIN", "MANGO", "MANGO", "MELON", "ORANGE", "PEACH", "PEAR", "PINEAPPLE", 
            "PLUM", "RAISIN", "RASPBERRY", "STRAWBERRY", "TANGERINE", "WATERMELON"
        };

        readonly Dictionary<int, List<string>> testWordsBySize = new Dictionary<int, List<string>>
        {
            { 3, new List<string> { "FIG" } },
            { 4, new List<string> { "DATE", "KIWI", "LIME", "PEAR", "PLUM" } },
            { 5, new List<string> { "APPLE", "BERRY", "GRAPE", "LEMON", "MANGO", "MANGO", "MELON", "PEACH" } },
            { 6, new List<string> { "BANANA", "CHERRY", "ORANGE", "RAISIN" } },
            { 7, new List<string> { "APRICOT", "AVOCADO", "COCONUT" } },
            { 8, new List<string> { "MANDARIN" } },
            { 9, new List<string> { "BLUEBERRY", "PINEAPPLE", "RASPBERRY", "TANGERINE" } },
            { 10, new List<string> { "BLACKBERRY", "STRAWBERRY", "WATERMELON" } }
        };

        readonly Dictionary<char, List<string>> testWordsByLetter = new Dictionary<char, List<string>>
        {
            { 'A', new List<string> { "APPLE", "APRICOT", "AVOCADO" } },
            { 'B', new List<string> { "BANANA", "BERRY", "BLACKBERRY", "BLUEBERRY" } },
            { 'C', new List<string> { "CHERRY", "COCONUT" } },
            { 'D', new List<string> { "DATE" } },
            { 'F', new List<string> { "FIG" } },
            { 'G', new List<string> { "GRAPE" } },
            { 'K', new List<string> { "KIWI" } },
            { 'L', new List<string> { "LEMON", "LIME" } },
            { 'M', new List<string> { "MANDARIN", "MANGO", "MANGO", "MELON" } },
            { 'O', new List<string> { "ORANGE" } },
            { 'P', new List<string> { "PEACH", "PEAR", "PINEAPPLE", "PLUM" } },
            { 'R', new List<string> { "RAISIN", "RASPBERRY" } },
            { 'S', new List<string> { "STRAWBERRY" } },
            { 'T', new List<string> { "TANGERINE" } },
            { 'W', new List<string> { "WATERMELON" } }
        };

        readonly Dice[] testDices = { new Dice('A'), new Dice('R'), new Dice('D'), new Dice('D'), new Dice('C'), new Dice('K'), new Dice('G'), new Dice('A'), new Dice('F'), new Dice('I'), new Dice('E'), new Dice('T'), new Dice('I'), new Dice('W'), new Dice('G'), new Dice('X') };

        readonly DictionaryWords testDictionnary = new DictionaryWords("");

        #region Player.cs

        [TestMethod]
        public void Player_Constructor_ShouldInitializeNameCorrectly()
        {
            // Teste que le constructeur initialise correctement le joueur avec son nom.
            string expectedName = "Ezreal";

            Player player = new Player(expectedName);
            string actualName = player.Name;

            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        public void Player_Constructor_ShouldInitializeScoreToZero()
        {
            // Teste que le score est bien initialisé à 0 au départ
            string playerName = "Pyke";
            int expectedScore = 0;
            Player p = new Player(playerName);

            int actualScore = p.Score;

            Assert.AreEqual(expectedScore, actualScore);

        }

        [TestMethod]
        public void Player_InitializeWordsFoundedAsEmpty()
        {
            // Teste que la liste des mots trouvés par le joueur est vide au départ
            string playerName = "Xerath";
            Player player = new Player(playerName);

            List<string> wordsFound = player.WordsFound;

            Assert.IsNotNull(wordsFound);
            Assert.AreEqual(0, wordsFound.Count);
        }

        [TestMethod]
        public void Contain_WordNotInWordsFound_ReturnsFalse()
        {
            // Teste que la méthode `Contain` retourne `false` pour un mot qui n'a pas été trouvé
            string playerName = "Galio";
            string wordToCheck = "Epee";
            Player player = new Player(playerName);

            bool isContained = player.Contain(wordToCheck);

            Assert.IsFalse(isContained);
        }

        [TestMethod]
        public void Contain_WordInWordsFound_ReturnsTrue()
        {
            // Teste que la méthode `Contain` retourne `true` pour un mot déjà ajouté
            string playerName = "Akali";
            string wordToCheck = "Trinite";
            Player p = new Player(playerName);
            p.AddWord("trinite", lettersInformation);
                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            bool isContained = p.Contain(wordToCheck);

            Assert.IsTrue(isContained);
        }

        [TestMethod]
        public void AddWord_UniqueWord_AddsToWordsFound()
        {
            // Teste que la méthode `AddWord` ajoute correctement un mot à la liste wordsFound
            string playerName = "Jinx";
            string word = "Piltover";
            Player player = new Player(playerName);

            player.AddWord(word, lettersInformation);

            bool isWordAdded = player.WordsFound.Contains("PILTOVER");
            Assert.IsTrue(isWordAdded);
        }

        [TestMethod]
        public void AddWord_DuplicateWord_DoesNotAddToWordsFound()
        {
            // Teste que la méthode `AddWord` n'ajoute pas un mot déjà trouvé
            string playerName = "Jayce";
            string word = "Magie";
            Player player = new Player(playerName);
            player.AddWord(word, lettersInformation);
            int scoreAfterFirstAdd = player.Score;

            player.AddWord(word, lettersInformation);

            List<string> wordsFound = player.WordsFound;
            Assert.AreEqual(1, wordsFound.Count);
            Assert.AreEqual(scoreAfterFirstAdd, player.Score);
        }

        [TestMethod]
        public void AddWord_UniqueWord_UpdatesScoreCorrectly()
        {
            // Teste que la méthode `AddWord` met à jour le score correctement en fonction des lettres
            string playerName = "Viktor";
            string word = "LOL";
            Player player = new Player(playerName);
            int expectedScore = lettersInformation['L'].Item1 + lettersInformation['O'].Item1 + lettersInformation['L'].Item1;

            player.AddWord(word, lettersInformation);

            Assert.AreEqual(expectedScore, player.Score);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectPlayerDescription()
        {
            // Teste que la méthode `toString` retourne une description correcte du joueur avec le nom, score et mots trouvés
            string playerName = "Powder";
            string word1 = "Nexus";
            string word2 = "Canon";
            Player player = new Player(playerName);
            player.AddWord(word1, lettersInformation);
            player.AddWord(word2, lettersInformation);
            string expectedDescription = "Joueur : Powder \n" +
                                    "Score : " + player.Score + " \n" +
                                    "Mots trouvés : NEXUS ; CANON ; ";

            string actualDescription = player.toString();
            

            Assert.AreEqual(expectedDescription, actualDescription);
        }

        #endregion


        #region Dice.cs

        [TestMethod]
        public void Dice_IntializeLetters()
        {
            // Teste que le constructeur initialise correctement le tableau des lettres avec 6 éléments
            Dice d = new Dice(random, lettersInformation);
            char[] result = d.Letters;

            Assert.AreEqual(6, result.Length);
        }

        [TestMethod]
        public void VisibleLetter_ShouldReturnCorrectLetter()
        {
            // Teste que la lettre visible est bien présente dans l'alphabet
            Dice d = new Dice(random, lettersInformation);
            char result = d.VisibleLetter;

            Assert.IsTrue(lettersInformation.Keys.Contains(result));
        }

        [TestMethod]
        public void Roll_ShouldActualizeUsedLetters()
        {
            // Teste que la méthode `Roll` met à jour le compteur de lettres utilisées pour la lettre visible
            Dice d = new Dice(random, lettersInformation);
            char letter = d.VisibleLetter;
            int result = Dice.UsedLetters[letter];

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectDiceDescription()
        {
            // Teste que la méthode `toString` retourne la description correcte des lettres du dé
            var dice = new Dice(random, lettersInformation);
            var letters = dice.Letters;
            char visibleLetter = dice.VisibleLetter;
            string result = dice.toString();
            string expectedString = "Lettre visible : " + visibleLetter + " \nEnsemble des lettres : ";
            foreach (char Lettre in letters)
            {
                expectedString += Lettre + " ; ";
            }

            Assert.AreEqual(expectedString, result);
        }

        #endregion


        #region DictionaryWord.cs

        [TestMethod]
        public void DictionaryWords_InitializeWords()
        {
            // Teste que la création d'un dictionnaire à partir d'un fichier, en vérifiant que le fichier est bien lu et que les mots sont stockés correctement.
            DictionaryWords dico = new DictionaryWords("");
            List<string> result = dico.Words;

            CollectionAssert.AreEqual(testWords, result);
        }

        [TestMethod]
        public void DictionaryWords_InitializeStructures()
        {
            // Teste que les structure des dictionnaires `wordsBySize` et `wordsByLetter` contiennent les bons mots par longueur et première lettre.
            DictionaryWords dico = new DictionaryWords("");

            foreach(int key in dico.WordsBySize.Keys)
            {
                CollectionAssert.AreEqual(testWordsBySize[key], dico.WordsBySize[key]);
            }
            foreach (char key in dico.WordsByLetter.Keys)
            {
                CollectionAssert.AreEqual(testWordsByLetter[key], dico.WordsByLetter[key]);
            }
        }

        [TestMethod]
        public void BubbleSort_ShouldSortList()
        {
            // Teste que la méthode `BubbleSort` trie la liste de mots en paramètre correctement.
            List<string> UnsortedList = new List<string>() { "HEXTECH", "PILTOVER", "DEMACIA", "ENFORCER" };
            List<string> SortedList = new List<string>() { "DEMACIA", "ENFORCER", "HEXTECH", "PILTOVER" };
            DictionaryWords.BubbleSort(UnsortedList);

            CollectionAssert.AreEqual(SortedList, UnsortedList);
        }

        [TestMethod]
        public void CheckWord1_WordPresent_ReturnsTrue()
        {
            // Teste que la méthode `CheckWord1` renvoie `true` avec un mot présent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord1("Apple");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckWord1_WordAbsent_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord1` renvoie `false` avec un mot absent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord1("pple");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckWord2_WordPresent_ReturnsTrue()
        {
            // Teste que la méthode `CheckWord2` renvoie `true` avec un mot présent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord2("orange");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckWord2_WordAbsent_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord2` renvoie `false` avec un mot absent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord2("range");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckWord3_WordPresent_ReturnsTrue()
        {
            // Teste que la méthode `CheckWord3` renvoie `true` avec un mot présent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord3("mAnDaRiN");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckWord3_WordAbsent_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord3` renvoie `false` avec un mot absent dans le dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord3("mand arin");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RecursiveBinarySearch_WordPresent_ReturnsTrue()
        {
            // Teste que la recherche dichotomique `RecursiveBinarySearch` renvoie `true` avec un mot présent dans la liste.
            bool result = DictionaryWords.RecursiveBinarySearch("PLUM", testWords, 0, testWords.Count - 1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RecursiveBinarySearch_WordAbsent_ReturnsFalse()
        {
            // Teste que la recherche dichotomique `RecursiveBinarySearch` renvoie `false` avec un mot absent dans la liste.
            bool result = DictionaryWords.RecursiveBinarySearch("PLU", testWords, 0, testWords.Count - 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckWord1_SizeNotExist_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord1` renvoie `false` avec un mot dont la taille n'existe pas dans `wordsBySize`.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord1("STRAWBERRIES");

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void CheckWord2_LetterNotExist_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord2` renvoie `false` avec un mot dont la première lettre n'existe pas dans `wordsByLetter`.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord2("EKKO");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckWord3_SizeAndLetterNotExist_ReturnsFalse()
        {
            // Teste que la méthode `CheckWord3` renvoie `false` avec un mot dont la taille n'existe pas dans `wordsBySize`.
            DictionaryWords dico = new DictionaryWords("");
            bool result = dico.CheckWord2("HELLICOPTEREEEE");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectDictionaryWordsDescription()
        {
            // Teste que la méthode `toString` retourne la description correcte du dictionnaire.
            DictionaryWords dico = new DictionaryWords("");
            string expectedString = "Description du Dictionnaire \n\n" +
                            "Langue : EN \n\n" +
                            "Nombre de mots par longueur :\n" +
                            "3 : 1\n" +
                            "4 : 5\n" +
                            "5 : 8\n" +
                            "6 : 4\n" +
                            "7 : 3\n" +
                            "8 : 1\n" +
                            "9 : 4\n\n" +
                            "Nombre de mots par lettre :\n" +
                            "A : 3\n" +
                            "B : 4\n" +
                            "C : 2\n" +
                            "D : 1\n" +
                            "F : 1\n" +
                            "G : 1\n" +
                            "K : 1\n" +
                            "L : 2\n" +
                            "M : 4\n" +
                            "O : 1\n" +
                            "P : 4\n" +
                            "R : 2\n" +
                            "S : 1\n" +
                            "T : 1\n" +
                            "W : 1\n";

            string result = dico.toString();

            Assert.AreEqual(expectedString, result);
        }


        #endregion


        #region Board.cs

        [TestMethod]
        public void Board_InitializeVisbleLetters()
        {
            // Teste que le constructeur initialise correctement le tableau des lettres visibles du plateau.
            Board b = new Board(testDices);
            char[] expectedLetters = { 'A', 'R', 'D', 'D', 'C', 'K', 'G', 'A', 'F', 'I', 'E', 'T', 'I', 'W', 'G', 'X' };
            char[] result = b.VisibleLetter;

            CollectionAssert.AreEqual(expectedLetters, result);
        }

        [TestMethod]
        public void Board_InitializeBoardOfLetters()
        {
            // Teste que le constructeur initialise correctement la matrice qui représente le plateau.
            Board b = new Board(testDices);
            char[,] expectedBoard = { { 'A', 'R', 'D', 'D' }, 
                                      { 'C', 'K', 'G', 'A' }, 
                                      { 'F', 'I', 'E', 'T' }, 
                                      { 'I', 'W', 'G', 'X' } };
            char[,] result = b.BoardOfLetters;

            CollectionAssert.AreEqual(expectedBoard, result);
        }

        [TestMethod]
        public void GameBoardTest_WordPresent_ReturnTrue()
        {
            // Teste que la méthode `GameBoardTest` renvoie `true` avec un mot présent sur le plateau.
            Board b = new Board(testDices);
            bool result = b.GameBoardTest("fig", testDictionnary);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GameBoardTest_WordAbsent_ReturnFalse()
        {
            // Teste que la méthode `GameBoardTest` renvoie `false` avec un mot absent sur le plateau.
            Board b = new Board(testDices);
            bool result = b.GameBoardTest("LIME", testDictionnary);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FindAllWordPaths_WordPresentOnce_ReturnListOfOneElement()
        {
            // Test que la méthode `FindAllWordPaths` renvoie une liste de taille 1 qui contient les coordonnées des lettres avec un mot présent en une fois sur le plateau.
            Board b = new Board(testDices);
            List<List<(int,int)>> result = b.FindAllWordPaths("Kiwi");

            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod]
        public void FindAllWordPaths_WordPresentTwice_ReturnListOfTwoElement()
        {
            // Test que la méthode `FindAllWordPaths` renvoie une liste de taille 2 qui contient les coordonnées des lettres avec un mot présent en deux fois sur le plateau.
            Board b = new Board(testDices);
            List<List<(int, int)>> result = b.FindAllWordPaths("fiG");

            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void FindAllWordPaths_WordAbsent_ReturnListOfZeroElement()
        {
            // Test que la méthode `FindAllWordPaths` renvoie une liste de taille 0 avec un mot absent sur le plateau.
            Board b = new Board(testDices);
            List<List<(int, int)>> result = b.FindAllWordPaths("PEAR");

            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectBoardDescription()
        {
            // Test que la méthode `toString` retourne la description correcte du plateau de jeu.
            Board b = new Board(testDices);
            string expectedString = "A R D D \n" +
                                    "C K G A \n" +
                                    "F I E T \n" +
                                    "I W G X \n";

            Assert.AreEqual(expectedString, b.toString());
        }

        #endregion
    }
}