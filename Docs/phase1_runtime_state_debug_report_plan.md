# Phase 1 Runtime State Debug Report Plan

## Summary

The runtime state debug report prints a compact read-only snapshot of the current runtime state. It is meant for quick Phase 1 verification after building query, command, validation, and event log layers.

This is not UI, save data, replay, combat, or AI.

## Report Contents

The report includes:

- Match id.
- Map id.
- Started and completed flags.
- Elapsed time.
- Blue and red team ids.
- Hero state count.
- Unit state count.
- Blue and red unit counts.
- Hero, tower, and nexus unit counts.
- Event count.
- Accepted and rejected event counts.

## Unity Menu Check

Use:

```text
Tools > MOBA Manager > Print Runtime State Debug Report
```

Expected key lines:

- Runtime state debug report OK
- Hero State: 10
- Unit State: 18
- Blue Unit: 9
- Red Unit: 9
- Event Count: 4
- Accepted Event: 3
- Rejected Event: 1

## Boundaries

This layer must not:

- Modify runtime state.
- Serialize save data.
- Replay commands.
- Simulate combat.
- Run AI decisions.
- Create Unity scenes or GameObjects.

## Design Notes

`RuntimeStateDebugReport` depends on `RuntimeQueryService` and optionally reads `RuntimeEventLog`. It returns a string so any future editor, console, or debug UI can decide how to display it.

## Next Step

After this report passes, the next safe step is a Phase 1 consolidation checkpoint: one menu action that runs validation, runtime build, query, command validation, event log, and debug report checks together.
