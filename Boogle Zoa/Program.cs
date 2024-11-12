namespace Boogle_Zoa
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test \n");

            Random random = new Random();

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

            Dice A1 = new Dice(random, lettersInformation);
            Dice A2 = new Dice(random, lettersInformation);
            Dice A3 = new Dice(random, lettersInformation);
            Dice A4 = new Dice(random, lettersInformation);
            Dice B1 = new Dice(random, lettersInformation);
            Dice B2 = new Dice(random, lettersInformation);
            Dice B3 = new Dice(random, lettersInformation);
            Dice B4 = new Dice(random, lettersInformation);
            Dice C1 = new Dice(random, lettersInformation);
            Dice C2 = new Dice(random, lettersInformation);
            Dice C3 = new Dice(random, lettersInformation);
            Dice C4 = new Dice(random, lettersInformation);
            Dice D1 = new Dice(random, lettersInformation);
            Dice D2 = new Dice(random, lettersInformation);
            Dice D3 = new Dice(random, lettersInformation);
            Dice D4 = new Dice(random, lettersInformation);

            Dice[] listOfDice = new Dice[16] { A1, A2, A3, A4, B1, B2, B3, B4, C1, C2, C3, C4, D1, D2, D3, D4 };

            Board B = new Board(listOfDice);

            Console.WriteLine(B.toString());

            DictionaryWords Dico = new DictionaryWords("../../../data/MotsPossiblesFR.txt", "FR");
            
            string word = Console.ReadLine();

            Console.WriteLine(B.GameBoard_Test(word, Dico));

            
        }
    }
}
