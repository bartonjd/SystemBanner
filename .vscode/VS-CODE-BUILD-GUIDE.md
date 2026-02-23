# VS Code Build Guide for Desktop Banner

## 🎯 Quick Start - Building in VS Code

You now have **complete VS Code integration**! Build your installer with keyboard shortcuts.

### ✅ VS Code Files Created

```
SystemBanner/.vscode/
├── tasks.json          ← Build tasks and shortcuts
├── launch.json         ← Debug configurations
├── settings.json       ← Project settings
└── extensions.json     ← Recommended extensions
```

---

## 🚀 How to Build the Installer in VS Code

### Method 1: Keyboard Shortcut (Recommended) ⭐

1. **Press:** `Ctrl+Shift+B` (or `Cmd+Shift+B` on Mac)
2. **Select:** `Build Installer (Full Build + Compile)`
3. **Done!** The installer will be created at `Output/DesktopBanner-Setup-1.0.exe`

This is set as the **default build task**, so you can also just press:
- `Ctrl+Shift+B` → `Enter` (selects default task)

### Method 2: Command Palette

1. **Press:** `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
2. **Type:** `Tasks: Run Task`
3. **Select:** `Build Installer (Full Build + Compile)`

### Method 3: Terminal Menu

1. **Click:** `Terminal` → `Run Task...`
2. **Select:** `Build Installer (Full Build + Compile)`

---

## 📋 Available Build Tasks

All tasks are accessible via `Ctrl+Shift+P` → `Tasks: Run Task` or `Ctrl+Shift+B`

### 🔨 Main Build Tasks

| Task Name | Shortcut | What It Does |
|-----------|----------|--------------|
| **Build Installer (Full Build + Compile)** | `Ctrl+Shift+B` | ⭐ Builds app + creates installer (DEFAULT) |
| **Build Application (Release)** | `Ctrl+Shift+B` | Builds app in Release mode |
| **Build Application (Debug)** | `Ctrl+Shift+B` | Builds app in Debug mode |
| **Compile Installer Only** | `Ctrl+Shift+B` | Quick recompile of installer only |
| **Build App Then Compile Installer** | `Ctrl+Shift+B` | Sequential: build → installer |

### 🔧 Alternative Installers

| Task Name | What It Does |
|-----------|--------------|
| **Build Installer (PowerShell)** | Uses PowerShell script with verbose output |
| **Build WiX MSI Installer** | Creates MSI installer (alternative to .exe) |

### 🎮 Run & Debug Tasks

| Task Name | What It Does |
|-----------|--------------|
| **Run DesktopBanner (Debug)** | Builds and runs debug version |
| **Run DesktopBanner (Release)** | Builds and runs release version |
| **Launch DesktopBanner (Debug)** | F5 - Debug with breakpoints |

### 🧹 Utility Tasks

| Task Name | What It Does |
|-----------|--------------|
| **Clean Build** | Removes bin/obj folders |
| **Clean All (App + Installer)** | Removes bin/obj/Output folders |
| **Open Output Folder** | Opens Output folder in Explorer |

---

## 🎯 Recommended Workflow in VS Code

### First Time Setup:

1. **Open folder in VS Code:**
   ```bash
   code SystemBanner
   ```

2. **Install recommended extensions** (VS Code will prompt):
   - C# Dev Kit
   - PowerShell
   - XML

3. **Trust the workspace** (if prompted)

### Daily Development Workflow:

```
1. Make code changes
   ↓
2. Press Ctrl+Shift+B (default build task)
   ↓
3. Installer is created at Output/DesktopBanner-Setup-1.0.exe
   ↓
4. Test the installer
```

### Quick Iterations:

**If you only changed the .iss file:**
1. `Ctrl+Shift+P`
2. Type: `Compile Installer Only`
3. `Enter`

**If you changed C# code:**
1. `Ctrl+Shift+B` (builds app + installer)

---

## 🐛 Debugging in VS Code

### Start Debugging:

1. **Press:** `F5`
2. Or click: `Run and Debug` (left sidebar)
3. Or select: `Launch DesktopBanner (Debug)`

### Available Debug Configurations:

- **Launch DesktopBanner (Debug)** - F5 - Start with debugging
- **Launch DesktopBanner (Release)** - Run optimized build
- **Attach to DesktopBanner** - Attach to running process

### Set Breakpoints:

1. Click in the gutter (left of line numbers)
2. Red dot appears
3. Press F5 to start debugging
4. Code will pause at breakpoint

---

## ⚙️ VS Code Settings Configured

Your workspace now has these optimizations:

✅ **Build Folders Hidden** - Cleaner file explorer
- `bin/`, `obj/`, `.vs/` are hidden from view

✅ **Format on Save** - Code auto-formats when you save

✅ **File Associations** - Syntax highlighting for:
- `.iss` files (Inno Setup)
- `.wxs` files (WiX)
- `.admx/.adml` files (Group Policy)

✅ **PowerShell Terminal** - Default terminal is PowerShell

✅ **C# IntelliSense** - Full code completion and analysis

---

## 🎹 Keyboard Shortcuts Quick Reference

| Action | Windows/Linux | Mac |
|--------|---------------|-----|
| **Build Installer** | `Ctrl+Shift+B` | `Cmd+Shift+B` |
| **Command Palette** | `Ctrl+Shift+P` | `Cmd+Shift+P` |
| **Start Debugging** | `F5` | `F5` |
| **Run Without Debug** | `Ctrl+F5` | `Cmd+F5` |
| **Stop Debugging** | `Shift+F5` | `Shift+F5` |
| **Toggle Terminal** | `Ctrl+\`` | `Cmd+\`` |
| **Open File** | `Ctrl+P` | `Cmd+P` |
| **Save** | `Ctrl+S` | `Cmd+S` |
| **Find** | `Ctrl+F` | `Cmd+F` |
| **Find in Files** | `Ctrl+Shift+F` | `Cmd+Shift+F` |

