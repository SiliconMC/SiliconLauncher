[Setup]
AppName=Silicon
AppVersion=1.6.1
WizardStyle=modern
DefaultDirName={autopf}\Silicon
DefaultGroupName=Silicon
UninstallDisplayIcon={app}\UninstallSilicon.exe
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
SetupIconFile=InstallerIcon.ico

[Files]
Source: "Silicon.exe"; DestDir: "{app}"
Source: "RestSharp.dll"; DestDir: "{app}"
Source: "Newtonsoft.Json.dll"; DestDir: "{app}"
Source: "AdonisUI.dll"; DestDir: "{app}"
Source: "AdonisUI.ClassicTheme.dll"; DestDir: "{app}"
Source: "System.ValueTuple.dll"; DestDir: "{app}"
Source: "Microsoft.Toolkit.Uwp.Notifications.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\Silicon"; Filename: "{app}\Silicon.exe"
