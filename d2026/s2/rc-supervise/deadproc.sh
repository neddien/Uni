#!/usr/bin/env bash

if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <proc_name> <restart_command>"
    exit 1
fi

PROC_NAME="$1"
RESTART_CMD="$2"

log() {
    level="${1,,}"
    payload="[$(date '+%Y-%m-%d %H:%M:%S')] $level: $2"

    if [[ "$level" == "warn" ]] || [[ "$level" == "err" ]]; then
      echo "$payload" >&2
    else
      echo "$payload"
    fi

    echo "$payload" >> "/tmp/restarter.log"
}

warn() {
    log "warn" "$1"
}

info() {
    log "info" "$1"
}

err() {
    log "err" "$1"
}

# The first two greps are for filtering ourselves (this script) 
# This is so that it doesn't think that the dead process is alive
# just because we typed its name
ps aux | grep -v grep | grep -v "$0" | grep -q "$PROC_NAME"
STATUS=$?

#set -x

if [ "$STATUS" -eq 0 ]; then
    # If running: log INFO and print "OK: <process> is running"
    info "$PROC_NAME is running"
else
    warn "$PROC_NAME is NOT running"
    
    info "Attempting restart..."
    $RESTART_CMD > /dev/null 2>&1 &
    info "Restart command launched: $RESTART_CMD"

    # After attempting restart: sleep 2 and re-check and Log SUCCESS or FAILED
    sleep 2
    
    # Re-check the process status.
    if ps aux | grep -v grep | grep -v "$0" | grep -q "$PROC_NAME"; then
        info "[SUCESS] $PROC_NAME is now running"
    else
        err "[FAILED] $PROC_NAME failed to restart"
    fi
fi

info "Log saved to /tmp/restarter.log"
