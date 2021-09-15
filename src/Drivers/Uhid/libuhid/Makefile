CC=gcc
ArmCC=arm-linux-gnueabi-gcc
Arm64CC=aarch64-linux-gnu-gcc
CFlags=-fPIC

all: clean build link buildArm linkArm buildArm64 linkArm64

x64: clean build link

arm: clean buildArm linkArm

arm64: clean buildArm64 linkArm64

clean:
	rm -rf bin
	rm -rf obj
	mkdir obj
	mkdir -p bin/x64
	mkdir -p bin/arm
	mkdir -p bin/arm64

build: *.c
	$(CC) -c $(CFlags) *.c -o obj/libuhid-x64.o

link: 
	$(CC) -shared 'obj/libuhid-x64.o' -o 'bin/x64/libuhid.so'

buildArm: *.c
	$(ArmCC) -c $(CFlags) *.c -o obj/libuhid-arm.o

linkArm: 
	$(ArmCC) -shared 'obj/libuhid-arm.o' -o 'bin/arm/libuhid.so'

buildArm64: *.c
	$(Arm64CC) -c $(CFlags) *.c -o obj/libuhid-arm64.o

linkArm64: 
	$(Arm64CC) -shared 'obj/libuhid-arm64.o' -o 'bin/arm64/libuhid.so'
