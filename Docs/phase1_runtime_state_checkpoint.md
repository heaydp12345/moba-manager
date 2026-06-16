# Phase 1 Runtime State Checkpoint

## Summary

This checkpoint records the current Unity runtime state foundation. It verifies that formal JSON data can be loaded, cross-file references can be validated, runtime entity models can be built, and match-level runtime state containers can be created without starting combat, AI, pathfinding, or scene logic.

## Current Scope

- Data loading from formal JSON files in `Assets/Data`.
- Cross-file validation before runtime state creation.
- Runtime entity models for heroes, maps, nexuses, towers, and lane route nodes.
- Runtime match state with blue and red team containers.
- Runtime hero state containers for position, health, resource, skill slots, and item slots.
- Runtime unit registry for heroes, towers, and nexuses.
- Manual match clock state for smoke testing only.

## Unity Menu Checks

- `Tools > MOBA Manager > Validate Game Data`
- `Tools > MOBA Manager > Build Runtime Entity Model`
- `Tools > MOBA Manager > Create Team Runtime State`
- `Tools > MOBA Manager > Create Match Runtime State`
- `Tools > MOBA Manager > Test Match Clock State`
- `Tools > MOBA Manager > Create Runtime Position State`
- `Tools > MOBA Manager > Create Runtime Health State`
- `Tools > MOBA Manager > Create Runtime Resource State`
- `Tools > MOBA Manager > Create Runtime Skill Slot State`
- `Tools > MOBA Manager > Create Runtime Item Slot State`
- `Tools > MOBA Manager > Create Runtime Unit Registry`
- `Tools > MOBA Manager > Run Runtime State Checkpoint`

## Checkpoint Expected Output

The aggregate checkpoint should report:

- Hero Model: 1
- Map Model: 1
- Match State: 1
- Team State: 2
- Hero State: 10
- Skill Slot State: 50
- Item Slot State: 30
- Position State: 18
- Health State: 18
- Resource State: 10
- Unit Registry: 18
- Blue Unit: 9
- Red Unit: 9
- Clock Completed: True

## Boundaries

This checkpoint must not:

- Create Unity scenes.
- Spawn GameObjects.
- Run combat.
- Run AI behavior.
- Run pathfinding.
- Apply damage, healing, buffs, or item effects.
- Change JSON schema or formal data shape.

## Notes

The current demo state intentionally reuses the available formal hero and team data to create a complete 5v5 runtime shape. This is acceptable for Phase 1 state verification, but it is not final game content.

The match clock is manually advanced in the smoke test to verify state transitions. It is not a gameplay loop.

## Validation

Run the formal data validation first:

```powershell
node Tools\ValidateData\validate_data.js
```

Then run the Unity aggregate check:

```text
Tools > MOBA Manager > Run Runtime State Checkpoint
```

## Next Step

After this checkpoint passes, the next safe step is a read-only runtime query layer. It should expose common lookups such as units by team, units by type, hero state by instance id, and map structures by team, without adding combat decisions or mutation logic.
