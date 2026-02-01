# 🚀 Kompletní průvodce nastavením serveru pro Bookong

Tento dokument tě provede od čistého Ubuntu serveru až po plně funkční automatické nasazení.

## 📋 Co budeme potřebovat

- Ubuntu server s root přístupem
- Uživatel `deploy` s SSH přístupem
- Docker nainstalovaný
- DNS záznamy: `*.bookong.spstrutnov.cz` → IP serveru

---

## 1️⃣ Nastavení deploy usera

Na serveru jako **root** nebo uživatel s sudo:

```bash
# Pokud ještě nemáš deploy usera:
adduser deploy
# (zadej heslo, ostatní info přeskoč)

# Přidat deploy do důležitých skupin
usermod -aG sudo deploy
usermod -aG docker deploy

# Vytvořit specifické sudo pravidlo (bezpečnější)
echo "deploy ALL=(ALL) NOPASSWD: /usr/bin/docker, /usr/bin/docker-compose" | sudo tee /etc/sudoers.d/deploy
chmod 0440 /etc/sudoers.d/deploy

# Vytvořit .ssh adresář (pokud ještě neexistuje)
mkdir -p /home/deploy/.ssh
chmod 700 /home/deploy/.ssh
chown deploy:deploy /home/deploy/.ssh
```

**Test**: Odhlásit se a přihlásit jako `deploy`, zkusit:
```bash
docker ps  # Nemělo by chtít sudo
```

---

## 2️⃣ SSH přístup z GitHub Actions

Na serveru jako **deploy** uživatel:

### A) Vygeneruj SSH klíč pro GitHub Actions (pokud ještě nemáš)

Tento klíč bude uložen jako `SSH_PRIVATE_KEY` secret v GitHubu.

```bash
# Vygeneruj nový klíč (bez hesla)
ssh-keygen -t ed25519 -C "github-actions@bookong" -f ~/.ssh/github_actions_key -N ""

# Přidej veřejný klíč do authorized_keys
cat ~/.ssh/github_actions_key.pub >> ~/.ssh/authorized_keys
chmod 600 ~/.ssh/authorized_keys

# Zobraz privátní klíč (zkopíruj CELÝ včetně hlaviček)
cat ~/.ssh/github_actions_key
```

☝️ **Zkopíruj obsah** `~/.ssh/github_actions_key` (začíná `-----BEGIN OPENSSH PRIVATE KEY-----`)  
→ Tento klíč dáš do GitHub Secrets jako `SSH_PRIVATE_KEY`

### B) Test SSH připojení

Z jiného počítače (nebo z GitHub Actions):
```bash
ssh -i /path/to/private/key deploy@bookong.spstrutnov.cz
```

---

## 3️⃣ Přístup serveru k privátnímu GitHub repozitáři

Na serveru jako **deploy** uživatel:

```bash
# Vygeneruj deploy klíč pro GitHub
ssh-keygen -t ed25519 -C "deploy@bookong-server" -f ~/.ssh/github_deploy_key -N ""

# Zobraz veřejný klíč
cat ~/.ssh/github_deploy_key.pub
```

☝️ **Zkopíruj veřejný klíč** a přidej ho na GitHub:

1. Jdi na: https://github.com/sps-trutnov-eps/projekt-4ep-bookong/settings/keys
2. Klikni **Add deploy key**
3. Title: `Bookong Production Server`
4. Key: Vlož veřejný klíč
5. ✅ **NEZAŠKRTÁVEJ** "Allow write access" (read-only stačí)
6. Klikni **Add key**

### Konfigurace SSH pro použití deploy klíče

```bash
cat >> ~/.ssh/config << 'EOF'
Host github.com
  HostName github.com
  User git
  IdentityFile ~/.ssh/github_deploy_key
  StrictHostKeyChecking no
EOF

chmod 600 ~/.ssh/config
```

### Test přístupu k repo

```bash
ssh -T git@github.com
# Mělo by vypsat: "Hi sps-trutnov-eps/projekt-4ep-bookong! You've successfully authenticated..."

# Zkus klonovat repo
cd ~
git clone git@github.com:sps-trutnov-eps/projekt-4ep-bookong.git test-clone
# Pokud uspěje, smaž testovací klon
rm -rf test-clone
```

---

## 4️⃣ GitHub Secrets

V GitHub repozitáři:

1. Jdi na: https://github.com/sps-trutnov-eps/projekt-4ep-bookong/settings/secrets/actions
2. Klikni **New repository secret**
3. Přidej tyto secrets:

