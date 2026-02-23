# Desktop Banner - Installer Build Guide for Inno Setup 6.2.2

## ✅ Great News: Inno Setup 6.2.2 is 100% FREE for ANY USE!

**Your version (6.2.2) is from before the commercial licensing requirement (started at 6.5.0)**
- ✅ Completely free for commercial use
- ✅ Completely free for non-commercial use
- ✅ No restrictions whatsoever
- ✅ Perfect for your project!

**Installation Path Detected:**
```
C:\Program Files (x86)\Inno Setup 6\ISCC.exe
```

---

## 🚀 How to Build the Installer

You now have **THREE easy options** to build your installer:

### Option 1: Double-Click Batch File (Easiest) ⭐

**Full build (app + installer):**
```
SystemBanner/ClassBanner/Resources/Build-Installer.bat
```
Double-click this file - it will:
1. Build DesktopBanner in Release mode
2. Verify the build output
3. Compile the Inno Setup installer
4. Show you where the installer was created

**Installer only (if app already built):**
```
SystemBanner/ClassBanner/Resources/Compile-Installer-Only.bat
```
Quick recompile of just the installer.

---

### Option 2: PowerShell Script (More Control)

```powershell
cd SystemBanner/ClassBanner/Resources
.\Build-Installer.ps1
```

**With custom configuration:**
```powershell
.\Build-Installer.ps1 -Configuration Release
```

---

### Option 3: MSBuild Integration (Automated)

**Add to your project to auto-build installer on Release builds:**

#### A. Standalone MSBuild Command:
```cmd
msbuild SystemBanner\ClassBanner\Resources\BuildInstaller.proj /t:BuildInstaller /p:Configuration=Release
```

#### B. Integrate into DesktopBanner.csproj:

Add this to your `DesktopBanner.csproj` (before the closing `</Project>` tag):

```xml
<!-- Auto-build installer on Release builds -->
<Import Project="Resources\BuildInstaller.proj" Condition="'$(Configuration)' == 'Release'" />
```

Then just build normally:
```cmd
dotnet build -c Release
```
The installer will automatically compile after the build!

---

### Option 4: Direct ISCC Command Line

```cmd
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "SystemBanner\ClassBanner\Resources\DesktopBanner-Installer.iss"
```

---

## 📁 File Locations

**Source Files:**
```
SystemBanner/ClassBanner/Resources/
├── DesktopBanner-Installer.iss   ← Main installer script
├── Build-Installer.bat            ← Batch file: full build
├── Compile-Installer-Only.bat     ← Batch file: installer only
├── Build-Installer.ps1            ← PowerShell script
├── BuildInstaller.proj            ← MSBuild integration
└── DesktopBanner.admx/adml        ← Group Policy templates
```

**Output:**
```
SystemBanner/Output/
└── DesktopBanner-Setup-1.0.exe   ← Your installer!
```

---

## 🎯 Quick Start

1. **Build the app** (if not already done):
   ```cmd
   cd SystemBanner\ClassBanner
   dotnet build -c Release
   ```

2. **Create installer** (pick one):
   - **Easiest:** Double-click `Build-Installer.bat`
   - **PowerShell:** Run `.\Build-Installer.ps1`
   - **Command line:** Run ISCC directly

3. **Test the installer:**
   ```
   SystemBanner\Output\DesktopBanner-Setup-1.0.exe
   ```

---

## 🔧 Customization

### Change Inno Setup Path

If your Inno Setup is in a different location, edit any of these files:

**Build-Installer.bat:**
```batch
set "ISCC=C:\Your\Custom\Path\ISCC.exe"
```

**Build-Installer.ps1:**
```powershell
.\Build-Installer.ps1 -InnoSetupPath "C:\Your\Custom\Path\ISCC.exe"
```

**BuildInstaller.proj:**
```xml
<PropertyGroup>
  <InnoSetupPath>C:\Your\Custom\Path\ISCC.exe</InnoSetupPath>
</PropertyGroup>
```

---

## 🛠️ Visual Studio Integration

### Method 1: Post-Build Event

1. Open DesktopBanner project properties
2. Go to **Build** → **Events** → **Post-build event**
3. Add for Release configuration:
   ```cmd
   if $(ConfigurationName)==Release (
     "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "$(ProjectDir)Resources\DesktopBanner-Installer.iss"
   )
   ```

### Method 2: Build Task

Add to `DesktopBanner.csproj`:
```xml
<Target Name="BuildInstaller" AfterTargets="Build" Condition="'$(Configuration)' == 'Release'">
  <Exec Command="&quot;C:\Program Files (x86)\Inno Setup 6\ISCC.exe&quot; &quot;$(ProjectDir)Resources\DesktopBanner-Installer.iss&quot;" />
</Target>
```

### Method 3: Use BuildInstaller.proj

Import the provided MSBuild project:
```xml
<Import Project="Resources\BuildInstaller.proj" Condition="'$(Configuration)' == 'Release'" />
```

---

## 📋 Build Checklist

Before creating the installer, verify:

- ✅ App builds successfully in Release mode
- ✅ Binary location: `bin/Release/net6.0-windows/win-x64/DesktopBanner.exe`
- ✅ All dependencies are in the bin folder
- ✅ ADMX/ADML files are in Resources folder
- ✅ LICENSE.txt exists in ClassBanner folder
- ✅ Snap.png exists (for icon)

---

## 🐛 Troubleshooting

**"Cannot find file: DesktopBanner.exe"**
- Build the app first: `dotnet build -c Release`
- Check: `SystemBanner/ClassBanner/bin/Release/net6.0-windows/win-x64/DesktopBanner.exe`

**"ISCC.exe not found"**
- Verify Inno Setup is installed at: `C:\Program Files (x86)\Inno Setup 6\`
- Or update the path in the build script

**"Cannot find install-bg.bmp"**
- The installer will work without it (just no custom background)
- Or comment out this line in the .iss file: `WizardImageFile=install-bg.bmp`

**"Permission denied" when building**
- Run command prompt/PowerShell as Administrator
- Or build from Visual Studio (already has elevation)

---

## 📊 Build Script Comparison

| Method | Ease | Features | Best For |
|--------|------|----------|----------|
| **Build-Installer.bat** | ⭐⭐⭐⭐⭐ Easy | Full build + installer | Quick builds, beginners |
| **Compile-Installer-Only.bat** | ⭐⭐⭐⭐⭐ Easy | Installer only | Quick updates to .iss |
| **Build-Installer.ps1** | ⭐⭐⭐⭐ Moderate | Full build + options | Power users |
| **BuildInstaller.proj** | ⭐⭐⭐ Advanced | MSBuild integration | CI/CD, automation |
| **Direct ISCC** | ⭐⭐⭐ Moderate | Compile only | Manual control |

---

## 🎉 Summary

You're all set! You have:

✅ **Inno Setup 6.2.2** - 100% free for any use (commercial or non-commercial)
✅ **4 batch/script files** ready to use
✅ **MSBuild integration** for automated builds
✅ **Complete installer** with registry settings, ADMX deployment, and more

**Recommended workflow:**
1. Make code changes
2. Double-click `Build-Installer.bat`
3. Test `Output/DesktopBanner-Setup-1.0.exe`
4. Deploy! 🚀

No licensing concerns, no limitations, just build and ship!
