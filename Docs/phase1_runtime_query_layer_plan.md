# Phase 1 Runtime Query Layer Plan

## Summary

The runtime query layer provides read-only access to the current match state. It is a small service that wraps `RuntimeMatchState` and `RuntimeUnitRegistry` so future systems can look up units, heroes, roster slots, and map route nodes without duplicating query code.

## Scope

- Query all runtime units.
- Query units by team.
- Query units by type.
- Query units by team and type.
- Query hero state by runtime instance id.
- Query hero states by team.
- Query towers and nexus by team.
- Query roster slots by team.
- Query lane path nodes by lane type.

## Non-Goals

This layer must not:

- Move units.
- Apply damage or healing.
- Level skills.
- Equip items.
- Choose targets.
- Run AI decisions.
- Run combat or pathfinding.
- Create GameObjects or Unity scenes.

## Unity Menu Check

Use:

```text
Tools > MOBA Manager > Test Runtime Query Service
```

Expected output:

- All Unit: 18
- Blue Hero: 5
- Red Hero: 5
- Blue Tower: 3
- Red Tower: 3
- Hero Unit: 10
- Top Lane Node: 5
- Try Get Hero: True
- Try Get Unit: True
- Queried Hero Matches Unit: True

## Design Notes

`RuntimeQueryService` returns existing runtime state objects. It does not copy or mutate them. This keeps the layer thin and makes it safe to use as a shared read model for future combat, AI, or UI debug tools.

The service currently builds its own lookup dictionary for hero instance ids. Unit lookups continue to use `RuntimeUnitRegistry`.

## Next Step

After this query layer passes, the next safe step is a command boundary for controlled runtime mutations. That boundary should define what may change state, but still avoid implementing combat rules until the state mutation API is explicit.
