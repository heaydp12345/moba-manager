# Phase 1 Runtime Resource State Plan

## Goal

Build the first runtime resource state.

這一步讓 hero 擁有統一的資源狀態，並讓 match state 可以列出所有英雄資源。它只保存與查詢資源，不做技能消耗、自然回復、施法或資源規則。

## Resource Scope

Runtime resource state includes:

- Entity id
- Resource type
- Current value
- Max value

Supported resource types come from hero data, such as:

- mana
- energy
- none

## Out Of Scope

Do not implement:

- Skill resource cost
- Resource regeneration
- Resource spending
- Resource gain
- Skill casting
- Combat logic
- AI logic
- Scene or GameObject creation

## Demo Defaults

The current hero schema has `resourceType`, but does not yet define max resource values.

Conservative defaults:

- `none` starts at 0 / 0.
- `mana` starts at 0 / 0 until max resource data exists.
- `energy` starts at 0 / 0 until max resource data exists.
- No resource changes are performed.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Resource State
```

Expected success output includes:

```text
Runtime resource state create OK
Hero Resource: 10
Mana Resource: 10
Total Resource: 10
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Match runtime state creation still passes in Unity.
- Runtime resource state creation passes in Unity.
- No skill cost logic is created.
- No regeneration logic is created.
- No combat or AI logic is created.
- No Unity Scene is created or modified.
