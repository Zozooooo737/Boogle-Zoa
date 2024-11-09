namespace Boogle_Zoa
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test \n");

            Random random = new Random();

            Dictionary<char, int[]> lettersInforamtion = new Dictionary<char, int[]>
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
            }; // A FAIRE

            Dice A1 = new Dice(random, lettersInforamtion);
            Dice A2 = new Dice(random, lettersInforamtion);
            Dice A3 = new Dice(random, lettersInforamtion);
            Dice A4 = new Dice(random, lettersInforamtion);
            Dice B1 = new Dice(random, lettersInforamtion);
            Dice B2 = new Dice(random, lettersInforamtion);
            Dice B3 = new Dice(random, lettersInforamtion);
            Dice B4 = new Dice(random, lettersInforamtion);
            Dice C1 = new Dice(random, lettersInforamtion);
            Dice C2 = new Dice(random, lettersInforamtion);
            Dice C3 = new Dice(random, lettersInforamtion);
            Dice C4 = new Dice(random, lettersInforamtion);
            Dice D1 = new Dice(random, lettersInforamtion);
            Dice D2 = new Dice(random, lettersInforamtion);
            Dice D3 = new Dice(random, lettersInforamtion);
            Dice D4 = new Dice(random, lettersInforamtion);

            Dice[] listOfDice = new Dice[16] { A1, A2, A3, A4, B1, B2, B3, B4, C1, C2, C3, C4, D1, D2, D3, D4 };

            Board B = new Board(listOfDice);

            Console.WriteLine(B.toString());

            DictionaryWords Dico = new DictionaryWords("../../../data/MotsPossiblesFR.txt", "FR");

            Console.WriteLine(Dico.toString());

            Console.WriteLine(Dico.RechDichoRecursif("Table"));
            Player noa = new Player("noa");
            Player p = new Player("zozo");
            Console.WriteLine(p.Score);
            p.Contain("cc");
            p.AddWord("cc", lettersInforamtion);
            Console.WriteLine(p.toString());
            p.toString();
        }
    }
}
