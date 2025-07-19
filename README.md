# Calculadora de Fatorial com OpenTelemetry

## Visão Geral

Este é um simples aplicativo de console .NET que calcula o fatorial de um número inteiro fornecido como um argumento de linha de comando. O projeto é totalmente instrumentado com OpenTelemetry para gerar e exportar traces, métricas e logs.

## Recursos

- Calcula o fatorial de um número não negativo.
- Lida com entradas inválidas.
- Instrumentado com OpenTelemetry para observabilidade de ponta a ponta.
- Exporta dados de telemetria (traces, métricas, logs) para um endpoint OTLP/gRPC.

## Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior.
- Um coletor OpenTelemetry (ou qualquer backend compatível com OTLP) em execução e ouvindo no endpoint gRPC `http://localhost:9317`.

## Como Executar

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/seu-usuario/otel-dotnet-factorial.git
    cd otel-dotnet-factorial/Factorial
    ```

2.  **Restaure as dependências:**
    ```bash
    dotnet restore
    ```

3.  **Execute a aplicação:**
    Passe um número inteiro como argumento.
    ```bash
    dotnet run -- <numero>
    ```
    Por exemplo:
    ```bash
    dotnet run -- 5
    ```

    A saída será o fatorial do número fornecido, e os dados de telemetria serão enviados para o seu coletor OpenTelemetry.

## Telemetria

A aplicação está configurada para emitir os seguintes dados de telemetria:

-   **Traces:** Uma `Activity` é criada para cada cálculo de fatorial, fornecendo insights sobre a execução da função `Factorial`.
-   **Métricas:** Um contador (`total_calcs`) rastreia o número total de cálculos de fatorial realizados.
-   **Logs:** Logs são gerados para o resultado do cálculo e para quaisquer erros que possam ocorrer (por exemplo, entrada inválida).

Todos os dados de telemetria são enriquecidos com metadados do serviço (`service.name`, `service.namespace`, `service.version`) e exportados para `http://localhost:9317` usando o protocolo OTLP/gRPC.

## Dependências

O projeto utiliza os seguintes pacotes NuGet para a funcionalidade do OpenTelemetry:

-   `OpenTelemetry` (1.12.0)
-   `OpenTelemetry.Api` (1.12.0)
-   `OpenTelemetry.Exporter.Console` (1.12.0)
-   `OpenTelemetry.Exporter.OpenTelemetryProtocol` (1.12.0)
