using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

var serviceName = "factorial-app";

Sdk.CreateTracerProviderBuilder()
    .AddSource(serviceName)
    .AddConsoleExporter()
    .Build();

var activitySource = new ActivitySource(serviceName, "1.0");

int Factorial(int number)
{
    using var calFct = activitySource?.StartActivity("calc_fator");
    if (number == 0 || number == 1)
    {
        return 1;
    }

    return number * Factorial(number - 1);
}

try
{

    int number =  5;//int.Parse(args[0]);
    int factorial = Factorial(number);

    Console.WriteLine($"Factorial of {number} is {factorial}");
}
catch (Exception ex)
{
    Console.WriteLine("Invalid input. Please enter a valid integer."+ex.Message);
}
