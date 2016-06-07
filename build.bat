@echo off
SETLOCAL

".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
".nuget\NuGet.exe" "Install" "xunit.runner.console" "-OutputDirectory" "packages" "-ExcludeVersion"

SET branch=%APPVEYOR_REPO_BRANCH%
SET prnumber=%APPVEYOR_PULL_REQUEST_NUMBER%
SET publish=true
SET prerelease=false
SET version=%APPVEYOR_BUILD_VERSION%
SET semver=%version%

IF "%branch%" NEQ "master" SET prerelease=true
IF "%prnumber%" NEQ "" SET publish=false

IF %prerelease%==true (
  SET semver=%version%-%branch%
)

SET packageversion=%semver%

"packages\FAKE\tools\Fake.exe" "build.fsx" target=createpackages version=%version% semver=%semver% packageversion=%packageversion% projects=Sequin;Sequin.Owin nugetApiKey=%NUGET_API_KEY% %*
