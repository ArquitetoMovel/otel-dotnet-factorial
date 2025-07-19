using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Globalization;

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

Sdk.CreateMeterProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddOtlpExporter(otlpExporter => {
        otlpExporter.Endpoint = new Uri("http://localhost:9317");
        otlpExporter.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
    })
    .AddMeter("Factorial.Total")
    .SetMaxMetricStreams(1)
    .Build();

var logger = factory.CreateLogger("Program");

Meter sMeter = new("Factorial.Total", "1.0.0");
var totalCallsCounter = sMeter.CreateCounter<int>("total_calcs", "Total number of calls");

float Factorial(float number)
{
    var activityFuncFactorName = "calc_fator";
    using var calFct = activitySource?.StartActivity(activityFuncFactorName);
    totalCallsCounter.Add(1);
    if (number is 0 or 1)
    {
        return 1;
    }
    calFct?.AddTag("currentNumber", number.ToString(CultureInfo.InvariantCulture));
    return number * Factorial(number - 1);
}

try
{
    var number =  int.Parse(args[0]);
    for(var n=number; n <= number; n--) {
      var factorial = Factorial(n);
      logger.LogInformation("Factorial of {Number} is {Factorial}", n, factorial);
      Console.WriteLine($"Factorial of {n} is {factorial}");
      await Task.Delay(5_000);
    }
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
