# Phase 1 Runtime Command Validation Plan

## Summary

Runtime command validation centralizes the checks that must happen before runtime mutation commands change state. This keeps mutation commands small and makes future systems share the same validation rules.

This layer still does not implement combat, AI, pathfinding, damage, healing, or item effects.

## Current Validation Rules

- Hero state id must exist.
- Hero state id cannot be empty.
- Skill slot must exist on the hero.
- Skill level cannot be negative.
- Skill level cannot exceed the skill max level.
- Item reference cannot be null.
- Item slot index must exist on the hero.

## Unity Menu Check

Use:

```text
Tools > MOBA Manager > Test Runtime Command Validation
```

Expected output:

- Valid Check: 3
- Invalid Check: 4
- Missing Hero Blocked: True
- Invalid Skill Slot Blocked: True
- Invalid Skill Level Blocked: True
- Invalid Item Slot Blocked: True

The existing mutation boundary check should also still pass:

```text
Tools > MOBA Manager > Test Runtime Mutation Boundary
```

## Boundaries

This layer must not:

- Apply damage.
- Apply healing.
- Apply buffs or debuffs.
- Decide targets.
- Equip items based on strategy.
- Move units through pathfinding.
- Create Unity scenes or GameObjects.

## Design Notes

`RuntimeCommandValidator` depends on `RuntimeQueryService`. It only reads state and returns `RuntimeCommandValidationResult`.

`RuntimeMutationCommandService` now delegates validation before applying setup-style state changes.

## Next Step

After command validation passes, the next safe step is a runtime event log for command results. It should record what command was accepted or rejected without simulating combat.
