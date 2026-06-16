using System.Collections.Generic;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeQueryServiceSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Test Runtime Query Service")]
        public static void TestRuntimeQueryService()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime query service test failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);

                RuntimeHeroState firstHero = query.AllHeroStates[0];
                bool foundHero = query.TryGetHeroState(firstHero.InstanceId, out RuntimeHeroState queriedHero);
                bool foundUnit = query.TryGetUnit(firstHero.InstanceId, out RuntimeUnitState queriedUnit);

                Debug.Log(
                    "Runtime query service test OK\n" +
                    $"All Unit: {query.AllUnits.Count}\n" +
                    $"Blue Hero: {query.GetHeroStatesByTeam("blue").Count}\n" +
                    $"Red Hero: {query.GetHeroStatesByTeam("red").Count}\n" +
                    $"Blue Tower: {query.GetTowersByTeam("blue").Count}\n" +
                    $"Red Tower: {query.GetTowersByTeam("red").Count}\n" +
                    $"Hero Unit: {query.GetUnitsByType("hero").Count}\n" +
                    $"Top Lane Node: {query.GetLanePathNodes("top").Count}\n" +
                    $"Try Get Hero: {foundHero}\n" +
                    $"Try Get Unit: {foundUnit}\n" +
                    $"Queried Hero Matches Unit: {queriedHero.InstanceId == queriedUnit.UnitId}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime query service test failed: {exception}");
            }
        }
    }
}
