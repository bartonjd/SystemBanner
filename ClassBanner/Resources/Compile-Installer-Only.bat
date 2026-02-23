@echo off
REM ========================================
REM Desktop Banner - Compile Installer Only
REM ========================================
REM This batch file only compiles the Inno Setup installer
REM (assumes the app is already built)

echo.
echo ========================================
echo Desktop Banner - Compile Installer
echo ========================================
echo.

REM Set paths
set "ISCC=C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
set "SCRIPT_DIR=%~dp0"
set "ISS_FILE=%SCRIPT_DIR%DesktopBanner-Installer.iss"

REM Check if Inno Setup exists
if not exist "%ISCC%" (
    echo ERROR: Inno Setup not found at: %ISCC%
    echo.
    echo Please install Inno Setup or update the ISCC variable in this batch file
    pause
    exit /b 1
)

echo Compiling installer...
echo.
cd /d "%SCRIPT_DIR%"
"%ISCC%" "%ISS_FILE%"
if errorlevel 1 (
    echo.
    echo ERROR: Compilation failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo Installer compiled successfully!
echo ========================================
echo.
pause
