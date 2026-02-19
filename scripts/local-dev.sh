#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
BACKEND_DIR="$ROOT_DIR/backend/src/Clinic.Api"
FRONTEND_DIR="$ROOT_DIR/frontend"
WORKSTATION_DIR="$ROOT_DIR/workstation/Clinic.Workstation"

BACKEND_PID=""
FRONTEND_PID=""

cleanup() {
  echo "\n[local-dev] Stopping services..."
  if [[ -n "${BACKEND_PID}" ]] && kill -0 "${BACKEND_PID}" 2>/dev/null; then
    kill "${BACKEND_PID}" || true
  fi
  if [[ -n "${FRONTEND_PID}" ]] && kill -0 "${FRONTEND_PID}" 2>/dev/null; then
    kill "${FRONTEND_PID}" || true
  fi
}

trap cleanup EXIT INT TERM

require_cmd() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "[local-dev] Missing required command: $1"
    exit 1
  fi
}

require_cmd dotnet
require_cmd npm

build_backend() {
  echo "[local-dev] Restoring and building backend..."
  (cd "$BACKEND_DIR" && dotnet restore && dotnet build)
}

build_workstation() {
  echo "[local-dev] Restoring and building workstation module..."
  (cd "$WORKSTATION_DIR" && dotnet restore && dotnet build)
}

build_frontend() {
  echo "[local-dev] Installing frontend dependencies..."
  (cd "$FRONTEND_DIR" && npm install)
}

run_backend() {
  echo "[local-dev] Starting backend API on https://localhost:5001 ..."
  (
    cd "$BACKEND_DIR"
    ASPNETCORE_URLS="https://localhost:5001;http://localhost:5000" dotnet run
  ) &
  BACKEND_PID=$!
}

run_frontend() {
  echo "[local-dev] Starting frontend dev server on http://localhost:5173 ..."
  (
    cd "$FRONTEND_DIR"
    npm run dev -- --host 0.0.0.0 --port 5173
  ) &
  FRONTEND_PID=$!
}

wait_for_processes() {
  echo "[local-dev] Services started. Press Ctrl+C to stop both services."
  wait "$BACKEND_PID" "$FRONTEND_PID"
}

build_backend
build_workstation
build_frontend
run_backend
run_frontend
wait_for_processes
