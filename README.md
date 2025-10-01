Community Script Hook V .NET (SHVDN)
============================

[![NuGet](https://img.shields.io/nuget/v/scripthookvdotnet3.svg?label=nuget%20%28v3%29)](https://www.nuget.org/packages/scripthookvdotnet3)
[![Nightly Build Status](https://github.com/scripthookvdotnet/scripthookvdotnet/actions/workflows/nightly-release.yml/badge.svg)](https://github.com/scripthookvdotnet/scripthookvdotnet/actions/workflows/nightly-release.yml)
[![License](https://img.shields.io/github/license/scripthookvdotnet/scripthookvdotnet?color=%232A922A)](LICENSE.md)

This is an ASI plugin for Grand Theft Auto V, based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

The issues page should be primarily used for bug reports and focused enhancement ideas. Questions related to GTA V scripting in general, are better off on the [Discussions page](https://github.com/scripthookvdotnet/scripthookvdotnet/discussions/categories/q-a) or in forums dedicated to this purpose.

## Requirements

* [C++ Script Hook V by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads
The stable builds can be downloaded from the [Releases](https://github.com/scripthookvdotnet/scripthookvdotnet/releases) page.
You need to use the ASI file and the DLL files for APIs in an archive of the same version as the internal structure can be changed without notice.  
**If you are using the game version v1.0.3258.0 or later, use the nightly version `v3.6.0-nightly.89` or later (see below for nightly version details), or downgrade the game to v1.0.3179.0 or earlier! `v3.6.0` and `v3.5.1` have [a compatibility issue with the game version v1.0.3258.0 and later game versions](https://github.com/scripthookvdotnet/scripthookvdotnet/issues/1451)!**

For newer builds, check out the [Nightly Builds](https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases). You don't have to sign in to GitHub to download nightly releases.  
Here are some of the notes you should be aware of when using a nightly version (from v3.6.0):
* The default API version for raw scripts is changed from v2 to v3.
    * **For Users**: If you have raw scripts (`.cs` and `.vb` scripts) without an API version notation by file name, you will need to specify in nightly versions. You can specify an API version by adding a dot and a version number right before the extension name (e.g. `Script.cs` to `Script.2.cs`).
* The .ini settings are changed. You should use the .ini file that comes from a nightly release. SHVDN does not add missing settings currently.
* Warning messages are added for scripts built against the v2 API, which is not as maintained as the v3 one and will not have any new features. This does not mean the v2 API will not be even receiving compatibility fixes for new game updates in the *near* future. These messages are printed to urge users to ask the script authors to migrate to the v3 API.
* Some scripts *may* not be working that rely on the main thread of the game process (for game logic).
    * This is because we had to use a dedicated thread other than the main thread to avoid using ScriptHookV's fiber, so users won't have crucial compatibility problems with RAGE Plugin Hook and C++ scripts that use try-catch blocks. Although we are still searching for how to have SHVDN tick in the main game thread by hooking a function in the game process, we have not found one.

For script developers, please note that new APIs included in new nightly builds but not included in any stable versions are subject to change without notice, so it is not advisable to use any of them for public/production builds of your scripts.
In other words, **you should build your scripts against stable versions but not nightly versions unless you build your scripts for testing some of the new APIs added in nightly versions, so you won't accidentally use anything not available in any stable versions. Building scripts against nightly versions may make scripts not work as intended in SHVDN versions different from the versions they are built against! No compatibility support will be provided for nightly-only features!**

## Installation
* Extract all the files in the root folder from the zip file into your game folder except for `README.txt` and the 2 folders.
    * The XML files in the `Docs` folder are provided solely as API documentation for script developers.
* When you update, **always make sure to update at least all the asi and the .dll files together! No support will be provided if you cherry-pick them and that causes problems!** The following files are the ones you must update together:
    * `ScriptHookVDotNet.asi`
    * `ScriptHookVDotNet2.dll`
    * `ScriptHookVDotNet3.dll`

## Contributing

You'll need Visual Studio 2019 or higher to open the project file and the [Script Hook V SDK](http://www.dev-c.com/gtav/scripthookv/) extracted into [/sdk](/sdk).

Any contributions to the project are welcomed, it's recommended to use GitHub [pull requests](https://help.github.com/articles/using-pull-requests/).

## License

ScriptHookVDotNet is primarily distributed under the terms of the zlib license.
See [LICENSE](LICENSE.txt) and [COPYRIGHT](COPYRIGHT.md) for details.
