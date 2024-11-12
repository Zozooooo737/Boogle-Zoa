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


        #region Player.cs

        [TestMethod]
        public void Player_InitializeName()
        {
            // Test que le constructeur initialise correctement le joueur avec le nom
            Player p = new Player("Ezreal");
            string result = p.Name;

            Assert.AreEqual("Ezreal", result);
        }

        [TestMethod]
        public void Player_InitializeScoreToZero()
        {
            // Test que le score est bien initialisé à 0 au départ
            Player p = new Player("Pyke");
            int result = p.Score;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Player_InitializeWordsFoundedAsEmpty()
        {
            // Test que la liste des mots trouvés par le joueur est vide au départ
            Player p = new Player("Xerath");
            List<string> result = p.WordsFound;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Contain_WordNotInWordsFound_ReturnsFalse()
        {
            // Test que la méthode Contain retourne false pour un mot qui n'a pas été trouvé
            Player p = new Player("Galio");
            bool result = p.Contain("Epee");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contain_WordInWordsFound_ReturnsTrue()
        {
            // Test que la méthode Contain retourne true pour un mot déjà ajouté
            Player p = new Player("Galio");
            p.AddWord("trinite", lettersInformation);
            bool result = p.Contain("Trinite");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddWord_UniqueWord_AddsToWordsFound()
        {
            // Test que la méthode AddWord ajoute correctement un mot à la liste wordsFound
            Player player = new Player("Jinx");
            string word = "Piltover";
            player.AddWord(word, lettersInformation);
            bool result = player.WordsFound.Contains("PILTOVER");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddWord_DuplicateWord_DoesNotAddToWordsFound()
        {
            // Test que la méthode AddWord n'ajoute pas un mot déjà trouvé
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
            // Test que la méthode AddWord met à jour le score correctement en fonction des lettres
            Player player = new Player("Viktor");
            string word = "LOL";
            player.AddWord(word, lettersInformation);
            int expectedScore = lettersInformation['L'].Item1 + lettersInformation['O'].Item1 + lettersInformation['L'].Item1;

            Assert.AreEqual(expectedScore, player.Score);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectPlayerDescription()
        {
            // Test que la méthode toString retourne une description correcte du joueur avec le nom, score et mots trouvés
            Player player = new Player("Powder");
            player.AddWord("Nexus", lettersInformation);
            player.AddWord("Canon", lettersInformation);
            string result = player.toString();
            string expectedDescription = "Joueur : Powder \n" +
                                         "Score : " + player.Score + " \n" +
                                         "Mots trouvés : NEXUS ; CANON ; ";

            Assert.AreEqual(expectedDescription, result);
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
            // Teste que l'appel de Roll met à jour le compteur de lettres utilisées pour la lettre visible
            Dice d = new Dice(random, lettersInformation);
            char letter = d.VisibleLetter;
            int result = Dice.UsedLetters[letter];

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectDiceDescription()
        {
            // Teste que la méthode ToString retourne la description correcte des lettres du dé
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
    }
}