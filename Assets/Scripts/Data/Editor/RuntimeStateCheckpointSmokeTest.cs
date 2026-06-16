using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeStateCheckpointSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Run Runtime State Checkpoint")]
        public static void RunRuntimeStateCheckpoint()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime state checkpoint failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeUnitRegistry unitRegistry = matchState.CreateUnitRegistry();

                IReadOnlyList<RuntimeHeroState> heroStates = matchState.AllHeroStates;
                IReadOnlyList<RuntimeSkillState> skillStates = heroStates
                    .SelectMany(hero => hero.SkillsBySlot.Values)
                    .ToArray();
                IReadOnlyList<RuntimeItemSlotState> itemSlots = heroStates
                    .SelectMany(hero => hero.ItemSlots)
                    .ToArray();
                IReadOnlyList<RuntimePositionState> positionStates = matchState.GetAllPositionStates();
                IReadOnlyList<RuntimeHealthState> healthStates = matchState.GetAllHealthStates();
                IReadOnlyList<RuntimeResourceState> resourceStates = matchState.GetAllResourceStates();

                matchState.Start();
                matchState.Tick(matchState.TimeLimit);

                Debug.Log(
                    "Runtime state checkpoint OK\n" +
                    $"Hero Model: {runtimeDatabase.HeroesById.Count}\n" +
                    $"Map Model: {runtimeDatabase.MapsById.Count}\n" +
                    "Match State: 1\n" +
                    "Team State: 2\n" +
                    $"Hero State: {heroStates.Count}\n" +
                    $"Skill Slot State: {skillStates.Count}\n" +
                    $"Item Slot State: {itemSlots.Count}\n" +
                    $"Position State: {positionStates.Count}\n" +
                    $"Health State: {healthStates.Count}\n" +
                    $"Resource State: {resourceStates.Count}\n" +
                    $"Unit Registry: {unitRegistry.AllUnits.Count}\n" +
                    $"Blue Unit: {unitRegistry.GetUnitsByTeam("blue").Count}\n" +
                    $"Red Unit: {unitRegistry.GetUnitsByTeam("red").Count}\n" +
                    $"Clock Completed: {matchState.IsCompleted}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime state checkpoint failed: {exception}");
            }
        }
    }
}
