using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimePositionState
    {
        public RuntimePositionState(
            string entityId,
            string entityType,
            string team,
            RuntimePoint position)
        {
            EntityId = entityId;
            EntityType = entityType;
            Team = team;
            Position = position;
        }

        public string EntityId { get; }
        public string EntityType { get; }
        public string Team { get; }
        public RuntimePoint Position { get; private set; }

        public void SetPosition(RuntimePoint position)
        {
            Position = position;
        }
    }

    public static class RuntimePositionStateFactory
    {
        public static RuntimePositionState CreateHeroPosition(RuntimeHeroState heroState, RuntimePoint startPosition)
        {
            return new RuntimePositionState(heroState.InstanceId, "hero", heroState.Team, startPosition);
        }

        public static RuntimePositionState CreateTowerPosition(RuntimeTowerModel tower)
        {
            return new RuntimePositionState(tower.Id, "tower", tower.Team, tower.Position);
        }

        public static RuntimePositionState CreateNexusPosition(RuntimeNexusModel nexus)
        {
            return new RuntimePositionState(nexus.Id, "nexus", nexus.Team, nexus.Position);
        }

        public static IReadOnlyList<RuntimePositionState> CreateMapStructurePositions(RuntimeMapModel map)
        {
            return map.Towers
                .Select(CreateTowerPosition)
                .Concat(map.Nexuses.Select(CreateNexusPosition))
                .ToArray();
        }
    }
}
