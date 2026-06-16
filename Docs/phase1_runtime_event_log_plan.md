# Phase 1 Runtime Event Log Plan

## Summary

The runtime event log records accepted and rejected command results. It gives the runtime state layer a simple audit trail before combat, AI, replay, or save systems exist.

This is not a battle event system yet.

## Current Event Data

Each event record stores:

- Sequence number.
- Category.
- Event type.
- Target id.
- Accepted or rejected result.
- Message.
- Match elapsed time.

## Current Integration

`RuntimeMutationCommandService` writes command results to `RuntimeEventLog` when a log is provided.

The log records both accepted and rejected commands:

- Accepted setup commands show that state was changed through the command boundary.
- Rejected setup commands show that validation blocked the command.

## Unity Menu Check

Use:

```text
Tools > MOBA Manager > Test Runtime Event Log
```

Expected output:

- Event Count: 4
- Accepted Event: 3
- Rejected Event: 1
- First Sequence: 1
- Last Sequence: 4
- Last Event Accepted: False
- Last Event Type: set_skill_level_for_data_setup

## Boundaries

This layer must not:

- Simulate combat.
- Replay commands.
- Serialize save data.
- Apply damage.
- Apply healing.
- Apply buffs or debuffs.
- Run AI decisions.
- Create Unity scenes or GameObjects.

## Design Notes

The event log is append-only for now. It stores records in memory and exposes a read-only list.

The event log is deliberately generic enough to support future command, validation, debug, or simulation events, but Phase 1 only writes command results.

## Next Step

After the event log passes, the next safe step is a runtime state debug report that prints a compact snapshot of match state, units, commands, and event counts without adding simulation logic.
