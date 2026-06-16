using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeResourceState
    {
        public RuntimeResourceState(
            string entityId,
            string resourceType,
            float maxValue)
        {
            EntityId = entityId;
            ResourceType = resourceType;
            MaxValue = maxValue;
            CurrentValue = 0f;
        }

        public string EntityId { get; }
        public string ResourceType { get; }
        public float CurrentValue { get; private set; }
        public float MaxValue { get; }
    }

    public static class RuntimeResourceStateFactory
    {
        public static RuntimeResourceState CreateHeroResource(RuntimeHeroState heroState)
        {
            return new RuntimeResourceState(
                heroState.InstanceId,
                heroState.Hero.ResourceType,
                ResolveDefaultMaxValue(heroState.Hero.ResourceType));
        }

        public static IReadOnlyList<RuntimeResourceState> CreateHeroResources(IEnumerable<RuntimeHeroState> heroStates)
        {
            return heroStates
                .Select(CreateHeroResource)
                .ToArray();
        }

        private static float ResolveDefaultMaxValue(string resourceType)
        {
            return 0f;
        }
    }
}
