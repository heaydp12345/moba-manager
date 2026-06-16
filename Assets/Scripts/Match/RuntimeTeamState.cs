using System;
using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;

namespace MobaManager.Match
{
    public sealed class RuntimePlayerReference
    {
        public RuntimePlayerReference(PlayerData data)
        {
            Id = data.id;
            Name = new RuntimeLocalizedText(data.name);
            MainLane = data.mainLane;
            Role = data.role;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string MainLane { get; }
        public string Role { get; }
    }

    public sealed class RuntimeTeamReference
    {
        public RuntimeTeamReference(TeamData data)
        {
            Id = data.id;
            Name = new RuntimeLocalizedText(data.name);
            Region = data.region;
        }

        public string Id { get; }
        public RuntimeLocalizedText Name { get; }
        public string Region { get; }
    }

    public sealed class RuntimeRosterSlotState
    {
        public RuntimeRosterSlotState(
            string lane,
            RuntimePlayerReference player,
            RuntimeHeroState heroState)
        {
            Lane = lane;
            Player = player;
            HeroState = heroState;
        }

        public string Lane { get; }
        public RuntimePlayerReference Player { get; }
        public RuntimeHeroState HeroState { get; }
    }

    public sealed class RuntimeTeamState
    {
        public RuntimeTeamState(
            string side,
            RuntimeTeamReference team,
            IReadOnlyList<RuntimeRosterSlotState> rosterSlots,
            IReadOnlyList<string> substitutePlayerIds)
        {
            Side = side;
            Team = team;
            RosterSlots = rosterSlots;
            SubstitutePlayerIds = substitutePlayerIds;
        }

        public string Side { get; }
        public RuntimeTeamReference Team { get; }
        public IReadOnlyList<RuntimeRosterSlotState> RosterSlots { get; }
        public IReadOnlyList<string> SubstitutePlayerIds { get; }
        public IReadOnlyList<RuntimeHeroState> HeroStates => RosterSlots.Select(slot => slot.HeroState).ToArray();
    }

    public static class RuntimeTeamStateFactory
    {
        public static RuntimeTeamState Create(
            string side,
            TeamData team,
            GameDataDatabase gameDataDatabase,
            RuntimeEntityModelDatabase runtimeDatabase,
            int maxItemSlots)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team));
            }

            var rosterSlots = new List<RuntimeRosterSlotState>();
            foreach (RosterSlotData rosterSlot in team.roster ?? Array.Empty<RosterSlotData>())
            {
                PlayerData player = gameDataDatabase.PlayersById[rosterSlot.playerId];
                RuntimeHeroModel hero = SelectFirstListedHero(player, runtimeDatabase);
                RuntimeHeroState heroState = RuntimeHeroStateFactory.Create(
                    hero,
                    side,
                    maxItemSlots,
                    $"{side}_{rosterSlot.lane}_{player.id}_{hero.Id}");

                rosterSlots.Add(new RuntimeRosterSlotState(rosterSlot.lane, new RuntimePlayerReference(player), heroState));
            }

            return new RuntimeTeamState(
                side,
                new RuntimeTeamReference(team),
                rosterSlots,
                team.substitutePlayerIds ?? Array.Empty<string>());
        }

        public static RuntimeTeamState CreateDemoState(
            string side,
            GameDataDatabase gameDataDatabase,
            RuntimeEntityModelDatabase runtimeDatabase)
        {
            TeamData team = gameDataDatabase.TeamsById.Values.FirstOrDefault();
            if (team == null)
            {
                throw new InvalidOperationException("Cannot create team state without team data.");
            }

            MatchData match = gameDataDatabase.MatchesById.Values.FirstOrDefault();
            if (match == null)
            {
                throw new InvalidOperationException("Cannot create team state without match config.");
            }

            return Create(side, team, gameDataDatabase, runtimeDatabase, match.maxItems);
        }

        private static RuntimeHeroModel SelectFirstListedHero(
            PlayerData player,
            RuntimeEntityModelDatabase runtimeDatabase)
        {
            string heroId = player.heroPool == null ? null : player.heroPool.FirstOrDefault();
            if (heroId == null)
            {
                throw new InvalidOperationException($"Player \"{player.id}\" has no hero pool.");
            }

            return runtimeDatabase.HeroesById[heroId];
        }
    }
}
