# CRM Duo Creative

**CRM (Customer Management System)** stworzony dla wewnętrznych potrzeb firmy Duo Creative.

---

## 📦 Funkcjonalności

- Zarządzanie klientami (baza klientów, edycja, archiwizacja)
- Realizacje – przypisywanie klientów do pracowników i śledzenie postępu
- Leady – wstępne kontakty przed konwersją do klientów
- Panel administracyjny (zarządzanie użytkownikami, rolami, usługami, pakietami)
- Dashboard z widokiem użytkownika i opcją zmiany hasła/e-maila
- Obsługa ról: `Admin`, `Dyrektor`, `Kierownik`, `Manager`, `Pracownik`

---

## 🧑‍💻 Technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + MySQL
- Identity (zarządzanie kontami)
- Bootstrap 5 (UI)
- jQuery + Bootstrap Select (wielowybory)

---

## 🚀 Uruchomienie lokalne

1. Ustaw plik `appsettings.json` z połączeniem do bazy danych:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=crmdb;user=root;password=yourpassword"
}

Changelog

Aktualizacja 1.0 (NewBeginning)
-działajace realizacje , baza klientów , leady 
-częściowo sprawny panel admina
-w pełni sprawne funkcje admina z wiazane z zarzadzaniem uzytkownika
-cześciowo sprawne logi
-dodano tymczasowo funkcje w dashboard do zmiany hasła dla uzytkownika
