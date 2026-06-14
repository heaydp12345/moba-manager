# Map Creation Bilingual Template

This template explains how to create a new MOBA map data file during Phase 0.

這份模板說明如何在 Phase 0 建立新的 MOBA 地圖資料。

## 1. Map Identity

Use one stable lowercase `id` for the map. The `zhTw` field should be readable Traditional Chinese.

地圖需要一個穩定的小寫 `id`。`zhTw` 欄位請直接寫成可讀的繁體中文。

```json
{
  "id": "standard_moba_map",
  "name": {
    "zhTw": "標準 MOBA 地圖",
    "en": "Standard MOBA Map"
  },
  "mapType": "moba"
}
```

## 2. Coordinates

Map data uses 2D coordinates only.

地圖資料目前只使用 2D 座標。

```json
{
  "position": {
    "x": 0,
    "y": 0
  }
}
```

Coordinates are data only. Do not add pathfinding code in Phase 0.

座標只是資料。Phase 0 不加入尋路程式。

## 3. Required Map Parts

Every MOBA map should define these data groups.

每張 MOBA 地圖都應該定義以下資料群組。

- `nexuses`: blue nexus and red nexus
- `lanes`: top, mid, bottom
- `towers`: lane towers for both teams
- `jungleZones`: blue jungle, red jungle, neutral objective zones
- `minionWaveRoutes`: route node ids for minion waves
- `vision`: reserved vision settings

## 4. Lane Example

Each lane owns path nodes. Minion routes reference those node ids.

每條路線擁有自己的路線節點。小兵路線會引用這些節點 id。

```json
{
  "id": "mid_lane",
  "name": {
    "zhTw": "中路",
    "en": "Mid Lane"
  },
  "laneType": "mid",
  "pathNodes": [
    {
      "id": "mid_blue_base",
      "position": {
        "x": -7200,
        "y": -7200
      }
    },
    {
      "id": "mid_center",
      "position": {
        "x": 0,
        "y": 0
      }
    },
    {
      "id": "mid_red_base",
      "position": {
        "x": 7200,
        "y": 7200
      }
    }
  ]
}
```

## 5. Final Checklist

Before saving a new map, check:

儲存新地圖前請確認：

- There are exactly two nexuses: one blue and one red.
- There are three lanes: top, mid, bottom.
- Each lane has at least two path nodes.
- `minionWaveRoutes` references existing lane node ids.
- Towers have team, lane, position, range, damage, and health.
- Vision is only reserved data. Do not implement vision logic yet.
- Do not create C# scripts or Unity Scenes for Phase 0 map data.

