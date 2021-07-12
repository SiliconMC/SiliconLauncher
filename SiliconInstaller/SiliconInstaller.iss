[Setup]
AppName=Silicon
AppVersion=1.3.0
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
Source: "sxbla.exe"; DestDir: "{app}"
Source: "RestSharp.xml"; DestDir: "{app}"
Source: "RestSharp.dll"; DestDir: "{app}"
Source: "Newtonsoft.Json.xml"; DestDir: "{app}"
Source: "Newtonsoft.Json.dll"; DestDir: "{app}"
Source: "AdonisUI.dll"; DestDir: "{app}"
Source: "AdonisUI.xml"; DestDir: "{app}"
Source: "AdonisUI.ClassicTheme.dll"; DestDir: "{app}"
Source: "AdonisUI.ClassicTheme.xml"; DestDir: "{app}"

[Icons]
Name: "{group}\Silicon"; Filename: "{app}\Silicon.exe"
