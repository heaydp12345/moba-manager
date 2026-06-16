using System.Collections.Generic;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeUnitRegistrySmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Unit Registry")]
        public static void CreateRuntimeUnitRegistry()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime unit registry creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeUnitRegistry registry = matchState.CreateUnitRegistry();

                Debug.Log(
                    "Runtime unit registry create OK\n" +
                    $"All Unit: {registry.AllUnits.Count}\n" +
                    $"Hero Unit: {registry.GetUnitsByType("hero").Count}\n" +
                    $"Tower Unit: {registry.GetUnitsByType("tower").Count}\n" +
                    $"Nexus Unit: {registry.GetUnitsByType("nexus").Count}\n" +
                    $"Blue Unit: {registry.GetUnitsByTeam("blue").Count}\n" +
                    $"Red Unit: {registry.GetUnitsByTeam("red").Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime unit registry creation failed: {exception}");
            }
        }
    }
}
