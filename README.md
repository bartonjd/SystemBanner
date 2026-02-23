# Desktop Banner - DoD Security Classification Banner Tool

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-blue.svg)
![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)

A professional Windows desktop banner application designed for displaying security classification banners, system information, and user notifications across all monitors. Ideal for DoD, government, and enterprise environments requiring persistent visual indicators.

![Banner Display](https://img.shields.io/badge/display-multi--monitor-green.svg)
![Group Policy](https://img.shields.io/badge/management-Group%20Policy-orange.svg)

---

## 🎯 Features

### Display Capabilities
- ✅ **Multi-Monitor Support** - Automatically displays on all connected monitors
- ✅ **Three Display Modes:**
  - **Overlay** - Banner overlays on top without reserving screen space
  - **Rollover** - Banner auto-hides when mouse hovers over it
  - **Static (AppBar)** - Reserves screen space, applications cannot overlap
- ✅ **Customizable Text** - Three text fields (Left, Center, Right aligned)
- ✅ **Magic Tokens** - `@USER` and `@HOST` automatically replaced with username/hostname
- ✅ **Position Control** - Display on top only, or both top and bottom
- ✅ **Custom Colors** - Support for named colors or hex color codes
- ✅ **Opacity Control** - Adjustable transparency (0-100%)

### Management & Deployment
- ✅ **Group Policy Support** - ADMX/ADML templates for centralized or local management
- ✅ **Registry-Based Configuration** - Easy programmatic control
- ✅ **Silent Installation** - MSI and EXE installer options
- ✅ **Auto-Start** - Optional automatic startup with Windows
- ✅ **Enterprise Ready** - Suitable for domain or standalone deployment

---

## 📦 Installation

### Option 1: Run the Installer (Recommended)

Download and run the installer:
```
DesktopBanner-Setup-1.0.exe
```

During installation, choose:
1. **Banner Style:** Overlay / Rollover / Static
2. **Auto-start:** Optionally start with Windows
3. **Group Policy Templates:** Install ADMX/ADML for policy management

### Option 2: Silent Installation

```cmd
REM Standard installation with auto-start
DesktopBanner-Setup-1.0.exe /VERYSILENT /TASKS="autostart,installpolicy"

REM Install without auto-start
DesktopBanner-Setup-1.0.exe /VERYSILENT /TASKS="installpolicy"

REM Unattended with default settings
DesktopBanner-Setup-1.0.exe /SILENT
```

### Option 3: Manual Installation

1. Extract files to `C:\Program Files\DesktopBanner\`
2. Configure registry: `HKLM\SOFTWARE\DesktopBanner\`
3. (Optional) Deploy ADMX/ADML to `C:\Windows\PolicyDefinitions\`

---

## 🚀 Quick Start

### Using Group Policy (Recommended)

1. **Install ADMX templates** (during installation or manually)

2. **Open Group Policy Editor:**
   ```cmd
   gpedit.msc
   ```

3. **Navigate to:**
   ```
   Computer Configuration
     → Administrative Templates
       → System
         → Desktop Banner
           → Desktop Banner Configuration
   ```

4. **Enable and configure** the policy with your desired settings

5. **Apply the policy:**
   ```cmd
   gpupdate /force
   ```

### Using Registry

Set values in `HKLM\SOFTWARE\DesktopBanner\`:

```powershell
# Basic configuration
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "LeftDisplayText" -Value "@HOST" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "CenterDisplayText" -Value "UNCLASSIFIED" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "RightDisplayText" -Value "@USER" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "BackgroundColor" -Value "#008000" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "Opacity" -Value 100 -PropertyType DWord -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "ShowOnBottom" -Value 0 -PropertyType DWord -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "DisplayMode" -Value 0 -PropertyType DWord -Force
```

---

## ⚙️ Configuration

### Registry Settings

| Registry Value | Type | Default | Description |
|----------------|------|---------|-------------|
| `LeftDisplayText` | String | `@HOST` | Text displayed on the left (supports tokens) |
| `CenterDisplayText` | String | `UNCLASSIFIED` | Text displayed in the center |
| `RightDisplayText` | String | `@USER` | Text displayed on the right (supports tokens) |
| `BackgroundColor` | String | `#008000` | Color name (Green, Red) or hex code (#FF0000) |
| `Opacity` | DWORD | `100` | Transparency level (0-100, 100 = fully opaque) |
| `ShowOnBottom` | DWORD | `0` | 0 = Top only, 1 = Top and Bottom |
| `DisplayMode` | DWORD | `0` | 0 = Overlay, 1 = Rollover, 2 = Static |

### Magic Tokens

Special text values that are automatically replaced:

| Token | Replaced With | Example |
|-------|--------------|---------|
| `@USER` | Current Windows username | `JohnDoe` |
| `@HOST` | Computer hostname | `WORKSTATION01` |

**Examples:**
- `@USER` → Displays: `JohnDoe`
- `@HOST` → Displays: `WORKSTATION01`
- `@USER on @HOST` → Displays: `JohnDoe on WORKSTATION01`

### Display Modes

| Mode | Value | Behavior |
|------|-------|----------|
| **Overlay** | 0 | Banner overlays on screen without reserving space. Applications can draw beneath it. |
| **Rollover** | 1 | Banner hides automatically when mouse hovers over it. Useful for accessing items near screen edge. |
| **Static (AppBar)** | 2 | Banner reserves screen space using Windows AppBar API. Applications cannot overlap banner. Most secure. |

### Color Options

**Named Colors:**
- `Red`, `Green`, `Blue`, `Yellow`, `Orange`, `Purple`, `Black`, `White`

**Hex Color Codes:**
- `#FF0000` (Red)
- `#008000` (Green)
- `#0000FF` (Blue)
- `#FFA500` (Orange)

---

## 🛠️ Building from Source

### Prerequisites

- Visual Studio 2022 or later
- .NET 6.0 SDK or later
- Windows 10/11 SDK
- (Optional) Inno Setup 6 for creating installers
- (Optional) WiX Toolset for MSI installers

### Build the Application

```powershell
# Clone the repository
git clone https://github.com/yourusername/DesktopBanner.git
cd DesktopBanner/SystemBanner/ClassBanner

# Build in Release mode
dotnet build -c Release

# Output location:
# bin/Release/net6.0-windows/win-x64/DesktopBanner.exe
```

### Build the Installer

**Option 1: Using Batch File**
```cmd
cd SystemBanner\ClassBanner\Resources
Build-Installer.bat
```

**Option 2: Using PowerShell**
```powershell
cd SystemBanner\ClassBanner\Resources
.\Build-Installer.ps1
```

**Option 3: Using VS Code**
```
Press: Ctrl+Shift+B
Select: Build Installer (Full Build + Compile)
```

### Build Scripts

Located in `ClassBanner/Resources/`:
- `Build-Installer.bat` - Full build + installer (Windows)
- `Build-Installer.ps1` - PowerShell script with options
- `Build-WiX-Installer.ps1` - Create MSI installer (requires WiX)
- `BuildInstaller.proj` - MSBuild integration

---

## 📚 Documentation

Comprehensive documentation is available in the repository:

- **[CHANGELOG.md](CHANGELOG.md)** - Version history and release notes
- **[ClassBanner/Resources/INNO-SETUP-BUILD-GUIDE.md](ClassBanner/Resources/INNO-SETUP-BUILD-GUIDE.md)** - Detailed build instructions
- **[ClassBanner/Resources/INSTALLER-OPTIONS.md](ClassBanner/Resources/INSTALLER-OPTIONS.md)** - Comparison of installer options
- **[ClassBanner/Resources/INSTALLER-README.md](ClassBanner/Resources/INSTALLER-README.md)** - Installer feature documentation
- **[.vscode/QUICK-START.md](.vscode/QUICK-START.md)** - VS Code quick reference
- **[.vscode/VS-CODE-BUILD-GUIDE.md](.vscode/VS-CODE-BUILD-GUIDE.md)** - Complete VS Code integration guide

---

## 🎯 Use Cases

### Department of Defense (DoD)
Display security classification levels:
```
Left: @HOST
Center: TOP SECRET//SCI
Right: @USER
Background: #FF0000 (Red)
```

### Corporate Environments
Display environment indicators:
```
Left: PRODUCTION
Center: @HOST
Right: @USER
Background: #FFA500 (Orange)
```

### Development Environments
Display system information:
```
Left: DEV ENVIRONMENT
Center: @HOST
Right: @USER on @HOST
Background: #008000 (Green)
```

---

## 🔧 Troubleshooting

### Banner Not Displaying

1. **Check if service is running:**
   ```powershell
   Get-Process DesktopBanner
   ```

2. **Verify registry settings:**
   ```powershell
   Get-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner"
   ```

3. **Check Group Policy:**
   ```cmd
   gpresult /r
   ```

### Banner Displays But Text is Missing

- Ensure `CenterDisplayText` has a value
- Verify `Opacity` is not set to 0
- Check background color isn't the same as text color

### Auto-Start Not Working

1. **Check registry:**
   ```powershell
   Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" -Name "DesktopBanner"
   ```

2. **Manually add to startup:**
   ```powershell
   New-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" -Name "DesktopBanner" -Value '"C:\Program Files\DesktopBanner\DesktopBanner.exe"' -PropertyType String -Force
   ```

### Group Policy Not Applying

1. **Verify ADMX installation:**
   ```powershell
   Test-Path "C:\Windows\PolicyDefinitions\DesktopBanner.admx"
   Test-Path "C:\Windows\PolicyDefinitions\en-US\DesktopBanner.adml"
   ```

2. **Force Group Policy update:**
   ```cmd
   gpupdate /force
   ```

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](ClassBanner/LICENSE.txt) file for details.

```
Copyright 2022 Josh Barton

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
```

---

## 🙏 Acknowledgments

- **Inno Setup** - Free installer for Windows applications
- **WiX Toolset** - Free and open source toolset for creating Windows installers
- **WpfScreenHelper** - Multi-monitor support library
- **.NET Foundation** - For the excellent .NET platform

---

## 📞 Support

For issues, questions, or feature requests:

- **Issues:** [GitHub Issues](https://github.com/yourusername/DesktopBanner/issues)
- **Discussions:** [GitHub Discussions](https://github.com/yourusername/DesktopBanner/discussions)

---

## 🗺️ Roadmap

- [ ] Add support for custom fonts
- [ ] Implement banner templates for common classifications
- [ ] Add remote management API
- [ ] Support for images/logos in banner
- [ ] Multi-language support beyond English
- [ ] macOS and Linux versions

---

## 📊 Project Status

**Current Version:** 1.0.0  
**Status:** Stable Release  
**Last Updated:** February 2026

---

**Made with ❤️ for secure computing environments**
