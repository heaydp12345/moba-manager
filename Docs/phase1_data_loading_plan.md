# Phase 1 Data Loading Plan

## Goal

Build the first Unity-side data loading layer for Phase 1.

目標是在 Unity 端建立第一版資料載入層。

This step only reads Phase 0 JSON data, builds read-only runtime lookup tables, and validates cross-file references.

這一步只讀取 Phase 0 JSON 資料，建立唯讀查找表，並驗證跨檔案引用。

## Out Of Scope

Do not implement:

不要實作：

- Combat logic
- Bot decision logic
- Pathfinding
- Scene generation
- Team manager gameplay
- Save file format

## Load Order

The load order is fixed:

資料載入順序固定為：

1. Buff
2. Skill
3. Hero
4. Item
5. Match
6. Map
7. Bot
8. Player
9. Team
10. League

## Unity Entry Point

Use the editor menu item:

使用 Editor 選單：

```text
Tools/MOBA Manager/Validate Game Data
```

Expected success output:

成功時應在 Console 看到：

```text
Game data loaded successfully.
Hero: 1
Skill: 5
Item: 3
Buff: 2
Match: 1
Map: 1
Bot: 1
Player: 5
Team: 1
League: 1
Cross references OK
```

## Data Source

Unity reads the official JSON files under `Assets/Data`.

Unity 會讀取 `Assets/Data` 下的正式 JSON。

Example files are not loaded by the runtime data loader.

runtime data loader 不讀取 example 檔案。

## Validation Rules

Unity validates:

Unity 端驗證：

- Hero skill ids exist in skill data.
- Skill buff ids exist in buff data.
- Bot hero and item ids exist.
- Player hero and team ids exist.
- Team roster and substitute player ids exist.
- League team ids exist.
- Map minion route node ids exist in the matching lane.

## File Layout

Recommended layout:

建議檔案位置：

```text
Assets/Scripts/Data/GameDataModels.cs
Assets/Scripts/Data/GameDataDatabase.cs
Assets/Scripts/Data/GameDataLoader.cs
Assets/Scripts/Data/GameDataValidator.cs
Assets/Scripts/Data/Editor/DataLoadSmokeTest.cs
```

## Acceptance

This phase is complete when:

完成條件：

- Node data validation passes.
- Unity editor data loading passes.
- Unity Console prints the expected counts.
- No C# gameplay logic is created.
- No Unity Scene is created or modified.
