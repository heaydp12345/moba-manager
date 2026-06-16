# Phase 1 Runtime Unit Registry Plan

## Goal

Build the first runtime unit registry.

這一步把 hero、tower、nexus 的 position、health、team、entity type 統一登記成可查詢清單。它只建立索引，不做目標選擇、距離判定、攻擊範圍、戰鬥或 AI。

## Registry Scope

Runtime unit registry includes:

- Unit id
- Unit type
- Team side
- Position state
- Health state

Supported unit types:

- hero
- tower
- nexus

## Queries

The registry supports:

- All units
- Units by type
- Units by team
- Unit by id

## Out Of Scope

Do not implement:

- Target selection
- Attack range checks
- Distance sorting
- Threat logic
- Combat logic
- AI logic
- Scene or GameObject creation

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Unit Registry
```

Expected success output includes:

```text
Runtime unit registry create OK
All Unit: 18
Hero Unit: 10
Tower Unit: 6
Nexus Unit: 2
Blue Unit: 9
Red Unit: 9
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Runtime position and health states still pass in Unity.
- Runtime unit registry creation passes in Unity.
- No target selection logic is created.
- No combat or AI logic is created.
- No Unity Scene is created or modified.
