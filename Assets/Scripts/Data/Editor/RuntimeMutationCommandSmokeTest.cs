using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeMutationCommandSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Test Runtime Mutation Boundary")]
        public static void TestRuntimeMutationBoundary()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime mutation boundary test failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);
                RuntimeMutationCommandService commands = RuntimeMutationCommandServiceFactory.Create(query);
                IReadOnlyDictionary<string, RuntimeItemReference> itemReferences = RuntimeItemReferenceFactory.BuildReferences(database);

                RuntimeHeroState firstHero = query.AllHeroStates[0];
                RuntimeItemReference firstItem = itemReferences.Values.First();

                RuntimeMutationResult positionResult = commands.SetHeroPositionForDataSetup(
                    firstHero.InstanceId,
                    new RuntimePoint(123f, 456f));
                RuntimeMutationResult skillResult = commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 1);
                RuntimeMutationResult itemResult = commands.SetItemForDataSetup(firstHero.InstanceId, 0, firstItem);

                bool positionUpdated = firstHero.Position.Position.X == 123f && firstHero.Position.Position.Y == 456f;
                int qSkillLevel = firstHero.SkillsBySlot["q"].Level;
                int filledItemSlotCount = firstHero.ItemSlots.Count(slot => !slot.IsEmpty);
                int successCount = new[] { positionResult, skillResult, itemResult }.Count(result => result.IsSuccess);

                Debug.Log(
                    "Runtime mutation boundary test OK\n" +
                    $"Command Success: {successCount}\n" +
                    $"Position Updated: {positionUpdated}\n" +
                    $"Q Skill Level: {qSkillLevel}\n" +
                    $"Filled Item Slot: {filledItemSlotCount}\n" +
                    $"Equipped Item: {firstHero.ItemSlots[0].ItemId}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime mutation boundary test failed: {exception}");
            }
        }
    }
}
