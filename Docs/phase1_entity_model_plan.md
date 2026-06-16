# Phase 1 Entity Model Plan

## Goal

Build the first Unity-side runtime entity model for MOBA match data.

這一步的目標是把已經通過驗證的 JSON 資料，轉成 Unity 端可讀取的 runtime 資料模型。它只建立資料容器，不建立戰鬥、AI、尋路、出兵或 Scene 物件。

## Data Source

The source is the official data loaded by `GameDataLoader`.

資料來源固定為 `Assets/Data` 底下的正式 JSON：

- `hero.json`
- `skill.json`
- `buff.json`
- `item.json`
- `match.json`
- `map.json`
- `bot.json`
- `player.json`
- `team.json`
- `league.json`

Example JSON files are not used by this runtime layer.

## Runtime Model Scope

The runtime entity model supports:

- Hero runtime data containers
- Map runtime data containers
- Nexus runtime data containers
- Tower runtime data containers
- Lane path node containers
- Minion wave route node references

The model does not support:

- Combat calculation
- Bot decisions
- Pathfinding
- Minion spawning
- Tower targeting
- Nexus destruction win checks
- Unity Scene or GameObject creation

## Build Flow

The build flow is:

1. Load data with `GameDataLoader`.
2. Validate references with `GameDataValidator`.
3. Build runtime containers with `RuntimeEntityFactory`.
4. Print model counts in Unity Console.

## Unity Entry Point

Use the editor menu item:

```text
Tools/MOBA Manager/Build Runtime Entity Model
```

Expected success output includes:

```text
Runtime entity model build OK
Hero Model: 1
Map Model: 1
Nexus: 2
Tower: 6
Lane Route Node: 15
```

## Acceptance

This step is complete when:

- Node data validation passes.
- Unity data validation passes.
- Runtime entity model build passes.
- Unity Console prints runtime model counts.
- No gameplay logic is created.
- No Unity Scene is created or modified.
- Existing JSON schemas remain unchanged.
