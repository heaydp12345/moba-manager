using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeCommandValidationSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Test Runtime Command Validation")]
        public static void TestRuntimeCommandValidation()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime command validation test failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);
                RuntimeCommandValidator validator = RuntimeCommandValidatorFactory.Create(query);
                RuntimeHeroState firstHero = query.AllHeroStates[0];
                RuntimeItemReference firstItem = RuntimeItemReferenceFactory.BuildReferences(database).Values.First();

                RuntimeCommandValidationResult validHero = validator.ValidateHeroExists(firstHero.InstanceId);
                RuntimeCommandValidationResult missingHero = validator.ValidateHeroExists("missing_hero");
                RuntimeCommandValidationResult validSkill = validator.ValidateSkillLevel(firstHero.InstanceId, "q", 1);
                RuntimeCommandValidationResult invalidSkillSlot = validator.ValidateSkillLevel(firstHero.InstanceId, "x", 1);
                RuntimeCommandValidationResult invalidSkillLevel = validator.ValidateSkillLevel(firstHero.InstanceId, "q", 99);
                RuntimeCommandValidationResult validItemSlot = validator.ValidateItemSlot(firstHero.InstanceId, 0, firstItem);
                RuntimeCommandValidationResult invalidItemSlot = validator.ValidateItemSlot(firstHero.InstanceId, 99, firstItem);

                int validCount = new[] { validHero, validSkill, validItemSlot }.Count(result => result.IsValid);
                int invalidCount = new[] { missingHero, invalidSkillSlot, invalidSkillLevel, invalidItemSlot }.Count(result => !result.IsValid);

                Debug.Log(
                    "Runtime command validation test OK\n" +
                    $"Valid Check: {validCount}\n" +
                    $"Invalid Check: {invalidCount}\n" +
                    $"Missing Hero Blocked: {!missingHero.IsValid}\n" +
                    $"Invalid Skill Slot Blocked: {!invalidSkillSlot.IsValid}\n" +
                    $"Invalid Skill Level Blocked: {!invalidSkillLevel.IsValid}\n" +
                    $"Invalid Item Slot Blocked: {!invalidItemSlot.IsValid}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime command validation test failed: {exception}");
            }
        }
    }
}
