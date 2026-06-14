# ROADMAP.md

# Phase 0

Data Architecture

狀態：大部分核心模擬器資料架構已完成。

## Step 1

hero.json

狀態：完成

已建立：

* hero.schema.json
* hero.example.json
* hero.json

---

## Step 2

skill.json

狀態：完成

已建立：

* skill.schema.json
* skill.example.json
* skill.json

技能等級規則：

* Passive：1 等
* Q / W / E：4 等
* R：3 等

---

## Step 3

item.json

狀態：完成

已建立：

* item.schema.json
* item.example.json
* item.json

裝備規則：

* 每位英雄最多 3 件裝備
* 9 種散件
* 2 個散件合成 1 件成裝

---

## Step 4

buff.json

狀態：完成

已建立：

* buff.schema.json
* buff.example.json
* buff.json

Buff 類型：

* 增益
* 減益
* 控制
* DOT
* HOT

---

## Step 5

map.json

狀態：完成

已建立：

* map.schema.json
* map.example.json
* map.json

地圖資料包含：

* 藍方主堡
* 紅方主堡
* 上路
* 中路
* 下路
* 野區
* 防禦塔
* 小兵路線節點
* 視野設定預留欄位

---

## Step 6

bot.json

狀態：完成

已建立：

* bot.schema.json
* bot.example.json
* bot.json

Bot 資料包含：

* 英雄池
* 路線偏好
* 行為權重
* 決策門檻
* 出裝偏好
* Seed policy

---

## Step 7

Data Validation

狀態：完成

已建立：

* Tools/ValidateData/validate_data.js
* Docs/data_validation_guide.md

驗證內容：

* JSON 語法
* Schema 結構
* hero -> skill 引用
* skill -> buff 引用
* bot -> hero / item 引用
* map route -> lane node 引用

---

## Optional Phase 0 Extensions

狀態：未開始

AGENTS.md 必要資料中仍有下列資料尚未建立 schema：

* league.json
* team.json
* player.json

這些資料較偏向後續聯賽與經營系統。若嚴格完成所有必要資料檔案，可先建立：

* league.schema.json
* team.schema.json
* player.schema.json

---

# Phase 1

MOBA Core

狀態：未開始

Phase 1 目標：

建立可以讀取 Phase 0 JSON 資料的 MOBA 比賽模擬核心。

## Step 1

2D Map Core

目標：

* 載入 map.json
* 建立主堡、三路、野區、防禦塔資料模型
* 建立路線節點資料結構
* 不先實作完整視野系統

---

## Step 2

Entity Data Model

目標：

* Hero
* Minion
* Monster
* Tower
* Nexus

---

## Step 3

Hero Runtime Model

目標：

* 載入 hero.json
* 載入 skill.json
* 載入 buff.json
* 建立等級、能力值、技能引用

---

## Step 4

Item Runtime Model

目標：

* 載入 item.json
* 支援散件
* 支援成裝
* 支援每位英雄最多 3 件裝備

---

## Step 5

Combat Foundation

目標：

* 普通攻擊
* 物理傷害
* 魔法傷害
* 真實傷害
* Buff / Debuff 資料套用
* 基礎傷害計算模組

---

## Step 6

Minion And Tower Foundation

目標：

* 小兵出兵資料
* 小兵沿路線節點移動
* 防禦塔攻擊範圍
* 防禦塔基礎仇恨資料

---

## Step 7

Bot Data Loading

目標：

* 載入 bot.json
* 讀取行為權重
* 讀取路線偏好
* 不先實作完整 AI 決策

---

# Phase 2

Bot System

狀態：未開始

目標：

讓 10 名 Bot 可以根據資料與規則自動完成 5v5 比賽。

---

# Phase 3

Player Career

狀態：未開始

---

# Phase 4

Career Transition

狀態：未開始

---

# Phase 5

Team Manager

狀態：未開始

注意：

在 MOBA 比賽模擬器完成前，不開始經理模式。
