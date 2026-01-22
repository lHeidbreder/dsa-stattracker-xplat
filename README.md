# DSA Stattracker
## Build
`build.sh` produziert Builds für
- Windows und Linux (je x64)
- Android
- WebAssembly (zum aktuellen Zeitpunkt wohl kaputt?)

Für andere Plattformen händisch compilen.
- Desktop: `dotnet publish -c Release -r <PLATFORM> -o dist/ dsa_stattracker_xplat.Desktop/`
- iOS: `dotnet publish -c Release -o dist/ dsa_stattracker_xplat.iOS/`
