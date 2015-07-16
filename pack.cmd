@echo off
pushd "%~dp0"
PowerShell -C "(type StackTraceParser.cs) -replace 'namespace\s+[a-z]+', 'namespace $rootnamespace$' | Out-File -Encoding ascii StackTraceParser.cs.pp" ^
 && call build ^
 && nuget pack StackTraceParser.Source.nuspec
popd
