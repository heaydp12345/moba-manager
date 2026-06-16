using System;
using System.Linq;
using System.Text;

namespace MobaManager.Match
{
    public sealed class RuntimeStateDebugReport
    {
        private readonly RuntimeQueryService query;
        private readonly RuntimeEventLog eventLog;

        public RuntimeStateDebugReport(RuntimeQueryService query, RuntimeEventLog eventLog)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            this.query = query;
            this.eventLog = eventLog;
        }

        public string Build()
        {
            RuntimeMatchState matchState = query.MatchState;
            int acceptedEventCount = eventLog == null ? 0 : eventLog.Records.Count(record => record.IsAccepted);
            int rejectedEventCount = eventLog == null ? 0 : eventLog.Records.Count(record => !record.IsAccepted);

            var builder = new StringBuilder();
            builder.AppendLine("Runtime state debug report");
            builder.AppendLine($"Match: {matchState.InstanceId}");
            builder.AppendLine($"Map: {matchState.Map.Id}");
            builder.AppendLine($"Started: {matchState.IsStarted}");
            builder.AppendLine($"Completed: {matchState.IsCompleted}");
            builder.AppendLine($"Elapsed Time: {matchState.ElapsedTime}");
            builder.AppendLine($"Blue Team: {matchState.BlueTeam.Team.Id}");
            builder.AppendLine($"Red Team: {matchState.RedTeam.Team.Id}");
            builder.AppendLine($"Hero State: {query.AllHeroStates.Count}");
            builder.AppendLine($"Unit State: {query.AllUnits.Count}");
            builder.AppendLine($"Blue Unit: {query.GetUnitsByTeam("blue").Count}");
            builder.AppendLine($"Red Unit: {query.GetUnitsByTeam("red").Count}");
            builder.AppendLine($"Hero Unit: {query.GetUnitsByType("hero").Count}");
            builder.AppendLine($"Tower Unit: {query.GetUnitsByType("tower").Count}");
            builder.AppendLine($"Nexus Unit: {query.GetUnitsByType("nexus").Count}");
            builder.AppendLine($"Event Count: {eventLog?.Count ?? 0}");
            builder.AppendLine($"Accepted Event: {acceptedEventCount}");
            builder.AppendLine($"Rejected Event: {rejectedEventCount}");
            return builder.ToString();
        }
    }

    public static class RuntimeStateDebugReportFactory
    {
        public static RuntimeStateDebugReport Create(RuntimeQueryService query, RuntimeEventLog eventLog)
        {
            return new RuntimeStateDebugReport(query, eventLog);
        }
    }
}
