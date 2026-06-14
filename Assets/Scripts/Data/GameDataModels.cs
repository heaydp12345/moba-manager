using System;

namespace MobaManager.Data
{
    [Serializable]
    public class LocalizedText
    {
        public string zhTw;
        public string en;
    }

    [Serializable]
    public class StatBlockData
    {
        public float attackDamage;
        public float abilityPower;
        public float armor;
        public float magicResist;
        public float maxHealth;
        public float manaRegen;
        public float critChance;
        public float lifeSteal;
        public float attackSpeed;
        public float moveSpeed;
        public float attackRange;
    }

    [Serializable]
    public class HeroSkillSetData
    {
        public string passive;
        public string q;
        public string w;
        public string e;
        public string r;
    }

    [Serializable]
    public class HeroData
    {
        public string id;
        public LocalizedText name;
        public string role;
        public string attackType;
        public string damageProfile;
        public string resourceType;
        public StatBlockData baseStats;
        public StatBlockData growthStats;
        public HeroSkillSetData skills;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class HeroDataFile
    {
        public string schemaVersion;
        public HeroData[] heroes;
    }

    [Serializable]
    public class ScalingData
    {
        public string stat;
        public float[] ratio;
    }

    [Serializable]
    public class TargetingData
    {
        public string targetType;
        public float[] range;
        public float[] radius;
        public int maxTargets;
    }

    [Serializable]
    public class SkillEffectData
    {
        public string effectType;
        public string damageType;
        public string controlType;
        public float[] baseValue;
        public float[] duration;
        public ScalingData[] scaling;
        public string buffId;
        public float[] projectileSpeed;
        public string[] tags;
    }

    [Serializable]
    public class SkillData
    {
        public string id;
        public LocalizedText name;
        public LocalizedText description;
        public string slot;
        public int maxLevel;
        public float[] cooldown;
        public float[] resourceCost;
        public float[] castTime;
        public TargetingData targeting;
        public SkillEffectData[] effects;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class SkillDataFile
    {
        public string schemaVersion;
        public SkillData[] skills;
    }

    [Serializable]
    public class StatModifiersData
    {
        public float attackDamage;
        public float abilityPower;
        public float armor;
        public float magicResist;
        public float maxHealth;
        public float manaRegen;
        public float critChance;
        public float lifeSteal;
        public float attackSpeed;
        public float moveSpeed;
        public float attackRange;
    }

    [Serializable]
    public class ItemRecipeData
    {
        public string[] componentItemIds;
    }

    [Serializable]
    public class ItemEffectData
    {
        public string effectType;
        public LocalizedText description;
        public string trigger;
        public float value;
        public string sourceStat;
        public string targetStat;
        public string damageType;
        public string buffId;
        public string[] tags;
    }

    [Serializable]
    public class ItemData
    {
        public string id;
        public LocalizedText name;
        public string tier;
        public string componentType;
        public ItemRecipeData recipe;
        public StatModifiersData stats;
        public ItemEffectData[] effects;
        public string notes;
    }

    [Serializable]
    public class ItemDataFile
    {
        public string schemaVersion;
        public int maxItemsPerHero;
        public string[] componentTypes;
        public ItemData[] items;
    }

    [Serializable]
    public class StatModifierData
    {
        public string stat;
        public string operation;
        public float value;
    }

    [Serializable]
    public class PeriodicEffectData
    {
        public string effectType;
        public string damageType;
        public float tickInterval;
        public float value;
        public string sourceStat;
        public float scalingRatio;
    }

    [Serializable]
    public class BuffData
    {
        public string id;
        public LocalizedText name;
        public LocalizedText description;
        public string category;
        public float duration;
        public int maxStacks;
        public string stackingRule;
        public string controlType;
        public StatModifierData[] statModifiers;
        public PeriodicEffectData periodicEffect;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class BuffDataFile
    {
        public string schemaVersion;
        public BuffData[] buffs;
    }

    [Serializable]
    public class MatchData
    {
        public string id;
        public LocalizedText name;
        public int teamSize;
        public int maxLevel;
        public int maxItems;
        public int banCount;
        public int pickCount;
        public float matchTimeLimit;
        public string gameMode;
        public string winCondition;
        public string seedPolicy;
        public string notes;
    }

    [Serializable]
    public class MatchDataFile
    {
        public string schemaVersion;
        public MatchData[] matches;
    }

    [Serializable]
    public class PointData
    {
        public float x;
        public float y;
    }

    [Serializable]
    public class MapSizeData
    {
        public float width;
        public float height;
    }

    [Serializable]
    public class NexusData
    {
        public string id;
        public string team;
        public PointData position;
        public float maxHealth;
    }

