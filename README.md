# DKC3MapEditionHelper

This project aims to make the use of DKC3 map editing tools easier. Auto backup rom file when making change.

### Prerequisites

  - Platinium
https://www.romhacking.net/utilities/641/
  - Map tools
http://www.dkc-atlas.com/forum/viewtopic.php?f=37&t=1325
  - DKC3 rom
  - Snes emulator of your choice

### Installation

  - Publish project
  - Make sure you got `platinium_template.xml`, `appsettings.json`, `dkc3tool.sh` files in the published directory
  - Add `dkc3tool.sh` to your PATH
  - Open `appsettings.json` and configure all in the AppSettings section

### Usage

Export all maps
```sh
$ dkc3tool.sh export
```
Export specific map
```sh
$ dkc3tool.sh export [map_number]
```
Import all maps
```sh
$ dkc3tool.sh export
```
Import specific maps
```sh
$ dkc3tool.sh export [map_number]
```
Auto open Platinium with specific map and auto save in rom on exit
```sh
$ dkc3tool.sh edit [map_number]
```
Lunch rom in emulator
```sh
$ dkc3tool.sh test
```