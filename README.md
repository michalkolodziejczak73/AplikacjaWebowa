Opis projektu

Projekt został wykonany w ramach przedmiotu Programowanie zaawansowane.
Aplikacja umożliwia tworzenie i wypełnianie ankiet internetowych. Zalogowani użytkownicy mogą przeglądać dostępne ankiety, oddawać głosy oraz sprawdzać wyniki. Wyniki przedstawiane są w formie liczbowej, procentowej oraz na wykresie słupkowym.

W aplikacji występują dwie role użytkowników:

Ankieter – może tworzyć, edytować i usuwać ankiety, zmieniać ich status oraz przeglądać pełną listę oddanych głosów.
Respondent – może przeglądać ankiety, oddawać głosy oraz sprawdzać wyniki.

Każdy respondent może oddać tylko jeden głos w danej ankiecie.

Projekt został wykonany z użyciem:

C#,
.NET 8,
ASP.NET Core MVC,
Entity Framework Core,
ASP.NET Core Identity,
Razor Views,
Bootstrap,
Chart.js,
HTML,
CSS,
JavaScript.
Funkcje aplikacji
Rejestracja i logowanie

Aplikacja korzysta z ASP.NET Core Identity. Użytkownik może utworzyć konto, zalogować się oraz wylogować.

Nowe konta otrzymują rolę Respondent. Wybrane konto testowe posiada rolę Ankieter.

Tworzenie ankiet

Ankieter może utworzyć ankietę, podając:

-pytanie ankiety,
-dwie obowiązkowe opcje odpowiedzi,
-maksymalnie dwie dodatkowe opcje odpowiedzi.

Po zapisaniu ankieta pojawia się na liście dostępnych ankiet.

Głosowanie

Respondent może otworzyć aktywną ankietę i wybrać jedną odpowiedź.

Aplikacja sprawdza, czy użytkownik wcześniej głosował w danej ankiecie. Jeżeli głos został już oddany, formularz głosowania nie jest ponownie wyświetlany.

Wyniki ankiety

Wyniki zawierają:

-liczbę wszystkich oddanych głosów,
-liczbę głosów na każdą odpowiedź,
-wynik procentowy każdej odpowiedzi,
-wykres słupkowy wykonany przy użyciu Chart.js.
-Edycja ankiety

Ankieter może:

-zmienić pytanie ankiety,
-ustawić ankietę jako aktywną,
-ustawić ankietę jako nieaktywną.

W nieaktywnej ankiecie nie można oddawać głosów. Nadal można jednak przeglądać jej wyniki.

Usuwanie ankiety

Ankieter może usunąć ankietę po wcześniejszym potwierdzeniu operacji.

Usunięcie ankiety powoduje również usunięcie powiązanych z nią opcji odpowiedzi oraz głosów.

Lista głosów

Ankieter może przeglądać pełną listę głosów dla wybranej ankiety. Lista zawiera:

-wybraną odpowiedź,
-identyfikator respondenta,
-datę oddania głosu.
-Modele danych

W projekcie wykorzystano trzy główne modele.

Survey

Model reprezentujący ankietę.
Najważniejsze pola:

Id – identyfikator ankiety,
Question – pytanie ankiety,
CreatedAt – data utworzenia,
IsActive – status ankiety,
CreatorId – identyfikator ankietera,
Options – lista opcji odpowiedzi.


Option

Model reprezentujący pojedynczą opcję odpowiedzi.
Najważniejsze pola:

Id – identyfikator opcji,
Text – treść odpowiedzi,
SurveyId – identyfikator ankiety,
Votes – lista głosów przypisanych do opcji.


Vote

Model reprezentujący oddany głos.
Najważniejsze pola:

Id – identyfikator głosu,
OptionId – identyfikator wybranej odpowiedzi,
RespondentId – identyfikator użytkownika,
VotedAt – data oddania głosu.

Relacje między modelami:

jedna ankieta może posiadać wiele opcji,
jedna opcja może posiadać wiele głosów.
Struktura projektu

Najważniejsze foldery i pliki:

Controllers

Zawiera kontrolery obsługujące żądania użytkowników.

Najważniejszym kontrolerem jest:

SurveysController.cs – obsługuje listę ankiet, tworzenie, głosowanie, wyniki, edycję, usuwanie oraz listę głosów.
Models

Zawiera modele danych:

Survey.cs,
Option.cs,
Vote.cs.
ViewModels

Zawiera klasy wykorzystywane do przekazywania danych pomiędzy kontrolerem a widokami:

CreateSurveyViewModel.cs,
EditSurveyViewModel.cs,
VoteViewModel.cs,
SurveyResultsViewModel.cs.


Views

Zawiera widoki aplikacji.
Widoki ankiet znajdują się w folderze:

Views/Surveys

Znajdują się tam między innymi:

Index.cshtml – lista ankiet,
Create.cshtml – tworzenie ankiety,
Details.cshtml – głosowanie,
Results.cshtml – wyniki i wykres,
Edit.cshtml – edycja ankiety,
Delete.cshtml – potwierdzenie usunięcia,
Votes.cshtml – lista oddanych głosów.


Data

Zawiera konfigurację bazy danych oraz inicjalizację ról:

ApplicationDbContext.cs,
DbInitializer.cs.
wwwroot

Zawiera pliki statyczne aplikacji, między innymi:

style CSS,
pliki JavaScript,
biblioteki wykorzystywane przez interfejs.

Główna kolorystyka aplikacji znajduje się w pliku:

wwwroot/css/site.css

Konta testowe

Ankieter

Login:

michal@test1.pl

Hasło:

Asdasd123!

Konto ankietera może:

-tworzyć ankiety,
-edytować ankiety,
-usuwać ankiety,
-zmieniać status ankiety,
-przeglądać wyniki,
-przeglądać pełną listę głosów.


Respondent

Login:

respondent@test.pl

Hasło:

Asdasd123!

Konto respondenta może:

-przeglądać ankiety,
-oddawać głosy,
-przeglądać wyniki.

Respondent nie ma dostępu do tworzenia, edytowania ani usuwania ankiet.

Uruchomienie projektu
Sklonuj repozytorium:
git clone github.com/michalkolodziejczak73/AplikacjaWebowa
Otwórz plik rozwiązania:
AplikacjaWebowa.sln
Uruchom projekt w Visual Studio Community.
W razie potrzeby otwórz Konsolę Menedżera pakietów i wykonaj:
Update-Database
Uruchom aplikację przyciskiem https lub klawiszem:
F5
Aplikacja otworzy się w przeglądarce pod lokalnym adresem localhost.
Dodatkowe informacje

Aplikacja korzysta z lokalnej bazy danych utworzonej przez Entity Framework Core.

Role Ankieter i Respondent są tworzone automatycznie podczas uruchamiania aplikacji.

Interfejs został dostosowany przy użyciu własnych stylów CSS. Poszczególne działania mają wyróżniające się kolory, na przykład:

zielony – główne działania i wyniki,
żółty – edycja,
czerwony – usuwanie,
granatowy – elementy nawigacji.