| Secret Name | Value | Poznámka |
|------------|-------|----------|
| `SSH_HOST` | `bookong.spstrutnov.cz` | IP nebo hostname serveru |
| `SSH_USER` | `deploy` | Uživatel na serveru |
| `SSH_PRIVATE_KEY` | *obsah ~/.ssh/github_actions_key* | Celý privátní klíč včetně hlaviček |
| `SSH_PORT` | `22` | Jen pokud používáš jiný port |

---

## 5️⃣ Nastavení Traefik (reverse proxy + HTTPS)

Na serveru jako **deploy** uživatel:

### A) Vytvoř strukturu pro Traefik

```bash
mkdir -p ~/traefik/letsencrypt
cd ~/traefik
```

### B) Vytvoř `docker-compose.yml`

```bash
cat > ~/traefik/docker-compose.yml << 'EOF'
services:
  traefik:
    image: traefik:v2.10
    container_name: traefik
    restart: unless-stopped
    security_opt:
      - no-new-privileges:true
    networks:
      - traefik-public
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock:ro
      - ./letsencrypt:/letsencrypt
      - ./traefik.yml:/traefik.yml:ro
    labels:
      - "traefik.enable=true"
      - "traefik.http.routers.traefik.rule=Host(\`traefik.bookong.spstrutnov.cz\`)"
      - "traefik.http.routers.traefik.entrypoints=https"
      - "traefik.http.routers.traefik.tls=true"
      - "traefik.http.routers.traefik.tls.certresolver=letsencrypt"
      - "traefik.http.routers.traefik.service=api@internal"

networks:
  traefik-public:
    name: traefik-public
    external: true
EOF
```

### C) Vytvoř `traefik.yml`

**⚠️ ZMĚŇ EMAIL!**

```bash
cat > ~/traefik/traefik.yml << 'EOF'
api:
  dashboard: true
  debug: false

entryPoints:
  http:
    address: ":80"
    http:
      redirections:
        entryPoint:
          to: https
          scheme: https
  https:
    address: ":443"

providers:
  docker:
    endpoint: "unix:///var/run/docker.sock"
    exposedByDefault: false
    network: traefik-public

certificatesResolvers:
  letsencrypt:
    acme:
      email: vas-email@spstrutnov.cz  # ← ZMĚŇ TO!
      storage: /letsencrypt/acme.json
      httpChallenge:
        entryPoint: http

log:
  level: INFO
EOF
```

### D) Spusť Traefik

```bash
# Vytvoř network
docker network create traefik-public

# Nastav práva pro certifikáty
touch ~/traefik/letsencrypt/acme.json
chmod 600 ~/traefik/letsencrypt/acme.json

# Spusť Traefik
cd ~/traefik
docker compose up -d

# Zkontroluj logy
docker logs traefik -f
# Mělo by vypsat "Configuration loaded" a žádné chyby
```

---

## 6️⃣ Příprava aplikačních složek

Na serveru jako **deploy**:

```bash
# Vytvoř adresář pro aplikaci
mkdir -p ~/apps/bookong

# První manuální klon (nebo počkej na první GitHub Actions deploy)
cd ~/apps/bookong
git clone -b staging git@github.com:sps-trutnov-eps/projekt-4ep-bookong.git .
```

### Vytvoř `.env.staging` soubor

```bash
cat > ~/apps/bookong/deploy/.env.staging << 'EOF'
# SQL Server heslo (ZMĚŇ TO!)
SA_PASSWORD=ChangeMe_StrongStaging123!

# Demo data pro staging
DEMO_SEED=true
DEMO_RESET=false

# Master switch pro migrations/seeding
SEED_ENABLED=true
EOF
```

---

## 7️⃣ První manuální deploy (TEST)

Na serveru jako **deploy**:

```bash
cd ~/apps/bookong
bash deploy/deploy.sh staging
```

Co se stane:
1. Docker buildne image z Dockerfile
2. Spustí SQL Server kontejner
3. Spustí migrace + seeding (Bookong.Cli)
4. Spustí aplikaci

**Kontrola**:
```bash
docker ps
# Měly by běžet kontejnery: traefik, bookong-staging-app, bookong-staging-db

docker logs bookong-staging-app -f
# Mělo by vypsat "Now listening on: http://0.0.0.0:8080"
```

**Test v browseru**:
- `https://staging.bookong.spstrutnov.cz` → Měla by se načíst aplikace s HTTPS 🎉

---

## 8️⃣ GitHub Actions automatický deployment

Teď už stačí jen pushnout do větve `staging` a GitHub Actions udělá zbytek!

### Test workflow:

