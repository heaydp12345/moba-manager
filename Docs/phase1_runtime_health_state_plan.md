# Phase 1 Runtime Health State Plan

## Goal

Build the first runtime health state.

這一步讓 hero、tower、nexus 都有統一的生命值狀態，並讓 match state 可以列出所有可查詢生命值。它只保存與查詢生命值，不做扣血、治療、死亡、重生或勝負判定。

## Health Scope

Runtime health state includes:

- Entity id
- Entity type
- Team side
- Current health
- Max health

Supported entity types:

- hero
- tower
- nexus

## Out Of Scope

Do not implement:

- Damage
- Healing
- Death checks
- Respawn
- Nexus destruction win condition
- Combat logic
- AI logic
- Scene or GameObject creation

## Demo Defaults

The first smoke test uses conservative defaults:

- Hero current health starts at hero base max health.
- Tower current health starts at tower max health.
- Nexus current health starts at nexus max health.
- No health changes are performed.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Health State
```

Expected success output includes:

```text
Runtime health state create OK
Hero Health: 10
Tower Health: 6
Nexus Health: 2
Total Health: 18
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Match runtime state creation still passes in Unity.
- Runtime health state creation passes in Unity.
- No damage logic is created.
- No healing logic is created.
- No death or win logic is created.
- No Unity Scene is created or modified.
