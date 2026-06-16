using System.Collections.Generic;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class MatchRuntimeStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Match Runtime State")]
        public static void CreateMatchRuntimeState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Match runtime state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);

                Debug.Log(
                    "Match runtime state create OK\n" +
                    "Match State: 1\n" +
                    $"Blue Team: {matchState.BlueTeam.Team.Id}\n" +
                    $"Red Team: {matchState.RedTeam.Team.Id}\n" +
                    $"Blue Hero State: {matchState.BlueHeroStates.Count}\n" +
                    $"Red Hero State: {matchState.RedHeroStates.Count}\n" +
                    $"Total Hero State: {matchState.TotalHeroStateCount}\n" +
                    $"Map Model: {matchState.Map.Id}\n" +
                    $"Elapsed Time: {matchState.ElapsedTime}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Match runtime state creation failed: {exception}");
            }
        }
    }
}
