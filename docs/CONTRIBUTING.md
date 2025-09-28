# Jak přispívat do projektu

## Obsah
- [Pravidla a vysvětlivky](#pravidla-a-vysvětlivky)
  - [Jazyk](#jazyk)
  - [Issues](#issues)
  - [Pull Requests](#pull-requests-prs)
    - [Velikost Pull Requestů](#velikost-pull-requestů)
  - [Větve](#větve)
  - [GitHub Project](#github-project)
- [Krok za krokem](#krok-za-krokem)
- [Důležité pravidla](#důležité-pravidla)
- [Potřebuješ pomoc?](#potřebuješ-pomoc)

---

## Pravidla a vysvětlivky

### Jazyk
Obecně platí že většinu věcí kolem gitu budeme psát česky. Samotný kód má oproti gitu svá pravidla a je psán v angličtině.

### Issues
Issues jsou jako TODO list pro projekt.

**Co to je:**
- Místo kde se řeší problémy, nové funkce nebo bugy
- Obsahuje komentáře pro diskuzi
- Sleduje průběh práce

**Co by měl obsahovat:**
- Stručný název
- Dostatečný popis
- Seznam přiřazených osob (assignees)
- Správný štítek (label)
- Připojenou pracovní větev

**Kde to najdeš:** Záložka **Issues** na GitHubu

### Pull Requests (PRs)
Pull Request je žádost o sloučení tvé práce do hlavní větve projektu.

**Co to je:**
- Místo kde se tvůj kód kontroluje před přidáním do projektu
- Ostatní mohou komentovat a schvalovat změny
- Automaticky zavře související Issue při použití `Closes #číslo-issue`

**Co by měl obsahovat:**
- Stručný název - co jsi udělal/opravil
- Podrobný popis změn
- Odkaz na související Issue
- Screenshoty při UI změnách
- Správně nastavený cílovou větev (většinou `development`)

**Kde to najdeš:** Záložka **Pull Requests** na GitHubu

#### Velikost Pull Requestů
**Ideální velikost:** 200-400 řádků změn (bez automaticky generovaných souborů)

**Pokud je Issue příliš velký:**
1. **Rozděl Issue na menší části (sub-issues)** - například:
   - Issue #123: Přidat uživatelské účty
   - → Issue #124: Databáze pro uživatele
   - → Issue #125: Registrační formulář
   - → Issue #126: Přihlašovací logika

2. **Draft PR** - pro velké změny:
   - Vytvoř Draft Pull Request už při začátku
   - Commituj postupně a žádej o feedback během vývoje
   - Převeď na normální PR až při dokončení

**Proč menší PR:** rychlejší review, snazší nalezení chyb, menší riziko konfliktů

### Větve
Každá větev řeší konkrétní Issue.

**Hlavní projektové větve:**
- **main** - produkční (hlavní) větev nasazená na serveru; obsahuje pouze stabilní a otestované verze; merguje se do ní jen ověřený release
- **staging** - předprodukční integrační větev pro společné testování více hotových funkcí; sem se mergují dokončené změny z `development` před releasem do `main`
- **development** - hlavní vývojová větev; sem se průběžně mergují `feature/` a `bugfix/` větve; z ní se připravují přechody do `staging`

**Nazývání větví:**
- Formát: `typ/číslo-issue-krátký-popis`
- Příklady: 
  - `feature/123-pridat-login`
  - `bugfix/456-oprava-validace`
  - `hotfix/789-oprava-crash`
  - `docs/124-dokumentace-api`

**Typy větví:**
- **feature/** - nová funkcionalita (z `development`)
- **bugfix/** - oprava chyby (z `development`) 
- **hotfix/** - kritická oprava v produkci (z `main` → merguje do `main` + zpět do `development`)
- **docs/** - pouze změny v dokumentaci (z `development`)

### GitHub Project
Kanban tabule pro organizaci práce.

**Co umí:**
- Sledování stavu Issues (Ready, In Progress, Done)
- Nastavování priorit
- Přehled o průběhu projektu

**Kde to najdeš:** Záložka **Projects** na GitHubu

---

## Krok za krokem

### 1. Najdi si úkol (Issue)
1. Jdi na záložku **Issues** v GitHubu
2. Najdi Issue přiřazený tobě nebo tvému týmu
3. Ujisti se, že je připraven k řešení:
   - Není blokovaný jiným Issue
   - Má jasný popis a požadavky
   - Máš čas na jeho řešení

### 2. Vytvoř si větev
1. Přepni se na `development` větev
2. Stáhni nejnovější změny (`git pull`)
3. Vytvoř novou větev podle [pravidel pro nazývání](#větve), buďto na GitHubu nebo lokálně 

### 3. Programuj a commituj
- Pracuj na svém úkolu
- Commituj pravidelně s popisnými zprávami v češtině
- Příklad: `git commit -m "Přidat validaci emailu"`

### 4. Vytvoř Pull Request
1. Jdi na záložku **Pull Requests**
2. Klikni **New pull request**
3. Nastav:
   - **Cílová větev (base):** `development`
   - **Tvoje větev:** tvoje pracovní větev
4. Vyplň název a popis
5. Přidej `Closes #číslo-issue` do popisu

### 5. Počkej na kontrolu
- Někdo zkontroluje tvůj kód
- Oprav případné připomínky
- Po schválení se kód sloučí do projektu

---

## Důležité pravidla

❌ **NIKDY nepushuj přímo do `main` nebo `development`**  
✅ **Vždy používej vlastní větev**

❌ **Nevytvářej Issues a Pull Requesty bez potřebných informací**  
✅ **Vždy vysvětli, o co jde, co jsi změnil a proč**

❌ **Neignoruj připomínky při code review**  
✅ **Diskutuj a oprav, co je potřeba**

---

## Potřebuješ pomoc?

👥 **Zeptej se na Discordu nebo přímo v komentářích k Issue/PR!**

🤖 **Řekni si Gitmasterovi:**  
Pokud si nejsi **čímkoliv** jistý nebo jsi se v dostal s gitem do problému, neváhej kontaktovat **gitmastera**. 

**Typické situace kdy se vyplatí se zeptat:**
- Nejsi si jistý s Issues, PRs a dalšími GitHub funkcemi
- Máš problém s git operacemi
- Nevyznáš se ve větvích
- Cokoliv ti přijde "podezřelé" nebo složité
