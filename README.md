<img src="https://github.com/relay-dev/core-plugins/raw/master/resources/icon.png?raw=true" alt="Core Plugins" height="200" width="200">

# Core.Plugins

[![Build status](https://ci.appveyor.com/api/projects/status/uy3l50i1p1gxu1pe/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/core-plugins/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/relay.core.plugins.svg)](https://www.nuget.org/packages/Relay.Core.Plugins/)
[![MyGet Release](https://img.shields.io/myget/relay-dev/v/Relay.Core.Plugins.svg)](https://www.myget.org/feed/relay-dev/package/nuget/Relay.Core.Plugins)
[![License](https://img.shields.io/github/license/relay-dev/core-plugins.svg)](https://github.com/relay-dev/core-plugins/blob/master/LICENSE)

Plugin implementations available for the Core framework

> <sup>Core.Plugins.* is a set of base class libraries written on .NET Core 2.2. The [Core](https://github.com/relay-dev/core) library can be considered it's base; this NuGet package consists only of implementations of [Core](https://github.com/relay-dev/core) abstractions.</sup>
>
> <sup>The full list of available Plugins can be found on [NuGet](https://www.nuget.org/profiles/Relay).
>
> <sup>Core.Plugins is delivered to consuming applications by way of the main public NuGet feed (see section [Installation](#installation)).</sup>
>
> <sup>A formal architectural diagram and documention coming soon.</sup>

<br />

<img src="https://github.com/relay-dev/core-plugins/raw/master/resources/break.jpg?raw=true">

<br />

<div id="installation"></div>

### Installation

Here's how you can install the Relay.Core.Plugins application [NuGet Package](https://www.nuget.org/packages/Relay.Core.Plugins):

<br />

> #### *.NET Core CLI*
> 
> ```
> dotnet add package Relay.Core.Plugins
> ```
>
> #### *Package Manager Console*
> 
> ```
> Install-Package Relay.Core.Plugins
> ```

<br />

### Development Environment

While you do not need to reflect my exact development environment on your machine, it's sometimes useful to provide potential Core developers with a list of the exact products and versions I currently use to develop this product:

<br />

Type | Version | Requires Local Setup?
--- | --- | --- 
Operating System | [Windows 10 Home](https://www.microsoft.com/en-us/windows) | <img src="https://github.com/relay-dev/core/raw/master/resources/yes.png?raw=true" alt="Yes" height="20" width="20">
IDE | [Visual Studio Community 2017](https://visualstudio.microsoft.com/downloads/) | <img src="https://github.com/relay-dev/core/raw/master/resources/yes.png?raw=true" alt="Yes" height="20" width="20">
Database | [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) | <img src="https://github.com/relay-dev/core/raw/master/resources/yes.png?raw=true" alt="Yes" height="20" width="20">
Version Control (git client) | [TortoiseGit](https://tortoisegit.org/) or [GitKraken](https://www.gitkraken.com/download) or [Sourcetree](https://www.sourcetreeapp.com/) | <img src="https://github.com/relay-dev/core/raw/master/resources/yes.png?raw=true" alt="Yes" height="20" width="20">
Continous Integration | [AppVeyor](https://ci.appveyor.com/projects) | <img src="https://github.com/relay-dev/core/raw/master/resources/no.png?raw=true" alt="No" height="20" width="20">
Host | [Microsoft Azure](https://azure.microsoft.com/) | <img src="https://github.com/relay-dev/core/raw/master/resources/no.png?raw=true" alt="No" height="20" width="20">
Containers | [Docker](https://www.docker.com/) | <img src="https://github.com/relay-dev/core/raw/master/resources/no.png?raw=true" alt="No" height="20" width="20">
NuGet Deployment | [AppVeyor](https://ci.appveyor.com/projects) | <img src="https://github.com/relay-dev/core/raw/master/resources/no.png?raw=true" alt="No" height="20" width="20">

<br />

### Package Sources

Here are the NuGet Package sources I have configured in Visual Studio and use to develop this product:

<br />

Name | Source | Required?
--- | --- | --- 
NuGet (v3) | [https://api.nuget.org/v3/index.json](https://api.nuget.org/v3/index.json) | <img src="https://github.com/relay-dev/core/raw/master/resources/yes.png?raw=true" alt="Yes" height="20" width="20">
MyGet (Relay) | [https://www.myget.org/F/relay-dev/api/v2/](https://www.myget.org/F/relay-dev/api/v2/) | <img src="https://github.com/relay-dev/core/raw/master/resources/no.png?raw=true" alt="No" height="20" width="20"> (pre-release only)

<br />

### Folder structure

The folder structure has a definite form, which should be mainained. The standard was derived by the great David Fowler [here](https://gist.github.com/davidfowl/ed7564297c61fe9ab814).

<br />

```
$/
  artifacts/
  build/
  docs/
  lib/
  packages/
  resources/
  samples/
  src/
  tests/
  .editorconfig
  .gitignore
  .gitattributes
  build.cmd
  build.sh
  LICENSE
  NuGet.Config
  README.md
  {solution}.sln
```


- `src` - Main projects (the product code)
- `tests` - Test projects
- `docs` - Documentation stuff, markdown files, help files etc.
- `samples` (optional) - Sample projects
- `lib` - Libraries that your application depends on, which you cannot obtian by way of NuGet
- `artifacts` - Build outputs go here. Doing a build.cmd/build.sh generates artifacts here (nupkgs, dlls, pdbs, etc.)
- `packages` - NuGet packages
- `resources` - Misc. files referenced by the application (NuGet package image, etc.)
- `build` - Build customizations (custom msbuild files/psake/fake/albacore/etc) scripts
- `build.cmd` - Bootstrap the build for windows
- `build.sh` - Bootstrap the build for *nix
- `global.json` - ASP.NET vNext only
