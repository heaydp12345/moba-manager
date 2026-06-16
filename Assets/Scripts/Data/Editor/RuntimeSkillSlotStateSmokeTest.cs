using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeSkillSlotStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Runtime Skill Slot State")]
        public static void CreateRuntimeSkillSlotState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime skill slot state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                IReadOnlyList<RuntimeHeroState> heroStates = matchState.AllHeroStates;
                IReadOnlyList<RuntimeSkillState> skillStates = heroStates
                    .SelectMany(hero => hero.SkillsBySlot.Values)
                    .ToArray();

                int passiveMaxLevel = MaxLevelForSlot(skillStates, "passive");
                int qMaxLevel = MaxLevelForSlot(skillStates, "q");
                int wMaxLevel = MaxLevelForSlot(skillStates, "w");
                int eMaxLevel = MaxLevelForSlot(skillStates, "e");
                int rMaxLevel = MaxLevelForSlot(skillStates, "r");
                int canLevelUpCount = skillStates.Count(skill => skill.CanLevelUp);

                Debug.Log(
                    "Runtime skill slot state create OK\n" +
                    $"Hero State: {heroStates.Count}\n" +
                    $"Skill Slot State: {skillStates.Count}\n" +
                    $"Can Level Up State: {canLevelUpCount}\n" +
                    $"Passive Max Level: {passiveMaxLevel}\n" +
                    $"Q Max Level: {qMaxLevel}\n" +
                    $"W Max Level: {wMaxLevel}\n" +
                    $"E Max Level: {eMaxLevel}\n" +
                    $"R Max Level: {rMaxLevel}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime skill slot state creation failed: {exception}");
            }
        }

        private static int MaxLevelForSlot(IEnumerable<RuntimeSkillState> skillStates, string slot)
        {
            return skillStates.First(skill => skill.Slot == slot).MaxLevel;
        }
    }
}
