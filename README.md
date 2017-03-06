# .NET framework sniffer
Console app that processes assemblies in a given directory at parses out CLR and .NET Framework targeted version.

### Purpose

The reason I put this together was while converting (to .NET Core) a large .NET Framework project I needed more information on the Framework Targets of the assemblies contained in the project so that we could make decisions / get a better picture on converting the solution to .NET Core.

### Usage

1. Build your project (to make sure to generate all of the modules (nuget, bin, GAC, etc...) 

2. `FrameworkSniffer.exe 'c:\path-to-project'`

### Options

**Default** (pass in a path)

`FrameworkSniffer.exe 'c:\path-to-project'`







