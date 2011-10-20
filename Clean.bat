rem Nettoyage des fichiers inutiles, avant archivage
echo off 
cls
del /S /Q Clipboards\bin
del /S /Q Clipboards\obj
del /S /Q Setup\Debug
del /S /Q Setup64\Debug
del /S /Q Setup\Release
del /S /Q Setup64\Release
