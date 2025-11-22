# malimo - Markdown Linked Images Mover

[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/wonderbird/malimo)
[![Build Status Badge](https://github.com/wonderbird/malimo/actions/workflows/build_wo_release.yml/badge.svg?branch=main)](https://github.com/wonderbird/malimo/actions?query=workflow%3A%22.NET%20build%20w/o%20release%22)
[![Maintainability](https://qlty.sh/gh/wonderbird/projects/malimo/maintainability.svg)](https://qlty.sh/gh/wonderbird/projects/malimo)
[![Code Coverage](https://qlty.sh/gh/wonderbird/projects/malimo/coverage.svg)](https://qlty.sh/gh/wonderbird/projects/malimo)

Move all images used by a markdown file into a folder.

## Motivation

During online conferences and workshops I take notes in markdown format using [Obsidian.md](https://obsidian.md). As a result I have a folder with many screenshots from different talks in the evening.

The Markdown Linked Images Mover "`malimo`" moves all images addressed by a markdown file into a separate folder.

This way I can separate screenshots by topic at the end of a conference or workshop day.

## Development and Support Status

I am developing during my spare time and use this project for learning purposes. Please assume that I will need some days to answer your questions. At some point I might lose interest in the project. Please keep this in mind when using this project in a production environment.

## Acknowledgements

The [application icon](assets/application-icon) is the `folder-document.png` from the "Tango harm-on-icons" icon set, which is published in the [Open Icon Library](https://sourceforge.net/projects/openiconlibrary/) using the [CC-BY 3.0](https://creativecommons.org/licenses/by/3.0/) license. Thanks to the author(s) for their work ❤️.

## Installation

### macOS

For macOS I am maintaining a [Homebrew](https://brew.sh) installer:

```shell
brew install wonderbird/tools/malimo
```

This creates a symlink named `malimo` in your `$(brew --prefix)/bin` directory.

The corresponding Homebrew cask is at [wonderbird / homebrew-tools / Casks / malimo.rb](https://github.com/wonderbird/homebrew-tools/blob/main/Casks/malimo.rb).

### Windows

#### Use the Chocolatey Package Manager

For Windows, the [release process](./.github/workflows/dotnet.yml) publishes a wrapper package to the
[Chocolatey Community Repository](https://community.chocolatey.org/packages). The package will download and install the release ZIP file from the [malimo GitHub release page](https://github.com/wonderbird/malimo/releases).

You can install malimo by:

```powershell
choco install malimo --pre
```

At the moment I consider the software a prerelease. This is why the `--pre` argument is required.

#### Manual Installation

If you do not manage your Windows software using [Chocolatey](https://community.chocolatey.org/), then

- download `malimo.win-x64.zip` from the latest [GitHub release](https://github.com/wonderbird/malimo/releases),
- extract the files into a folder
- add the folder to your PATH environment variable.

### Verification

After successful installation you can run the program and get help by entering

```shell
malimo --help
```

and you can check the version

```shell
malimo --version
```

The displayed version number contains 4 numbers separated by a ".", e.g. `0.1.7.178`. The first three numbers reflect the [semantic version](https://semver.org/spec/v2.0.0.html) of the release, here `0.1.7`. The last number is the [GitHub workflow run number](https://github.com/wonderbird/malimo/actions?query=workflow%3A%22.NET%22), during which the package was created, in the example case it was [run #178](https://github.com/wonderbird/malimo/actions/runs/4669300742).

## Usage Examples

The most basic usage is

```shell
malimo --file ./my-markdown-file.md --target-dir ./target
```

`malimo` will list the images linked in `./my-markdown-file.md`. It will ensure that every image exists in the same  directory `./`. Then it will move all linked images to the directory `./target`.

If you want to double check what would be moved before actually moving the files, then add the `--dry-run` option. This way, no file will be moved.

```shell
malimo --dry-run --file ./my-markdown-file.md --target-dir ./will-be-ignored
```

To get help, specify the `--help` option:

```shell
malimo --help
```

You can find elaborate usage examples in the `FeatureDocumentation` HTML file attached to each [release](https://github.com/wonderbird/malimo/releases) starting from 0.1.8-alpha.

## Development

### Architecture Overview

[docs/architecture.adoc](docs/architecture.adoc) gives an overview of the architecture. Section **8 Concepts** explains maintenance tasks.

### Quick-Start

Click
the [![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/wonderbird/malimo)
badge (also above) to launch a web IDE.

If that does not work for you or if you'd like to have the project on your local machine, then continue reading.

### Prerequisites

To compile, test and run this project the latest [.NET SDK](https://dotnet.microsoft.com/download) is required on
your machine. For calculating code metrics I recommend [metrix++](https://github.com/metrixplusplus/metrixplusplus).
This requires python.

If you are interested in test coverage, then you'll need the following tools installed:

```shell
dotnet tool install --global coverlet.console --configfile NuGet-OfficialOnly.config
dotnet tool install --global dotnet-reportgenerator-globaltool --configfile NuGet-OfficialOnly.config
```

## Build, Test, Run

Run the following commands from the folder containing the `.sln` file in order to build and test.

### Build and Test the Solution

```sh
# Build the project
dotnet build

# Run the tests once
dotnet test
```

```shell
# Continuously watch the tests while changing code
dotnet watch -p ./malimo.Tests test
```

```shell
# Produce a coverage report and open it in the default browser
rm -r malimo.Tests/TestResults && \
  dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./TestResults/coverage.cobertura.xml' && \
  reportgenerator "-reports:malimo.Tests/TestResults/*.xml" "-targetdir:report" "-reporttypes:Html;lcov" "-title:DotnetStarter"
open report/index.html
```

### Run the Application

```shell
# Get help
dotnet run --project malimo/malimo.csproj -- --help
```

```shell
# Prepare test data to run the application
cp -Rv malimo.Tests/data/seed malimo.Tests/data/source
mkdir malimo.Tests/data/target
```

```shell
# Run the application
dotnet run --project malimo/malimo.csproj -- --dry-run --file malimo.Tests/data/source/Testfile.md --target-dir malimo.Tests/data/target
```

```shell
# Inspect the result
ls -la malimo.Tests/data/source
ls -la malimo.Tests/data/target
```

```shell
# Cleanup test data
rm -r malimo.Tests/data/source malimo.Tests/data/target
```

### Before Creating a Pull Request ...

#### Fix Static Code Analysis Warnings

... fix static code analysis warnings reported by [SonarLint](https://www.sonarsource.com/products/sonarlint/)
and by [CodeClimate](https://codeclimate.com/github/wonderbird/malimo/issues).

#### Apply Code Formatting Rules

```shell
# Install https://csharpier.io globally, once
dotnet tool install -g csharpier

# Format code
dotnet csharpier .
```

#### Check Code Metrics

... check code metrics using [metrix++](https://github.com/metrixplusplus/metrixplusplus)

- Configure the location of the cloned metrix++ scripts
  ```shell
  export METRIXPP=/path/to/metrixplusplus
  ```

- Collect metrics
  ```shell
  python "$METRIXPP/metrix++.py" collect --std.code.complexity.cyclomatic --std.code.lines.code --std.code.todo.comments --std.code.maintindex.simple -- .
  ```

- Get an overview
  ```shell
  python "$METRIXPP/metrix++.py" view --db-file=./metrixpp.db
  ```

- Apply thresholds
  ```shell
  python "$METRIXPP/metrix++.py" limit --db-file=./metrixpp.db --max-limit=std.code.complexity:cyclomatic:5 --max-limit=std.code.lines:code:25:function --max-limit=std.code.todo:comments:0 --max-limit=std.code.mi:simple:1
  ```

At the time of writing, I want to stay below the following thresholds:

```text
--max-limit=std.code.complexity:cyclomatic:5
--max-limit=std.code.lines:code:25:function
--max-limit=std.code.todo:comments:0
--max-limit=std.code.mi:simple:1
```

Finally, remove all code duplication. The next section describes how to detect code duplication.

#### Remove Code Duplication Where Appropriate

To detect duplicates I use the [CPD Copy Paste Detector](https://docs.pmd-code.org/latest/pmd_userdocs_cpd.html)
tool from the [PMD Source Code Analyzer Project](https://docs.pmd-code.org/latest/index.html).

If you have installed PMD by download & unzip, replace `pmd` by `./run.sh`.
The [homebrew pmd formula](https://formulae.brew.sh/formula/pmd) makes the `pmd` command globally available.

```sh
# Remove temporary and generated files
# 1. dry run
git clean -ndx
```

```shell
# 2. Remove the files shown by the dry run
git clean -fdx
```

```shell
# Identify duplicated code in files to push to GitHub
pmd cpd --minimum-tokens 50 --language cs --dir .
```
