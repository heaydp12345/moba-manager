# Phase 1 Match Clock State Plan

## Goal

Build the first manual match clock state.

這一步只讓 match runtime state 可以手動開始與手動推進時間。它不建立 update loop、不接 Unity Scene、不觸發戰鬥、不觸發 AI。

## Clock Scope

Match clock state includes:

- Elapsed time
- Match time limit
- Started flag
- Completed flag
- Manual `Start` action
- Manual `Tick` action

## Out Of Scope

Do not implement:

- Unity `Update`
- Coroutine
- Combat events
- Bot decisions
- Skill cooldown ticking
- Minion spawning
- Win condition checks
- Scene or GameObject creation

## Rules

- A new match starts with elapsed time `0`.
- Calling `Start` sets `IsStarted` to true.
- Calling `Tick` before start does nothing.
- Negative delta time is rejected.
- Calling `Tick` after completed does nothing.
- Elapsed time is clamped to `matchTimeLimit`.
- Reaching `matchTimeLimit` sets `IsCompleted` to true.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Test Match Clock State
```

Expected success output includes:

```text
Match clock state test OK
Started: True
Completed: True
Elapsed Time: 1800
Time Limit: 1800
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Runtime entity model build still passes in Unity.
- Match runtime state creation still passes in Unity.
- Match clock smoke test passes in Unity.
- No combat logic is created.
- No AI logic is created.
- No Unity Scene is created or modified.