---

## 📝 Task Customization

### Change Default Build Task:

Edit `.vscode/tasks.json` and change `"isDefault": true` to a different task.

### Add New Task:

1. Open `.vscode/tasks.json`
2. Add a new task object:
   ```json
   {
       "label": "My Custom Task",
       "type": "shell",
       "command": "your-command-here",
       "group": "build"
   }
   ```

### Modify Inno Setup Path:

If Inno Setup is in a different location, edit the batch files or create a custom task:

```json
{
    "label": "Build Installer (Custom Path)",
    "type": "shell",
    "command": "\"C:\\Your\\Path\\ISCC.exe\"",
    "args": ["${workspaceFolder}/ClassBanner/Resources/DesktopBanner-Installer.iss"]
}
```

---

## 🔍 Troubleshooting

### "Task not found" error
- **Solution:** Reload VS Code window
- `Ctrl+Shift+P` → `Developer: Reload Window`

### Build task doesn't run
- **Solution:** Check that you're in the `SystemBanner` folder
- Verify `.vscode/tasks.json` exists

### "ISCC.exe not found"
- **Solution:** Check Inno Setup installation path
- Default: `C:\Program Files (x86)\Inno Setup 6\ISCC.exe`
- Edit `Build-Installer.bat` if path is different

### PowerShell execution policy error
- **Solution:** Run this in PowerShell (as admin):
  ```powershell
  Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
  ```

### Extension recommendations not showing
- **Solution:** Click the Extensions icon (left sidebar)
- Look for "Workspace Recommendations"
- Click "Install All"

---

## 💡 Pro Tips

### Tip 1: Quick Task Access
Create a keyboard shortcut for your most-used task:
1. `Ctrl+Shift+P` → `Preferences: Open Keyboard Shortcuts`
2. Search for task name
3. Add custom keybinding

### Tip 2: Output Panel
View build output:
- `Ctrl+Shift+U` - Opens Output panel
- Select "Tasks" from dropdown

### Tip 3: Multiple Terminals
Run multiple builds simultaneously:
- Each task can have its own terminal
- Use `Ctrl+Shift+\`` to open terminal dropdown

### Tip 4: Task Dependencies
Tasks can depend on each other:
- "Build App Then Compile Installer" runs sequentially
- Modify `dependsOn` in tasks.json

### Tip 5: Watch Mode
For rapid development:
```json
{
    "label": "Watch Build",
    "command": "dotnet",
    "args": ["watch", "build"],
    "isBackground": true
}
```

---

## 📦 Output Locations

After building, find your files here:

```
SystemBanner/
├── ClassBanner/
│   └── bin/Release/net6.0-windows/win-x64/
│       └── DesktopBanner.exe          ← Built application
└── Output/
    └── DesktopBanner-Setup-1.0.exe    ← Installer
```

**Open Output folder from VS Code:**
- `Ctrl+Shift+P` → `Tasks: Run Task` → `Open Output Folder`

---

## 🎨 UI Integration

### Status Bar:

When building, you'll see:
- 🔨 Build progress indicator
- ✅ "Build succeeded" message
- ❌ "Build failed" message (with error count)

### Problems Panel:

Compilation errors appear in:
- `Ctrl+Shift+M` - Opens Problems panel
- Click error to jump to source

### Terminal Panel:

Build output shows in:
- `Ctrl+\`` - Toggle terminal
- Select "Tasks" dropdown to see output

---

## 🚀 All Set!

You now have complete VS Code integration for Desktop Banner!

**To build the installer right now:**

1. Open `SystemBanner` folder in VS Code
2. Press `Ctrl+Shift+B`
3. Press `Enter` (selects default task)
4. Wait for build to complete
5. Installer ready at `Output/DesktopBanner-Setup-1.0.exe`

**Happy building! 🎉**
