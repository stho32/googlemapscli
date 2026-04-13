---
id: R00002
title: "Google Maps API Integration — Geolocation und Distance"
type: Feature
status: Open
created: 2026-04-13
source: "GitHub Issue: #1"
---

# R00002: Google Maps API Integration — Geolocation und Distance

## Description

Die googlemapscli-App soll zwei Google Maps API Funktionen als CLI-Befehle bereitstellen:

1. **Geolocation (Geocoding)**: Geokoordinaten (Breitengrad, Laengengrad) einer Adresse bestimmen.
2. **Distance (Distance Matrix)**: Dauer und Distanz zwischen zwei Orten fuer ein bestimmtes Verkehrsmittel berechnen.

## CLI-Schnittstelle

```bash
# Geolocation: Koordinaten einer Adresse abfragen
googlemapscli --geolocation --of "Alexanderplatz, Berlin"

# Distance: Entfernung und Dauer zwischen zwei Orten
googlemapscli --distance --from "Berlin" --to "Hamburg" --using "driving"
```

### Verkehrsmittel (--using)

Verwendet direkt die Google Maps API Begriffe:

- `driving`
- `walking`
- `bicycling`
- `transit`

### Ausgabeformat

**Geolocation:**
```
Adresse: Alexanderplatz, Berlin
Breitengrad: 52.5219
Laengengrad: 13.4132
```

**Distance:**
```
Von: Berlin
Nach: Hamburg
Verkehrsmittel: driving
Distanz: 289 km
Dauer: 2 h 52 min
```

## API-Key Konfiguration

- Umgebungsvariable: `GOOGLE_MAPS_API_KEY`
- Wenn nicht gesetzt: Fehlermeldung mit Hinweis auf die Variable
- Key wird NIEMALS geloggt oder in Ausgaben angezeigt

## Benoetigte Google APIs

- Geocoding API (`https://maps.googleapis.com/maps/api/geocode/json`)
- Distance Matrix API (`https://maps.googleapis.com/maps/api/distancematrix/json`)

## Technische Aufschluesselung

### BL-Schicht (googlemapscli.BL)

| Verzeichnis/Datei | Zweck |
|---|---|
| `CommandLineArguments/CommandLineOptions.cs` | Erweitern um --geolocation, --of, --distance, --from, --to, --using |
| `GoogleMaps/IGoogleMapsClient.cs` | Interface fuer API-Aufrufe |
| `GoogleMaps/GoogleMapsClient.cs` | HTTP-Implementierung gegen Google Maps API |
| `GoogleMaps/Models/GeolocationResult.cs` | Ergebnis-Record fuer Geocoding |
| `GoogleMaps/Models/DistanceResult.cs` | Ergebnis-Record fuer Distance Matrix |
| `GoogleMaps/GeolocationCommand.cs` | Geolocation-Logik |
| `GoogleMaps/DistanceCommand.cs` | Distance-Logik |
| `Configuration/ApiKeyProvider.cs` | ENV-Variable lesen und validieren |

### Entry Point (googlemapscli)

| Datei | Zweck |
|---|---|
| `Program.cs` | Routing: --geolocation oder --distance aufrufen, Ergebnis ausgeben |

### Tests

| Projekt | Testdateien |
|---|---|
| `googlemapscli.BL.Tests` | `GoogleMaps/GeolocationCommandTests.cs`, `GoogleMaps/DistanceCommandTests.cs`, `Configuration/ApiKeyProviderTests.cs`, `CommandLineArguments/CommandLineOptionsTests.cs`, `Mocks/MockGoogleMapsClient.cs` |
| `googlemapscli.BL.IntegrationTests` | `GoogleMaps/GoogleMapsClientIntegrationTests.cs` (benoetigt echten API Key) |

## Acceptance Criteria

- [ ] `googlemapscli --geolocation --of "Adresse"` gibt Breitengrad und Laengengrad aus
- [ ] `googlemapscli --distance --from "A" --to "B" --using "driving"` gibt Distanz und Dauer aus
- [ ] Alle vier Verkehrsmittel (driving, walking, bike, transit) werden unterstuetzt
- [ ] Fehlermeldung wenn `GOOGLE_MAPS_API_KEY` nicht gesetzt ist
- [ ] Fehlermeldung bei ungueltiger Adresse (Geocoding liefert kein Ergebnis)
- [ ] Fehlermeldung bei ungueltigem Verkehrsmittel
- [ ] Exit-Code 0 bei Erfolg, non-zero bei Fehler
- [ ] API Key wird nicht geloggt
- [ ] Unit-Tests mit Mock-Client fuer alle Erfolgsfaelle und Fehlerfaelle
- [ ] Integration-Tests gegen echte Google Maps API (optional, benoetigt Key)

## Status

- [ ] Open
