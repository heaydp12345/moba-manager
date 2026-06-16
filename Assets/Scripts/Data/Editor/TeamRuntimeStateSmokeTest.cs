using System.Collections.Generic;
using System.Linq;
using MobaManager.Data;
using MobaManager.Match;
using UnityEditor;
using UnityEngine;

namespace MobaManager.Editor
{
    public static class TeamRuntimeStateSmokeTest
    {
        [MenuItem("Tools/MOBA Manager/Create Team Runtime State")]
        public static void CreateTeamRuntimeState()
        {
            try
            {
                GameDataDatabase database = GameDataLoader.LoadFromAssets();
                List<string> errors = GameDataValidator.Validate(database);

                if (errors.Count > 0)
                {
                    Debug.LogError("Team runtime state creation failed because game data is invalid:\n- " + string.Join("\n- ", errors));
                    return;
                }

                RuntimeEntityModelDatabase runtimeDatabase = RuntimeEntityFactory.Build(database);
                RuntimeTeamState blueTeam = RuntimeTeamStateFactory.CreateDemoState("blue", database, runtimeDatabase);
                RuntimeTeamState redTeam = RuntimeTeamStateFactory.CreateDemoState("red", database, runtimeDatabase);

                int totalHeroStateCount = blueTeam.RosterSlots.Count + redTeam.RosterSlots.Count;
                string blueLanes = string.Join(", ", blueTeam.RosterSlots.Select(slot => slot.Lane));
                string redLanes = string.Join(", ", redTeam.RosterSlots.Select(slot => slot.Lane));

                Debug.Log(
                    "Team runtime state create OK\n" +
                    $"Blue Team: {blueTeam.Team.Id}\n" +
                    $"Red Team: {redTeam.Team.Id}\n" +
                    $"Blue Roster Slot: {blueTeam.RosterSlots.Count}\n" +
                    $"Red Roster Slot: {redTeam.RosterSlots.Count}\n" +
                    $"Total Hero State: {totalHeroStateCount}\n" +
                    $"Blue Lanes: {blueLanes}\n" +
                    $"Red Lanes: {redLanes}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Team runtime state creation failed: {exception}");
            }
        }
    }
}
