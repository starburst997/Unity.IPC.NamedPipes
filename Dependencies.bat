@Echo off
Pushd "%~dp0"

echo Makes sure to install ".NET Core 5 SDK" https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net50

"C:\Program Files\dotnet\dotnet.exe" build "Dependencies\CSNamedPipes\IpcLib\IpcLib.csproj" -c Release

REM Copy DLLs

copy Dependencies\CSNamedPipes\IpcLib\bin\Release\netstandard2.0\IpcLib.dll Unity.IPC\Runtime\Plugins\IpcLib.dll
copy Dependencies\CSNamedPipes\IpcLib\bin\Release\netstandard2.0\IpcLib.pdb Unity.IPC\Runtime\Plugins\IpcLib.pdb

@pause
popd