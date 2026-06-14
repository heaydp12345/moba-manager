using System.Collections.Generic;
using MobaManager.Data;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class DataLoadSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Validate Game Data")]
        public static void ValidateGameData()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Game data validation failed:\n- " + string.Join("\n- ", errors));
                    return;
                }

                Debug.Log(
                    "Game data loaded successfully.\n" +
                    $"Hero: {database.HeroesById.Count}\n" +
                    $"Skill: {database.SkillsById.Count}\n" +
                    $"Item: {database.ItemsById.Count}\n" +
                    $"Buff: {database.BuffsById.Count}\n" +
                    $"Match: {database.MatchesById.Count}\n" +
                    $"Map: {database.MapsById.Count}\n" +
                    $"Bot: {database.BotsById.Count}\n" +
                    $"Player: {database.PlayersById.Count}\n" +
                    $"Team: {database.TeamsById.Count}\n" +
                    $"League: {database.LeaguesById.Count}\n" +
                    "Cross references OK");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Game data loading failed: {exception}");
            }
        }
    }
}
