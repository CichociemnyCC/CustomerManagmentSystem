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

Aktualizacja 1.2 (Emergency FIX)
-Naprawiono Krytyczny Problem z Wpisywaniem Dat

Aktualizacja 1.1 (Bugfixy)
-Naprawiono Problemy z wprowadzaniem Dat w bazie/realizacjach/leadach
-Dodano nowy wyglad wprowadzania dat
-Dodano ograniczenie w wprowadzaniu dat w leadach
-Usunieto guzik archiwum z realizacji

Aktualizacja 1.0 (NewBeginning)
-Działajace realizacje , baza klientów , leady 
-Częściowo sprawny panel admina
-W pełni sprawne funkcje admina z wiazane z zarzadzaniem uzytkownika
-Cześciowo sprawne logi
-Dodano tymczasowo funkcje w dashboard do zmiany hasła dla uzytkownika