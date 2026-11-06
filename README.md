# Custom Soundboard

A communicator app for non-verbal autism, built as a big-button sound board for an Android
tablet. Each tile is a large picture on a colour, and tapping it plays a spoken word. It is
fully configurable: use your own images and audio files for any tile. Built with .NET MAUI.

<img width="2400" height="1080" alt="Screenshot_20261105-085008" src="https://github.com/user-attachments/assets/f2fd6532-0308-405d-adab-3dde6abf0a96" />


> **Older device?** There is an alternative **Lite** edition (JAMESSOUNDS) for older Android
> tablets. It is smaller and simpler, and uses the device's own text-to-speech voice (English,
> British) instead of the on-device AI voices, with a smaller symbol set (500 pictures and
> 500 emoji). Use Lite if the Full edition will not install or runs too slowly.

## Minimum specification to install

**Recommended: Android 8 or later. Supports Android 5 or later. 64-bit devices only.**

| | |
|---|---|
| **Android** | Set to 5.0 (API 21) in the project, but only tested on Android 16. A 64-bit device is needed, and 64-bit Android 5 devices are rare, so use Android 8 or later to be safe. |
| **Processor** | 64-bit only (arm64-v8a or x86_64). 32-bit ARM devices will not install it. |
| **Free storage** | About 530 MB: 318 MB to install, plus up to 212 MB once every voice has been used |
| **Download** | 251.5 MB APK |
| **Screen** | Landscape tablet recommended |
| **Network** | Not needed. All voices run offline |

Install: copy the APK to the tablet, open it, and allow "Install unknown apps" when asked.
Voice generation runs on the device, so it is slow on very old or low-end devices.

## Tiles

The default tiles use the speech clips in `Resources/Sounds` and the icons in
`Resources/Images`. Pages one to three are the built-in pages. Extra pages (4 to 6) can be
added from the menu, and each tile on them can take its own picture and sound.

### Customising within the app

No code changes or rebuild are needed to personalise the board. A parent, carer or family
member can customise tiles from inside the app: set a tile's text, choose its picture, and
choose how its sound is made (a generated voice, or your own uploaded audio file in MP3, WAV
or M4A format). Extra pages can also be added from the menu.

### Customising in code

To change the built-in tiles in the source, edit `PageOneTiles`, `PageTwoTiles` and `PageThreeTiles` in
`MainPage.cs`. Each tile is a `new SoundTile(emoji, label, colour, soundFile, imageName)`
entry, and its sound file goes in `Resources/Sounds`.

The app runs in landscape on a grid, sized for wide-screen tablets. Page one has five
columns across two rows.

## Requirements

- .NET 10 SDK with the MAUI Android workload
- Android SDK 36
- Visual Studio 2026 or the .NET CLI
- Android 5.0 (API 21) or later

## Build

```powershell
dotnet build CustomSoundboard.csproj
```

## Publish

See [PUBLISHING.md](PUBLISHING.md).

## Licence

Custom Soundboard is free software, released under the GNU General Public License v3.0 (see `LICENSE`).
The Full edition includes eSpeak NG (GPL-3.0) through sherpa-onnx, so the app's source is published under
the same licence when the app is distributed. The pictures (Mulberry Symbols, OpenMoji) are CC BY-SA 4.0 and the
voices have their own terms; see `Resources/Packs/ATTRIBUTION.md` and the in-app Credits.
