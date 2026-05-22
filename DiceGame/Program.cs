GuessingGame game = new GuessingGame(new Die(new Random()), new ConsoleInterface());
game.Run();

public class Die
{
    private readonly Random _random;

    public Die(Random random)
    {
        _random = random;
    }

    public int Roll()
    {
        return _random.Next(1, 7);
    }
}

public class ConsoleInterface
{
    public void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

    public string ReadInput()
    {
        return Console.ReadLine();
    }
}

public class GuessingGame
{
    private readonly int _targetNumber;
    private readonly ConsoleInterface _console;
    private int _remainingChances = 3;

    public GuessingGame(Die die, ConsoleInterface console)
    {
        _targetNumber = die.Roll();
        _console = console;
    }

    public void Run()
    {
        while (true)
        {
            _console.PrintMessage("Dice rolled. Enter what number it shows in three tries.");
            string? input = _console.ReadInput();

            if (!int.TryParse(input, out int guess))
            {
                _console.PrintMessage("Incorrect input, try again.");
                continue;
            }

            if (guess == _targetNumber)
            {
                _console.PrintMessage("Congratulations, you won!");
                WaitForExit();
                return;
            }

            _remainingChances--;
            _console.PrintMessage("Wrong number!");

            if (_remainingChances == 0)
            {
                _console.PrintMessage("You lose");
                WaitForExit();
                return;
            }
        }
    }

    private static void WaitForExit()
    {
        Console.ReadKey(true);
    }
}

