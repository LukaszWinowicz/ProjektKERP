@echo off
REM Ustaw pełną ścieżkę do katalogu z plikiem .exe
set ProgramPath=C:\kerp_dotnet\KERP\Module.Tasks\Task.SendReprocessLog\bin\Debug\net8.0

REM Ustaw zmienną dla nazwy programu
set ProgramName=Task.SendReprocessLog.exe

REM Ustawienie nazwy pliku logu
set LOGFILE=C:\kerp_dotnet\KERP\Module.Tasks\Common\Logs\SendReprocessLog.txt

REM Przejdź do katalogu programu
cd /d "%ProgramPath%"

REM Sprawdź, czy katalog istnieje
if not exist "%ProgramPath%" (
    echo Ścieżka %ProgramPath% nie istnieje.
    pause
    exit /b
)

REM Sprawdź, czy plik exe istnieje
if not exist "%ProgramName%" (
    echo Plik %ProgramName% nie istnieje w katalogu %ProgramPath%.
    pause
    exit /b
)

REM Uruchomienie programu C# i zapisanie logów
"%ProgramName%" >> "%LOGFILE%" 2>&1

REM Zatrzymanie okna wiersza poleceń, aby zobaczyć wynik działania programu
exit
