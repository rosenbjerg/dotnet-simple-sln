# dotnet-simple-sln

# dotnet-simple-sln

A .NET tool for cleaning up and simplyfing usage of the sln file


## Install
```
dotnet tool install -g dotnet-simple-sln
```


## Examples

## Create solution from yaml specification

Create a sln file from a yaml specification file by running 
```
simple-sln create -i Test.sln.yaml
```
with `Test.sln.yaml` containing:
```yaml
projects:
  - MyProject.csproj
  - AlsoMyProject

directories:
  Test:
    projects:
      - Tests/MyProject.Test/MyProject.Test.csproj
      - Tests/AlsoMyProject.Test
    directories:
      Deep directory:
        projects:
          - DeeplyNested
          - MyFSharpProj/MyFSharpProj.fsproj
```
The resulting `Test.sln` file will be created in the same directory, with the content:

<details><summary>Test.sln content</summary><p>

---
```
Microsoft Visual Studio Solution File, Format Version 12.00

Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "MyProject", "MyProject.csproj", "{00000000-0000-3400-3131-363135373234}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "AlsoMyProject", "AlsoMyProject\AlsoMyProject.csproj", "{00000000-0000-3132-3333-383234393630}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Test", "Test", "{00000000-0000-3700-3938-313232323432}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "MyProject.Test", "Tests\MyProject.Test\MyProject.Test.csproj", "{00000000-0000-382D-3837-393730333730}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "AlsoMyProject.Test", "Tests\AlsoMyProject.Test\AlsoMyProject.Test.csproj", "{00000000-2D00-3431-3136-393530333434}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Deep directory", "Deep directory", "{00000000-0000-3200-3539-313431363031}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "DeeplyNested", "DeeplyNested\DeeplyNested.csproj", "{00000000-0000-3200-3738-353931353830}"
EndProject
Project("{F2A71F9B-5D33-465A-A702-920D77279786}") = "MyFSharpProj", "MyFSharpProj\MyFSharpProj.fsproj", "{00000000-0000-3331-3536-383639373139}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{00000000-0000-3400-3131-363135373234}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-0000-3400-3131-363135373234}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-0000-3400-3131-363135373234}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-0000-3400-3131-363135373234}.Release|Any CPU.Build.0 = Release|Any CPU
		{00000000-0000-3132-3333-383234393630}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-0000-3132-3333-383234393630}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-0000-3132-3333-383234393630}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-0000-3132-3333-383234393630}.Release|Any CPU.Build.0 = Release|Any CPU
		{00000000-0000-382D-3837-393730333730}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-0000-382D-3837-393730333730}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-0000-382D-3837-393730333730}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-0000-382D-3837-393730333730}.Release|Any CPU.Build.0 = Release|Any CPU
		{00000000-2D00-3431-3136-393530333434}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-2D00-3431-3136-393530333434}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-2D00-3431-3136-393530333434}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-2D00-3431-3136-393530333434}.Release|Any CPU.Build.0 = Release|Any CPU
		{00000000-0000-3200-3738-353931353830}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-0000-3200-3738-353931353830}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-0000-3200-3738-353931353830}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-0000-3200-3738-353931353830}.Release|Any CPU.Build.0 = Release|Any CPU
		{00000000-0000-3331-3536-383639373139}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{00000000-0000-3331-3536-383639373139}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{00000000-0000-3331-3536-383639373139}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{00000000-0000-3331-3536-383639373139}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		{00000000-0000-382D-3837-393730333730} = {00000000-0000-3700-3938-313232323432}
		{00000000-2D00-3431-3136-393530333434} = {00000000-0000-3700-3938-313232323432}
		{00000000-0000-3200-3539-313431363031} = {00000000-0000-3700-3938-313232323432}
		{00000000-0000-3200-3738-353931353830} = {00000000-0000-3200-3539-313431363031}
		{00000000-0000-3331-3536-383639373139} = {00000000-0000-3200-3539-313431363031}
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {00000000-0000-392d-3136-313039363930}
	EndGlobalSection
EndGlobal
```
---
</p></details>
</br>
Can optionally scaffold any missing projects with the `-p` argument.

</br>

## Derive yaml specification from sln

Derive a yaml specification file from existing sln file: 
```
simple-sln derive -i Test.sln
```
The resulting yaml file will be named `Test.sln.yaml` and will be written to the same directory the input sln file.

## Cleanup sln file
Clean up a sln file by running:
```
simple-sln cleanup -i Test.sln
```
Which will `derive` a specification and then `create`s a sln file, overwriting the existing one with one cleaned by this tool.

Can optionally output the specification yaml file with the `-s` argument.
