# CLAUDE.md

## Projekt

googlemapscli — CLI-Tool fuer Google Maps API

## Architektur

Layered CLI-Tool-Architektur: Entry Point -> BL (Business Logic)

- **googlemapscli** — Einstiegspunkt, CLI-Argument-Parsing, Konsolenausgabe
- **googlemapscli.BL** — Geschaeftslogik, Commands, Validierung, Logging-Interfaces

## Befehle

- Build: `dotnet build Source/googlemapscli/googlemapscli.sln`
- Test: `dotnet test Source/googlemapscli/googlemapscli.sln`
- Run: `dotnet run --project Source/googlemapscli/googlemapscli/googlemapscli.csproj -- [args]`

## Regeln

- Entry Point enthaelt KEINE Geschaeftslogik
- Alle oeffentlichen Methoden in BL geben Result-Objekte zurueck (keine Exceptions ueber Layer-Grenzen)
- `Main()` gibt int zurueck: 0 = Erfolg, non-zero = Fehler
- `<Nullable>enable</Nullable>` und `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` in allen Projekten
- Hand-written Mocks statt Mocking-Frameworks
- File-scoped Namespaces (`namespace X;`)
