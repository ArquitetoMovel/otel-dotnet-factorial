# Repository Guidelines

## Project Structure & Module Organization

`Factorial/Factorial.sln` contains one .NET 10 console project, `Factorial/Factorial.csproj`. `Factorial/Program.cs` holds top-level startup code, recursive factorial calculation, and OpenTelemetry trace, metric, and log configuration. `README.md` explains usage; `Prerequisitos.md` lists observability tools. There are no test projects or application assets. Treat `bin/`, `obj/`, and `.idea/` as generated or local tooling files; avoid including their changes in contributions, even where older artifacts are tracked.

## Build, Test, and Development Commands

Run these commands from the repository root with the .NET 10 SDK installed:

- `dotnet restore Factorial/Factorial.sln` — restore NuGet dependencies.
- `dotnet build Factorial/Factorial.sln --no-incremental` — compile a full build; resolve new warnings.
- `dotnet run --project Factorial/Factorial.csproj -- 5` — smoke-test calculation; the first result should be `Factorial of 5 is 120`.
- `dotnet test Factorial/Factorial.sln` — run tests when a test project exists; currently this executes no tests.

The current calculation loop continues decrementing past zero. Stop the numeric smoke test with Ctrl+C after checking its initial output. For invalid input, use `-- invalid` and press `q` at the exit prompt.

## Coding Style & Naming Conventions

Use four-space indentation for C# and two-space indentation for project XML. Use PascalCase for types and methods, camelCase for locals and parameters, and descriptive names. Preserve nullable reference types and implicit usings. Use structured logging placeholders, such as `LogInformation("Factorial of {Number} is {Factorial}", number, result)`. No repository formatter or linter configuration exists; keep formatting changes focused.

## Testing Guidelines

No testing framework or coverage threshold is configured. Verify builds and both valid and invalid input manually. Confirm telemetry reaches a collector when changing instrumentation. For new automated tests, use a dedicated `Factorial.Tests` project and descriptive names such as `Factorial_Zero_ReturnsOne`; document its framework and execution command.

## Commit & Pull Request Guidelines

History uses short descriptive subjects in English and Portuguese, without a consistent prefix convention. Write focused, action-oriented subjects, such as `Upgrade OpenTelemetry packages`. PRs should explain the change, link relevant issues, report build and smoke-test results, and describe telemetry or dependency impacts. Update the Portuguese documentation when requirements or usage change.

## Observability Configuration

Traces, metrics, and logs use OTLP/gRPC at `http://localhost:9317`, configured in `Program.cs`. Run a compatible collector to verify delivery. Preserve service identity and existing instrument names unless intentionally changing the telemetry contract. Keep credentials out of source control.
