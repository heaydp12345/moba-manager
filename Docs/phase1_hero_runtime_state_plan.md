# Phase 1 Hero Runtime State Plan

## Goal

Build the first hero runtime state container.

這一步把英雄從只讀資料模型推進到「比賽中可追蹤的狀態容器」。它只保存狀態，不計算傷害、不施放技能、不處理 AI、不套用裝備效果。

## State Scope

Hero runtime state includes:

- Runtime instance id
- Hero model reference
- Team side
- Level
- Current health
- Current resource
- Skill levels by slot
- Item slots

## Out Of Scope

Do not implement:

- Damage or healing logic
- Skill casting logic
- Level up rules
- Item effects
- Buff application
- Bot behavior
- Scene or GameObject creation

## Defaults

The first version uses conservative defaults:

- Level starts at 1.
- Current health starts from hero base max health.
- Current resource starts at 0 because max mana is not present in the current data schema.
- All skill levels start at 0.
- Item slots are empty and sized from match `maxItems`.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Hero Runtime State
```

Expected success output includes:

```text
Hero runtime state create OK
Hero State: 1
Skill Slot State: 5
Item Slot State: 3
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Runtime entity model build still passes in Unity.
- Hero runtime state creation passes in Unity.
- No combat logic is created.
- No AI logic is created.
- No Unity Scene is created or modified.
