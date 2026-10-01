# Fatorial com OpenTelemetry e .NET 10

A branch `otel-support` apresenta o primeiro exemplo instrumentado.

Exemplo de instrumentação manual de uma aplicação de console que calcula um fatorial e exporta spans para o console.

Repositório: [ArquitetoMovel/otel-dotnet-factorial](https://github.com/ArquitetoMovel/otel-dotnet-factorial).

## Ambiente e dependências

- SDK do .NET 10. O `global.json` aceita as faixas de recursos estáveis do SDK 10.0 a partir de 10.0.100.
- Projeto `Factorial/Factorial.csproj` com alvo `net10.0`.
- `OpenTelemetry`, `OpenTelemetry.Api` e `OpenTelemetry.Exporter.Console` na versão estável `1.19.1`.

## Executando

```shell
git clone --branch otel-support https://github.com/ArquitetoMovel/otel-dotnet-factorial.git
cd otel-dotnet-factorial
dotnet restore Factorial/Factorial.sln
dotnet build Factorial/Factorial.sln --configuration Release --no-restore
dotnet run --project Factorial/Factorial.csproj --configuration Release --no-build
```

O exemplo usa o número `5` definido em `Program.cs` e exibe `Factorial of 5 is 120`. Os argumentos de linha de comando ainda não são lidos. Cada chamada recursiva abre um span `calc_fator` na fonte `factorial-app`: para 5, são cinco spans do mesmo trace.

O provedor de traces e a fonte permanecem ativos durante o cálculo e são descartados ao encerrar o processo. O exemplo inicial exporta apenas para o console. Nos exercícios de exportação, logs e métricas, adicione o pacote `OpenTelemetry.Exporter.OpenTelemetryProtocol` em `1.19.1` e aplique a configuração do Collector descrita no livro.
