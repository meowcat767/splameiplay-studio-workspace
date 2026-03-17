
![SplameiPlay Studio Banner - Banner](https://raw.githubusercontent.com/splameiplay/splameiplay-studio/refs/heads/main/Images/Misc/GitHub%20Banner.png)

<h1 align="center">
	<span align="center">SplameiPlay Studio
</h1>
<p align="center">
	<img src="https://img.shields.io/github/check-runs/splameiplay/splameiplay-studio/main">
	<img src="https://www.codefactor.io/repository/github/splameiplay/splameiplay-studio/badge">
	<img src="https://img.shields.io/github/issues/splameiplay/splameiplay-studio">
	<img src="https://img.shields.io/github/license/splameiplay/splameiplay-studio">
	<img src="https://img.shields.io/github/repo-size/splameiplay/splameiplay-studio">
</p>
<p align="center">
	<span align="center">An app designed to make editing SplameiPlay Files and developing for SplameiPlay easier</span>
</p>

> [!NOTE]  
>  SplameiPlay Studio is intended for project developers, distributors and system administrators and is not for public use.

## Screenshots

> SplameiPlay Studio is still under early development. We'll add screenshots once it's closer to a release

## Requirements
- Windows 7 (SP1) or later
  - Windows 7, 8 and 8.1 may require additional apps to be installed to work
- .NET Framework 4.7.2 (pre-installed on modern Windows)
- An active internet connection

## Installation

### Via SplameiPlay (Mainline)

SplameiPlay Studio (should) be a native project to SplameiPlay. Because of this, you should be able to go into the store page, find 'SplameiPlay Studio' and install like you would with other projects.

> [!NOTE]  
>  As of writing, SplameiPlay Studio is not available on the Stable and Beta channels and can only be natively installed on the Alpha channel

### Via an installer file

> [!NOTE]
> The official installer file using the 2.0 (SplameiPlay Flow 1.2) server syntax. This means you cannot currently use the installer file on SplameiPlay Lite or Beta + Stable SplameiPlay Mainline builds

1. Download the latest installer file from the [release page](https://github.com/splameiplay/splameiplay-studio/releases) (It will end in `.spinstaller`)
2. Open the downloaded file and accept the add prompt
3. After adding, you should see it in the main window (Lite) or the home page (Mainline after refreshing). You can now install it the same way you normally would

### Portable install

1. Download the latest portable release from the [release page](https://github.com/splameiplay/splameiplay-studio/releases) (It will end in `.zip`)
2. Extract the downloaded file
3. Run the file named `SplameiPlay Studio.exe`

> It's not recommended to use this method because SplameiPlay Studio will not be automatically updated which can result in you making outdated SplameiPlay files

## SplameiPlay Studio's documentation

You can access SplameiPlay Studio's official documentation on it's Splamei Docs page found [here](https://docs.veemo.uk/splameiplay-studio).

## Building from source

This section presumes you have the .NET (Framework) SDKs, Windows SDKs and Git installed and a basic knowledge of how to use them.

> [!NOTE]  
> When building from source, it will lack the digital signatures used by official builds and is subject to the branding rights section

### Visual Studio

1. Clone the repo in Visual Studio by pressing 'Clone a repository' and pasting the repo's URL (`https://github.com/splameiplay/splameiplay-studio.git`)
3. Build the code by pressing `Build > Build SplameiPlay Studio` or CTRL + B
4. The build files should be located in `<Repo directory>\bin\Debug\SplameiPlay Studio.exe`

### MSBuild

1. Clone the repo by running `git clone https://github.com/splameiplay/splameiplay-studio.git` in the target directory
2. Change into the directory via `cd splameiplay-studio`
3. Build the project using `msbuild  "SplameiPlay Studio.sln"  /p:Configuration=Release  /p:Platform="Any CPU"  /p:RestorePackages=false  /m  /verbosity:minimal`
4. The build files should be located in `<Repo directory>\bin\Debug\SplameiPlay Studio.exe`

## Contributing

Any support to SplameiPlay Studio will go a long way to support it's development. You can help by:

 - Reporting issues / feature requests on the [issue page](https://github.com/splameiplay/splameiplay-studio/issues)
 - Forking the repo
 - Making pull requests
 - Staring or watching the repo
 - Sharing the repo

All contributions to SplameiPlay Studio will be licenced under the MPL 2.0 to keep the open-source nature of the app

## License

All code written directly for SplameiPlay Studio is licenced under the Mozilla Public Licence 2.0 (MPL) unless otherwise stated

## Branding Rights

The name, branding, logo, etc. and any related assets for SplameiPlay and SplameiPlay is property of Splamei. These assets are not licensed under the MPL 2.0.

Any and all forks and derived works must use a different name and cannot imply endorsement or affiliation with Splamei, SplameiPlay or projects.

## Socials

[YouTube](https://youtube.com/@splameiplay)
[Twitter](https://twitter.com/splameiplay)
[Bluesky](https://bsky.app/profile/splameiplay.bsky.social)
[Discord](https://discord.gg/g2KTP5X9At)

## Built with ❤️ in Visual Studio by Splamei and community
