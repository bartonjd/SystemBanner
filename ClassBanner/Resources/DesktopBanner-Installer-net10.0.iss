; DesktopBanner Installer Script
; Desktop Security Banner Tool
; Supports x64 Windows 10/11

#define MyAppName "Desktop Banner"
#define MyAppVersion "1.0"
#define MyAppPublisher "Josh Barton"
#define MyAppExeName "DesktopBanner.exe"
#define MyAppURL "https://github.com/bartonjd/SystemBanner"

[Setup]
AppId={{B8F3D9E2-7A4C-4E1F-9B2D-5C8E6A1F3D4E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\DesktopBanner
DefaultGroupName=Desktop Banner
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE.txt
OutputDir=.\Output
OutputBaseFilename=DesktopBanner-Setup-{#MyAppVersion}
; SetupIconFile=..\Snap.png
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
WizardImageFile=installer-bg.bmp
; WizardSmallImageFile=..\Snap.png
PrivilegesRequired=admin
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
DisableDirPage=yes
DisableWelcomePage=no
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription=Desktop Security Banner Tool for Windows Desktop
VersionInfoProductName={#MyAppName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "autostart"; Description: "Start Desktop Banner automatically with Windows"; GroupDescription: "Startup Options:"; Flags: checkedonce
Name: "installpolicy"; Description: "Install Group Policy ADMX templates (for centralized or local management)"; GroupDescription: "Additional Options:"

[Files]
; Main application files
Source: "..\bin\Release\net10.0-windows\win-x64\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\net10.0-windows\win-x64\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "..\bin\Release\net10.0-windows\win-x64\*.json"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "..\bin\Release\net10.0-windows\win-x64\*.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\net10.0-windows\win-x64\*.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; ADMX/ADML files for Group Policy
Source: "DesktopBanner.admx"; DestDir: "{win}\PolicyDefinitions"; Flags: ignoreversion; Tasks: installpolicy
Source: "DesktopBanner.adml"; DestDir: "{win}\PolicyDefinitions\en-US"; Flags: ignoreversion; Tasks: installpolicy
; Documentation
Source: "..\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Configure Desktop Banner"; Filename: "gpedit.msc"; Comment: "Configure banner via Group Policy Editor"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Registry]
; Default registry values for DesktopBanner (HKLM\SOFTWARE\DesktopBanner)
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "LeftDisplayText"; ValueData: "@HOST"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "CenterDisplayText"; ValueData: "UNCLASSIFIED"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "RightDisplayText"; ValueData: "@USER"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: dword; ValueName: "ShowOnBottom"; ValueData: "0"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: dword; ValueName: "Opacity"; ValueData: "100"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: string; ValueName: "BackgroundColor"; ValueData: "#008000"; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: "SOFTWARE\DesktopBanner"; ValueType: dword; ValueName: "DisplayMode"; ValueData: "{code:GetDisplayModeValue}"; Flags: createvalueifdoesntexist

; Autostart registry entry
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "DesktopBanner"; ValueData: """{app}\{#MyAppExeName}"""; Flags: uninsdeletevalue; Tasks: autostart

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
var
  BannerStylePage: TWizardPage;
  OverlayRadioButton: TNewRadioButton;
  RolloverRadioButton: TNewRadioButton;
  StaticRadioButton: TNewRadioButton;
  OverlayDescLabel: TLabel;
  RolloverDescLabel: TLabel;
  StaticDescLabel: TLabel;

procedure InitializeWizard;
begin
  // Create custom page for banner style selection
  BannerStylePage := CreateCustomPage(wpSelectTasks, 'Select Banner Style', 
    'Choose how the desktop banner should behave');

  // Overlay Radio Button
  OverlayRadioButton := TNewRadioButton.Create(WizardForm);
  OverlayRadioButton.Parent := BannerStylePage.Surface;
  OverlayRadioButton.Caption := 'Overlay';
  OverlayRadioButton.Top := 20;
  OverlayRadioButton.Width := BannerStylePage.SurfaceWidth;
  OverlayRadioButton.Font.Style := [fsBold];
  OverlayRadioButton.Font.Size := 9;
  OverlayRadioButton.Checked := True;

  // Overlay Description
  OverlayDescLabel := TLabel.Create(WizardForm);
  OverlayDescLabel.Parent := BannerStylePage.Surface;
  OverlayDescLabel.Caption := 'Banner displays as an overlay without reserving screen space. Applications can draw beneath it.';
  OverlayDescLabel.Top := OverlayRadioButton.Top + OverlayRadioButton.Height + 5;
  OverlayDescLabel.Left := 20;
  OverlayDescLabel.Width := BannerStylePage.SurfaceWidth - 20;
  OverlayDescLabel.WordWrap := True;
  OverlayDescLabel.AutoSize := True;

  // Rollover Radio Button
  RolloverRadioButton := TNewRadioButton.Create(WizardForm);
  RolloverRadioButton.Parent := BannerStylePage.Surface;
  RolloverRadioButton.Caption := 'Rollover';
  RolloverRadioButton.Top := OverlayDescLabel.Top + OverlayDescLabel.Height + 20;
  RolloverRadioButton.Width := BannerStylePage.SurfaceWidth;
  RolloverRadioButton.Font.Style := [fsBold];
  RolloverRadioButton.Font.Size := 9;

  // Rollover Description
  RolloverDescLabel := TLabel.Create(WizardForm);
  RolloverDescLabel.Parent := BannerStylePage.Surface;
  RolloverDescLabel.Caption := 'Banner hides automatically when you move the mouse over it. Useful for accessing items near the screen edge.';
  RolloverDescLabel.Top := RolloverRadioButton.Top + RolloverRadioButton.Height + 5;
  RolloverDescLabel.Left := 20;
  RolloverDescLabel.Width := BannerStylePage.SurfaceWidth - 20;
  RolloverDescLabel.WordWrap := True;
  RolloverDescLabel.AutoSize := True;

  // Static Radio Button
  StaticRadioButton := TNewRadioButton.Create(WizardForm);
  StaticRadioButton.Parent := BannerStylePage.Surface;
  StaticRadioButton.Caption := 'Static (AppBar)';
  StaticRadioButton.Top := RolloverDescLabel.Top + RolloverDescLabel.Height + 20;
  StaticRadioButton.Width := BannerStylePage.SurfaceWidth;
  StaticRadioButton.Font.Style := [fsBold];
  StaticRadioButton.Font.Size := 9;

  // Static Description
  StaticDescLabel := TLabel.Create(WizardForm);
  StaticDescLabel.Parent := BannerStylePage.Surface;
  StaticDescLabel.Caption := 'Banner reserves screen space using Windows AppBar API. Applications cannot overlap the banner area. Most secure option for ensuring banner visibility.';
  StaticDescLabel.Top := StaticRadioButton.Top + StaticRadioButton.Height + 5;
  StaticDescLabel.Left := 20;
  StaticDescLabel.Width := BannerStylePage.SurfaceWidth - 20;
  StaticDescLabel.WordWrap := True;
  StaticDescLabel.AutoSize := True;
end;

function GetDisplayModeValue(Param: String): String;
begin
  // Return the DisplayMode value based on user selection
  // 0 = Overlay, 1 = Rollover, 2 = Static
  if OverlayRadioButton.Checked then
    Result := '0'
  else if RolloverRadioButton.Checked then
    Result := '1'
  else
    Result := '2';
end;

function GetBannerStyleName: String;
begin
  if OverlayRadioButton.Checked then
    Result := 'Overlay'
  else if RolloverRadioButton.Checked then
    Result := 'Rollover'
  else
    Result := 'Static';
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Log installation details
    Log('Desktop Banner installed successfully');
    Log('Selected banner style: ' + GetBannerStyleName);
    Log('DisplayMode registry value: ' + GetDisplayModeValue(''));
  end;
end;
