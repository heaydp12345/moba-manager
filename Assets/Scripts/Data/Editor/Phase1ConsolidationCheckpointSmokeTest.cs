using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class Phase1ConsolidationCheckpointSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Run Phase 1 Consolidation Checkpoint")]
        public static void RunPhase1ConsolidationCheckpoint()
        {
            try
            {
                var checkpointErrors = new List<string>();
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> dataErrors = GameDataValidator.Validate(database);

                if (dataErrors.Count > 0)
                {
                    Debug.LogError("Phase 1 consolidation checkpoint failed because game data is invalid:\n- " + string.Join("\n- ", dataErrors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);
                RuntimeCommandValidator validator = RuntimeCommandValidatorFactory.Create(query);
                var eventLog = new RuntimeEventLog();
                RuntimeMutationCommandService commands = RuntimeMutationCommandServiceFactory.Create(query, eventLog);
                RuntimeHeroState firstHero = query.AllHeroStates[0];
                RuntimeItemReference firstItem = RuntimeItemReferenceFactory.BuildReferences(database).Values.First();

                RuntimeCommandValidationResult validSkill = validator.ValidateSkillLevel(firstHero.InstanceId, "q", 1);
                RuntimeCommandValidationResult invalidSkill = validator.ValidateSkillLevel(firstHero.InstanceId, "q", 99);

                commands.SetHeroPositionForDataSetup(firstHero.InstanceId, new RuntimePoint(10f, 20f));
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 1);
                commands.SetItemForDataSetup(firstHero.InstanceId, 0, firstItem);
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 99);

                string report = RuntimeStateDebugReportFactory.Create(query, eventLog).Build();

                Expect(checkpointErrors, runtimeDatabase.HeroesById.Count == 1, "Hero model count should be 1.");
                Expect(checkpointErrors, runtimeDatabase.MapsById.Count == 1, "Map model count should be 1.");
                Expect(checkpointErrors, query.AllHeroStates.Count == 10, "Hero state count should be 10.");
                Expect(checkpointErrors, query.AllUnits.Count == 18, "Unit state count should be 18.");
                Expect(checkpointErrors, query.GetUnitsByTeam("blue").Count == 9, "Blue unit count should be 9.");
                Expect(checkpointErrors, query.GetUnitsByTeam("red").Count == 9, "Red unit count should be 9.");
                Expect(checkpointErrors, query.GetUnitsByType("hero").Count == 10, "Hero unit count should be 10.");
                Expect(checkpointErrors, query.GetUnitsByType("tower").Count == 6, "Tower unit count should be 6.");
                Expect(checkpointErrors, query.GetUnitsByType("nexus").Count == 2, "Nexus unit count should be 2.");
                Expect(checkpointErrors, validSkill.IsValid, "Valid skill command should pass validation.");
                Expect(checkpointErrors, !invalidSkill.IsValid, "Invalid skill command should fail validation.");
                Expect(checkpointErrors, eventLog.Count == 4, "Event log count should be 4.");
                Expect(checkpointErrors, eventLog.Records.Count(record => record.IsAccepted) == 3, "Accepted event count should be 3.");
                Expect(checkpointErrors, eventLog.Records.Count(record => !record.IsAccepted) == 1, "Rejected event count should be 1.");
                Expect(checkpointErrors, report.Contains("Runtime state debug report"), "Debug report should contain header.");
                Expect(checkpointErrors, report.Contains("Event Count: 4"), "Debug report should include event count.");

                if (checkpointErrors.Count > 0)
                {
                    Debug.LogError("Phase 1 consolidation checkpoint failed:\n- " + string.Join("\n- ", checkpointErrors));
                    return;
                }

                Debug.Log(
                    "Phase 1 consolidation checkpoint OK\n" +
                    "Data Validation: OK\n" +
                    $"Hero Model: {runtimeDatabase.HeroesById.Count}\n" +
                    $"Map Model: {runtimeDatabase.MapsById.Count}\n" +
                    $"Hero State: {query.AllHeroStates.Count}\n" +
                    $"Unit State: {query.AllUnits.Count}\n" +
                    $"Blue Unit: {query.GetUnitsByTeam("blue").Count}\n" +
                    $"Red Unit: {query.GetUnitsByTeam("red").Count}\n" +
                    $"Command Validation: OK\n" +
                    $"Event Count: {eventLog.Count}\n" +
                    $"Accepted Event: {eventLog.Records.Count(record => record.IsAccepted)}\n" +
                    $"Rejected Event: {eventLog.Records.Count(record => !record.IsAccepted)}\n" +
                    "Debug Report: OK");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Phase 1 consolidation checkpoint failed: {exception}");
            }
        }

        private static void Expect(List<string> errors, bool condition, string message)
        {
            if (!condition)
            {
                errors.Add(message);
            }
        }
    }
}
