# Build and Create Installer for DesktopBanner
# This script builds the application and compiles the installer

param(
    [string]$Configuration = "Release",
    [string]$DotNetVersion = "net10.0",
    [string]$InnoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Desktop Banner - Build & Package Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir
$SolutionDir = Split-Path -Parent $ProjectRoot

# Define paths
$ProjectFile = Join-Path $ProjectRoot "DesktopBanner.csproj"
$InstallerScript = Join-Path $ScriptDir "DesktopBanner-Installer.iss"
$OutputDir = Join-Path $SolutionDir "Output"

Write-Host "Project Root: $ProjectRoot" -ForegroundColor Gray
Write-Host "Solution Dir: $SolutionDir" -ForegroundColor Gray
Write-Host ""

# Step 1: Clean previous builds
Write-Host "[1/4] Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path (Join-Path $ProjectRoot "bin\$Configuration")) {
    Remove-Item -Path (Join-Path $ProjectRoot "bin\$Configuration") -Recurse -Force -ErrorAction SilentlyContinue
}
if (Test-Path $OutputDir) {
    Remove-Item -Path (Join-Path $OutputDir "*.exe") -Force -ErrorAction SilentlyContinue
}
Write-Host "      Clean completed." -ForegroundColor Green
Write-Host ""

# Step 2: Build the application
Write-Host "[2/4] Building DesktopBanner application..." -ForegroundColor Yellow
Push-Location $ProjectRoot
try {
    dotnet build -c $Configuration --no-incremental
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE"
    }
    Write-Host "      Build completed successfully." -ForegroundColor Green
} catch {
    Write-Host "      ERROR: Build failed - $_" -ForegroundColor Red
    Pop-Location
    exit 1
}
Pop-Location
Write-Host ""

# Step 3: Verify build output
Write-Host "[3/4] Verifying build output..." -ForegroundColor Yellow
$ExePath = Join-Path $ProjectRoot "bin\$Configuration\$DotNetVersion-windows\win-x64\DesktopBanner.exe"
if (-not (Test-Path $ExePath)) {
    Write-Host "      ERROR: Build output not found at: $ExePath" -ForegroundColor Red
    exit 1
}
$FileSize = (Get-Item $ExePath).Length / 1MB
Write-Host "      DesktopBanner.exe found ($([math]::Round($FileSize, 2)) MB)" -ForegroundColor Green
Write-Host ""

# Step 4: Compile installer
Write-Host "[4/4] Compiling installer..." -ForegroundColor Yellow
if (-not (Test-Path $InnoSetupPath)) {
    Write-Host "      ERROR: Inno Setup not found at: $InnoSetupPath" -ForegroundColor Red
    Write-Host "      Please install Inno Setup from: https://jrsoftware.org/isinfo.php" -ForegroundColor Yellow
    exit 1
}

try {
    & $InnoSetupPath $InstallerScript
    if ($LASTEXITCODE -ne 0) {
        throw "Installer compilation failed with exit code $LASTEXITCODE"
    }
    Write-Host "      Installer compiled successfully." -ForegroundColor Green
} catch {
    Write-Host "      ERROR: Installer compilation failed - $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Display results
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Build Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if (Test-Path $OutputDir) {
    $InstallerFiles = Get-ChildItem -Path $OutputDir -Filter "*.exe"
    if ($InstallerFiles) {
        Write-Host "Installer created:" -ForegroundColor Green
        foreach ($file in $InstallerFiles) {
            $size = $file.Length / 1MB
            Write-Host "  $($file.Name) ($([math]::Round($size, 2)) MB)" -ForegroundColor White
            Write-Host "  Location: $($file.FullName)" -ForegroundColor Gray
        }
    }
}

Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Test the installer on a clean system" -ForegroundColor White
Write-Host "  2. Verify banner displays correctly" -ForegroundColor White
Write-Host "  3. Test Group Policy configuration (if ADMX installed)" -ForegroundColor White
Write-Host ""
