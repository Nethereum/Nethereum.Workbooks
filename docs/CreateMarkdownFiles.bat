setlocal
set DIR=%~dp0
for /R %DIR% %%a in (*.workbook) do type "%%a" > "%%a.md"
