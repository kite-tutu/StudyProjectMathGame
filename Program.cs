/*
Requirements:
1. You need to create a Math game containing the 4 basic operations

2.The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.

3. Users should be presented with a menu to choose an operation

4. You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.

Additional Challenges:

1. Try to implement levels of difficulty.

2. Add a timer to track how long the user takes to finish the game.

3. Create a 'Random Game' option where the players will be presented with questions from random operations
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;

class MathGame
{
    // List to store game history
    static List<string> history = new List<string>();

    static void Main(string[] args)
    {
        // Difficulty levels
        Dictionary<string, (int Min, int Max)> difficultyLevels = new Dictionary<string, (int, int)>
        {
            { "easy", (1, 10) },
            { "medium", (11, 50) },
            { "hard", (51, 100) }
        };

        while (true)
        {
            // Display menu
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Addition");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Random Game");
            Console.WriteLine("6. View History");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            if (choice == "7")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            if (choice == "6")
            {
                DisplayHistory();
                continue;
            }

            // Get operation
            string? operation = choice switch
            {
                "1" => "Addition",
                "2" => "Subtraction",
                "3" => "Multiplication",
                "4" => "Division",
                "5" => GetRandomOperation(),
                _ => null
            };

            if (operation == null)
            {
                Console.WriteLine("Invalid choice, try again!");
                continue;
            }

            // Get difficulty
            Console.Write("Choose difficulty (easy, medium, hard): ");
            string? difficulty = Console.ReadLine();

            if (difficulty == null || !difficultyLevels.ContainsKey(difficulty.ToLower()))
            {
                Console.WriteLine("Invalid difficulty level!");
                continue;
            }

            PlayGame(operation, difficultyLevels[difficulty]);
        }
    }

    static void PlayGame(string operation, (int Min, int Max) range)
    {
        Random rand = new Random();
        int num1, num2, correctAnswer;

        if (operation == "Division")
        {

            num2 = rand.Next(1, range.Max + 1); // Avoid zero division
            int minMultiplier = Math.Max(1, range.Min / num2);
            int maxMultiplier = range.Max / num2;
            num1 = num2 * rand.Next(minMultiplier, maxMultiplier + 1);
            correctAnswer = num1 / num2;

        }
        else
        {
            num1 = rand.Next(range.Min, range.Max + 1);
            num2 = rand.Next(range.Min, range.Max + 1);
            correctAnswer = operation switch
            {
                "Addition" => num1 + num2,
                "Subtraction" => num1 - num2,
                "Multiplication" => num1 * num2,
                _ => 0
            };
        }


        string question = $"{num1} {GetSymbol(operation)} {num2} = ?";
        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine($"Solve: {question}");
        int userAnswer;
        while (!int.TryParse(Console.ReadLine(), out userAnswer))
        {
            Console.WriteLine("Invalid input. Please enter an integer.");
        }

        stopwatch.Stop();
        bool isCorrect = userAnswer == correctAnswer;

        Console.WriteLine(isCorrect ? "Correct!" : $"Wrong! The correct answer is {correctAnswer}");
        history.Add($"{question} Your Answer: {userAnswer} Correct Answer: {correctAnswer} Time: {stopwatch.Elapsed.Seconds}s Result: {(isCorrect ? "Correct" : "Wrong")}");
    }

    static void DisplayHistory()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("No games played yet.");
        }
        else
        {
            Console.WriteLine("\nGame History:");
            foreach (var record in history)
            {
                Console.WriteLine(record);
            }
        }
    }

    static string GetRandomOperation()
    {
        string[] operations = { "Addition", "Subtraction", "Multiplication", "Division" };
        Random rand = new Random();
        return operations[rand.Next(operations.Length)];
    }

    static string GetSymbol(string operation) => operation switch
    {
        "Addition" => "+",
        "Subtraction" => "-",
        "Multiplication" => "*",
        "Division" => "/",
        _ => "?"
    };
}

