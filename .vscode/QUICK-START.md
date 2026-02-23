# ⚡ VS Code Quick Start - Desktop Banner

## 🚨 **IMPORTANT: Use BUILD, Not DEBUG!**

### ❌ **DON'T Press F5** (That's for debugging)
### ✅ **DO Press Ctrl+Shift+B** (That's for building)

---

## 🎯 **Step-by-Step: Build Your Installer**

### **1. Open the Project**
```bash
code C:\Users\barto\Documents\repos\SystemBanner
```

### **2. Build the Installer**

**Press: `Ctrl+Shift+B`**

Then you'll see a menu. Select:
```
> Build Installer (Full Build + Compile)
```

**Or just press `Enter`** (it's the default)

### **3. Wait for Build**

You'll see a terminal window open and show:
```
========================================
Desktop Banner - Build and Package
========================================

[1/3] Building DesktopBanner application...
[2/3] Verifying build output...
[3/3] Compiling installer with Inno Setup...

Build Complete!
```

### **4. Find Your Installer**
```
SystemBanner\Output\DesktopBanner-Setup-1.0.exe
```

---

## 🔑 **Keyboard Shortcuts**

| What You Want | Press This | NOT This |
|---------------|------------|----------|
| **Build Installer** | `Ctrl+Shift+B` ✅ | ~~F5~~ ❌ |
| **Show Tasks Menu** | `Ctrl+Shift+P` → "Tasks" ✅ | |
| **Debug Code** | `F5` (only after building) | |
| **Open Terminal** | `Ctrl+\`` | |

---

## 🎯 **Common Mistakes**

### ❌ Mistake 1: Pressing F5
**Problem:** Opens launch.json, asks for build config
**Solution:** Press `Ctrl+Shift+B` instead!

### ❌ Mistake 2: Wrong Folder
**Problem:** Tasks don't appear
**Solution:** Make sure you opened `SystemBanner` folder, not `ClassBanner`

### ❌ Mistake 3: Can't Find Task
**Problem:** Build task not in list
**Solution:** Reload window: `Ctrl+Shift+P` → "Developer: Reload Window"

---

## 📋 **All Available Build Options**

Press `Ctrl+Shift+B` to see all tasks:

1. **Build Installer (Full Build + Compile)** ⭐ DEFAULT
   - Builds app + creates installer
   - **This is what you want!**

2. **Build Application (Release)**
   - Just builds the app, no installer

3. **Compile Installer Only**
   - Quick recompile if app is already built

4. **Build Installer (PowerShell)**
   - Same as #1 but uses PowerShell script

---

## 🐛 **If You Accidentally Pressed F5**

1. **Close the launch.json file** (don't modify it)
2. **Press `Ctrl+Shift+B`** instead
3. **Select: Build Installer (Full Build + Compile)**

---

## 🎨 **Visual Guide**

### What You'll See After Pressing Ctrl+Shift+B:

```
┌─────────────────────────────────────────────┐
│ Select a task to run:                       │
│                                             │
│ > Build Installer (Full Build + Compile)   │ ← Select this!
│   Build Application (Release)              │
│   Build Application (Debug)                │
│   Compile Installer Only                   │
│   ...more tasks...                         │
└─────────────────────────────────────────────┘
```

### What You'll See During Build:

```
TERMINAL (Bottom Panel)
┌─────────────────────────────────────────────┐
│ > Executing task: Build Installer...       │
│                                             │
│ ========================================    │
│ Desktop Banner - Build and Package          │
│ ========================================    │
│                                             │
│ [1/3] Building DesktopBanner application... │
│       Build completed successfully.         │
│                                             │
│ [2/3] Verifying build output...            │
│       DesktopBanner.exe found              │
│                                             │
│ [3/3] Compiling installer with Inno Setup..│
│       Installer compiled successfully!      │
│                                             │
│ ========================================    │
│ Build Complete!                             │
│ ========================================    │
│                                             │
│ Press any key to continue . . .             │
└─────────────────────────────────────────────┘
```

---

## ✅ **Success Checklist**

After building, verify:

- ✅ No errors in terminal
- ✅ "Build Complete!" message shown
- ✅ File exists: `SystemBanner\Output\DesktopBanner-Setup-1.0.exe`
- ✅ File size is reasonable (several MB)

---

## 🚀 **Quick Reference Card**

```
╔════════════════════════════════════════════╗
║  Desktop Banner - VS Code Cheat Sheet     ║
╠════════════════════════════════════════════╣
║                                            ║
║  BUILD INSTALLER:   Ctrl+Shift+B → Enter  ║
║  OPEN TASKS MENU:   Ctrl+Shift+P → Tasks  ║
║  OPEN TERMINAL:     Ctrl+`                ║
║  VIEW PROBLEMS:     Ctrl+Shift+M          ║
║  RELOAD WINDOW:     Ctrl+Shift+P → Reload ║
║                                            ║
║  DON'T USE F5 FOR BUILDING!               ║
║  F5 is for debugging, not building        ║
║                                            ║
╚════════════════════════════════════════════╝
```

---

## 💡 **Remember**

- 🏗️ **Building** = `Ctrl+Shift+B` (creates installer)
- 🐛 **Debugging** = `F5` (runs with breakpoints)
- 📁 **Output** = `SystemBanner\Output\` folder

**You want to BUILD, not DEBUG!**

Press `Ctrl+Shift+B` and you're good to go! 🎉
