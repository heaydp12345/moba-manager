# Phase 1 Team Runtime State Plan

## Goal

Build the first team runtime state container.

這一步把 team data、player data、hero runtime state 組成比賽中的隊伍狀態。它只保存隊伍狀態，不建立 BP、不做戰術、不做 AI、不進入經理模式。

## State Scope

Team runtime state includes:

- Runtime team side
- Team data reference
- Roster slots
- Player reference per lane
- Hero state per lane
- Substitute player ids

## Out Of Scope

Do not implement:

- Draft or pick logic
- Player ability calculation
- Training or manager gameplay
- Bot decisions
- Combat logic
- Scene or GameObject creation

## Demo Build Defaults

The first smoke test uses conservative demo defaults:

- Use the first available team data.
- Use roster order from `team.json`.
- Assign each player the first hero in their `heroPool`.
- Reuse the same formal team data for blue and red demo team states.
- This is only a runtime container test, not real matchmaking.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Team Runtime State
```

Expected success output includes:

```text
Team runtime state create OK
Blue Team: taipei_titans
Red Team: taipei_titans
Blue Roster Slot: 5
Red Roster Slot: 5
Total Hero State: 10
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Runtime entity model build still passes in Unity.
- Match runtime state creation still passes in Unity.
- Team runtime state creation passes in Unity.
- No combat logic is created.
- No AI logic is created.
- No Unity Scene is created or modified.
