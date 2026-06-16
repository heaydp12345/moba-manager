using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class RuntimeStateDebugReportSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Print Runtime State Debug Report")]
        public static void PrintRuntimeStateDebugReport()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Runtime state debug report failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeMatchState matchState = RuntimeMatchStateFactory.CreateDemoState(runtimeDatabase, database);
                RuntimeQueryService query = RuntimeQueryServiceFactory.Create(matchState);
                var eventLog = new RuntimeEventLog();
                RuntimeMutationCommandService commands = RuntimeMutationCommandServiceFactory.Create(query, eventLog);
                RuntimeHeroState firstHero = query.AllHeroStates[0];
                RuntimeItemReference firstItem = RuntimeItemReferenceFactory.BuildReferences(database).Values.First();

                commands.SetHeroPositionForDataSetup(firstHero.InstanceId, new RuntimePoint(10f, 20f));
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 1);
                commands.SetItemForDataSetup(firstHero.InstanceId, 0, firstItem);
                commands.SetSkillLevelForDataSetup(firstHero.InstanceId, "q", 99);

                string report = RuntimeStateDebugReportFactory.Create(query, eventLog).Build();

                Debug.Log(
                    "Runtime state debug report OK\n" +
                    report);
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Runtime state debug report failed: {exception}");
            }
        }
    }
}
