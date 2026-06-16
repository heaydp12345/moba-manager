using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeHealthStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Health State")]
        public static void CreateRuntimeHealthState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime health state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                IReadOnlyList<RuntimeHealthState> healthStates = matchState.GetAllHealthStates();

                int heroHealthCount = healthStates.Count(health => health.EntityType == "hero");
                int towerHealthCount = healthStates.Count(health => health.EntityType == "tower");
                int nexusHealthCount = healthStates.Count(health => health.EntityType == "nexus");

                Debug.Log(
                    "Runtime health state create OK\n" +
                    $"Hero Health: {heroHealthCount}\n" +
                    $"Tower Health: {towerHealthCount}\n" +
                    $"Nexus Health: {nexusHealthCount}\n" +
                    $"Total Health: {healthStates.Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime health state creation failed: {exception}");
            }
        }
    }
}
