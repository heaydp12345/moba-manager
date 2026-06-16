# Phase 1 Runtime Item Slot State Plan

## Goal

Strengthen runtime item slot state.

這一步整理英雄裝備槽狀態，讓每個槽都有 slot index、是否為空、item id、item reference。它不做裝備效果、合成、屬性套用、裝備或卸裝規則。

## Item Slot Scope

Runtime item slot state includes:

- Slot index
- Empty state
- Item id
- Item reference

## Out Of Scope

Do not implement:

- Equipping rules
- Unequipping rules
- Item effects
- Item stat application
- Item recipe or combining
- Combat logic
- AI logic
- Scene or GameObject creation

## Demo Defaults

The first smoke test uses current formal data:

- Each hero state has 3 item slots.
- All item slots start empty.
- Formal item references are available for future assignment.
- No item is equipped in this step.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Item Slot State
```

Expected success output includes:

```text
Runtime item slot state create OK
Hero State: 10
Item Slot State: 30
Empty Item Slot: 30
Filled Item Slot: 0
Formal Item Reference: 3
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Hero runtime state creation still passes in Unity.
- Runtime item slot state creation passes in Unity.
- No item effect logic is created.
- No recipe or combining logic is created.
- No combat or AI logic is created.
- No Unity Scene is created or modified.
