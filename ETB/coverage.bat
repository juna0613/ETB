@ECHO OFF
cd /d %~dp0
if exist .\coverage rmdir /Q /S .\coverage
if exist TestResult.xml del /Q /F TestResult.xml
if exist CovResult.xml del /Q /F CovResult.xml

SET NUNIT=.\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe
SET OCOVR=.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe
SET REPO=.\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe

%OCOVR% -target:%NUNIT% -targetargs:"ETB.nunit --output:TestResult.xml" -register:user -filter:"+[*]ETB.* -[*]*.Test*" -output:CovResult.xml

%REPO% "-reports:CovResult.xml" "-targetdir:.\coverage" -sourcedirs:.\ "-assemblyfilters:+ETB*;-*Test*"