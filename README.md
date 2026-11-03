# Custom Soundboard

A big-button sound board for an Android tablet. Each tile is a large picture on a
colour, and tapping it plays a spoken word. Built with .NET MAUI.

## Tiles

The default tiles use the speech clips in `Resources/Sounds` and the icons in
`Resources/Images`. Pages one to three are the built-in pages. Extra pages (4 to 6) can be
added from the menu, and each tile on them can take its own picture and sound.

To change the built-in tiles, edit `PageOneTiles`, `PageTwoTiles` and `PageThreeTiles` in
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
