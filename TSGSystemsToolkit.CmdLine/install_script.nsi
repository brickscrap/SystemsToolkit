# name of installer file

!include "TextFunc.nsh"
!include "StrFunc.nsh"
!include "FileFunc.nsh"

OutFile "installer\systk_installer_${version}.exe"

Name "Systems Toolkit Installer"


RequestExecutionLevel highest
ShowInstDetails Show

Page InstFiles

Unicode True

InstallDir "$PROGRAMFILES\TSG Systems Toolkit"

# default section start
Section

var /GLOBAL version
${GetParameters} $0
ClearErrors
${GetOptions} $0 "-v" $version
${IfNot} ${Errors}
    !define OUTFILE "systk_installer_${version}.exe"
${EndIf}

; Set to HKCU
EnVar::SetHKCU
; Check for write access
EnVar::Check "NULL" "NULL"
Pop $0
DetailPrint "EnVar::Check write access HKLM returned=|$0|"

; Add to PATH
EnVar::AddValue "PATH" $INSTDIR
Pop $0
DetailPrint "EnVar::AddValue return |$0|"

SetOutPath $INSTDIR

File /r bin\Release\net6.0\publish\*.exe
File /r bin\Release\net6.0\publish\*.dll
File /r bin\Release\net6.0\publish\*.json

WriteUninstaller "uninstall.exe"

WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SystemsToolkit" "DisplayName" "TSG Systems Toolkit"
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SystemsToolkit" "UninstallString" '"$INSTDIR\uninstall.exe"'

# default section end
SectionEnd

Section "Uninstall"

Delete $INSTDIR\uninstaller.exe

Delete $INSTDIR\*.*

RMDir $INSTDIR
EnVar::DeleteValue "PATH" $INSTDIR
Pop $0
DetailPrint "EnVar::DeleteValue returned =|$0|"

SectionEnd