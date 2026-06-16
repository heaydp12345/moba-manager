using System;
using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;

namespace MobaManager.Match
{
    public sealed class RuntimeSkillState
    {
        public RuntimeSkillState(RuntimeSkillReference skill)
        {
            Skill = skill;
            SkillId = skill.Id;
            Slot = skill.Slot;
            MaxLevel = skill.MaxLevel;
            Level = 0;
        }

        public RuntimeSkillReference Skill { get; }
        public string SkillId { get; }
        public string Slot { get; }
        public int Level { get; private set; }
        public int MaxLevel { get; }
        public bool CanLevelUp => Level < MaxLevel;

        public void SetLevelForDataSetup(int level)
        {
            if (level < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Skill level cannot be negative.");
            }

            if (level > MaxLevel)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Skill level cannot exceed max level.");
            }

            Level = level;
        }
    }

    public sealed class RuntimeItemSlotState
    {
        public RuntimeItemSlotState(int slotIndex)
        {
            SlotIndex = slotIndex;
        }

        public int SlotIndex { get; }
        public RuntimeItemReference Item { get; private set; }
        public string ItemId => Item == null ? string.Empty : Item.Id;
        public bool IsEmpty => Item == null;

        public void SetItemForDataSetup(RuntimeItemReference item)
        {
            Item = item;
        }
    }

    public sealed class RuntimeItemReference
    {
        public RuntimeItemReference(ItemData data)
        {
            Id = data.id;
            Name = new RuntimeLocalizedText(data.name);
            Tier = data.tier;
            ComponentType = data.componentType;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string Tier { get; }
        public string ComponentType { get; }
    }

    public static class RuntimeItemReferenceFactory
    {
        public static IReadOnlyDictionary<string, RuntimeItemReference> BuildReferences(GameDataDatabase database)
        {
            return database.ItemsById.ToDictionary(
                pair => pair.Key,
                pair => new RuntimeItemReference(pair.Value));
        }
    }

    public sealed class RuntimeHeroState
    {
        public RuntimeHeroState(
            string instanceId,
            string team,
            RuntimeHeroModel hero,
            IReadOnlyDictionary<string, RuntimeSkillState> skillsBySlot,
            IReadOnlyList<RuntimeItemSlotState> itemSlots)
        {
            InstanceId = instanceId;
            Team = team;
            Hero = hero;
            Level = 1;
            Health = RuntimeHealthStateFactory.CreateHeroHealth(this);
            Resource = RuntimeResourceStateFactory.CreateHeroResource(this);
            SkillsBySlot = skillsBySlot;
            ItemSlots = itemSlots;
            Position = RuntimePositionStateFactory.CreateHeroPosition(this, new RuntimePoint(0f, 0f));
        }

        public string InstanceId { get; }
        public string Team { get; }
        public RuntimeHeroModel Hero { get; }
        public int Level { get; set; }
        public RuntimeHealthState Health { get; }
        public float CurrentHealth => Health.CurrentHealth;
        public RuntimeResourceState Resource { get; }
        public float CurrentResource => Resource.CurrentValue;
        public IReadOnlyDictionary<string, RuntimeSkillState> SkillsBySlot { get; }
        public IReadOnlyList<RuntimeItemSlotState> ItemSlots { get; }
        public RuntimePositionState Position { get; }

        public void SetPosition(RuntimePoint position)
        {
            Position.SetPosition(position);
        }
    }

    public static class RuntimeHeroStateFactory
    {
        public static RuntimeHeroState Create(
            RuntimeHeroModel hero,
            string team,
            int maxItemSlots,
            string instanceId)
        {
            if (hero == null)
            {
                throw new ArgumentNullException(nameof(hero));
            }

            if (maxItemSlots < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxItemSlots), "Item slot count cannot be negative.");
            }

            var skillsBySlot = hero.SkillsBySlot.ToDictionary(
                pair => pair.Key,
                pair => new RuntimeSkillState(pair.Value));

            var itemSlots = Enumerable.Range(0, maxItemSlots)
                .Select(index => new RuntimeItemSlotState(index))
                .ToArray();

            return new RuntimeHeroState(instanceId, team, hero, skillsBySlot, itemSlots);
        }

        public static IReadOnlyList<RuntimeHeroState> CreateDemoStates(
            RuntimeEntityModelDatabase runtimeDatabase,
            GameDataDatabase gameDataDatabase)
        {
            int maxItemSlots = gameDataDatabase.MatchesById.Values.FirstOrDefault()?.maxItems ?? 0;
            return runtimeDatabase.HeroesById.Values
                .Select((hero, index) => Create(hero, "blue", maxItemSlots, $"blue_{index}_{hero.Id}"))
                .ToArray();
        }
    }
}
