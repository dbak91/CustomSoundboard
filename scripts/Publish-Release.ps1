<#
.SYNOPSIS
    Builds signed release APKs of both the Full and the Lite app.

.DESCRIPTION
    Both are signed with the release key from scripts\New-SigningKey.ps1, then checked, and copied to
    artifacts\release\<version>\ as CustomSoundboard-Full-<version>.apk and CustomSoundboard-Lite-<version>.apk.

    Before each release, raise ApplicationDisplayVersion and ApplicationVersion in CustomSoundboard.csproj,
    or the tablets will not accept the new APK as an update.
#>
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$project = Join-Path $root "CustomSoundboard.csproj"
$framework = "net10.0-android36.0"
$signing = Join-Path $root "release-signing\CustomSoundboard.signing.props"

if (-not (Test-Path $signing)) {
    Write-Error "There is no release signing key yet. Run .\scripts\New-SigningKey.ps1 once first."
}

[xml]$csproj = Get-Content $project
$version = ($csproj.Project.PropertyGroup | Where-Object { $_.ApplicationDisplayVersion } | Select-Object -First 1).ApplicationDisplayVersion
$output = Join-Path $root "artifacts\release\$version"
New-Item -ItemType Directory -Force $output | Out-Null

# apksigner checks each APK is signed, and shows which key signed it.
$sdk = @("${env:ProgramFiles(x86)}\Android\android-sdk", "$env:LOCALAPPDATA\Android\Sdk", $env:ANDROID_HOME) |
    Where-Object { $_ -and (Test-Path "$_\build-tools") } | Select-Object -First 1
$apksigner = if ($sdk) {
    Get-ChildItem "$sdk\build-tools\*\apksigner.bat" | Sort-Object { [version]$_.Directory.Name } | Select-Object -Last 1
}

$expected = $null
foreach ($edition in "Full", "Lite") {
    Write-Host "Building $edition $version..." -ForegroundColor Cyan
    & dotnet publish $project -c Release -f $framework -p:SoundboardEdition=$edition -p:AndroidPackageFormat=apk -nologo -v q
    if ($LASTEXITCODE -ne 0) {
        Write-Error "The $edition build failed."
    }

    $bin = if ($edition -eq "Lite") { "bin\Lite" } else { "bin" }
    $apk = Get-ChildItem (Join-Path $root "$bin\Release\$framework\publish\*-Signed.apk") |
        Sort-Object LastWriteTime | Select-Object -Last 1
    $target = Join-Path $output "CustomSoundboard-$edition-$version.apk"
    Copy-Item $apk.FullName $target -Force

    if ($apksigner) {
        $report = & $apksigner.FullName verify --print-certs $target
        if ($LASTEXITCODE -ne 0) {
            Write-Error "$target is not correctly signed."
        }

        # Both editions must carry the same release certificate, never the debug one.
        $certificate = ($report | Select-String "certificate SHA-256 digest: (.+)").Matches[0].Groups[1].Value
        if ($report -match "CN=Android Debug") {
            Write-Error "$target was signed with the debug key, not the release key."
        }
        if ($expected -and $certificate -ne $expected) {
            Write-Error "Full and Lite were signed with different keys."
        }
        $expected = $certificate
    }

    $size = [math]::Round((Get-Item $target).Length / 1MB, 1)
    Write-Host "  $target ($size MB)" -ForegroundColor Green
}

if ($expected) {
    Write-Host "Both APKs are signed with the release key (SHA-256 $expected)."
} else {
    Write-Warning "apksigner was not found, so the signatures were not checked."
}
