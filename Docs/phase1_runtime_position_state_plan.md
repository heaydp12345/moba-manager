# Phase 1 Runtime Position State Plan

## Goal

Build the first runtime position state.

這一步讓 hero state 擁有目前位置，並讓 match state 可以列出場上的 hero、tower、nexus 位置。它只保存與查詢位置，不做移動、尋路、碰撞、視野或 GameObject。

## Position Scope

Runtime position state includes:

- Entity id
- Entity type
- Team side
- Position point

Supported entity types:

- hero
- tower
- nexus

## Out Of Scope

Do not implement:

- Movement
- Pathfinding
- Collision
- Vision
- Targeting
- Combat logic
- AI logic
- Scene or GameObject creation

## Demo Defaults

The first smoke test uses conservative defaults:

- Blue hero states start at the blue nexus position.
- Red hero states start at the red nexus position.
- Tower and nexus positions come from map data.
- No position changes are performed.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Create Runtime Position State
```

Expected success output includes:

```text
Runtime position state create OK
Hero Position: 10
Tower Position: 6
Nexus Position: 2
Total Position: 18
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Match runtime state creation still passes in Unity.
- Runtime position state creation passes in Unity.
- No movement logic is created.
- No pathfinding logic is created.
- No combat or AI logic is created.
- No Unity Scene is created or modified.
