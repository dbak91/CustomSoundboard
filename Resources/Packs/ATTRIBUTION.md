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
- English (British) male (`bm_george`) and English (American) female (`af_bella`): Kokoro-82M by hexgrad, int8 build `kokoro-int8-en-v0_19` from the sherpa-onnx tts-models release. Weights are Apache-2.0 (https://huggingface.co/hexgrad/Kokoro-82M); licence text in `Resources/Voices/kokoro-LICENSE.txt`. Its training data is described by the author as permissively licensed or public domain audio, which I have not verified independently. The 134 MB `kokoro.onnx` is not stored in git (GitHub's 100 MB limit); run `scripts/Get-Voices.ps1` to fetch it.
- The Irish female voice (Jenny, Dioco) and the Piper northern male voice were removed because they were fine-tuned from the research-only Lessac voice. The male voice is now Kokoro `bm_george`.
- Robot voices ("classic hum", "Dalek", "retro 8-bit", "tin can") are the Cori voice run through simple effects in `Services/RoboticVoice.cs`. They add no models or licences.
- English (British) female: Piper `en_GB-cori-medium` ("Cori", by Bryce Beattie, https://brycebeattie.com/files/tts/). Trained from scratch on LibriVox recordings (https://librivox.org), which are public domain. It is not derived from the Lessac data.
- English (British) male: Piper `en_GB-northern_english_male-medium`. Dataset: Crowdsourced high-quality UK and Ireland English dataset, http://www.openslr.org/83/, CC BY-SA 4.0 International. Its Piper model card says it was fine-tuned from the Lessac voice, so it is NOT cleared for publishing and must be replaced or removed. The Irish female voice (Jenny, Dioco) has the same Lessac base and the same problem.
- **Blocking licence issue (checked 2026-11-05):** the Piper `en_US-lessac-medium` voice was trained on the Blizzard 2013 Lessac data (https://www.cstr.ed.ac.uk/projects/blizzard/2013/lessac_blizzard2013/license.html). That licence is for research only. It forbids any commercial use, including developing or selling voice synthesis products, and it does not allow redistribution of derived material. Any voice fine-tuned from lessac therefore must not be published on GitHub or in the app until it is removed or replaced. Check each voice's origin before publishing.
- Software: Piper (MIT), sherpa-onnx (Apache-2.0), eSpeak NG (GPL-3.0). Because eSpeak NG is GPL-3.0, publish the app's source under a GPL-compatible licence when distributing it.
