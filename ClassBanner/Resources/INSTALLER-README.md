# Desktop Banner Installer - Build Instructions

## Prerequisites

1. **Inno Setup Compiler** (version 6.0 or later)
   - Download from: https://jrsoftware.org/isinfo.php
   - Install with default options

2. **Build the Application First**
   ```powershell
   # Navigate to the ClassBanner directory
   cd SystemBanner/ClassBanner
   
   # Build in Release mode
   dotnet build -c Release
   ```

## Building the Installer

### Option 1: Using Inno Setup Compiler GUI

1. Open Inno Setup Compiler
2. Click "File" → "Open" 
3. Navigate to: `SystemBanner/ClassBanner/Resources/DesktopBanner-Installer.iss`
4. Click "Build" → "Compile" (or press Ctrl+F9)
5. The installer will be created in: `SystemBanner/Output/DesktopBanner-Setup-1.0.exe`

### Option 2: Using Command Line

```powershell
# Compile the installer using ISCC (Inno Setup Command Line Compiler)
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "SystemBanner\ClassBanner\Resources\DesktopBanner-Installer.iss"
```

## Installer Features

### User Selectable Options

1. **Banner Style Selection**
   - **Overlay (Default)**: Banner displays as an overlay without reserving screen space
   - **Rollover**: Banner hides when mouse hovers over it
   - **Static (AppBar)**: Banner reserves screen space using Windows AppBar API

2. **Startup Options**
   - Auto-start with Windows (creates HKLM Run registry entry)

3. **Group Policy Templates**
   - Option to install ADMX/ADML files to `C:\Windows\PolicyDefinitions`
   - Enables centralized management via Group Policy

### Default Registry Settings

The installer creates the following default registry values in `HKLM\SOFTWARE\DesktopBanner`:

| Value Name          | Type   | Default Value   | Description                                    |
|---------------------|--------|-----------------|------------------------------------------------|
| LeftDisplayText     | String | @HOST           | Text displayed on left (supports @HOST token)  |
| CenterDisplayText   | String | UNCLASSIFIED    | Text displayed in center                       |
| RightDisplayText    | String | @USER           | Text displayed on right (supports @USER token) |
| ShowOnBottom        | DWORD  | 0               | 0 = Top only, 1 = Top and Bottom              |
| Opacity             | DWORD  | 100             | Opacity level (0-100)                          |
| BackgroundColor     | String | #008000         | Background color (hex or color name)           |
| DisplayMode         | DWORD  | User Selected   | 0 = Overlay, 1 = Rollover, 2 = Static         |

### Token Substitution

The banner supports these tokens in text fields:
- **@USER** - Replaced with current Windows username
- **@HOST** - Replaced with computer hostname

## File Structure

After installation, files are deployed to `C:\Program Files\DesktopBanner\`:

```
DesktopBanner/
├── DesktopBanner.exe          # Main application
├── *.dll                       # Required dependencies
├── *.json                      # Configuration files
└── LICENSE.txt                # MIT License
```

If ADMX installation is selected:
```
C:\Windows\PolicyDefinitions\
├── DesktopBanner.admx         # Group Policy template
└── en-US\
    └── DesktopBanner.adml     # Localized strings
```

## Group Policy Configuration

After installing with ADMX templates:

1. Open Group Policy Editor: `gpedit.msc` (Local) or `gpmc.msc` (Domain)
2. Navigate to: **Computer Configuration** → **Administrative Templates** → **System** → **Desktop Banner**
3. Configure "Desktop Banner Configuration" policy
4. Set the desired text, colors, position, and display mode
5. Apply the policy (run `gpupdate /force` on target machines)

## Uninstallation

The uninstaller will:
- Remove all application files from `C:\Program Files\DesktopBanner\`
- Remove registry key: `HKLM\SOFTWARE\DesktopBanner`
- Remove autostart entry (if enabled)
- **Does NOT remove** ADMX/ADML files (to preserve domain policies)

To manually remove ADMX files:
```powershell
Remove-Item "C:\Windows\PolicyDefinitions\DesktopBanner.admx" -Force
Remove-Item "C:\Windows\PolicyDefinitions\en-US\DesktopBanner.adml" -Force
```

## Troubleshooting

### Build Errors

**Error: "Cannot find file: ..\bin\Release\net6.0-windows\win-x64\DesktopBanner.exe"**
- Solution: Build the application first using `dotnet build -c Release`

**Error: "Cannot find file: install-bg.bmp"**
- Solution: Ensure `install-bg.bmp` exists in the Resources directory
- Or comment out line: `WizardImageFile=install-bg.bmp`

**Error: "Cannot find file: ..\Snap.png"**
- Solution: Ensure `Snap.png` exists in the ClassBanner directory
- Or remove/comment out the icon-related lines

### Runtime Issues

**Banner doesn't start automatically**
- Check if autostart was selected during installation
- Verify registry: `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\DesktopBanner`

**Banner not visible**
- Check registry values in `HKLM\SOFTWARE\DesktopBanner`
- Ensure `CenterDisplayText` has a value
- Verify `Opacity` is set to a value > 0

## Customization

### Changing Default Settings

Edit the `[Registry]` section in `DesktopBanner-Installer.iss`:

```pascal
; Example: Change default to "SECRET" classification
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "CenterDisplayText"; ValueData: "SECRET"; Flags: createvalueifdoesntexist

; Example: Change default background to red
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "BackgroundColor"; ValueData: "#FF0000"; Flags: createvalueifdoesntexist
```

### Adding Custom Pages

Add custom wizard pages in the `[Code]` section's `InitializeWizard` procedure.

## Support

For issues or questions:
- Check the LICENSE.txt file for terms of use
- Review the application source code in the SystemBanner directory
