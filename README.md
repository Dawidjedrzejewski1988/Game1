# GameMVC

## Wymagania systemowe:
      - System operacyjny: np. Windows 10/11, Linux (dystrybucja)
    Oprogramowanie:
      - .NET SDK 6.0 lub nowszy
      - Serwer bazy danych SQL Server
      - Narzędzia developerskie Visual Studio


## Opis instalacji:
           - Klonowanie repozytorium: pobierz repozytorium
           - W pliku appseting.json ustaw parametry połączenia:
              Należy podać własny: "Server=twójlocalhost" w pliku appsettings.json
      {
        "ConnectionStrings": {
          "DefaultConnection": "Server=twójlocalhost;Database=GameDb;Trusted_Connection=True;
                                MultipleActiveResultSets=true;TrustServerCertificate=True"
      },
           - Wykonaj migrację za pomocą konsoli menadżera pakietów w ścieżce projektu:
              ADD-MIGRATION "NewBase"
              UPDATE-DATABASE

      Uruchom aplikację za pomocą Visual Studio

## Testowi użytkownicy:
    Admin:
        Login: admin@game.pl
        Hasło: ADMIN123
    Gracz:
        Login: user@game.pl
        Hasło: QWER1234

## Opis działania aplikacji:
      Rejestracja: Użytkownicy mogą się zarejestrować, wprowadzając dane email, hasło.
      Logowanie: Zalogowanie się do aplikacji za pomocą e-maila i hasła.
      Tworzenie nowego bohatera: tworzenie (Nazwa, wybór klasy), edytowanie (zmiana nazwy bohatera) i usuwanie.
      Interfejs użytkownika: Krótkie opisy wyglądu aplikacji i elementów, z którymi użytkownik będzie wchodził w interakcję.
      Gra: Gracz może przeglądać misje, kupować i sprzedawać przedmioty w sklepie, przeglądać swoją postać,
           zakładać/ściągać przedmioty które zwiększają statystyki. 

      Administrator: dodawanie, edytowanie oraz usuwanie misji i przedmiotów.

      Forum
      Niezalogowany użytkownik może przeglądać forum.
      Zalogowany użytkownik może towrzyć nowe wątki, edytować swoje wątki oraz odpowiadać na wszystkie wątki.
      
      Forum Administrator
      Administrator może robić to samo co zalogowany użytkownik oraz dodatkowo usuwać wątki.


# Autorzy:
    Michał Handzel
    Dawid Jędrzejewski