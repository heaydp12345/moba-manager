# Data Validation Guide

This guide explains how to validate Phase 0 data files.

這份文件說明如何驗證 Phase 0 的資料檔案。

## What Gets Validated

The validator checks official data files, not example files.

驗證工具會檢查正式資料檔，不檢查 example 檔。

- `Assets/Data/Heroes/hero.json`
- `Assets/Data/Skills/skill.json`
- `Assets/Data/Items/item.json`
- `Assets/Data/Buffs/buff.json`
- `Assets/Data/Matches/match.json`
- `Assets/Data/Maps/map.json`
- `Assets/Data/Bots/bot.json`

## How To Run

Run this command from the project root:

請在專案根目錄執行：

```powershell
node Tools/ValidateData/validate_data.js
```

Expected success output:

成功時會看到類似輸出：

```text
Assets/Data/Heroes/hero.json OK
Assets/Data/Skills/skill.json OK
Assets/Data/Items/item.json OK
Assets/Data/Buffs/buff.json OK
Assets/Data/Matches/match.json OK
Assets/Data/Maps/map.json OK
Assets/Data/Bots/bot.json OK
Cross-file references OK
All data validation checks passed.
```

## Cross-File Checks

The validator also checks references between files.

驗證工具也會檢查跨檔案引用。

- Hero skill ids must exist in `skill.json`.
- Skill `buffId` values must exist in `buff.json`.
- Bot hero ids must exist in `hero.json`.
- Bot item ids must exist in `item.json`.
- Map minion route node ids must exist in the matching lane path nodes.

## Common Errors

常見錯誤：

- `missing required property`: a required schema field is missing.
- `additional property`: the JSON contains a field not allowed by the schema.
- `unknown skill id`: a hero references a skill id that does not exist.
- `unknown buff id`: a skill references a buff id that does not exist.
- `unknown item id`: a bot references an item id that does not exist.
- `unknown path node id`: a map route references a lane node id that does not exist.

## Phase 0 Rule

Validation is a development tool only. It must not add C# gameplay logic or Unity Scenes.

驗證工具只是開發輔助。Phase 0 不加入 C# 遊戲邏輯，也不建立 Unity Scene。
