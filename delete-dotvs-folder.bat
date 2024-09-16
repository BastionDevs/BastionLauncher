@echo off
title Delete .vs Folder
echo This tool will delete all .vs project folders.
pause
echo Deleting...
rmdir BastionLauncher\.vs /s /q
echo.
echo Done!
pause
exit