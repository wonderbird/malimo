# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.5-alpha] - 2023-04-04

### Added

- Log and abort if a mandatory argument is missing, i.e. --file or --target-dir

### Fixed

- Print correct version number when --version argument is given
- Print description when --help argument is given

### Changed

- Renamed the application to "malimo"

## [0.1.4-alpha] - 2023-04-01

### Added

- Release package for Windows x64

## [0.1.3-alpha] - 2023-03-30

### Fixed

- Intel x64 macs run the application without complaining about unsigned DLLs 

## [0.1.2-alpha] - 2023-03-26

### Added

- DLLs of the published package are also code-signed and notarized

## [0.1.1-alpha] - 2023-03-26

### Added

- Release package for macOS x64 architecture

## [0.1.0-alpha] - 2023-03-24

### Added

- Move all images linked by a markdown file to a specified folder
