[![Build status](https://ci.appveyor.com/api/projects/status/uy3l50i1p1gxu1pe/branch/master?svg=true)](https://ci.appveyor.com/project/sferguson82/core-plugins/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/relay.core.plugins.svg)](https://www.nuget.org/packages/Relay.Core.Plugins/)
[![MyGet Release](https://img.shields.io/myget/relay-dev/v/Relay.Core.Plugins.svg)](https://www.myget.org/feed/relay-dev/package/nuget/Relay.Core.Plugins)
[![License](https://img.shields.io/github/license/relay-dev/core-plugins.svg)](https://github.com/relay-dev/core-plugins/blob/master/LICENSE)
[![Awesome](https://cdn.rawgit.com/sindresorhus/awesome/d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/relay-dev/core-plugins)

# Core.Plugins

Plugins available for the Core framework


## Installation

Install the [Relay.Core.Plugins NuGet Package](https://www.nuget.org/packages/Relay.Core.Plugins).

### Package Manager Console

```
Install-Package Relay.Core.Plugins
```

### .NET Core CLI

```
dotnet add package Relay.Core.Plugins
```


## Folder structure
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

## .gitignore
```
[Oo]bj/
[Bb]in/
.nuget/
_ReSharper.*
packages/
artifacts/
*.user
*.suo
*.userprefs
*DS_Store
*.sln.ide
```

