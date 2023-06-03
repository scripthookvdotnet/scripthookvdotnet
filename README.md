Community Script Hook V .NET
============================

[![Build Status](https://github.com/scripthookvdotnet/scripthookvdotnet/actions/workflows/build.yml/badge.svg)](https://github.com/scripthookvdotnet/scripthookvdotnet/actions)

This is an ASI plugin for Grand Theft Auto V, based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

The issues page should be primarily used for bug reports and enhancement ideas. Questions related to GTA V scripting in general are better off in forums dedicated to this purpose, like [gta5-mods.com](https://forums.gta5-mods.com/category/5/general-modding-discussion) or [gtaforums.com](https://gtaforums.com/forum/372-coding/).

## Requirements

* [C++ ScriptHook by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads

Pre-built binaries can be found on the [releases](https://github.com/crosire/scripthookvdotnet/releases) page.
You need to use the ASI file and the DLL files for APIs in an archive of the same version as internal structure can be changed without notices.

## Contributing

You'll need Visual Studio 2019 or higher to open the project file and the [Script Hook V SDK](http://www.dev-c.com/gtav/scripthookv/) extracted into [/sdk](/sdk).

Any contributions to the project are welcomed, it's recommended to use GitHub [pull requests](https://help.github.com/articles/using-pull-requests/).

## License

All the source code except for the Vector, Matrix and Quaternion classes, which are licensed separately, is licensed under the conditions of the [zlib license](LICENSE.txt).
