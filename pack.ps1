Remove-Item .\packaging\*.nupkg
dotnet pack .\src\AgileSwitch -c Release -o .\packaging