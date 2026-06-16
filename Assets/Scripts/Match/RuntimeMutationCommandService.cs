using System;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeMutationResult
    {
        private RuntimeMutationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }
        public string Message { get; }

        public static RuntimeMutationResult Success(string message)
        {
            return new RuntimeMutationResult(true, message);
        }

        public static RuntimeMutationResult Failure(string message)
        {
            return new RuntimeMutationResult(false, message);
        }
    }

    public sealed class RuntimeMutationCommandService
    {
        private readonly RuntimeQueryService query;
        private readonly RuntimeCommandValidator validator;
        private readonly RuntimeEventLog eventLog;

        public RuntimeMutationCommandService(
            RuntimeQueryService query,
            RuntimeCommandValidator validator,
            RuntimeEventLog eventLog)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.query = query;
            this.validator = validator;
            this.eventLog = eventLog;
        }

        public RuntimeMutationResult SetHeroPositionForDataSetup(string heroInstanceId, RuntimePoint position)
        {
            RuntimeCommandValidationResult validation = validator.ValidateHeroExists(heroInstanceId);
            if (!validation.IsValid)
            {
                return RecordCommandResult(
                    "set_hero_position_for_data_setup",
                    heroInstanceId,
                    RuntimeMutationResult.Failure(validation.Message));
            }

            RuntimeHeroState heroState = query.GetHeroState(heroInstanceId);
            heroState.SetPosition(position);
            return RecordCommandResult(
                "set_hero_position_for_data_setup",
                heroInstanceId,
                RuntimeMutationResult.Success($"Hero state \"{heroInstanceId}\" position was updated."));
        }

        public RuntimeMutationResult SetSkillLevelForDataSetup(string heroInstanceId, string skillSlot, int level)
        {
            RuntimeCommandValidationResult validation = validator.ValidateSkillLevel(heroInstanceId, skillSlot, level);
            if (!validation.IsValid)
            {
                return RecordCommandResult(
                    "set_skill_level_for_data_setup",
                    heroInstanceId,
                    RuntimeMutationResult.Failure(validation.Message));
            }

            RuntimeHeroState heroState = query.GetHeroState(heroInstanceId);
            RuntimeSkillState skillState = heroState.SkillsBySlot[skillSlot];
            skillState.SetLevelForDataSetup(level);
            return RecordCommandResult(
                "set_skill_level_for_data_setup",
                heroInstanceId,
                RuntimeMutationResult.Success($"Skill slot \"{skillSlot}\" level was updated."));
        }

        public RuntimeMutationResult SetItemForDataSetup(
            string heroInstanceId,
            int itemSlotIndex,
            RuntimeItemReference item)
        {
            RuntimeCommandValidationResult validation = validator.ValidateItemSlot(heroInstanceId, itemSlotIndex, item);
            if (!validation.IsValid)
            {
                return RecordCommandResult(
                    "set_item_for_data_setup",
                    heroInstanceId,
                    RuntimeMutationResult.Failure(validation.Message));
            }

            RuntimeHeroState heroState = query.GetHeroState(heroInstanceId);
            RuntimeItemSlotState itemSlot = heroState.ItemSlots.FirstOrDefault(slot => slot.SlotIndex == itemSlotIndex);
            itemSlot.SetItemForDataSetup(item);
            return RecordCommandResult(
                "set_item_for_data_setup",
                heroInstanceId,
                RuntimeMutationResult.Success($"Item slot \"{itemSlotIndex}\" was updated."));
        }

        private RuntimeMutationResult RecordCommandResult(
            string commandType,
            string targetId,
            RuntimeMutationResult result)
        {
            eventLog?.RecordCommand(commandType, targetId, result, query.MatchState.ElapsedTime);
            return result;
        }
    }

    public static class RuntimeMutationCommandServiceFactory
    {
        public static RuntimeMutationCommandService Create(RuntimeQueryService query)
        {
            return Create(query, new RuntimeEventLog());
        }

        public static RuntimeMutationCommandService Create(RuntimeQueryService query, RuntimeEventLog eventLog)
        {
            return new RuntimeMutationCommandService(query, RuntimeCommandValidatorFactory.Create(query), eventLog);
        }
    }
}
