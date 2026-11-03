# Image pack attribution

Both packs are licensed Creative Commons Attribution-ShareAlike 4.0 (CC BY-SA 4.0), which allows free use
in a published app, including on Google Play, provided credit is given (e.g. in the app's About screen).
The SVGs are unmodified copies of the originals, renamed only.

## Mulberry Symbols (`Mulberry/`, files `mb_*.svg`, 407 symbols; the Full edition also uses 2,587 more from the same release, as WebP only)
Mulberry Symbols by Steve Lee are licensed under the Creative Commons Attribution-ShareAlike 4.0 License.
https://mulberrysymbols.org - version 3.6.1 - https://creativecommons.org/licenses/by-sa/4.0/

## OpenMoji (`OpenMoji/`, files `om_*.svg`, 93 emoji)
All emojis designed by OpenMoji - the open-source emoji and icon project. License: CC BY-SA 4.0.
https://openmoji.org - release 17.0.0

`manifest.csv` maps each file to its tile label.

`Full/` holds all 4,731 pictures (2,994 Mulberry, 1,737 OpenMoji) as 384px WebP files, rendered from the SVGs
(resized only), for the Full edition. The extra Mulberry symbols come from release 3.6.1 (mulberry-symbols.zip),
leaving out the country flags and maps. They are under the same licences and credits as the SVGs they were made from.

## Voices (`Resources/Voices/`)
The Full edition packages four offline text-to-speech voices. Each is a Piper model run with sherpa-onnx.
- English (Irish) female: "Jenny (Dioco)", Piper `en_GB-jenny_dioco-medium`. Dataset: https://github.com/dioco-group/jenny-tts-dataset. Attribution is required in software that generates audio with this voice: the voice must be referred to as "Jenny", and where practical "Jenny (Dioco)". Commercial use is permitted.
- English (British) female: Piper `en_GB-southern_english_female-medium`, and English (British) male: Piper `en_GB-northern_english_male-medium`. Dataset: Crowdsourced high-quality UK and Ireland English dataset, http://www.openslr.org/83/, CC BY-SA 4.0 International.
- All voices were fine-tuned by the Piper project from its `en_US-lessac-medium` voice; that dataset's licence was not checked.
- Software: Piper (MIT), sherpa-onnx (Apache-2.0), eSpeak NG (GPL-3.0). Because eSpeak NG is GPL-3.0, publish the app's source under a GPL-compatible licence when distributing it.
