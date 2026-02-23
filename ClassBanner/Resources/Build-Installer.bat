@echo off
REM ========================================
REM Desktop Banner - Quick Build Batch File
REM ========================================
REM This batch file builds the app and creates the installer

echo.
echo ========================================
echo Desktop Banner - Build and Package
echo ========================================
echo.

REM Set paths
set "ISCC=C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
set "SCRIPT_DIR=%~dp0"
set "PROJECT_ROOT=%SCRIPT_DIR%.."
set "SOLUTION_DIR=%PROJECT_ROOT%\.."
set "ISS_FILE=%SCRIPT_DIR%DesktopBanner-Installer.iss"

REM Check if Inno Setup exists
if not exist "%ISCC%" (
    echo ERROR: Inno Setup not found at: %ISCC%
    echo.
    echo Please install Inno Setup or update the ISCC variable in this batch file
    pause
    exit /b 1
)

echo [1/3] Building DesktopBanner application...
echo.
cd /d "%PROJECT_ROOT%"
dotnet build -c Release --no-incremental
if errorlevel 1 (
    echo.
    echo ERROR: Build failed!
    pause
    exit /b 1
)
echo.
echo       Build completed successfully.
echo.

REM Check if build output exists
set "EXE_PATH=%PROJECT_ROOT%\bin\Release\net6.0-windows\win-x64\DesktopBanner.exe"
if not exist "%EXE_PATH%" (
    echo ERROR: Build output not found at:
    echo %EXE_PATH%
    pause
    exit /b 1
)

echo [2/3] Verifying build output...
echo       DesktopBanner.exe found
echo.

echo [3/3] Compiling installer with Inno Setup...
echo.
cd /d "%SCRIPT_DIR%"
"%ISCC%" "%ISS_FILE%"
if errorlevel 1 (
    echo.
    echo ERROR: Installer compilation failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build Complete!
echo ========================================
echo.
echo Installer location:
dir /b "%SOLUTION_DIR%\Output\DesktopBanner-Setup-*.exe" 2>nul
echo.
echo Full path: %SOLUTION_DIR%\Output\
echo.
pause
