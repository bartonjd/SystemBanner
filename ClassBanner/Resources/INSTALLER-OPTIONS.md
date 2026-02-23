# Desktop Banner - Installer Guide

## Licensing Clarification ✅

### Inno Setup Licensing (RECOMMENDED)
**You CAN use Inno Setup for FREE!**

- ✅ **FREE for NON-COMMERCIAL use** - Your situation qualifies
- ✅ Developing on your own time = non-commercial
- ✅ Not currently selling = non-commercial
- ✅ Can use Inno Setup 6.4.3 (last fully free version) or 6.5.0+ (free for non-commercial)

**If you decide to sell later:**
- Purchase perpetual license (~$129 USD one-time, includes 2 years of updates)
- Or continue using version 6.4.3 which remains free for commercial use

**Conclusion:** Use the Inno Setup installer I created. You're fully compliant!

---

## Free Installer Options

You have **THREE free options** for creating installers:

### Option 1: Inno Setup (RECOMMENDED ⭐)

**Status:** FREE for you (non-commercial)

**Pros:**
- ✅ Easy to use and configure
- ✅ Modern, professional UI
- ✅ Small installer size
- ✅ Wide industry adoption
- ✅ Excellent documentation

**Cons:**
- ⚠️ Need license if you sell commercially later (~$129)
- ⚠️ Script-based (not declarative like MSI)

**Files:**
- `DesktopBanner-Installer.iss` - Main installer script
- `Build-Installer.ps1` - Automated build script

**Build:**
```powershell
cd SystemBanner/ClassBanner/Resources
.\Build-Installer.ps1
```

**Download Inno Setup:**
- Latest: https://jrsoftware.org/isdl.php
- Or v6.4.3 (always free): https://jrsoftware.org/isdl.php#stable

---

### Option 2: WiX Toolset (100% FREE FOREVER)

**Status:** FREE forever (MS Public License, Open Source)

**Pros:**
- ✅ Creates MSI installers (industry standard)
- ✅ 100% free for any use (commercial or non-commercial)
- ✅ Declarative XML-based
- ✅ Full control over installation
- ✅ Trusted by enterprises

**Cons:**
- ⚠️ Steeper learning curve
- ⚠️ XML configuration (more complex)
- ⚠️ Requires WiX Toolset installation

**Files:**
- `DesktopBanner.wxs` - WiX source file
- `Build-WiX-Installer.ps1` - Automated build script

**Install WiX:**

**WiX v3 (Stable, Recommended):**
```powershell
# Download installer from:
https://github.com/wixtoolset/wix3/releases/latest

# Install WiX v3, then build:
cd SystemBanner/ClassBanner/Resources
.\Build-WiX-Installer.ps1 -WixVersion v3
```

**WiX v4 (Modern, .NET-based):**
```powershell
# Install via .NET CLI:
dotnet tool install --global wix

# Build:
cd SystemBanner/ClassBanner/Resources
.\Build-WiX-Installer.ps1 -WixVersion v4
```

---

### Option 3: .NET Single-File Deployment (No Installer)

**Status:** FREE (built into .NET)

**Pros:**
- ✅ No installer needed
- ✅ Single .exe file
- ✅ No dependencies
- ✅ Easy distribution

**Cons:**
- ⚠️ No automatic registry setup
- ⚠️ No Start Menu shortcuts
- ⚠️ No ADMX deployment
- ⚠️ Users must configure manually

**Build:**
```powershell
cd SystemBanner/ClassBanner
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

This creates a single `DesktopBanner.exe` that users can run directly.

---

## What's in Your `inst` Folder?

Your `inst/DemoInstaller` folder contains a **Windows Application Packaging Project (MSIX/APPX)**, NOT WiX Toolset.

**MSIX is NOT suitable for DesktopBanner because:**
- ❌ Sandboxed environment - limited HKLM registry access
- ❌ Your app needs `HKLM\SOFTWARE\DesktopBanner` (requires full system access)
- ❌ DoD/enterprise environments prefer traditional installers
- ❌ ADMX deployment requires unrestricted file system access

**MSIX is designed for:**
- Microsoft Store apps
- Modern UWP applications
- Sandboxed consumer apps

---

## Comparison Matrix

| Feature | Inno Setup | WiX Toolset | MSIX (inst folder) |
|---------|-----------|-------------|-------------------|
| **License** | Free (non-commercial) | Free forever | Free |
| **File Type** | .exe | .msi | .msix/.appx |
| **HKLM Registry** | ✅ Yes | ✅ Yes | ❌ Limited |
| **Learning Curve** | Easy | Moderate | Moderate |
| **Enterprise Ready** | ✅ Yes | ✅ Yes | ⚠️ Limited |
| **ADMX Deploy** | ✅ Yes | ✅ Yes | ❌ No |
| **File Size** | Small | Medium | Large |
| **Admin Required** | Yes | Yes | Optional |
| **Good for DoD** | ✅ Yes | ✅ Yes | ❌ No |

---

## My Recommendation

### For Now: Use Inno Setup ⭐
**Why:**
- You're non-commercial (FREE)
- Easiest to use
- Already created and ready to use
- Professional results
- Can upgrade to commercial license later if needed

### For Long-Term Commercial: Consider WiX
**Why:**
- 100% free forever
- MSI is more "enterprise" friendly
- No licensing concerns ever
- Worth the learning investment

### Avoid: MSIX (inst folder)
**Why:**
- Not suitable for system-level tools
- Registry limitations
- Not designed for DoD environments

---

## Quick Start Guide

### Using Inno Setup (Recommended):

```powershell
# 1. Download Inno Setup
# Visit: https://jrsoftware.org/isdl.php

# 2. Build application
cd SystemBanner/ClassBanner
dotnet build -c Release

# 3. Create installer
cd Resources
.\Build-Installer.ps1

# Output: SystemBanner/Output/DesktopBanner-Setup-1.0.exe
```

### Using WiX Toolset:

```powershell
# 1. Install WiX v3
# Download from: https://github.com/wixtoolset/wix3/releases

# 2. Build application
cd SystemBanner/ClassBanner
dotnet build -c Release

# 3. Create MSI
cd Resources
.\Build-WiX-Installer.ps1 -WixVersion v3

# Output: SystemBanner/Output/DesktopBanner.msi
```

---

## License Summary

| Tool | Commercial Use | Cost | Your Status |
|------|---------------|------|-------------|
| **Inno Setup 6.4.3** | ✅ Always Free | $0 | ✅ Use it |
| **Inno Setup 6.5.0+** | ⚠️ Need License | $129 (if commercial) | ✅ FREE (non-commercial) |
| **WiX Toolset** | ✅ Always Free | $0 | ✅ Use it |
| **MSIX** | ✅ Always Free | $0 | ❌ Not suitable |

---

## Support

For issues or questions:
- Inno Setup: https://jrsoftware.org/isfaq.php
- WiX Toolset: https://wixtoolset.org/documentation/
- This Project: Check the LICENSE.txt file (MIT License)

**Bottom Line:** You have zero licensing concerns with either Inno Setup or WiX for your current situation! 🎉
