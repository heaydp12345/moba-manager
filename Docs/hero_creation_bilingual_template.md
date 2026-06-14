# Hero Creation Bilingual Template

This template explains how to create a new hero from zero using the current Phase 0 data files.

這份模板說明如何依照目前 Phase 0 的資料結構，從零建立一名新英雄。

## 1. Naming

Use one stable machine id everywhere.

所有資料都要使用同一個穩定的機器用 id。

```json
{
  "id": "new_hero_id",
  "name": {
    "zhTw": "新英雄名稱",
    "en": "New Hero Name"
  }
}
```

`zhTw` should be written in readable Traditional Chinese and saved as UTF-8.

`zhTw` 請直接寫成看得懂的繁體中文，並用 UTF-8 儲存。

## 2. Hero File

Create or extend `Assets/Data/Heroes/hero.example.json`.

建立或擴充 `Assets/Data/Heroes/hero.example.json`。

```json
{
  "id": "new_hero_id",
  "name": {
    "zhTw": "新英雄",
    "en": "New Hero"
  },
  "role": "fighter",
  "attackType": "melee",
  "damageProfile": "physical",
  "resourceType": "mana",
  "baseStats": {
    "attackDamage": 60,
    "abilityPower": 0,
    "armor": 35,
    "magicResist": 32,
    "maxHealth": 620,
    "manaRegen": 7,
    "critChance": 0,
    "lifeSteal": 0,
    "attackSpeed": 0.65,
    "moveSpeed": 340,
    "attackRange": 175
  },
  "growthStats": {
    "attackDamage": 4,
    "abilityPower": 0,
    "armor": 4,
    "magicResist": 2,
    "maxHealth": 100,
    "manaRegen": 0.3,
    "critChance": 0,
    "lifeSteal": 0,
    "attackSpeed": 0.02,
    "moveSpeed": 0.01,
    "attackRange": 0.01
  },
  "skills": {
    "passive": "new_hero_passive",
    "q": "new_hero_q",
    "w": "new_hero_w",
    "e": "new_hero_e",
    "r": "new_hero_r"
  }
}
```

Role options: `tank`, `fighter`, `assassin`, `mage`, `marksman`, `support`.

定位選項：坦克、戰士、刺客、法師、射手、輔助，資料中分別使用上面的英文 id。

## 3. Skill File

Create five skills for every hero: passive, Q, W, E, R.

每名英雄都需要五個技能：被動、Q、W、E、R。

Level rules:

等級規則：

- Passive max level: `1`
- Q/W/E max level: `4`
- R max level: `3`

```json
{
  "id": "new_hero_q",
  "name": {
    "zhTw": "技能名稱",
    "en": "Skill Name"
  },
  "description": {
    "zhTw": "技能說明",
    "en": "Skill description."
  },
  "slot": "q",
  "maxLevel": 4,
  "cooldown": [8, 7, 6, 5],
  "resourceCost": [40],
  "targeting": {
    "targetType": "enemy",
    "range": [500],
    "maxTargets": 1
  },
  "effects": [
    {
      "effectType": "damage",
      "damageType": "physical",
      "baseValue": [60, 90, 120, 150],
      "scaling": [
        {
          "stat": "attackDamage",
          "ratio": [1.0]
        }
      ]
    }
  ]
}
```

## 4. Buff File

If a skill applies a buff, debuff, control, DOT, or HOT, create it in the buff data.

如果技能會施加增益、減益、控制、DOT 或 HOT，需要在 Buff 資料中建立對應項目。

```json
{
  "id": "new_hero_bleed",
  "name": {
    "zhTw": "流血",
    "en": "Bleed"
  },
  "category": "dot",
  "duration": 4,
  "maxStacks": 3,
  "stackingRule": "addStack",
  "periodicEffect": {
    "effectType": "damage",
    "damageType": "physical",
    "tickInterval": 1,
    "value": 10
  }
}
```

## 5. Final Checklist

Before adding another hero, check:

新增英雄前請確認：

- Hero id and skill ids use lowercase snake_case.
- `hero.skills` references all five skill ids.
- Q/W/E use `maxLevel: 4`.
- R uses `maxLevel: 3`.
- Every `buffId` points to a buff entry.
- No C# or Unity Scene is needed for Phase 0 data work.
