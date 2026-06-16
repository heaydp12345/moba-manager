using System;
using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeUnitState
    {
        public RuntimeUnitState(
            string unitId,
            string unitType,
            string team,
            RuntimePositionState position,
            RuntimeHealthState health)
        {
            UnitId = unitId;
            UnitType = unitType;
            Team = team;
            Position = position;
            Health = health;
        }

        public string UnitId { get; }
        public string UnitType { get; }
        public string Team { get; }
        public RuntimePositionState Position { get; }
        public RuntimeHealthState Health { get; }
    }

    public sealed class RuntimeUnitRegistry
    {
        public RuntimeUnitRegistry(IReadOnlyList<RuntimeUnitState> units)
        {
            AllUnits = units;
            UnitsById = units.ToDictionary(unit => unit.UnitId);
        }

        public IReadOnlyList<RuntimeUnitState> AllUnits { get; }
        public IReadOnlyDictionary<string, RuntimeUnitState> UnitsById { get; }

        public RuntimeUnitState GetUnit(string unitId)
        {
            return UnitsById[unitId];
        }

        public IReadOnlyList<RuntimeUnitState> GetUnitsByType(string unitType)
        {
            return AllUnits
                .Where(unit => unit.UnitType == unitType)
                .ToArray();
        }

        public IReadOnlyList<RuntimeUnitState> GetUnitsByTeam(string team)
        {
            return AllUnits
                .Where(unit => unit.Team == team)
                .ToArray();
        }
    }

    public static class RuntimeUnitRegistryFactory
    {
        public static RuntimeUnitRegistry Create(RuntimeMatchState matchState)
        {
            var healthById = matchState.GetAllHealthStates()
                .ToDictionary(health => health.EntityId);
            var units = new List<RuntimeUnitState>();

            foreach (RuntimePositionState position in matchState.GetAllPositionStates())
            {
                if (!healthById.TryGetValue(position.EntityId, out RuntimeHealthState health))
                {
                    throw new InvalidOperationException($"Missing health state for unit \"{position.EntityId}\".");
                }

                units.Add(new RuntimeUnitState(
                    position.EntityId,
                    position.EntityType,
                    position.Team,
                    position,
                    health));
            }

            return new RuntimeUnitRegistry(units);
        }
    }
}
