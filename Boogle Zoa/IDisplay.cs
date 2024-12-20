namespace Boogle_Zoa
{
    /// <summary>
    /// Interface qui permet de définir des classes d'affichage comme ConsoleDisplay. Elle permet de séparer la logique de jeu et la logique d'affichage.
    /// </summary>
    public interface IDisplay
    {
        void SetupDisplay();

        void DisplayCentered(string text = "", int n = 1, bool selected = false);


        void DisplayWelcome();
        void DisplayWinner(Player winner);
        void DisplayCountDown();
        void DisplayGoodbye();



        string[] InitializePlayerName(int numberOfPlayer);
        void DisplayGame(int round, string name, Board board);
        string GetWord();
        void DisplayMessage(string message);


        int Menu(string[] options);


        int GetValideNumber(string prompt, int min, int max);
        TimeSpan GetValideTime(string prompt, string min, string max);
        string GetValideWord(string prompt, string[] options);

        
        bool IsWordAvailable();


        void PlayWelcomeSound();
        void PlayEndingSound();
        void PlaySoundButton1();
        void PlaySoundButton2();
    }
}
