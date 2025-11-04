# Seedování DB (CLI)

Nástroj pro seedování databáze demo daty. Slouží pouze pro vývoj, data nejsou pravdivá.

## Použití
- Základní běh (včetně demo dat):
  
  dotnet run --project Bookong.Cli seed

- Soft reset demo dat a znovu seed:
  
  dotnet run --project Bookong.Cli seed --reset

- Bez demo dat:
  
  dotnet run --project Bookong.Cli seed --demo false

- Vlastní připojení:
  
  dotnet run --project Bookong.Cli seed --connection "Server=.;Database=BookongDb;Trusted_Connection=True;TrustServerCertificate=True;"
  
  nebo nastav proměnnou:
  
  ConnectionStrings__DefaultConnection="Server=.;Database=BookongDb;Trusted_Connection=True;TrustServerCertificate=True;" dotnet run --project Bookong.Cli seed

## Poznámky
- Seeduje `Kinds (Epika, Lyrika, Drama)`, `Genres` (Román, Novela, Povídka, …), `Periods` (3 kategorie) a demo `Authors`, `Publishers`, `Books`.
- Všechny knihy jsou `Borrowable = true`.
- Soft reset maže tabulky: `Books`, `Authors`, `Warehouses`, `Addresses` a `Publishers`.
- Idempotentní – existující záznamy se nevytvářejí dvakrát.