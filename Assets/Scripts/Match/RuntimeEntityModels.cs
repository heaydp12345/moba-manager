using System.Collections.Generic;
using MobaManager.Data;

namespace MobaManager.Match
{
    public sealed class RuntimeLocalizedText
    {
        public RuntimeLocalizedText(LocalizedText data)
        {
            ZhTw = data == null ? string.Empty : data.zhTw;
            En = data == null ? string.Empty : data.en;
        }

        public string ZhTw { get; }
        public string En { get; }
    }

    public readonly struct RuntimePoint
    {
        public static RuntimePoint FromData(PointData point)
        {
            return point == null ? new RuntimePoint(0f, 0f) : new RuntimePoint(point.x, point.y);
        }

        public RuntimePoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
    }

    public sealed class RuntimeStatBlock
    {
        public RuntimeStatBlock(StatBlockData data)
        {
            if (data == null)
            {
                return;
            }

            AttackDamage = data.attackDamage;
            AbilityPower = data.abilityPower;
            Armor = data.armor;
            MagicResist = data.magicResist;
            MaxHealth = data.maxHealth;
            ManaRegen = data.manaRegen;
            CritChance = data.critChance;
            LifeSteal = data.lifeSteal;
            AttackSpeed = data.attackSpeed;
            MoveSpeed = data.moveSpeed;
            AttackRange = data.attackRange;
        }

        public float AttackDamage { get; }
        public float AbilityPower { get; }
        public float Armor { get; }
        public float MagicResist { get; }
        public float MaxHealth { get; }
        public float ManaRegen { get; }
        public float CritChance { get; }
        public float LifeSteal { get; }
        public float AttackSpeed { get; }
        public float MoveSpeed { get; }
        public float AttackRange { get; }
    }

    public sealed class RuntimeSkillReference
    {
        public RuntimeSkillReference(SkillData data)
        {
            Id = data.id;
            Slot = data.slot;
            MaxLevel = data.maxLevel;
        }

        public string Id { get; }
        public string Slot { get; }
        public int MaxLevel { get; }
    }

    public sealed class RuntimeHeroModel
    {
        public RuntimeHeroModel(
            HeroData source,
            RuntimeStatBlock baseStats,
            RuntimeStatBlock growthStats,
            IReadOnlyDictionary<string, RuntimeSkillReference> skillsBySlot)
        {
            Id = source.id;
            Name = new RuntimeLocalizedText(source.name);
            Role = source.role;
            AttackType = source.attackType;
            DamageProfile = source.damageProfile;
            ResourceType = source.resourceType;
            BaseStats = baseStats;
            GrowthStats = growthStats;
            SkillsBySlot = skillsBySlot;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string Role { get; }
        public string AttackType { get; }
        public string DamageProfile { get; }
        public string ResourceType { get; }
        public RuntimeStatBlock BaseStats { get; }
        public RuntimeStatBlock GrowthStats { get; }
        public IReadOnlyDictionary<string, RuntimeSkillReference> SkillsBySlot { get; }
    }

    public sealed class RuntimePathNodeModel
    {
        public RuntimePathNodeModel(PathNodeData source)
        {
            Id = source.id;
            Position = RuntimePoint.FromData(source.position);
        }

        public string Id { get; }
        public RuntimePoint Position { get; }
    }

    public sealed class RuntimeLaneModel
    {
        public RuntimeLaneModel(
            LaneData source,
            IReadOnlyList<RuntimePathNodeModel> pathNodes,
            IReadOnlyDictionary<string, RuntimePathNodeModel> pathNodesById)
        {
            Id = source.id;
            Name = new RuntimeLocalizedText(source.name);
            LaneType = source.laneType;
            PathNodes = pathNodes;
            PathNodesById = pathNodesById;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string LaneType { get; }
        public IReadOnlyList<RuntimePathNodeModel> PathNodes { get; }
        public IReadOnlyDictionary<string, RuntimePathNodeModel> PathNodesById { get; }
    }

    public sealed class RuntimeNexusModel
    {
        public RuntimeNexusModel(NexusData source)
        {
            Id = source.id;
            Team = source.team;
            Position = RuntimePoint.FromData(source.position);
            MaxHealth = source.maxHealth;
        }

        public string Id { get; }
        public string Team { get; }
        public RuntimePoint Position { get; }
        public float MaxHealth { get; }
    }

    public sealed class RuntimeTowerModel
    {
        public RuntimeTowerModel(TowerData source)
        {
            Id = source.id;
            Team = source.team;
            LaneType = source.laneType;
            Position = RuntimePoint.FromData(source.position);
            AttackRange = source.attackRange;
            AttackDamage = source.attackDamage;
            MaxHealth = source.maxHealth;
        }

        public string Id { get; }
        public string Team { get; }
        public string LaneType { get; }
        public RuntimePoint Position { get; }
        public float AttackRange { get; }
        public float AttackDamage { get; }
        public float MaxHealth { get; }
    }

    public sealed class RuntimeMinionWaveRouteModel
    {
        public RuntimeMinionWaveRouteModel(
            MinionWaveRouteData source,
            IReadOnlyList<RuntimePathNodeModel> bluePathNodes,
            IReadOnlyList<RuntimePathNodeModel> redPathNodes)
        {
            LaneType = source.laneType;
            BluePathNodes = bluePathNodes;
            RedPathNodes = redPathNodes;
        }

        public string LaneType { get; }
        public IReadOnlyList<RuntimePathNodeModel> BluePathNodes { get; }
        public IReadOnlyList<RuntimePathNodeModel> RedPathNodes { get; }
    }

    public sealed class RuntimeMapModel
    {
        public RuntimeMapModel(
            MapData source,
            IReadOnlyList<RuntimeNexusModel> nexuses,
            IReadOnlyList<RuntimeLaneModel> lanes,
            IReadOnlyList<RuntimeTowerModel> towers,
            IReadOnlyList<RuntimeMinionWaveRouteModel> minionWaveRoutes)
        {
            Id = source.id;
            Name = new RuntimeLocalizedText(source.name);
            MapType = source.mapType;
            Nexuses = nexuses;
            Lanes = lanes;
            Towers = towers;
            MinionWaveRoutes = minionWaveRoutes;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string MapType { get; }
        public IReadOnlyList<RuntimeNexusModel> Nexuses { get; }
        public IReadOnlyList<RuntimeLaneModel> Lanes { get; }
        public IReadOnlyList<RuntimeTowerModel> Towers { get; }
        public IReadOnlyList<RuntimeMinionWaveRouteModel> MinionWaveRoutes { get; }
    }

    public sealed class RuntimeEntityModelDatabase
    {
        public RuntimeEntityModelDatabase(
            IReadOnlyDictionary<string, RuntimeHeroModel> heroesById,
            IReadOnlyDictionary<string, RuntimeMapModel> mapsById)
        {
            HeroesById = heroesById;
            MapsById = mapsById;
        }

        public IReadOnlyDictionary<string, RuntimeHeroModel> HeroesById { get; }
        public IReadOnlyDictionary<string, RuntimeMapModel> MapsById { get; }
    }
}
