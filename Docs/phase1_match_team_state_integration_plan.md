# Phase 1 Match Team State Integration Plan

## Goal

Upgrade match runtime state to hold team runtime state.

這一步把 `RuntimeMatchState` 從直接保存 blue/red hero state list，升級為保存 blue/red `RuntimeTeamState`。Hero state 仍可透過 team state 取得。這是結構整理，不是比賽邏輯。

## Integration Scope

Match runtime state includes:

- Match config
- Map model
- Blue runtime team state
- Red runtime team state
- Derived blue hero states
- Derived red hero states
- Manual clock state

## Out Of Scope

Do not implement:

- Team matchmaking
- Draft or pick logic
- Player rating calculation
- Strategy execution
- Combat logic
- AI logic
- Scene or GameObject creation

## Demo Build Defaults

The first smoke test uses conservative demo defaults:

- Use the first available formal team data for both blue and red.
- Use each player's first listed hero from `heroPool`.
- Keep this as a container test only.

## Unity Entry Point

Use the existing editor menu item:

```text
Tools/MOBA Manager/Create Match Runtime State
```

Expected success output includes:

```text
Match runtime state create OK
Match State: 1
Blue Team: taipei_titans
Red Team: taipei_titans
Blue Hero State: 5
Red Hero State: 5
Total Hero State: 10
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Team runtime state creation still passes in Unity.
- Match runtime state creation passes in Unity.
- Match clock smoke test still passes in Unity.
- No combat logic is created.
- No AI logic is created.
- No Unity Scene is created or modified.
