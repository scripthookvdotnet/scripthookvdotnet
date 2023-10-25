Community Script Hook V .NET (SHVDN)
============================

[![NuGet](https://img.shields.io/nuget/v/scripthookvdotnet3.svg?label=nuget%20%28v3%29)](https://www.nuget.org/packages/scripthookvdotnet3)
[![Build Status](https://github.com/scripthookvdotnet/scripthookvdotnet/actions/workflows/build.yml/badge.svg)](https://github.com/scripthookvdotnet/scripthookvdotnet/actions)
[![License](https://img.shields.io/github/license/scripthookvdotnet/scripthookvdotnet?color=%232A922A)](LICENSE.md)

This is an ASI plugin for Grand Theft Auto V, based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

The issues page should be primarily used for bug reports and focused enhancement ideas. Questions related to GTA V scripting in general are better off in [Discussions page](https://github.com/scripthookvdotnet/scripthookvdotnet/discussions/categories/q-a) or forums dedicated to this purpose, like [gta5-mods.com](https://forums.gta5-mods.com/category/5/general-modding-discussion).

## Requirements

* [C++ ScriptHook by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads
The stable builds can be downloaded from the [Releases](https://github.com/scripthookvdotnet/scripthookvdotnet/releases) page.
You need to use the ASI file and the DLL files for APIs in an archive of the same version as internal structure can be changed without notices.

For newer builds, check out the [Nightly Builds](https://github.com/scripthookvdotnet/scripthookvdotnet/actions/workflows/build.yml?query=branch%3Amain). Click some build you want to try and download the artifact zip file from the Artifacts section.
You'll need your GitHub account to download nightly artifacts at the moment, but nightly releases will also be released via GitHub releases in the future so your GitHub account is not required to download nightly artifacts (at least for the latest one).

For script developers, please note that new APIs included in new nightly builds but not included in any stable versions are subject to change without notice, so it is not advisable to use any of them for public builds of your scripts.

## Installation
* Extract all the files except for LICENSE.txt, README.txt, and all of xml files from the zip file into your game folder.
    * The XML files are provided solely as API documentations for script developers. The .pdb files are not mandatory to run ScriptHookVDotNet as they are provided for easier diagnosing.

## Contributing

You'll need Visual Studio 2019 or higher to open the project file and the [Script Hook V SDK](http://www.dev-c.com/gtav/scripthookv/) extracted into [/sdk](/sdk).

Any contributions to the project are welcomed, it's recommended to use GitHub [pull requests](https://help.github.com/articles/using-pull-requests/).

## License

All the source code except for the Vector, Matrix and Quaternion classes, which are licensed separately, is licensed under the conditions of the [zlib license](LICENSE.txt).
