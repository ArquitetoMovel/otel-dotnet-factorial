using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

const string serviceName = "factorial-app";
const string serviceVersion = "1.0";

ActivitySource activitySource = new(serviceName, serviceVersion);

var resourceBuilder = 
    ResourceBuilder.CreateDefault()
    .AddService(serviceName, "net.alexandre", serviceVersion);
    
Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource(serviceName)
    .AddOtlpExporter(otlpExporter => {
        otlpExporter.Endpoint = new Uri("http://localhost:9317");
        otlpExporter.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
    })
     .Build();

using ILoggerFactory factory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(logging =>
    {
        logging.SetResourceBuilder(resourceBuilder);
        logging.AddOtlpExporter(otlpExporter =>{
            otlpExporter.Endpoint = new Uri("http://localhost:9317");
            otlpExporter.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
    });
});

ILogger logger = factory.CreateLogger("Program");

int Factorial(int number)
{
    using var calFct = activitySource?.StartActivity("calc_fator");
    if (number == 0 || number == 1)
    {
        return 1;
    }

    calFct?.AddTag("currentNumber", number.ToString());

    return number * Factorial(number - 1);
}

try
{

    int number =  int.Parse(args[0]);
    int factorial = Factorial(number);

    logger.LogInformation("Factorial of {Number} is {Factorial}", number, factorial);
    Console.WriteLine($"Factorial of {number} is {factorial}");
}
catch (Exception ex)
{
    logger.LogError(ex,"Numero informado é inválido");
    Console.WriteLine("Invalid input. Please enter a valid integer."+ex.Message);
}
Console.WriteLine(">>> press q to exit <<<");

while (true)
{
    if (Console.ReadKey().KeyChar == 'q')
    {
        break;
    } else {
        await Task.Delay(100);
    }
}
