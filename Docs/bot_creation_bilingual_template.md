# Bot Creation Bilingual Template

This template explains how to create Bot AI data during Phase 0.

這份模板說明如何在 Phase 0 建立 Bot AI 資料。

## 1. Bot Identity

Use one stable lowercase `id`. The `zhTw` field should be readable Traditional Chinese.

Bot 需要一個穩定的小寫 `id`。`zhTw` 欄位請直接寫成可讀的繁體中文。

```json
{
  "id": "new_fighter_bot",
  "name": {
    "zhTw": "新戰士 Bot",
    "en": "New Fighter Bot"
  }
}
```

## 2. Hero Pool And Role

`heroPool` references hero ids from hero data.

`heroPool` 會引用英雄資料中的 hero id。

```json
{
  "difficulty": "normal",
  "role": "fighter",
  "heroPool": [
    "darius"
  ]
}
```

Role options match hero roles: `tank`, `fighter`, `assassin`, `mage`, `marksman`, `support`.

定位選項與英雄定位相同：坦克、戰士、刺客、法師、射手、輔助。

## 3. Lane Preference

Lane preference is data only. It does not move the hero by itself.

路線偏好只是資料，不會自行移動英雄。

```json
{
  "lanePreference": {
    "primaryLane": "top",
    "secondaryLanes": [
      "jungle"
    ]
  }
}
```

## 4. Behavior Weights

Behavior weights are tuning values from `0` to `1`.

行為權重是 `0` 到 `1` 的調整值。

```json
{
  "behaviorWeights": {
    "lastHit": 0.65,
    "laning": 0.75,
    "retreat": 0.55,
    "teamFight": 0.7,
    "itemBuild": 0.6,
    "pushTower": 0.5,
    "jungle": 0.25,
    "objectiveControl": 0.45
  }
}
```

These values describe preference only. Do not implement AI decision logic in Phase 0.

這些數值只描述偏好。Phase 0 不實作 AI 決策邏輯。

## 5. Seed Policy

Use `matchSeed` when the bot should follow the match seed.

如果 Bot 要跟隨比賽 seed，使用 `matchSeed`。

```json
{
  "seedPolicy": "matchSeed"
}
```

Use `fixedSeed` only when you need a stable test bot.

只有需要固定測試 Bot 時才使用 `fixedSeed`。

## 6. Final Checklist

Before saving new bot data, check:

儲存新 Bot 資料前請確認：

- `id` uses lowercase snake_case.
- `heroPool` references existing hero ids.
- `preferredItemIds` references existing item ids.
- All behavior weights are between `0` and `1`.
- Thresholds are between `0` and `1`.
- No C# scripts or Unity Scenes are needed for Phase 0 bot data.

