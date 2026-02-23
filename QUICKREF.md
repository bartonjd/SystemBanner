# Desktop Banner - Quick Reference Guide

## 🚀 Installation

```cmd
# Run installer
DesktopBanner-Setup-1.0.exe

# Silent installation with all features
DesktopBanner-Setup-1.0.exe /VERYSILENT /TASKS="autostart,installpolicy"
```

## ⚙️ Configuration

### Registry Key
```
HKLM\SOFTWARE\DesktopBanner\
```

### Quick Setup via PowerShell
```powershell
# Display "@USER on @HOST" with green background
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "LeftDisplayText" -Value "@HOST" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "CenterDisplayText" -Value "CLASSIFIED" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "RightDisplayText" -Value "@USER" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "BackgroundColor" -Value "#008000" -PropertyType String -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "Opacity" -Value 100 -PropertyType DWord -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\DesktopBanner" -Name "DisplayMode" -Value 2 -PropertyType DWord -Force
```

## 🎨 Common Configurations

### DoD Top Secret
```powershell
CenterDisplayText = "TOP SECRET//SCI"
BackgroundColor = "#FF0000"  # Red
DisplayMode = 2  # Static (most secure)
```

### DoD Secret
```powershell
CenterDisplayText = "SECRET"
BackgroundColor = "#FF0000"  # Red
DisplayMode = 2  # Static
```

### DoD Unclassified
```powershell
CenterDisplayText = "UNCLASSIFIED"
BackgroundColor = "#008000"  # Green
DisplayMode = 0  # Overlay
```

### Production Environment
```powershell
LeftDisplayText = "PRODUCTION"
CenterDisplayText = "@HOST"
RightDisplayText = "@USER"
BackgroundColor = "#FFA500"  # Orange
```

### Development Environment
```powershell
LeftDisplayText = "DEV"
CenterDisplayText = "@HOST"
RightDisplayText = "@USER"
BackgroundColor = "#0000FF"  # Blue
```

## 🎯 Display Modes

| Mode | Value | When to Use |
|------|-------|-------------|
| Overlay | 0 | Default, least intrusive |
| Rollover | 1 | Need to access top of screen easily |
| Static | 2 | Maximum security, banner always visible |

## 🔤 Magic Tokens

| Token | Replaced With | Example Output |
|-------|--------------|----------------|
| `@USER` | Current username | `JohnDoe` |
| `@HOST` | Computer name | `WORKSTATION01` |
| `@USER on @HOST` | Both combined | `JohnDoe on WORKSTATION01` |

## 🎨 Color Options

### Named Colors
`Red`, `Green`, `Blue`, `Yellow`, `Orange`, `Purple`, `Black`, `White`

### Hex Colors
- `#FF0000` - Red
- `#008000` - Green
- `#0000FF` - Blue
- `#FFA500` - Orange
- `#800080` - Purple

## 📋 Registry Settings Reference

| Setting | Type | Values | Default |
|---------|------|--------|---------|
| LeftDisplayText | String | Any text, tokens | `@HOST` |
| CenterDisplayText | String | Any text, tokens | `UNCLASSIFIED` |
| RightDisplayText | String | Any text, tokens | `@USER` |
| BackgroundColor | String | Color name or hex | `#008000` |
| Opacity | DWORD | 0-100 | `100` |
| ShowOnBottom | DWORD | 0=Top, 1=Both | `0` |
| DisplayMode | DWORD | 0/1/2 (see above) | `0` |

## 🔧 Group Policy

### Enable Policy
```
Computer Configuration
  → Administrative Templates
    → System
      → Desktop Banner
        → Desktop Banner Configuration [ENABLED]
```

### Apply Changes
```cmd
gpupdate /force
```

## 🛠️ Troubleshooting

### Banner not showing?
```powershell
# Check if running
Get-Process DesktopBanner

# Start manually
& "C:\Program Files\DesktopBanner\DesktopBanner.exe"

# Check registry
Get-ItemProperty "HKLM:\SOFTWARE\DesktopBanner"
```

### Need to restart?
```powershell
# Stop
Stop-Process -Name DesktopBanner -Force

# Start
Start-Process "C:\Program Files\DesktopBanner\DesktopBanner.exe"
```

## 📞 Support

**Issues:** [GitHub Issues](https://github.com/yourusername/DesktopBanner/issues)

**Documentation:** See [README.md](README.md) for full documentation
