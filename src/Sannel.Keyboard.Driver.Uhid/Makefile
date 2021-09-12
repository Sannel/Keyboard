CC=gcc
ArmCC=arm-linux-gnueabi-gcc
Arm64CC=aarch64-linux-gnu-gcc
CFlags=-fPIC

all: build link buildArm linkArm buildArm64 linkArm64 netbuild

build: Sannel.Keyboard.Driver.Uhid.c
	$(CC) -c $(CFlags) Sannel.Keyboard.Driver.Uhid.c -o obj/Sannel.Keyboard.Driver.Uhid.o

link: 
	$(CC) -shared 'obj/Sannel.Keyboard.Driver.Uhid.o' -o 'bin/x64/libuhiddriver.so'

buildArm: Sannel.Keyboard.Driver.Uhid.c
	$(ArmCC) -c $(CFlags) Sannel.Keyboard.Driver.Uhid.c -o obj/Sannel.Keyboard.Driver.Uhid.o

linkArm: 
	$(ArmCC) -shared 'obj/Sannel.Keyboard.Driver.Uhid.o' -o 'bin/arm/libuhiddriver.so'

buildArm64: Sannel.Keyboard.Driver.Uhid.c
	$(Arm64CC) -c $(CFlags) Sannel.Keyboard.Driver.Uhid.c -o obj/Sannel.Keyboard.Driver.Uhid.o

linkArm64: 
	$(Arm64CC) -shared 'obj/Sannel.Keyboard.Driver.Uhid.o' -o 'bin/arm64/libuhiddriver.so'

netbuild:
	dotnet build Sannel.Keyboard.Driver.Uhid.csproj