using System;

static int Factorial(int number)
{
    if (number == 0 || number == 1)
    {
        return 1;
    }

    return number * Factorial(number - 1);
}

try
{
    int number = int.Parse(args[0]);
    int factorial = Factorial(number);

    Console.WriteLine($"Factorial of {number} is {factorial}");
}
catch (Exception ex)
{
    Console.WriteLine("Invalid input. Please enter a valid integer."+ex.Message);
}
