# 📚 Bookong - Teaching Materials Module

> **Správa výukových materiálů pro školní knihovnu**

## 🎯 Popis modulu

Teaching Materials je klíčová část Bookong platformy, která umožňuje učitelům a pedagógům centralizovaně spravovat a sdílet své výukové materiály. Modul poskytuje intuitivní rozhraní pro vytváření, úpravu a organizaci vzdělávacího obsahu v jednom přehledném místě.

## ✨ Hlavní funkce

### 📝 Správa materiálů
- **Přidávání materiálů** - Jednoduché nahrání nových výukových materiálů
- **Organizace** - Seskupení materiálů dle předmětů, tříd a témat
- **Vyhledávání a filtrování** - Rychlý přístup k potřebným materiálům
- **Úprava metadat** - Přiřazování popisů, značek a kategorizace

### 👥 Správa přístupu
- **Vazba na uživatele** - Každý materiál je přidružen ke konkrétnímu učiteli
- **Sdílení** - Možnost sdílení materiálů s kolegy nebo žáky
- **Řízení přístupu** - Kontrola kdo může материál zobrazit či upravit

### 🎨 Uživatelské rozhraní
- Moderní a intuitivní design
- Responsivní layout pro všechna zařízení
- Předválby pro rychlý přístup

## 🏗️ Architektura

Modul je postaven na **ASP.NET** backend architektuře s moderním frontend řešením:

```
┌─────────────────────────────────────┐
│   Web Interface (Frontend)          │
│   HTML/CSS/JavaScript               │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   ASP.NET Controllers               │
│   & Services (Backend)              │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   Database (Domain Models)          │
│   Entity Framework Core             │
└─────────────────────────────────────┘
```

## 📦 Komponenty

### Backend
- **Bookong.Application** - Aplikační logika a use cases
- **Bookong.Domain** - Domain modely a business logika
- **Bookong.Infrastructure** - Přístup k databázi a externím službám

### Frontend
- **Bookong.Web** - Web aplikace s uživatelským rozhraním
- **Pages** - Razor Pages pro Teaching Materials

## 🚀 Klíčové vlastnosti

✅ Asociace materiálů s aktuálním uživatelem (učitelem)  
✅ CRUD operace (Create, Read, Update, Delete)  
✅ Moderní validační formuláře  
✅ Responsivní design  
✅ Unit testy pro kritické komponenty  

## 🔄 Integrace

Teaching Materials modul se integruje s:
- **Systémem uživatelů** - Pro identifikaci a autentizaci
- **Databází** - Pro persistenci dat
- **Web rozhraním** - Pro prezentaci a interakci

## 📖 Jak používat

1. **Přihlášení** - Učitel se přihlásí do systému
2. **Přístup k modulu** - Navigace na Teaching Materials sekci
3. **Přidání materiálu** - Vytvoření nového materiálu přes formulář
4. **Správa** - Úprava, mazání nebo sdílení materiálů

## 🛠️ Technologický stack

| Vrstva | Technologie |
|--------|-------------|
| Backend | ASP.NET Core, C# |
| Frontend | HTML5, CSS3, JavaScript |
| Database | Entity Framework Core |
| Testing | xUnit, Moq |
| Verzování | Git/GitHub |

## 📝 Projekt školní knihovny

Být součástí Bookong - moderní řešení pro správu školní knihovny a výukových materiálů.

---

**Status**: Aktivní vývoj 🔄  
**Poslední aktualizace**: 2025  
**Vedení projektu**: Tým SPS Trutnov
