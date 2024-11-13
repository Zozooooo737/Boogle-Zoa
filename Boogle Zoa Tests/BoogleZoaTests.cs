using Boogle_Zoa;

namespace Boogle_Zoa_Tests
{
    [TestClass]
    public class BoogleZoaTests
    {
        Dictionary<char, (int, int)> lettersInformation = new Dictionary<char, (int, int)>
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

        Random random = new Random();

        List<string> TestWords = new List<string>
        {
            "APPLE", "BANANA", "ORANGE", "GRAPE", "MELON", "BERRY", "KIWI", "MANGO",
            "PEACH", "LEMON", "LIME", "PEAR", "PLUM", "CHERRY", "DATE", "FIG",
            "APRICOT", "RAISIN", "BLUEBERRY", "STRAWBERRY", "RASPBERRY", "BLACKBERRY",
            "PINEAPPLE", "WATERMELON", "AVOCADO", "COCONUT", "MANGO", "TANGERINE", "MANDARIN"
        };

        Dictionary<int, List<string>> TestWordsBySize = new Dictionary<int, List<string>>
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

        Dictionary<char, List<string>> TestWordsByLetter = new Dictionary<char, List<string>>
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



        #region Player.cs

        [TestMethod]
        public void Player_InitializeName()
        {
            // Teste que le constructeur initialise correctement le joueur avec le nom
            Player p = new Player("Ezreal");
            string result = p.Name;

            Assert.AreEqual("Ezreal", result);
        }

