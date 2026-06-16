using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeHealthState
    {
        public RuntimeHealthState(
            string entityId,
            string entityType,
            string team,
            float maxHealth)
        {
            EntityId = entityId;
            EntityType = entityType;
            Team = team;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public string EntityId { get; }
        public string EntityType { get; }
        public string Team { get; }
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; }
    }

    public static class RuntimeHealthStateFactory
    {
        public static RuntimeHealthState CreateHeroHealth(RuntimeHeroState heroState)
        {
            return new RuntimeHealthState(heroState.InstanceId, "hero", heroState.Team, heroState.Hero.BaseStats.MaxHealth);
        }

        public static RuntimeHealthState CreateTowerHealth(RuntimeTowerModel tower)
        {
            return new RuntimeHealthState(tower.Id, "tower", tower.Team, tower.MaxHealth);
        }

        public static RuntimeHealthState CreateNexusHealth(RuntimeNexusModel nexus)
        {
            return new RuntimeHealthState(nexus.Id, "nexus", nexus.Team, nexus.MaxHealth);
        }

        public static IReadOnlyList<RuntimeHealthState> CreateMapStructureHealthStates(RuntimeMapModel map)
        {
            return map.Towers
                .Select(CreateTowerHealth)
                .Concat(map.Nexuses.Select(CreateNexusHealth))
                .ToArray();
        }
    }
}
