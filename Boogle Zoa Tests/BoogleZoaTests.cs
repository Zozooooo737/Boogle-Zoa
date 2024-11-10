using Boogle_Zoa;

namespace Boogle_Zoa_Tests
{
    [TestClass]
    public class BoogleZoaTests
    {
        Dictionary<char, int[]> lettersInformation = new Dictionary<char, int[]>
            {
                {'A', new int[] {1, 9}},
                {'B', new int[] {3, 2}},
                {'C', new int[] {3, 2}},
                {'D', new int[] {2, 3}},
                {'E', new int[] {1, 15}},
                {'F', new int[] {4, 2}},
                {'G', new int[] {2, 2}},
                {'H', new int[] {4, 2}},
                {'I', new int[] {1, 8}},
                {'J', new int[] {8, 1}},
                {'K', new int[] {10, 1}},
                {'L', new int[] {1, 5}},
                {'M', new int[] {2, 3}},
                {'N', new int[] {1, 6}},
                {'O', new int[] {1, 6}},
                {'P', new int[] {3, 2}},
                {'Q', new int[] {8, 1}},
                {'R', new int[] {1, 6}},
                {'S', new int[] {1, 6}},
                {'T', new int[] {1, 6}},
                {'U', new int[] {1, 6}},
                {'V', new int[] {4, 2}},
                {'W', new int[] {10, 1}},
                {'X', new int[] {10, 1}},
                {'Y', new int[] {10, 1}},
                {'Z', new int[] {10, 1}}
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
            int expectedScore = lettersInformation['L'][0] + lettersInformation['O'][0] + lettersInformation['L'][0];

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
            Dice d = new Dice(random, lettersInformation);
            char[] result = d.Letters;

            Assert.AreEqual(6, result.Length);
        }

        [TestMethod]
        public void VisibleLetter_ShouldReturnCorrectLetter()
        {
            Dice d = new Dice(random, lettersInformation);
            char result = d.VisibleLetter;
            Assert.IsTrue(lettersInformation.Keys.Contains(result));
        }

        [TestMethod]
        public void Roll_ShouldActualizeUsedLetters()
        {
            Dice d = new Dice(random, lettersInformation);
            char letter = d.VisibleLetter;
            int result = Dice.UsedLetters[letter];

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectDiceDescription()
        {
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