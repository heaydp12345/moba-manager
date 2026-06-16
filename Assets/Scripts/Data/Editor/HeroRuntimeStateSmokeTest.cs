using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class HeroRuntimeStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Hero Runtime State")]
        public static void CreateHeroRuntimeState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Hero runtime state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                IReadOnlyList<RuntimeHeroState> heroStates = RuntimeHeroStateFactory.CreateDemoStates(runtimeDatabase, database);

                int skillSlotStateCount = heroStates.Sum(state => state.SkillsBySlot.Count);
                int itemSlotStateCount = heroStates.Sum(state => state.ItemSlots.Count);

                Debug.Log(
                    "Hero runtime state create OK\n" +
                    $"Hero State: {heroStates.Count}\n" +
                    $"Skill Slot State: {skillSlotStateCount}\n" +
                    $"Item Slot State: {itemSlotStateCount}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Hero runtime state creation failed: {exception}");
            }
        }
    }
}
