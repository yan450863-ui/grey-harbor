@echo off
setlocal
set "GAME_EXE=%~dp0Build\GreyHarbor.exe"
if not exist "%GAME_EXE%" (
  echo Build not found. Download and extract the Windows release, or build the Unity project first.
  pause
  exit /b 1
)
start "" "%GAME_EXE%" -force-d3d11 -screen-fullscreen 0 -screen-width 1600 -screen-height 900
endlocal
