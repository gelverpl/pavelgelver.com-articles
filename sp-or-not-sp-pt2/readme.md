# Sp or Not Sp pt.2

This folder contains all code samples and supporting files for the article.

## Article Details
- Title: Sp or Not Sp pt.2
- Published: September 8, 2025
- URL: https://pavelgelver.com/posts/sp-or-not-sp-pt2/

## Structure
- [`data/benchmarks-results/`](data/benchmarks-results/) - BenchmarkDotNet reports
- [`SpOrNotSpPt2/`](SpOrNotSpPt2/) - main project
- [`SpOrNotSpPt2.Tests/`](SpOrNotSpPt2.Tests/) - main tests and demos
    - [`ConnectionStatisticsDemo.cs`](SpOrNotSpPt2.Tests/ConnectionStatisticsDemo.cs) - Demonstration of working with connection statistics
    - [`LogToDemo.cs`](SpOrNotSpPt2.Tests/LogToDemo.cs) - Example usage of Entity Framework's `LogTo()` method
    - [`ToQueryStringDemo.cs`](SpOrNotSpPt2.Tests/ToQueryStringDemo.cs) - Example usage of Entity Framework's `ToQueryString()` method

## Requirements
- .net 9 SDK
- MS SQL Server 2019 and later (I haven't actually tested if it works on earlier versions)

## How to Run
1. Clone the repository
2. Go to `sp-or-not-sp-pt2/`
3. Set your connection string by changing `./SpOrNotSpPt2/appSettings.json` file.

### - BenchmarkDotNet
Execute
```console
dotnet run --project .\SpOrNotSpPt2\SpOrNotSpPt2.csproj --configuration Release
```
Find artifacts in `/SpOrNotSpPt2/bin/Release/net9.0/BenchmarkDotNet.Artifacts/`

### - Demo Tests
You can run all tests
```console
dotnet test .\SpOrNotSpPt2.Tests\SpOrNotSpPt2.Tests.csproj
```
or just one specific test using the filter parameter
```console
# Demonstration of working with connection statistics
dotnet test .\SpOrNotSpPt2.Tests\SpOrNotSpPt2.Tests.csproj --filter "ConnectionStatisticsDemo"

# Example usage of Entity Framework's `LogTo()` method
dotnet test .\SpOrNotSpPt2.Tests\SpOrNotSpPt2.Tests.csproj --filter "LogToDemo"

# Example usage of Entity Framework's `ToQueryString()` method
dotnet test .\SpOrNotSpPt2.Tests\SpOrNotSpPt2.Tests.csproj --filter "ToQueryStringDemo"
```