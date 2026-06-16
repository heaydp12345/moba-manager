# Phase 1 Runtime Skill Slot State Plan

## Goal

Strengthen runtime skill slot state.

這一步整理英雄技能槽狀態，讓每個技能槽都有技能 id、slot、目前等級、等級上限，以及是否還能升級的純狀態判斷。它不做施法、冷卻、資源消耗或技能效果。

## Skill Slot Scope

Runtime skill slot state includes:

- Skill id
- Slot
- Current level
- Max level
- Can level up state check

Supported slots:

- passive
- q
- w
- e
- r

## Out Of Scope

Do not implement:

- Skill casting
- Cooldown ticking
- Resource cost checks
- Skill effects
- Buff application
- Damage or healing
- AI logic
- Scene or GameObject creation

## Demo Defaults

The first smoke test uses current formal data:

- Each hero state has 5 skill slot states.
- Current level starts at 0.
- Max level comes from skill data.
- Passive max level is 1.
- Q/W/E max level is 4.
- R max level is 3.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Skill Slot State
```

Expected success output includes:

```text
Runtime skill slot state create OK
Hero State: 10
Skill Slot State: 50
Passive Max Level: 1
Q Max Level: 4
W Max Level: 4
E Max Level: 4
R Max Level: 3
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Hero runtime state creation still passes in Unity.
- Runtime skill slot state creation passes in Unity.
- No casting logic is created.
- No cooldown logic is created.
- No combat or AI logic is created.
- No Unity Scene is created or modified.
