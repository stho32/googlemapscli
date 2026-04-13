# googlemapscli

CLI-Tool fuer Google Maps API

## Tech Stack

| Technology | Version | Purpose |
|---|---|---|
| .NET | 10.0 | Runtime and SDK |
| CommandLineParser | 2.9.1 | CLI argument parsing |
| NUnit | 4.3.2 | Unit and integration testing |
| coverlet | 6.0.4 | Code coverage collection |

## Getting Started

### Prerequisites

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download/dotnet/10.0)

### Build

```bash
dotnet build Source/googlemapscli/googlemapscli.sln
```

### Run

```bash
dotnet run --project Source/googlemapscli/googlemapscli/googlemapscli.csproj -- [args]
```

### Test

```bash
dotnet test Source/googlemapscli/googlemapscli.sln
```

## Projektstruktur

```
googlemapscli/
├── Anforderungen/           # Anforderungs-Dokumentation
├── Source/googlemapscli/     # Solution-Root
│   ├── googlemapscli/        # Entry Point (Console App)
│   ├── googlemapscli.BL/     # Business Logic
│   ├── googlemapscli.BL.Tests/           # Unit Tests
│   └── googlemapscli.BL.IntegrationTests/ # Integration Tests
├── CLAUDE.md
└── README.md
```

## Anforderungen

Siehe [Anforderungen/](./Anforderungen/) fuer alle Projekt-Anforderungen.

Neue Anforderung erstellen: `/erstelle-anforderung`