1. **V lokálním repozitáři**:
```bash
git checkout staging
git pull origin staging

# Udělej nějakou změnu (např. v README)
echo "# Test deployment" >> README.md
git add README.md
git commit -m "test: GitHub Actions deployment"
git push origin staging
```

2. **Na GitHubu**:
   - Jdi na: https://github.com/sps-trutnov-eps/projekt-4ep-bookong/actions
   - Měl by běžet workflow "Deploy Staging"
   - Počkej až doběhne (zelená ✅)

3. **Na serveru** (můžeš sledovat):
```bash
# Sleduj logy
docker logs bookong-staging-app -f
```

4. **Test**:
   - Otevři `https://staging.bookong.spstrutnov.cz`
   - Měla by se zobrazit aktualizovaná verze

---

## 9️⃣ Production deployment (později)

Až budeš chtít spustit production:

1. **Vytvoř `.env.production`** na serveru:
```bash
cat > ~/apps/bookong/deploy/.env.production << 'EOF'
SA_PASSWORD=SuperStrongProductionPassword123!
DEMO_SEED=false
DEMO_RESET=false
SEED_ENABLED=true
EOF
```

2. **Merge staging → main** a push
3. GitHub Actions automaticky nasadí na production → `https://bookong.spstrutnov.cz`

---

## ✅ Checklist

- [ ] Deploy user má sudo a je v docker skupině
- [ ] SSH klíč pro GitHub Actions v `~/.ssh/github_actions_key`
- [ ] SSH klíč přidán do `~/.ssh/authorized_keys`
- [ ] GitHub Secrets nastaveny: `SSH_HOST`, `SSH_USER`, `SSH_PRIVATE_KEY`
- [ ] Deploy klíč pro GitHub v `~/.ssh/github_deploy_key`
- [ ] Deploy klíč přidán jako Deploy Key v GitHub repo
- [ ] `~/.ssh/config` nakonfigurován pro github.com
- [ ] Traefik běží a má correct email v `traefik.yml`
- [ ] `docker network create traefik-public` proběhlo
- [ ] `~/apps/bookong` vytvořeno a naklonováno
- [ ] `.env.staging` vytvořen s silným heslem
- [ ] První manuální deploy úspěšný
- [ ] `https://staging.bookong.spstrutnov.cz` funguje
- [ ] GitHub Actions workflow úspěšně nasadil

---

## 🆘 Troubleshooting

### Chyba: "Permission denied" při docker operacích
```bash
# Přidej deploy do docker skupiny a znovu se přihlaš
sudo usermod -aG docker deploy
newgrp docker
```

### Chyba: "Could not resolve host: github.com"
```bash
# Test SSH připojení
ssh -T git@github.com

# Zkontroluj ~/.ssh/config
cat ~/.ssh/config

# Zkus manuální klon
git clone git@github.com:sps-trutnov-eps/projekt-4ep-bookong.git test
```

### Aplikace neběží
```bash
# Zkontroluj logy
docker logs bookong-staging-app
docker logs bookong-staging-db

# Zkontroluj že kontejnery běží
docker ps -a

# Restartuj
cd ~/apps/bookong
docker compose -f deploy/docker-compose.staging.yml --env-file deploy/.env.staging restart
```

### Traefik nevydává certifikáty
```bash
# Zkontroluj email v traefik.yml je správný
cat ~/traefik/traefik.yml

# Zkontroluj logy
docker logs traefik

# Zkontroluj acme.json
cat ~/traefik/letsencrypt/acme.json

# Restartuj Traefik
cd ~/traefik
docker compose restart
```

### GitHub Actions selhává při SSH
- Zkontroluj že `SSH_PRIVATE_KEY` secret obsahuje CELÝ klíč včetně `-----BEGIN` a `-----END`
- Zkontroluj že klíč je přidán do `~/.ssh/authorized_keys` na serveru
- Zkontroluj že `SSH_HOST` je dostupný z internetu

---

## 📚 Užitečné příkazy

```bash
# Restart staging
cd ~/apps/bookong
bash deploy/deploy.sh staging

# Sledovat logy
docker logs -f bookong-staging-app
docker logs -f traefik

# Zastavit vše
docker compose -f deploy/docker-compose.staging.yml down

# Smazat data (POZOR!)
docker volume rm bookong-staging-sqldata

# Rebuild bez cache
docker compose -f deploy/docker-compose.staging.yml build --no-cache

# Aktualizovat z GitHubu
cd ~/apps/bookong
git fetch origin staging
git reset --hard origin/staging
bash deploy/deploy.sh staging
```
