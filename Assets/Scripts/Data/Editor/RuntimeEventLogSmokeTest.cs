using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeEventLogSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Test Runtime Event Log")]
        public static void TestRuntimeEventLog()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime event log test failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);
                var eventLog = new RuntimeEventLog();
                RuntimeMutationCommandService commands = RuntimeMutationCommandServiceFactory.Create(query, eventLog);
                RuntimeHeroState firstHero = query.AllHeroStates[0];
                RuntimeItemReference firstItem = RuntimeItemReferenceFactory.BuildReferences(database).Values.First();

                commands.SetHeroPositionForDataSetup(firstHero.InstanceId, new RuntimePoint(12f, 34f));
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 1);
                commands.SetItemForDataSetup(firstHero.InstanceId, 0, firstItem);
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 99);

                int acceptedCount = eventLog.Records.Count(item => item.IsAccepted);
                int rejectedCount = eventLog.Records.Count(item => !item.IsAccepted);
                RuntimeEventRecord firstRecord = eventLog.Records[0];
                RuntimeEventRecord lastRecord = eventLog.Records[eventLog.Records.Count - 1];

                Debug.Log(
                    "Runtime event log test OK\n" +
                    $"Event Count: {eventLog.Count}\n" +
                    $"Accepted Event: {acceptedCount}\n" +
                    $"Rejected Event: {rejectedCount}\n" +
                    $"First Sequence: {firstRecord.Sequence}\n" +
                    $"Last Sequence: {lastRecord.Sequence}\n" +
                    $"Last Event Accepted: {lastRecord.IsAccepted}\n" +
                    $"Last Event Type: {lastRecord.EventType}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime event log test failed: {exception}");
            }
        }
    }
}
