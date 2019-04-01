<img src="https://github.com/relay-dev/core-plugins/raw/master/resources/icon.png?raw=true" alt="Core Plugins" height="200" width="200">

# Core.Plugins

[![Build status](https://ci.appveyor.com/api/projects/status/uy3l50i1p1gxu1pe/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/core/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/relay.core.plugins.svg)](https://www.nuget.org/packages/Relay.Core.Plugins/)
[![MyGet Release](https://img.shields.io/myget/relay-dev/v/Relay.Core.Plugins.svg)](https://www.myget.org/feed/relay-dev/package/nuget/Relay.Core.Plugins)
[![License](https://img.shields.io/github/license/relay-dev/core-plugins.svg)](https://github.com/relay-dev/core-plugins/blob/master/LICENSE)

Plugin implementations available for the Core framework, located here (github: [core-plugins](https://github.com/relay-dev/core-plugins) // nuget: [Relay.Core.Plugins](https://www.nuget.org/packages/Relay.Core.Plugins/))

> <sup>Core.Plugins.* is a set of base class libraries written on .NET Core 2.2. The [Relay.Core.Plugins](https://github.com/relay-dev/core-plugins) can be considered it's base; the NuGet package consist of implementations of Core abstractions.</sup>
> 
> <sup>The full NuGet library of all Products I've produced is [here](https://www.nuget.org/profiles/Relay).
>
> <sup>Core and it's Plugins are delivered to consuming applications by way of the main public NuGet feed (see section [Installation](#installation)). Implementations of the Core stubs are manifested in the form of Core "Plugins", which can be found here (github: [core-plugins](https://github.com/relay-dev/core-plugins) // nuget: [Relay.Core.Plugins](https://www.nuget.org/packages/Relay.Core.Plugins/))</sup>
> 
> <sup>All APIs are fully documented in a .chm file located [here](https://github.com/relay-dev/core/raw/master/docs/Core%20API%20Documentation.chm). A formal architectural diagram and documention coming soon.</sup>

<br />

<img src="https://github.com/relay-dev/core-plugins/raw/master/resources/break.jpg?raw=true">

<br />

<div id="installation"></div>

### *Installation*

Here's how you can install the Relay.Core.Plugins application [NuGet Package](https://www.nuget.org/packages/Relay.Core):

> ### *Package Manager Console*
> 
> ```
> Install-Package Relay.Core.Plugins
> ```
> 
> ### *.NET Core CLI*
> 
> ```
> dotnet add package Relay.Core.Plugins
> ```


### Folder structure
<sup>(https://gist.github.com/davidfowl/ed7564297c61fe9ab814)</sup>

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
