# Phase 1 Runtime Mutation Boundary Plan

## Summary

The runtime mutation boundary defines the first controlled entry point for changing runtime state. It keeps mutation explicit and narrow so future systems do not write directly into state objects from scattered code.

This step is still not combat, AI, pathfinding, or simulation logic.

## Current Commands

- Set hero position for data setup.
- Set skill level for data setup.
- Set item slot for data setup.

These commands exist to verify that runtime state can be changed through one service. They are not final gameplay commands.

## Non-Goals

This boundary must not:

- Calculate damage.
- Apply healing.
- Apply buffs or debuffs.
- Choose targets.
- Resolve attacks.
- Run AI decisions.
- Move units through pathfinding.
- Create Unity scenes or GameObjects.

## Unity Menu Check

Use:

```text
Tools > MOBA Manager > Test Runtime Mutation Boundary
```

Expected output:

- Command Success: 3
- Position Updated: True
- Q Skill Level: 1
- Filled Item Slot: 1
- Equipped Item: a valid item id

## Design Notes

`RuntimeMutationCommandService` depends on `RuntimeQueryService`. It first finds the relevant runtime state, validates the requested target, then calls the narrow data setup method on that state.

The command result is explicit. A command returns success or failure with a message instead of silently doing nothing.

## Next Step

After this boundary passes, the next safe step is runtime command validation rules. That layer should centralize checks such as valid team, valid hero id, valid item slot, and valid skill level before any future combat or AI systems use commands.
