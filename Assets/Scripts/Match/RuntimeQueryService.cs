using System;
using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeQueryService
    {
        private readonly RuntimeMatchState matchState;
        private readonly RuntimeUnitRegistry unitRegistry;
        private readonly IReadOnlyDictionary<string, RuntimeHeroState> heroesByInstanceId;

        public RuntimeQueryService(RuntimeMatchState matchState, RuntimeUnitRegistry unitRegistry)
        {
            if (matchState == null)
            {
                throw new ArgumentNullException(nameof(matchState));
            }

            if (unitRegistry == null)
            {
                throw new ArgumentNullException(nameof(unitRegistry));
            }

            this.matchState = matchState;
            this.unitRegistry = unitRegistry;
            heroesByInstanceId = matchState.AllHeroStates.ToDictionary(hero => hero.InstanceId);
        }

        public RuntimeMatchState MatchState => matchState;
        public RuntimeUnitRegistry UnitRegistry => unitRegistry;
        public IReadOnlyList<RuntimeHeroState> AllHeroStates => matchState.AllHeroStates;
        public IReadOnlyList<RuntimeUnitState> AllUnits => unitRegistry.AllUnits;

        public bool TryGetHeroState(string instanceId, out RuntimeHeroState heroState)
        {
            return heroesByInstanceId.TryGetValue(instanceId, out heroState);
        }

        public RuntimeHeroState GetHeroState(string instanceId)
        {
            return heroesByInstanceId[instanceId];
        }

        public IReadOnlyList<RuntimeHeroState> GetHeroStatesByTeam(string team)
        {
            return AllHeroStates
                .Where(hero => hero.Team == team)
                .ToArray();
        }

        public bool TryGetUnit(string unitId, out RuntimeUnitState unit)
        {
            return unitRegistry.UnitsById.TryGetValue(unitId, out unit);
        }

        public RuntimeUnitState GetUnit(string unitId)
        {
            return unitRegistry.GetUnit(unitId);
        }

        public IReadOnlyList<RuntimeUnitState> GetUnitsByTeam(string team)
        {
            return unitRegistry.GetUnitsByTeam(team);
        }

        public IReadOnlyList<RuntimeUnitState> GetUnitsByType(string unitType)
        {
            return unitRegistry.GetUnitsByType(unitType);
        }

        public IReadOnlyList<RuntimeUnitState> GetUnitsByTeamAndType(string team, string unitType)
        {
            return unitRegistry.AllUnits
                .Where(unit => unit.Team == team && unit.UnitType == unitType)
                .ToArray();
        }

        public IReadOnlyList<RuntimeUnitState> GetTowersByTeam(string team)
        {
            return GetUnitsByTeamAndType(team, "tower");
        }

        public RuntimeUnitState GetNexusByTeam(string team)
        {
            return GetUnitsByTeamAndType(team, "nexus").First();
        }

        public IReadOnlyList<RuntimeRosterSlotState> GetRosterSlotsByTeam(string team)
        {
            if (team == matchState.BlueTeam.Side)
            {
                return matchState.BlueTeam.RosterSlots;
            }

            if (team == matchState.RedTeam.Side)
            {
                return matchState.RedTeam.RosterSlots;
            }

            return Array.Empty<RuntimeRosterSlotState>();
        }

        public IReadOnlyList<RuntimePathNodeModel> GetLanePathNodes(string laneType)
        {
            RuntimeLaneModel lane = matchState.Map.Lanes.FirstOrDefault(item => item.LaneType == laneType);
            return lane == null ? Array.Empty<RuntimePathNodeModel>() : lane.PathNodes;
        }
    }

    public static class RuntimeQueryServiceFactory
    {
        public static RuntimeQueryService Create(RuntimeMatchState matchState)
        {
            return new RuntimeQueryService(matchState, matchState.CreateUnitRegistry());
        }
    }
}
