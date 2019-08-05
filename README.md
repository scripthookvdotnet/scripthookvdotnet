Community Script Hook V .NET
============================

[![Build Status](https://ci.appveyor.com/api/projects/status/github/crosire/scripthookvdotnet?branch=master&svg=true)](https://ci.appveyor.com/project/crosire/scripthookvdotnet)
[![Join the chat at https://gitter.im/crosire/scripthookvdotnet](https://img.shields.io/badge/gitter-join%20chat-1dce73.svg)](https://gitter.im/crosire/scripthookvdotnet?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This is an ASI plugin for Grand Theft Auto V, based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

Feel free to ask questions related to GTA V scripting or this project in the public [Gitter chat room](https://gitter.im/crosire/scripthookvdotnet). The issues page should be primarily used for bug reports and enhancement ideas.

## Requirements

* [C++ ScriptHook by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads

Pre-built binaries can be found on the [releases](https://github.com/crosire/scripthookvdotnet/releases) page.

## Contributing

You'll need Visual Studio 2017 or higher to open the project file and the [Script Hook V SDK](http://www.dev-c.com/gtav/scripthookv/) extracted into "[/sdk](/sdk)".

Any contributions to the project are welcomed, it's recommended to use GitHub [pull requests](https://help.github.com/articles/using-pull-requests/).

## License

All the source code except for the Vector, Matrix and Quaternion classes, which are licensed separately, is licensed under the conditions of the [zlib license](LICENSE.txt).
