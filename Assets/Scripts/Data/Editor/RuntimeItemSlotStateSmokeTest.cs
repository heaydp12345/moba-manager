using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeItemSlotStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Item Slot State")]
        public static void CreateRuntimeItemSlotState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime item slot state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                IReadOnlyDictionary<string, RuntimeItemReference> itemReferences = RuntimeItemReferenceFactory.BuildReferences(database);
                IReadOnlyList<RuntimeHeroState> heroStates = matchState.AllHeroStates;
                IReadOnlyList<RuntimeItemSlotState> itemSlots = heroStates
                    .SelectMany(hero => hero.ItemSlots)
                    .ToArray();

                int emptySlotCount = itemSlots.Count(slot => slot.IsEmpty);
                int filledSlotCount = itemSlots.Count(slot => !slot.IsEmpty);

                Debug.Log(
                    "Runtime item slot state create OK\n" +
                    $"Hero State: {heroStates.Count}\n" +
                    $"Item Slot State: {itemSlots.Count}\n" +
                    $"Empty Item Slot: {emptySlotCount}\n" +
                    $"Filled Item Slot: {filledSlotCount}\n" +
                    $"Formal Item Reference: {itemReferences.Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime item slot state creation failed: {exception}");
            }
        }
    }
}
