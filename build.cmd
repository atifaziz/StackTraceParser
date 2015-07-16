@echo off
setlocal
pushd "%~dp0"
nuget restore && call :build Debug && call :build Release
popd
goto :EOF

:build
msbuild /p:Configuration=%1 /v:m %2 %3 %4 %5 %6 %7 %8 %9
goto :EOF
