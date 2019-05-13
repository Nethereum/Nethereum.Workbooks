setlocal
set DIR=%~dp0
for /R %DIR% %%f in (*.workbook) do (type "%%f" > "%%~dpnf.md")
