using System.Collections.Generic;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class MatchClockStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Test Match Clock State")]
        public static void TestMatchClockState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Match clock state test failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);

                matchState.Tick(1f);
                if (matchState.ElapsedTime != 0f)
                {
                    Debug.LogError("Match clock state test failed: tick before start changed elapsed time.");
                    return;
                }

                matchState.Start();
                matchState.Tick(matchState.TimeLimit + 10f);

                Debug.Log(
                    "Match clock state test OK\n" +
                    $"Started: {matchState.IsStarted}\n" +
                    $"Completed: {matchState.IsCompleted}\n" +
                    $"Elapsed Time: {matchState.ElapsedTime}\n" +
                    $"Time Limit: {matchState.TimeLimit}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Match clock state test failed: {exception}");
            }
        }
    }
}
