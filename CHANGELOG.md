# Changelog

All notable changes to Desktop Banner will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-02-23

### Added
- Initial stable release of Desktop Banner
- Multi-monitor support with automatic detection
- Three display modes: Overlay, Rollover, and Static (AppBar)
- Magic token support for `@USER` and `@HOST` substitution
- Group Policy ADMX/ADML templates for centralized management
- Comprehensive installer with Inno Setup
- Alternative WiX MSI installer option
- Full documentation suite
- VS Code build integration with tasks
- Registry-based configuration system
- Support for custom colors (named colors and hex codes)
- Adjustable opacity (0-100%)
- Position control (top only or top and bottom)
- Auto-start with Windows option
- BannerCleanupHelper utility for cleanup operations

### Features

#### Display Modes
- **Overlay Mode** - Banner displays as overlay without reserving screen space
- **Rollover Mode** - Banner auto-hides when mouse hovers over it
- **Static Mode** - Banner reserves screen space using Windows AppBar API

#### Magic Tokens
- `@USER` - Automatically replaced with current Windows username
- `@HOST` - Automatically replaced with computer hostname
- Token combinations supported (e.g., `@USER on @HOST`)

#### Configuration
- Registry path: `HKLM\SOFTWARE\DesktopBanner\`
- Group Policy support via ADMX/ADML templates
- Live configuration updates without restart
- Support for all standard color names and hex codes

#### Deployment
- Inno Setup installer (EXE)
- WiX Toolset installer (MSI)
- Silent installation support
- Group Policy deployment ready
- Auto-start configuration
- ADMX template deployment options

### Documentation
- Comprehensive README.md with examples
- Detailed build guides for multiple platforms
- Installer comparison documentation
- VS Code integration guide
- Quick start guide
- Troubleshooting section

### Build System
- Batch file builds (Windows)
- PowerShell build scripts
- MSBuild integration
- VS Code tasks integration
- Support for Inno Setup 6.x
- Support for WiX Toolset v3/v4

### Technical Details
- Built with .NET 6.0 (WPF)
- Targets Windows 10/11 (x64)
- Uses WpfScreenHelper for multi-monitor support
- Implements Windows AppBar API for Static mode
- MIT License

### Known Issues
- .NET 6.0 is approaching end-of-support (upgrade to .NET 8 recommended)
- Minor nullability warnings in debug builds (non-critical)
- PNG files cannot be used directly as installer icons (size limitation)

### Dependencies
- .NET 6.0 Runtime (Windows Desktop)
- WpfScreenHelper 2.1.0

### Installation Requirements
- Windows 10 version 1809 or later
- Windows 11 (all versions)
- Administrator rights for installation
- .NET 6.0 Desktop Runtime (included in installer)

---

## [Unreleased]

### Planned Features
- .NET 8.0 upgrade for extended support
- Custom font support
- Banner templates for common classifications
- Remote management API
- Image/logo support in banner
- Multi-language support
- Configuration GUI tool

### Under Consideration
- macOS version
- Linux version
- Web-based configuration portal
- Centralized logging
- Banner scheduling
- Multiple banner profiles

---

## Version History

### Pre-Release Versions

Development history prior to 1.0.0 release:
- Initial WPF application development
- Multi-monitor support implementation
- AppBar integration for Static mode
- Registry configuration system
- Group Policy template creation
- Installer development

---

## Upgrade Notes

### From Pre-Release to 1.0.0

If upgrading from a development version:

1. **Uninstall previous version** (if installed)
2. **Backup registry settings:**
   ```powershell
   reg export "HKLM\SOFTWARE\DesktopBanner" DesktopBanner_Backup.reg
   ```
3. **Install version 1.0.0**
4. **Restore settings if needed:**
   ```powershell
   reg import DesktopBanner_Backup.reg
   ```

### Registry Changes
- No breaking registry changes in 1.0.0
- All previous registry keys remain compatible

### ADMX/ADML Templates
- Templates updated with magic token documentation
- Existing deployments will continue to work
- Recommended to update templates for new documentation

---

## Support

For issues, questions, or feature requests:
- GitHub Issues: https://github.com/yourusername/DesktopBanner/issues
- GitHub Discussions: https://github.com/yourusername/DesktopBanner/discussions

---

## Contributors

- Josh Barton - Initial work and ongoing maintenance

---

**Note:** This is the first stable release (1.0.0) of Desktop Banner. All previous versions were development/pre-release versions.
