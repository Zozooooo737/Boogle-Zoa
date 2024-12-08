namespace Boogle_Zoa
{
    public interface IDisplay
    {
        void SetupDisplay();

        void DisplayCentered(string text = "", int n = 1, bool selected = false);


        void DisplayWelcome();
        void DisplayWinner(Player winner);
        void DisplayGoodbye();


        string[] InitializePlayerName(int numberOfPlayer);
        void DisplayGame(int round, string name, Board board);

        string GetWord();

        void DisplayMessage(string message);


        int Menu(string[] options);
        int DisplayMenu();


        int GetValideNumber(string prompt, int min, int max);
        TimeSpan GetValideTime(string prompt, string min, string max);
        string GetValideWord(string prompt, string[] options);


        void PlaySoundWelcome();
        void PlaySoundButton1();
        void PlaySoundButton2();
    }
}
