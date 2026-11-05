# Downloads the Kokoro voice model (too large to store in git) into Resources/Voices.
# Source: https://github.com/k2-fsa/sherpa-onnx/releases/tag/tts-models (kokoro-int8-en-v0_19, Apache-2.0)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$tmp = Join-Path ([IO.Path]::GetTempPath()) 'kokoro-voices'
New-Item -ItemType Directory -Force $tmp | Out-Null
$tar = Join-Path $tmp 'kokoro.tar.bz2'
Invoke-WebRequest 'https://github.com/k2-fsa/sherpa-onnx/releases/download/tts-models/kokoro-int8-en-v0_19.tar.bz2' -OutFile $tar
tar -xjf $tar -C $tmp
Copy-Item (Join-Path $tmp 'kokoro-int8-en-v0_19/model.int8.onnx') (Join-Path $root 'Resources/Voices/kokoro.onnx') -Force
Write-Host 'Done: Resources/Voices/kokoro.onnx'
