param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    [string]$RepoUrl = "https://github.com/GDcheeriosYT/GentrysQuest",
    [string]$ProjectPath = "GentrysQuest.Desktop/GentrysQuest.Desktop.csproj",
    [string]$Runtime = "win-x64",
    [bool]$SelfContained = $true,
    [string]$PublishDir = "publish",
    [switch]$FirstRelease,
    [switch]$SkipUpload
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-VpkCommand {
    $vpk = Get-Command vpk -ErrorAction SilentlyContinue
    if ($vpk) {
        return $vpk.Source
    }

    Write-Host "vpk not found in PATH. Installing/updating global tool..."
    dotnet tool update -g vpk | Out-Null

    $vpk = Get-Command vpk -ErrorAction SilentlyContinue
    if ($vpk) {
        return $vpk.Source
    }

    $fallback = Join-Path $env:USERPROFILE ".dotnet\tools\vpk.exe"
    if (Test-Path $fallback) {
        return $fallback
    }

    throw "Could not locate vpk CLI after install. Ensure dotnet global tools are available in PATH."
}

function Invoke-Step([string]$title, [scriptblock]$action) {
    Write-Host ""
    Write-Host "==> $title"
    & $action
}

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

$projectFullPath = Join-Path $root $ProjectPath
if (-not (Test-Path $projectFullPath)) {
    throw "Project file not found: $projectFullPath"
}

$publishFullPath = Join-Path $root $PublishDir
$vpkCommand = Get-VpkCommand

Invoke-Step "Updating csproj version to $Version" {
    [xml]$xml = Get-Content -Raw -Path $projectFullPath
    $projectNode = $xml.Project
    if (-not $projectNode) {
        throw "Invalid csproj format: missing Project node."
    }

    $propertyGroups = @($projectNode.PropertyGroup)
    if (-not $propertyGroups -or $propertyGroups.Count -eq 0) {
        throw "Invalid csproj format: missing PropertyGroup."
    }

    $targetGroup = $propertyGroups | Where-Object { $_.Label -eq "Project" } | Select-Object -First 1
    if (-not $targetGroup) {
        $targetGroup = $propertyGroups | Select-Object -First 1
    }

    if (-not $targetGroup.Version) {
        $versionNode = $xml.CreateElement("Version")
        $versionNode.InnerText = $Version
        $targetGroup.AppendChild($versionNode) | Out-Null
    } else {
        $targetGroup.Version = $Version
    }

    if (-not $targetGroup.FileVersion) {
        $fileVersionNode = $xml.CreateElement("FileVersion")
        $fileVersionNode.InnerText = $Version
        $targetGroup.AppendChild($fileVersionNode) | Out-Null
    } else {
        $targetGroup.FileVersion = $Version
    }

    $xml.Save($projectFullPath)
}

Invoke-Step "Publishing desktop app ($Runtime, self-contained=$SelfContained)" {
    if (Test-Path $publishFullPath) {
        Remove-Item -Recurse -Force $publishFullPath
    }

    dotnet publish $projectFullPath -c Release -r $Runtime --self-contained:$SelfContained -o $publishFullPath
}

if (-not $FirstRelease) {
    Invoke-Step "Downloading previous release metadata from GitHub" {
        $downloadArgs = @("download", "github", "--repoUrl", $RepoUrl)

        $token = $env:GITHUB_TOKEN
        if ([string]::IsNullOrWhiteSpace($token)) { $token = $env:GH_TOKEN }
        if ([string]::IsNullOrWhiteSpace($token)) { $token = $env:GENTRYSQUEST_GITHUB_TOKEN }

        if (-not [string]::IsNullOrWhiteSpace($token)) {
            $downloadArgs += @("--token", $token)
        }

        & $vpkCommand @downloadArgs
    }
}

Invoke-Step "Packing Velopack release $Version" {
    & $vpkCommand pack -u GentrysQuest -v $Version -p $publishFullPath
}

if (-not $SkipUpload) {
    Invoke-Step "Uploading GitHub release v$Version" {
        $uploadArgs = @(
            "upload", "github",
            "--repoUrl", $RepoUrl,
            "--publish",
            "--releaseName", "GentrysQuest $Version",
            "--tag", "v$Version"
        )

        $token = $env:GITHUB_TOKEN
        if ([string]::IsNullOrWhiteSpace($token)) { $token = $env:GH_TOKEN }
        if ([string]::IsNullOrWhiteSpace($token)) { $token = $env:GENTRYSQUEST_GITHUB_TOKEN }

        if (-not [string]::IsNullOrWhiteSpace($token)) {
            $uploadArgs += @("--token", $token)
        }

        & $vpkCommand @uploadArgs
    }
} else {
    Write-Host ""
    Write-Host "Skipping GitHub upload (requested by -SkipUpload)."
}

Write-Host ""
Write-Host "Release pipeline completed for version $Version."
