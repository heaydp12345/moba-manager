# Phase 1 Status

## Summary

Phase 1 currently has a Unity runtime foundation that can load formal JSON data, validate cross-file references, build read-only runtime models, create match state containers, run controlled setup commands, record command results, and print debug reports.

This is still foundation work. It is not combat, AI, pathfinding, replay, save serialization, UI, or manager mode.

## Completed Systems

- Formal data loading through `GameDataLoader`.
- Cross-file reference validation through `GameDataValidator`.
- Runtime entity model build through `RuntimeEntityFactory`.
- Runtime hero, team, match, map, tower, and nexus containers.
- Runtime position, health, resource, skill slot, and item slot states.
- Runtime unit registry.
- Runtime query service.
- Runtime command validation.
- Runtime mutation command boundary.
- Runtime event log.
- Runtime state debug report.
- Phase 1 consolidation checkpoint.

## Unity Menu Checks

- `Tools > MOBA Manager > Validate Game Data`
- `Tools > MOBA Manager > Build Runtime Entity Model`
- `Tools > MOBA Manager > Create Hero Runtime State`
- `Tools > MOBA Manager > Create Team Runtime State`
- `Tools > MOBA Manager > Create Match Runtime State`
- `Tools > MOBA Manager > Test Match Clock State`
- `Tools > MOBA Manager > Create Runtime Position State`
- `Tools > MOBA Manager > Create Runtime Health State`
- `Tools > MOBA Manager > Create Runtime Resource State`
- `Tools > MOBA Manager > Create Runtime Skill Slot State`
- `Tools > MOBA Manager > Create Runtime Item Slot State`
- `Tools > MOBA Manager > Create Runtime Unit Registry`
- `Tools > MOBA Manager > Test Runtime Query Service`
- `Tools > MOBA Manager > Test Runtime Mutation Boundary`
- `Tools > MOBA Manager > Test Runtime Command Validation`
- `Tools > MOBA Manager > Test Runtime Event Log`
- `Tools > MOBA Manager > Print Runtime State Debug Report`
- `Tools > MOBA Manager > Run Runtime State Checkpoint`
- `Tools > MOBA Manager > Run Phase 1 Consolidation Checkpoint`

## Primary Acceptance Check

Use this as the main Unity-side checkpoint:

```text
Tools > MOBA Manager > Run Phase 1 Consolidation Checkpoint
```

Expected key output:

- `Phase 1 consolidation checkpoint OK`
- `Data Validation: OK`
- `Hero Model: 1`
- `Map Model: 1`
- `Hero State: 10`
- `Unit State: 18`
- `Blue Unit: 9`
- `Red Unit: 9`
- `Command Validation: OK`
- `Event Count: 4`
- `Accepted Event: 3`
- `Rejected Event: 1`
- `Debug Report: OK`

## Command-Line Validation

Run:

```powershell
node Tools\ValidateData\validate_data.js
```

Expected result:

```text
All data validation checks passed.
```

## Current Boundaries

Do not add these yet:

- Combat simulation.
- Damage or healing formulas.
- Buff or debuff application.
- AI decisions.
- Pathfinding.
- Minion spawning.
- Tower targeting.
- Jungle monster behavior.
- Save serialization.
- Replay logic.
- Unity scenes or GameObjects.
- Manager mode.

## Known Limitations

- Demo runtime state reuses the available formal hero and team data to create a 5v5 shape.
- Resource max values are still placeholders because the current data schema does not define full resource pools.
- Runtime commands are marked as data setup commands and are not final gameplay commands.
- Event log is in-memory only and is not a save or replay format.

## Next Safe Step

After PR #2 is reviewed and merged, the next safe technical step is `Runtime Tick Pipeline`.

That step should only introduce a deterministic tick entry point and ordered phase list. It should not calculate damage, move units, run AI, or spawn units yet.
