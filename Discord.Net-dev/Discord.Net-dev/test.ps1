dotnet test test/Discord.Net.Tests/Discord.Net.Tests.csproj -c "Release" --no-build /p:BuildNumber="$Env:BUILD" /p:IsTagBuild="$Env:APPVEYOR_REPO_TAG"
if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode) }