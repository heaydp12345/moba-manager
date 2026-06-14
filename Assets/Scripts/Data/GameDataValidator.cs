using System.Collections.Generic;
using System.Linq;

namespace MobaManager.Data
{
    public static class GameDataValidator
    {
        public static List<string> Validate(GameDataDatabase database)
        {
            var errors = new List<string>();

            ValidateHeroSkillRefs(database, errors);
            ValidateSkillBuffRefs(database, errors);
            ValidateBotRefs(database, errors);
            ValidatePlayerRefs(database, errors);
            ValidateTeamRefs(database, errors);
            ValidateLeagueRefs(database, errors);
            ValidateMapRouteRefs(database, errors);

            return errors;
        }

        private static void ValidateHeroSkillRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (HeroData hero in database.HeroesById.Values)
            {
                if (hero.skills == null)
                {
                    errors.Add($"hero \"{hero.id}\" has no skills object.");
                    continue;
                }

                RequireId(errors, $"hero \"{hero.id}\" skills.passive", database.SkillsById, hero.skills.passive, "skill");
                RequireId(errors, $"hero \"{hero.id}\" skills.q", database.SkillsById, hero.skills.q, "skill");
                RequireId(errors, $"hero \"{hero.id}\" skills.w", database.SkillsById, hero.skills.w, "skill");
                RequireId(errors, $"hero \"{hero.id}\" skills.e", database.SkillsById, hero.skills.e, "skill");
                RequireId(errors, $"hero \"{hero.id}\" skills.r", database.SkillsById, hero.skills.r, "skill");
            }
        }

        private static void ValidateSkillBuffRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (SkillData skill in database.SkillsById.Values)
            {
                if (skill.effects == null)
                {
                    continue;
                }

                for (int i = 0; i < skill.effects.Length; i++)
                {
                    SkillEffectData effect = skill.effects[i];
                    if (!string.IsNullOrWhiteSpace(effect.buffId))
                    {
                        RequireId(errors, $"skill \"{skill.id}\" effects[{i}].buffId", database.BuffsById, effect.buffId, "buff");
                    }
                }
            }
        }

        private static void ValidateBotRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (BotData bot in database.BotsById.Values)
            {
                foreach (string heroId in bot.heroPool ?? EmptyStrings())
                {
                    RequireId(errors, $"bot \"{bot.id}\" heroPool", database.HeroesById, heroId, "hero");
                }

                if (bot.itemBuildPlan == null)
                {
                    errors.Add($"bot \"{bot.id}\" has no itemBuildPlan object.");
                    continue;
                }

                foreach (string itemId in bot.itemBuildPlan.preferredItemIds ?? EmptyStrings())
                {
                    RequireId(errors, $"bot \"{bot.id}\" preferredItemIds", database.ItemsById, itemId, "item");
                }

                foreach (string itemId in bot.itemBuildPlan.fallbackItemIds ?? EmptyStrings())
                {
                    RequireId(errors, $"bot \"{bot.id}\" fallbackItemIds", database.ItemsById, itemId, "item");
                }
            }
        }

        private static void ValidatePlayerRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (PlayerData player in database.PlayersById.Values)
            {
                foreach (string heroId in player.heroPool ?? EmptyStrings())
                {
                    RequireId(errors, $"player \"{player.id}\" heroPool", database.HeroesById, heroId, "hero");
                }

                if (player.contract != null && !string.IsNullOrWhiteSpace(player.contract.teamId))
                {
                    RequireId(errors, $"player \"{player.id}\" contract.teamId", database.TeamsById, player.contract.teamId, "team");
                }
            }
        }

        private static void ValidateTeamRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (TeamData team in database.TeamsById.Values)
            {
                var lanes = new HashSet<string>();
                foreach (RosterSlotData rosterSlot in team.roster ?? EmptyRosterSlots())
                {
                    if (!lanes.Add(rosterSlot.lane))
                    {
                        errors.Add($"team \"{team.id}\" roster has duplicate lane \"{rosterSlot.lane}\".");
                    }

                    RequireId(errors, $"team \"{team.id}\" roster.{rosterSlot.lane}", database.PlayersById, rosterSlot.playerId, "player");
                }

                foreach (string playerId in team.substitutePlayerIds ?? EmptyStrings())
                {
                    RequireId(errors, $"team \"{team.id}\" substitutePlayerIds", database.PlayersById, playerId, "player");
                }
            }
        }

        private static void ValidateLeagueRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (LeagueData league in database.LeaguesById.Values)
            {
                foreach (string teamId in league.teamIds ?? EmptyStrings())
                {
                    RequireId(errors, $"league \"{league.id}\" teamIds", database.TeamsById, teamId, "team");
                }
            }
        }

        private static void ValidateMapRouteRefs(GameDataDatabase database, List<string> errors)
        {
            foreach (MapData map in database.MapsById.Values)
            {
                var laneNodes = new Dictionary<string, HashSet<string>>();
                foreach (LaneData lane in map.lanes ?? EmptyLanes())
                {
                    laneNodes[lane.laneType] = new HashSet<string>((lane.pathNodes ?? EmptyPathNodes()).Select(node => node.id));
                }

                foreach (MinionWaveRouteData route in map.minionWaveRoutes ?? EmptyRoutes())
                {
                    if (!laneNodes.TryGetValue(route.laneType, out HashSet<string> nodeIds))
                    {
                        errors.Add($"map \"{map.id}\" route \"{route.laneType}\" has no matching lane.");
                        continue;
                    }

                    foreach (string nodeId in route.bluePathNodeIds ?? EmptyStrings())
                    {
                        RequireId(errors, $"map \"{map.id}\" {route.laneType}.bluePathNodeIds", nodeIds, nodeId, "path node");
                    }

                    foreach (string nodeId in route.redPathNodeIds ?? EmptyStrings())
                    {
                        RequireId(errors, $"map \"{map.id}\" {route.laneType}.redPathNodeIds", nodeIds, nodeId, "path node");
                    }
                }
            }
        }

        private static void RequireId<T>(
            List<string> errors,
            string source,
            IReadOnlyDictionary<string, T> targetIds,
            string id,
            string label)
        {
            if (string.IsNullOrWhiteSpace(id) || !targetIds.ContainsKey(id))
            {
                errors.Add($"{source}: unknown {label} id \"{id}\".");
            }
        }

        private static void RequireId(
            List<string> errors,
            string source,
            HashSet<string> targetIds,
            string id,
            string label)
        {
            if (string.IsNullOrWhiteSpace(id) || !targetIds.Contains(id))
            {
                errors.Add($"{source}: unknown {label} id \"{id}\".");
            }
        }

        private static string[] EmptyStrings()
        {
            return System.Array.Empty<string>();
        }

        private static RosterSlotData[] EmptyRosterSlots()
        {
            return System.Array.Empty<RosterSlotData>();
        }

        private static LaneData[] EmptyLanes()
        {
            return System.Array.Empty<LaneData>();
        }

        private static PathNodeData[] EmptyPathNodes()
        {
            return System.Array.Empty<PathNodeData>();
        }

        private static MinionWaveRouteData[] EmptyRoutes()
        {
            return System.Array.Empty<MinionWaveRouteData>();
        }
    }
}
