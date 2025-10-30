# Nasazení (Debian + Docker)

Tento adresář obsahuje minimální setup pro nasazení aplikace jako Docker containeru vedle SQL Server databáze. Jsou připraveny dvě konfigurace: `staging` a `production`.

## Předpoklady na serveru
- Debian/Ubuntu server s přístupem přes SSH
- Nainstalovaný Docker a Docker Compose plugin
  - Debian/Ubuntu (shrnutí):
    - `sudo apt-get update && sudo apt-get install -y ca-certificates curl gnupg`
    - `sudo install -m 0755 -d /etc/apt/keyrings`
    - `curl -fsSL https://download.docker.com/linux/$(. /etc/os-release; echo "$ID")/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg`
    - `echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/$(. /etc/os-release; echo "$ID") $(. /etc/os-release; echo "$VERSION_CODENAME") stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null`
    - `sudo apt-get update && sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin`
- Otevřený port `8080` (prod) a `5080` (staging) nebo reverzní proxy před aplikací

## Jak to funguje
- CI/CD (GitHub Actions) po pushi do větve `staging` nebo `main` přes SSH spustí na serveru skript `deploy/deploy.sh`.
- Skript:
  1) postaví image, 2) spustí migrace + seed (CLI), 3) spustí kontejnery přes `docker compose up -d`.
- Databáze běží jako separátní kontejner (SQL Server 2022 Express) s persistentním volume.

## Konfigurace tajemství (GitHub)
V repozitáři nastavte v Settings → Secrets and variables → Actions tyto Secrets:
- `SSH_HOST` – hostname/IP serveru
- `SSH_USER` – uživatel s právy k Dockeru (často člen skupiny `docker`)
- `SSH_PRIVATE_KEY` – privátní klíč (PEM) pro přístup na server
- `SSH_PORT` – volitelné, default `22`

Volitelné repo Variables (doporučeno):
- `APP_DIR` – cílový adresář na serveru (default: `~/apps/bookong`)
- `REPO_URL` – URL tohoto repozitáře (default se dopočítá z GitHub kontextu)

## Prostředí (env soubory)
Vzorové soubory jsou připravené:
- `deploy/env.staging.example`
- `deploy/env.production.example`

Klíče:
- `SA_PASSWORD` – silné heslo pro SQL Server (nutné změnit!)
- `DEMO_SEED` – `true/false` (na stagingu lze zapnout demo data)
 - `DEMO_RESET` – `true/false` (volitelně; při `true` provede skript čistý reseed demo dat – vhodné jednorázově po změnách seedů; po ověření vraťte zpět na `false`)
 - `SEED_ENABLED` – `true/false` (master přepínač; při `false` se přeskočí celý krok migrací/seedingu v deploy skriptu)

Pozn.: Aplikace získá connection string z proměnné `ConnectionStrings__DefaultConnection` (skládaný v `docker-compose.*.yml`).

## Ruční nasazení (bez CI)
Na serveru (v kořeni repozitáře):
- Staging: `bash deploy/deploy.sh staging`
- Production: `bash deploy/deploy.sh production`

## Služby a porty
- Staging: app na `:5080`, DB neveřejná
- Production: app na `:8080`, DB neveřejná

## Obnova a data
- Data SQL jsou v named volumes: `bookong-staging-sqldata`, `bookong-production-sqldata`.
- Záloha/obnova probíhá prací s docker volumes (mimo rozsah tohoto README).
