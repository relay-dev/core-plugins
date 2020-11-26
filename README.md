<img src="https://i.imgur.com/ptZKdWs.png" alt="Core Plugins" height="100" width="100">

# Core.Plugins

[![Build status](https://ci.appveyor.com/api/projects/status/uy3l50i1p1gxu1pe/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/core-plugins/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/relay.core.plugins.svg)](https://www.nuget.org/packages/Relay.Core.Plugins/)
[![MyGet Release](https://img.shields.io/myget/relay-dev/v/Relay.Core.Plugins.svg)](https://www.myget.org/feed/relay-dev/package/nuget/Relay.Core.Plugins)
[![License](https://img.shields.io/github/license/relay-dev/core-plugins.svg)](https://github.com/relay-dev/core-plugins/blob/master/LICENSE)

## Introduction

Plugin implementations available for the Core framework

Core.Plugins is a set of base class libraries written using .NET Standard. They serve as implementations to the abstractions from the [Core](https://github.com/relay-dev/core) NuGet package. Core.Plugins is built and designed to be consumed as a plugin model framework.

Core.Plugins is published to the main public NuGet feed (see section [Installation](#installation)).

*Update: .NET Core/.NET 5 has made the original intention of this project obsolete. Implementations of services like ILogger, IConfiguration, IServiceProvider, etc have been making their way out of this codebase. There is other useful functionality here so the package will stay.

## Getting Started

<a name="installation"></a>

### Installation

Follow the instructions below to install this NuGet package into your project:

#### .NET Core CLI

```sh
dotnet add package Relay.Core.Plugins
```

#### Package Manager Console

```sh
Install-Package Relay.Core.Plugins
```

### Latest releases

Latest releases and package version history can be found on [NuGet](https://www.nuget.org/packages/Relay.Core.Plugins/).

## Build and Test

Follow the instructions below to build and test this project:

### Build

#### .NET Core CLI

```sh
dotnet build
```

### Test

#### .NET Core CLI

```sh
dotnet test
```

## Contribute

When contributing to this repository, please first discuss the change you wish to make via issue,
email, or any other method with the owners of this repository before making a change.