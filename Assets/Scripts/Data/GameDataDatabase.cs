using System.Collections.Generic;

namespace MobaManager.Data
{
    public sealed class GameDataDatabase
    {
        public IReadOnlyDictionary<string, HeroData> HeroesById { get; }
        public IReadOnlyDictionary<string, SkillData> SkillsById { get; }
        public IReadOnlyDictionary<string, ItemData> ItemsById { get; }
        public IReadOnlyDictionary<string, BuffData> BuffsById { get; }
        public IReadOnlyDictionary<string, MatchData> MatchesById { get; }
        public IReadOnlyDictionary<string, MapData> MapsById { get; }
        public IReadOnlyDictionary<string, BotData> BotsById { get; }
        public IReadOnlyDictionary<string, PlayerData> PlayersById { get; }
        public IReadOnlyDictionary<string, TeamData> TeamsById { get; }
        public IReadOnlyDictionary<string, LeagueData> LeaguesById { get; }

        public GameDataDatabase(
            IReadOnlyDictionary<string, HeroData> heroesById,
            IReadOnlyDictionary<string, SkillData> skillsById,
            IReadOnlyDictionary<string, ItemData> itemsById,
            IReadOnlyDictionary<string, BuffData> buffsById,
            IReadOnlyDictionary<string, MatchData> matchesById,
            IReadOnlyDictionary<string, MapData> mapsById,
            IReadOnlyDictionary<string, BotData> botsById,
            IReadOnlyDictionary<string, PlayerData> playersById,
            IReadOnlyDictionary<string, TeamData> teamsById,
            IReadOnlyDictionary<string, LeagueData> leaguesById)
        {
            HeroesById = heroesById;
            SkillsById = skillsById;
            ItemsById = itemsById;
            BuffsById = buffsById;
            MatchesById = matchesById;
            MapsById = mapsById;
            BotsById = botsById;
            PlayersById = playersById;
            TeamsById = teamsById;
            LeaguesById = leaguesById;
        }
    }
}
