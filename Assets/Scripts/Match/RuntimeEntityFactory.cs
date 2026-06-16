using System;
using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;

namespace MobaManager.Match
{
    public static class RuntimeEntityFactory
    {
        public static RuntimeEntityModelDatabase Build(GameDataDatabase database)
        {
            List<string> errors = GameDataValidator.Validate(database);
            if (errors.Count > 0)
            {
                throw new InvalidOperationException("Cannot build runtime entity model from invalid game data:\n- " + string.Join("\n- ", errors));
            }

            return new RuntimeEntityModelDatabase(
                BuildHeroModels(database),
                BuildMapModels(database));
        }

        private static IReadOnlyDictionary<string, RuntimeHeroModel> BuildHeroModels(GameDataDatabase database)
        {
            var heroesById = new Dictionary<string, RuntimeHeroModel>();

            foreach (HeroData hero in database.HeroesById.Values)
            {
                var skillsBySlot = new Dictionary<string, RuntimeSkillReference>
                {
                    { "passive", new RuntimeSkillReference(database.SkillsById[hero.skills.passive]) },
                    { "q", new RuntimeSkillReference(database.SkillsById[hero.skills.q]) },
                    { "w", new RuntimeSkillReference(database.SkillsById[hero.skills.w]) },
                    { "e", new RuntimeSkillReference(database.SkillsById[hero.skills.e]) },
                    { "r", new RuntimeSkillReference(database.SkillsById[hero.skills.r]) }
                };

                heroesById.Add(
                    hero.id,
                    new RuntimeHeroModel(
                        hero,
                        new RuntimeStatBlock(hero.baseStats),
                        new RuntimeStatBlock(hero.growthStats),
                        skillsBySlot));
            }

            return heroesById;
        }

        private static IReadOnlyDictionary<string, RuntimeMapModel> BuildMapModels(GameDataDatabase database)
        {
            var mapsById = new Dictionary<string, RuntimeMapModel>();

            foreach (MapData map in database.MapsById.Values)
            {
                IReadOnlyList<RuntimeNexusModel> nexuses = (map.nexuses ?? Array.Empty<NexusData>())
                    .Select(nexus => new RuntimeNexusModel(nexus))
                    .ToArray();

                IReadOnlyList<RuntimeLaneModel> lanes = BuildLaneModels(map);
                IReadOnlyList<RuntimeTowerModel> towers = (map.towers ?? Array.Empty<TowerData>())
                    .Select(tower => new RuntimeTowerModel(tower))
                    .ToArray();

                IReadOnlyList<RuntimeMinionWaveRouteModel> minionWaveRoutes = BuildMinionWaveRouteModels(map, lanes);

                mapsById.Add(
                    map.id,
                    new RuntimeMapModel(
                        map,
                        nexuses,
                        lanes,
                        towers,
                        minionWaveRoutes));
            }

            return mapsById;
        }

        private static IReadOnlyList<RuntimeLaneModel> BuildLaneModels(MapData map)
        {
            var lanes = new List<RuntimeLaneModel>();

            foreach (LaneData lane in map.lanes ?? Array.Empty<LaneData>())
            {
                var pathNodes = (lane.pathNodes ?? Array.Empty<PathNodeData>())
                    .Select(node => new RuntimePathNodeModel(node))
                    .ToArray();

                var pathNodesById = pathNodes.ToDictionary(node => node.Id);
                lanes.Add(new RuntimeLaneModel(lane, pathNodes, pathNodesById));
            }

            return lanes;
        }

        private static IReadOnlyList<RuntimeMinionWaveRouteModel> BuildMinionWaveRouteModels(
            MapData map,
            IReadOnlyList<RuntimeLaneModel> lanes)
        {
            var lanesByType = lanes.ToDictionary(lane => lane.LaneType);
            var routes = new List<RuntimeMinionWaveRouteModel>();

            foreach (MinionWaveRouteData route in map.minionWaveRoutes ?? Array.Empty<MinionWaveRouteData>())
            {
                RuntimeLaneModel lane = lanesByType[route.laneType];
                routes.Add(
                    new RuntimeMinionWaveRouteModel(
                        route,
                        ResolvePathNodes(lane, route.bluePathNodeIds),
                        ResolvePathNodes(lane, route.redPathNodeIds)));
            }

            return routes;
        }

        private static IReadOnlyList<RuntimePathNodeModel> ResolvePathNodes(RuntimeLaneModel lane, string[] nodeIds)
        {
            return (nodeIds ?? Array.Empty<string>())
                .Select(nodeId => lane.PathNodesById[nodeId])
                .ToArray();
        }
    }
}
