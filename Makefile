# Requirements:
#  - MSBuild (found through vswhere.exe)
#  - dotnet (ensure dotnet.exe is in %PATH%)

CONFIG = Release
VSVERSION = 2022

VSWHERE = "${ProgramFiles(x86)}/Microsoft Visual Studio/Installer/vswhere.exe"
MSBUILD = "$(shell $(VSWHERE) -find msbuild | grep $(VSVERSION) | head -n 1)/Current/Bin/MSBuild.exe"

all:
	$(MSBUILD) Stfu.sln -t:clean -p:configuration=$(CONFIG)
	$(MSBUILD) Stfu.sln -t:build -p:configuration=$(CONFIG)
	dotnet pack --no-build -c $(CONFIG) Stfu/Stfu.csproj

clean:
	$(MSBUILD) Stfu.sln -t:clean -p:configuration=$(CONFIG)
	rm -f *.nupkg

