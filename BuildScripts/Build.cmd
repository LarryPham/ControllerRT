"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" ..\ControllerRT.sln /t:Rebuild /p:Configuration=Release

NuGet.exe pack ..\ControllerRT\ControllerRT.csproj -Verbose -Properties Configuration=Release
NuGet.exe pack ..\ControllerRT.Autofac\ControllerRT.Autofac.csproj -Verbose -Properties Configuration=Release
NuGet.exe pack ..\ControllerRT.Unity\ControllerRT.Unity.csproj -Verbose -Properties Configuration=Release

NuGet.exe pack ..\ControllerPortable\ControllerPortable.csproj -Verbose -Properties Configuration=Release
NuGet.exe pack ..\ControllerPortable.Autofac\ControllerPortable.Autofac.csproj -Verbose -Properties Configuration=Release

NuGet.exe pack ..\ControllerUniversal\ControllerUniversal.csproj -Verbose -Properties Configuration=Release
NuGet.exe pack ..\ControllerUniversal.Autofac\ControllerUniversal.Autofac.csproj -Verbose -Properties Configuration=Release