        [TestMethod]
        public void Player_InitializeScoreToZero()
        {
            // Teste que le score est bien initialis� � 0 au d�part
            Player p = new Player("Pyke");
            int result = p.Score;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Player_InitializeWordsFoundedAsEmpty()
        {
            // Teste que la liste des mots trouv�s par le joueur est vide au d�part
            Player p = new Player("Xerath");
            List<string> result = p.WordsFound;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Contain_WordNotInWordsFound_ReturnsFalse()
        {
            // Teste que la m�thode Contain retourne false pour un mot qui n'a pas �t� trouv�
            Player p = new Player("Galio");
            bool result = p.Contain("Epee");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contain_WordInWordsFound_ReturnsTrue()
        {
            // Teste que la m�thode Contain retourne true pour un mot d�j� ajout�
            Player p = new Player("Galio");
            p.AddWord("trinite", lettersInformation);
            bool result = p.Contain("Trinite");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddWord_UniqueWord_AddsToWordsFound()
        {
            // Teste que la m�thode AddWord ajoute correctement un mot � la liste wordsFound
            Player player = new Player("Jinx");
            string word = "Piltover";
            player.AddWord(word, lettersInformation);
            bool result = player.WordsFound.Contains("PILTOVER");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddWord_DuplicateWord_DoesNotAddToWordsFound()
        {
            // Teste que la m�thode AddWord n'ajoute pas un mot d�j� trouv�
            Player player = new Player("Jayce");
            string word = "Magie";
            player.AddWord(word, lettersInformation);
            int scoreAfterFirstAdd = player.Score;
            player.AddWord(word, lettersInformation);
            List<string> result = player.WordsFound;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(scoreAfterFirstAdd, player.Score);
        }

        [TestMethod]
        public void AddWord_UniqueWord_UpdatesScoreCorrectly()
        {
            // Teste que la m�thode AddWord met � jour le score correctement en fonction des lettres
            Player player = new Player("Viktor");
            string word = "LOL";
            player.AddWord(word, lettersInformation);
            int expectedScore = lettersInformation['L'].Item1 + lettersInformation['O'].Item1 + lettersInformation['L'].Item1;

            Assert.AreEqual(expectedScore, player.Score);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectPlayerDescription()
        {
            // Teste que la m�thode toString retourne une description correcte du joueur avec le nom, score et mots trouv�s
            Player player = new Player("Powder");
            player.AddWord("Nexus", lettersInformation);
            player.AddWord("Canon", lettersInformation);
            string result = player.toString();
            string expectedDescription = "Joueur : Powder \n" +
                                         "Score : " + player.Score + " \n" +
                                         "Mots trouv�s : NEXUS ; CANON ; ";

            Assert.AreEqual(expectedDescription, result);
        }

        #endregion


        #region Dice.cs

        [TestMethod]
        public void Dice_IntializeLetters()
        {
            // Teste que le constructeur initialise correctement le tableau des lettres avec 6 �l�ments
            Dice d = new Dice(random, lettersInformation);
            char[] result = d.Letters;

            Assert.AreEqual(6, result.Length);
        }

        [TestMethod]
        public void VisibleLetter_ShouldReturnCorrectLetter()
        {
            // Teste que la lettre visible est bien pr�sente dans l'alphabet
            Dice d = new Dice(random, lettersInformation);
            char result = d.VisibleLetter;

            Assert.IsTrue(lettersInformation.Keys.Contains(result));
        }

        [TestMethod]
        public void Roll_ShouldActualizeUsedLetters()
        {
            // Teste que l'appel de Roll met � jour le compteur de lettres utilis�es pour la lettre visible
            Dice d = new Dice(random, lettersInformation);
            char letter = d.VisibleLetter;
            int result = Dice.UsedLetters[letter];

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectDiceDescription()
        {
            // Teste que la m�thode ToString retourne la description correcte des lettres du d�
            var dice = new Dice(random, lettersInformation);
            var letters = dice.Letters;
            char visibleLetter = dice.VisibleLetter;
            string result = dice.toString();
            string expected = "Lettre visible : " + visibleLetter + " \nEnsemble des lettres : ";
            foreach (char Lettre in letters)
            {
                expected += Lettre + " ; ";
            }

            Assert.AreEqual(expected, result);
        }

        #endregion


        #region DictionaryWord.cs

        [TestMethod]
        public void DictionaryWords_InitializeWords()
        {
            // Teste la cr�ation d'un dictionnaire � partir d'un fichier, en v�rifiant que le fichier est bien lu et que les mots sont stock�s correctement.
            DictionaryWords Dico = new DictionaryWords("../../../../Boogle Zoa/data/TestWords.txt", "EN");
            List<string> result = Dico.Words;

            CollectionAssert.AreEqual(TestWords, result);
        }

        [TestMethod]
        public void DictionaryWords_InitializeStructures()
        {
            // Teste les structure des dictionnaires "wordsBySize" et "wordsByLetter" apr�s initialisation, en v�rifiant qu'ils contiennent les bons mots par longueur et premi�re lettre.
            DictionaryWords Dico = new DictionaryWords("../../../../Boogle Zoa/data/TestWords.txt", "EN");

            foreach(int key in Dico.WordsBySize.Keys)
            {
                CollectionAssert.AreEqual(TestWordsBySize[key], Dico.WordsBySize[key]);
            }
            foreach (char key in Dico.WordsByLetter.Keys)
            {
                CollectionAssert.AreEqual(TestWordsByLetter[key], Dico.WordsByLetter[key]);
            }
        }

        [TestMethod]
        // Teste l'affichage du dictionnaire avec `toString`, en v�rifiant que la description g�n�r�e correspond bien � la structure attendue.
        public void Test_DictionaryWords_toString()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord1` avec un mot pr�sent dans le dictionnaire, pour v�rifier qu'elle renvoie bien `true`.
        public void Test_CheckWord1_WordPresent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord1` avec un mot absent du dictionnaire, pour v�rifier qu'elle renvoie bien `false`.
        public void Test_CheckWord1_WordAbsent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord2` avec un mot pr�sent dans le dictionnaire, pour v�rifier qu'elle renvoie bien `true`.
        public void Test_CheckWord2_WordPresent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord2` avec un mot absent du dictionnaire, pour v�rifier qu'elle renvoie bien `false`.
        public void Test_CheckWord2_WordAbsent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord3` avec un mot pr�sent dans le dictionnaire, pour v�rifier qu'elle renvoie bien `true`.
        public void Test_CheckWord3_WordPresent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord3` avec un mot absent du dictionnaire, pour v�rifier qu'elle renvoie bien `false`.
        public void Test_CheckWord3_WordAbsent()
        {

        }

        [TestMethod]
        // Teste la recherche dichotomique `RecursiveBinarySearch` avec un mot pr�sent dans la liste, pour v�rifier qu'elle renvoie `true`.
        public void Test_RecursiveBinarySearch_WordPresent()
        {

        }

        [TestMethod]
        // Teste la recherche dichotomique `RecursiveBinarySearch` avec un mot absent dans la liste, pour v�rifier qu'elle renvoie `false`.
        public void Test_RecursiveBinarySearch_WordAbsent()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord1` avec un mot dont la taille n'existe pas dans `wordsBySize`, pour s'assurer qu'elle renvoie `false`.
        public void Test_CheckWord1_SizeNotExist()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord2` avec un mot commen�ant par une lettre non pr�sente dans `wordsByLetter`, pour v�rifier qu'elle renvoie `false`.
        public void Test_CheckWord2_LetterNotExist()
        {

        }

        [TestMethod]
        // Teste la m�thode `CheckWord3` avec un mot dont la taille et la premi�re lettre ne sont pas pr�sentes dans le dictionnaire, pour v�rifier qu'elle renvoie `false`.
        public void Test_CheckWord3_SizeAndLetterNotExist()
        {

        }

        [TestMethod]
        // Teste la M�thode 'BubbleSort' avec une liste de mot donn�e, pour v�rifier si il la trie correctement.
        public void Test_BubbleSort_jspCommentOnNomme()
        {

        }


        #endregion
    }
}