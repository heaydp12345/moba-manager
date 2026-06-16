using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimePositionStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Position State")]
        public static void CreateRuntimePositionState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime position state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                IReadOnlyList<RuntimePositionState> positions = matchState.GetAllPositionStates();

                int heroPositionCount = positions.Count(position => position.EntityType == "hero");
                int towerPositionCount = positions.Count(position => position.EntityType == "tower");
                int nexusPositionCount = positions.Count(position => position.EntityType == "nexus");

                Debug.Log(
                    "Runtime position state create OK\n" +
                    $"Hero Position: {heroPositionCount}\n" +
                    $"Tower Position: {towerPositionCount}\n" +
                    $"Nexus Position: {nexusPositionCount}\n" +
                    $"Total Position: {positions.Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime position state creation failed: {exception}");
            }
        }
    }
}
