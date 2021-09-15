#!/bin/env - pwsh
Remove-Item -Recurse -Force nuget/bin

cd src/Drivers/Uhid/libuhid
if($IsLinux)
{
	make
}
else
{
	wsl -- make
}

Copy-Item -Recurse bin ../../../../nuget/
cd ../Sannel.Keyboard.Drivers.Uhid
dotnet build -c Release Sannel.Keyboard.Drivers.Uhid.csproj
New-Item -type Directory ../../../../nuget/bin/net5.0
Copy-Item bin/Release/net5.0/Sannel.Keyboard.Drivers.Uhid.* ../../../../nuget/bin/net5.0/
cd ../../../..