using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeEntitySmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Build Runtime Entity Model")]
        public static void BuildRuntimeEntityModel()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime entity model build failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);

                int nexusCount = runtimeDatabase.MapsById.Values.Sum(map => map.Nexuses.Count);
                int towerCount = runtimeDatabase.MapsById.Values.Sum(map => map.Towers.Count);
                int laneRouteNodeCount = runtimeDatabase.MapsById.Values
                    .SelectMany(map => map.Lanes)
                    .Sum(lane => lane.PathNodes.Count);

                Debug.Log(
                    "Runtime entity model build OK\n" +
                    $"Hero Model: {runtimeDatabase.HeroesById.Count}\n" +
                    $"Map Model: {runtimeDatabase.MapsById.Count}\n" +
                    $"Nexus: {nexusCount}\n" +
                    $"Tower: {towerCount}\n" +
                    $"Lane Route Node: {laneRouteNodeCount}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime entity model build failed: {exception}");
            }
        }
    }
}
