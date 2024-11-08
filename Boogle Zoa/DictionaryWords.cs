namespace Boogle_Zoa
{
    internal class DictionaryWords
    {
        #region Attributes

        private string[] words;
        private string language;

        private Dictionary<int, int> wordsBySize;
        private Dictionary<char, int> wordByLetter;

        #endregion


        #region Constructor

        public DictionaryWords()
        {

        }

        #endregion
    }
}
