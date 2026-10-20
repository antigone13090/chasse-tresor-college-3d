#!/usr/bin/env bash
# Execute the V2 validation on a Linux host with Unity 6000.3.2f1 installed.
# Usage: UNITY_EDITOR=/path/to/Unity bash scripts/validate-v2-linux.sh
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
UNITY_EDITOR="${UNITY_EDITOR:-}"
if [[ -z "$UNITY_EDITOR" || ! -x "$UNITY_EDITOR" ]]; then
  echo "ERROR: set UNITY_EDITOR to executable Unity 6000.3.2f1 binary" >&2
  exit 2
fi
if ! command -v xvfb-run >/dev/null 2>&1; then
  echo "ERROR: xvfb-run is required (sudo apt install xvfb)" >&2
  exit 2
fi
mkdir -p "$ROOT/Logs"
LOG="$ROOT/Logs/campaign-v2-batch-review.log"
if ! xvfb-run -a "$UNITY_EDITOR" -batchmode -quit -projectPath "$ROOT" \
    -executeMethod CampaignV2Validator.Run -logFile "$LOG"; then
  echo "FAIL: Unity process returned nonzero. See $LOG" >&2
  exit 1
fi
# Guard against a Unity process exiting 0 despite a validation exception.
if ! grep -q 'CAMPAGNE V2 VALIDEE' "$LOG" || ! grep -q 'FAIL : 0' "$LOG"; then
  echo "FAIL: Unity did not log an unambiguous successful V2 validation. See $LOG" >&2
  exit 1
fi
echo "PASS: V2 validation completed. Log: $LOG"
