using System;
using System.Collections.Generic;

namespace MobaManager.Match
{
    public sealed class RuntimeEventRecord
    {
        public RuntimeEventRecord(
            int sequence,
            string category,
            string eventType,
            string targetId,
            bool isAccepted,
            string message,
            float elapsedTime)
        {
            Sequence = sequence;
            Category = category;
            EventType = eventType;
            TargetId = targetId;
            IsAccepted = isAccepted;
            Message = message;
            ElapsedTime = elapsedTime;
        }

        public int Sequence { get; }
        public string Category { get; }
        public string EventType { get; }
        public string TargetId { get; }
        public bool IsAccepted { get; }
        public string Message { get; }
        public float ElapsedTime { get; }
    }

    public sealed class RuntimeEventLog
    {
        private readonly List<RuntimeEventRecord> records = new List<RuntimeEventRecord>();

        public IReadOnlyList<RuntimeEventRecord> Records => records;
        public int Count => records.Count;

        public RuntimeEventRecord Record(
            string category,
            string eventType,
            string targetId,
            bool isAccepted,
            string message,
            float elapsedTime)
        {
            var record = new RuntimeEventRecord(
                records.Count + 1,
                category ?? string.Empty,
                eventType ?? string.Empty,
                targetId ?? string.Empty,
                isAccepted,
                message ?? string.Empty,
                elapsedTime);

            records.Add(record);
            return record;
        }

        public RuntimeEventRecord RecordCommand(
            string commandType,
            string targetId,
            RuntimeMutationResult result,
            float elapsedTime)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return Record(
                "command",
                commandType,
                targetId,
                result.IsSuccess,
                result.Message,
                elapsedTime);
        }
    }
}
