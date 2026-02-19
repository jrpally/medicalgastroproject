[CmdletBinding()]
param(
    [string]$BackendUrl = "https://localhost:5001;http://localhost:5000",
    [int]$FrontendPort = 5173
)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Resolve-Path (Join-Path $ScriptDir "..")
$BackendDir = Join-Path $RootDir "backend/src/Clinic.Api"
$FrontendDir = Join-Path $RootDir "frontend"
$WorkstationDir = Join-Path $RootDir "workstation/Clinic.Workstation"

$backendProcess = $null
$frontendProcess = $null

function Require-Command([string]$Name) {
    if (-not (Get-Command $Name -ErrorAction SilentlyContinue)) {
        throw "[local-dev] Missing required command: $Name"
    }
}

function Stop-Services {
    Write-Host "`n[local-dev] Stopping services..."

    if ($backendProcess -and -not $backendProcess.HasExited) {
        Stop-Process -Id $backendProcess.Id -Force -ErrorAction SilentlyContinue
    }

    if ($frontendProcess -and -not $frontendProcess.HasExited) {
        Stop-Process -Id $frontendProcess.Id -Force -ErrorAction SilentlyContinue
    }
}

try {
    Require-Command "dotnet"
    Require-Command "npm"

    Write-Host "[local-dev] Restoring and building backend..."
    Push-Location $BackendDir
    dotnet restore
    dotnet build
    Pop-Location

    Write-Host "[local-dev] Restoring and building workstation module..."
    Push-Location $WorkstationDir
    dotnet restore
    dotnet build
    Pop-Location

    Write-Host "[local-dev] Installing frontend dependencies..."
    Push-Location $FrontendDir
    npm install
    Pop-Location

    Write-Host "[local-dev] Starting backend API on $BackendUrl ..."
    $backendRunCommand = "set `"ASPNETCORE_URLS=$BackendUrl`" && dotnet run"
    $backendProcess = Start-Process -FilePath "cmd.exe" `
        -ArgumentList "/c", $backendRunCommand `
        -WorkingDirectory $BackendDir `
        -PassThru `
        -NoNewWindow

    Write-Host "[local-dev] Starting frontend dev server on http://localhost:$FrontendPort ..."
    $frontendProcess = Start-Process -FilePath "npm" `
        -ArgumentList "run", "dev", "--", "--host", "0.0.0.0", "--port", "$FrontendPort" `
        -WorkingDirectory $FrontendDir `
        -PassThru `
        -NoNewWindow

    Write-Host "[local-dev] Services started. Press Ctrl+C to stop both services."

    while ($true) {
        if ($backendProcess.HasExited -or $frontendProcess.HasExited) {
            throw "[local-dev] One of the services exited unexpectedly."
        }

        Start-Sleep -Seconds 2
    }
}
catch {
    Write-Error $_
    exit 1
}
finally {
    Stop-Services
}