    [Serializable]
    public class PathNodeData
    {
        public string id;
        public PointData position;
    }

    [Serializable]
    public class LaneData
    {
        public string id;
        public LocalizedText name;
        public string laneType;
        public PathNodeData[] pathNodes;
    }

    [Serializable]
    public class TowerData
    {
        public string id;
        public string team;
        public string laneType;
        public PointData position;
        public float attackRange;
        public float attackDamage;
        public float maxHealth;
    }

    [Serializable]
    public class JungleZoneData
    {
        public string id;
        public LocalizedText name;
        public string side;
        public PointData center;
        public float radius;
        public string[] campIds;
    }

    [Serializable]
    public class MinionWaveRouteData
    {
        public string laneType;
        public string[] bluePathNodeIds;
        public string[] redPathNodeIds;
    }

    [Serializable]
    public class VisionSettingsData
    {
        public bool enabled;
        public float defaultVisionRadius;
        public string notes;
    }

    [Serializable]
    public class MapData
    {
        public string id;
        public LocalizedText name;
        public string mapType;
        public MapSizeData size;
        public NexusData[] nexuses;
        public LaneData[] lanes;
        public TowerData[] towers;
        public JungleZoneData[] jungleZones;
        public MinionWaveRouteData[] minionWaveRoutes;
        public VisionSettingsData vision;
        public string notes;
    }

    [Serializable]
    public class MapDataFile
    {
        public string schemaVersion;
        public MapData[] maps;
    }

    [Serializable]
    public class BehaviorWeightsData
    {
        public float lastHit;
        public float laning;
        public float retreat;
        public float teamFight;
        public float itemBuild;
        public float pushTower;
        public float jungle;
        public float objectiveControl;
    }

    [Serializable]
    public class DecisionThresholdsData
    {
        public float retreatHealthRatio;
        public float allInHealthRatio;
        public float groupUpAllyRatio;
        public float objectiveContestRatio;
    }

    [Serializable]
    public class LanePreferenceData
    {
        public string primaryLane;
        public string[] secondaryLanes;
    }

    [Serializable]
    public class ItemBuildPlanData
    {
        public string[] preferredItemIds;
        public string[] fallbackItemIds;
    }

    [Serializable]
    public class BotData
    {
        public string id;
        public LocalizedText name;
        public string difficulty;
        public string role;
        public string[] heroPool;
        public LanePreferenceData lanePreference;
        public BehaviorWeightsData behaviorWeights;
        public DecisionThresholdsData decisionThresholds;
        public ItemBuildPlanData itemBuildPlan;
        public string seedPolicy;
        public int fixedSeed;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class BotDataFile
    {
        public string schemaVersion;
        public BotData[] bots;
    }

    [Serializable]
    public class PlayerAttributesData
    {
        public int mechanics;
        public int laning;
        public int teamFight;
        public int mapAwareness;
        public int championPool;
        public int mental;
    }

    [Serializable]
    public class ContractData
    {
        public string status;
        public string teamId;
        public int salary;
        public int monthsRemaining;
    }

    [Serializable]
    public class PlayerData
    {
        public string id;
        public LocalizedText name;
        public int age;
        public string mainLane;
        public string role;
        public string[] heroPool;
        public PlayerAttributesData attributes;
        public ContractData contract;
        public int reputation;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class PlayerDataFile
    {
        public string schemaVersion;
        public PlayerData[] players;
    }

    [Serializable]
    public class RosterSlotData
    {
        public string lane;
        public string playerId;
    }

    [Serializable]
    public class StrategyData
    {
        public string style;
        public string preferredMapSide;
        public string priority;
    }

    [Serializable]
    public class TeamData
    {
        public string id;
        public LocalizedText name;
        public string region;
        public int budget;
        public RosterSlotData[] roster;
        public string[] substitutePlayerIds;
        public StrategyData preferredStrategy;
        public string[] tags;
        public string notes;
    }

    [Serializable]
    public class TeamDataFile
    {
        public string schemaVersion;
        public TeamData[] teams;
    }

    [Serializable]
    public class ScheduleRulesData
    {
        public int roundRobinRounds;
        public int bestOf;
        public int playoffTeamCount;
    }

    [Serializable]
    public class LeagueData
    {
        public string id;
        public LocalizedText name;
        public string region;
        public int season;
        public string[] teamIds;
        public string format;
        public ScheduleRulesData scheduleRules;
        public int worldsQualificationSlots;
        public string notes;
    }

    [Serializable]
    public class LeagueDataFile
    {
        public string schemaVersion;
        public LeagueData[] leagues;
    }
}
