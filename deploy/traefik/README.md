# Traefik Setup

Tento adresář obsahuje konfiguraci pro Traefik reverse proxy s automatickým HTTPS (Let's Encrypt).

## Instalace na serveru

1. Zkopíruj celý adresář `traefik/` na server do `~/traefik/`
2. Uprav email v `traefik.yml` (pro Let's Encrypt notifikace)
3. Vytvoř network a spusť Traefik:

```bash
# Vytvoř network pro Traefik
docker network create traefik-public

# Vytvoř adresář pro certifikáty
mkdir -p ~/traefik/letsencrypt
touch ~/traefik/letsencrypt/acme.json
chmod 600 ~/traefik/letsencrypt/acme.json

# Spusť Traefik
cd ~/traefik
docker compose up -d

# Zkontroluj logy
docker logs traefik -f
```

## Dashboard

Traefik dashboard je dostupný na `https://traefik.bookong.spstrutnov.cz`

Pro zabezpečení basic auth:
1. Vygeneruj hash hesla: `htpasswd -nb admin yourpassword`
2. Odkomentuj řádky s `traefik-auth` v `docker-compose.yml`
3. Vlož vygenerovaný hash (pozor na escapování `$` znaků - použij `$$`)
4. Restartuj: `docker compose up -d`

## Routing

- `bookong.spstrutnov.cz` → Production (main branch)
- `staging.bookong.spstrutnov.cz` → Staging (staging branch)
- `traefik.bookong.spstrutnov.cz` → Traefik Dashboard

## Troubleshooting

```bash
# Zkontroluj Traefik běží
docker ps | grep traefik

# Logy
docker logs traefik

# Certifikáty
cat ~/traefik/letsencrypt/acme.json

# Restart
cd ~/traefik
docker compose restart
```
