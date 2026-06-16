# Phase 1 Match Runtime State Plan

## Goal

Build the first match runtime state container.

這一步把 match 設定、map runtime model、blue team hero states、red team hero states 放進同一個比賽狀態容器。它只保存狀態，不推進時間、不判定勝負、不執行戰鬥、不建立 Unity Scene。

## State Scope

Match runtime state includes:

- Match instance id
- Match config reference
- Map model reference
- Blue hero states
- Red hero states
- Elapsed time
- Started flag
- Completed flag

## Out Of Scope

Do not implement:

- Match timer update loop
- Combat resolution
- Skill casting
- Bot behavior
- Win or loss checks
- Draft or pick logic
- Scene or GameObject creation

## Demo Build Defaults

The first smoke test uses conservative demo defaults:

- Use the first available match config.
- Use the first available map model.
- Create blue and red hero states from available hero models.
- Reuse available hero models if current data has fewer heroes than `teamSize`.
- Keep elapsed time at 0.
- Keep started and completed flags false.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Match Runtime State
```

Expected success output includes:

```text
Match runtime state create OK
Match State: 1
Blue Hero State: 5
Red Hero State: 5
Total Hero State: 10
Map Model: standard_moba_map
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Runtime entity model build still passes in Unity.
- Match runtime state creation passes in Unity.
- No combat logic is created.
- No AI logic is created.
- No Unity Scene is created or modified.
