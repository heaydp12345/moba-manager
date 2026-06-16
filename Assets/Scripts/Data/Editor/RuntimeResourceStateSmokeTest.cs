using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeResourceStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Resource State")]
        public static void CreateRuntimeResourceState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime resource state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                IReadOnlyList<RuntimeResourceState> resourceStates = matchState.GetAllResourceStates();

                int manaResourceCount = resourceStates.Count(resource => resource.ResourceType == "mana");
                int energyResourceCount = resourceStates.Count(resource => resource.ResourceType == "energy");
                int noneResourceCount = resourceStates.Count(resource => resource.ResourceType == "none");

                Debug.Log(
                    "Runtime resource state create OK\n" +
                    $"Hero Resource: {resourceStates.Count}\n" +
                    $"Mana Resource: {manaResourceCount}\n" +
                    $"Energy Resource: {energyResourceCount}\n" +
                    $"None Resource: {noneResourceCount}\n" +
                    $"Total Resource: {resourceStates.Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime resource state creation failed: {exception}");
            }
        }
    }
}
