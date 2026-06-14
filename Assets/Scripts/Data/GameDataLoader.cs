using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MobaManager.Data
{
    public static class GameDataLoader
    {
        public static GameDataDatabase LoadFromAssets()
        {
            string dataRoot = Path.Combine(Application.dataPath, "Data");
            return LoadFromDirectory(dataRoot);
        }

        public static GameDataDatabase LoadFromDirectory(string dataRoot)
        {
            BuffDataFile buffFile = LoadJson<BuffDataFile>(dataRoot, "Buffs", "buff.json");
            SkillDataFile skillFile = LoadJson<SkillDataFile>(dataRoot, "Skills", "skill.json");
            HeroDataFile heroFile = LoadJson<HeroDataFile>(dataRoot, "Heroes", "hero.json");
            ItemDataFile itemFile = LoadJson<ItemDataFile>(dataRoot, "Items", "item.json");
            MatchDataFile matchFile = LoadJson<MatchDataFile>(dataRoot, "Matches", "match.json");
            MapDataFile mapFile = LoadJson<MapDataFile>(dataRoot, "Maps", "map.json");
            BotDataFile botFile = LoadJson<BotDataFile>(dataRoot, "Bots", "bot.json");
            PlayerDataFile playerFile = LoadJson<PlayerDataFile>(dataRoot, "Players", "player.json");
            TeamDataFile teamFile = LoadJson<TeamDataFile>(dataRoot, "Teams", "team.json");
            LeagueDataFile leagueFile = LoadJson<LeagueDataFile>(dataRoot, "Leagues", "league.json");

            return new GameDataDatabase(
                BuildIndex(heroFile.heroes, "hero"),
                BuildIndex(skillFile.skills, "skill"),
                BuildIndex(itemFile.items, "item"),
                BuildIndex(buffFile.buffs, "buff"),
                BuildIndex(matchFile.matches, "match"),
                BuildIndex(mapFile.maps, "map"),
                BuildIndex(botFile.bots, "bot"),
                BuildIndex(playerFile.players, "player"),
                BuildIndex(teamFile.teams, "team"),
                BuildIndex(leagueFile.leagues, "league"));
        }

        private static T LoadJson<T>(string dataRoot, string folder, string fileName)
        {
            string path = Path.Combine(dataRoot, folder, fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Missing data file: {path}", path);
            }

            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            if (data == null)
            {
                throw new InvalidOperationException($"Could not parse data file: {path}");
            }

            return data;
        }

        private static Dictionary<string, T> BuildIndex<T>(IEnumerable<T> items, string label) where T : class
        {
            var indexed = new Dictionary<string, T>();
            if (items == null)
            {
                throw new InvalidOperationException($"Data list is missing for {label}.");
            }

            foreach (T item in items)
            {
                string id = ReadId(item);
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new InvalidOperationException($"{label} contains an entry without id.");
                }

                if (indexed.ContainsKey(id))
                {
                    throw new InvalidOperationException($"{label} contains duplicate id: {id}");
                }

                indexed.Add(id, item);
            }

            return indexed;
        }

        private static string ReadId<T>(T item)
        {
            var field = typeof(T).GetField("id");
            return field == null ? null : field.GetValue(item) as string;
        }
    }
}
