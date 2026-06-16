using System;
using System.Linq;

namespace MobaManager.Match
{
    public sealed class RuntimeCommandValidationResult
    {
        private RuntimeCommandValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public bool IsValid { get; }
        public string Message { get; }

        public static RuntimeCommandValidationResult Valid()
        {
            return new RuntimeCommandValidationResult(true, string.Empty);
        }

        public static RuntimeCommandValidationResult Invalid(string message)
        {
            return new RuntimeCommandValidationResult(false, message);
        }
    }

    public sealed class RuntimeCommandValidator
    {
        private readonly RuntimeQueryService query;

        public RuntimeCommandValidator(RuntimeQueryService query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            this.query = query;
        }

        public RuntimeCommandValidationResult ValidateHeroExists(string heroInstanceId)
        {
            if (string.IsNullOrWhiteSpace(heroInstanceId))
            {
                return RuntimeCommandValidationResult.Invalid("Hero state id cannot be empty.");
            }

            return query.TryGetHeroState(heroInstanceId, out _)
                ? RuntimeCommandValidationResult.Valid()
                : RuntimeCommandValidationResult.Invalid($"Hero state \"{heroInstanceId}\" was not found.");
        }

        public RuntimeCommandValidationResult ValidateSkillLevel(
            string heroInstanceId,
            string skillSlot,
            int level)
        {
            RuntimeCommandValidationResult heroResult = ValidateHeroExists(heroInstanceId);
            if (!heroResult.IsValid)
            {
                return heroResult;
            }

            if (string.IsNullOrWhiteSpace(skillSlot))
            {
                return RuntimeCommandValidationResult.Invalid("Skill slot cannot be empty.");
            }

            RuntimeHeroState heroState = query.GetHeroState(heroInstanceId);
            if (!heroState.SkillsBySlot.TryGetValue(skillSlot, out RuntimeSkillState skillState))
            {
                return RuntimeCommandValidationResult.Invalid($"Skill slot \"{skillSlot}\" was not found on hero state \"{heroInstanceId}\".");
            }

            if (level < 0)
            {
                return RuntimeCommandValidationResult.Invalid("Skill level cannot be negative.");
            }

            if (level > skillState.MaxLevel)
            {
                return RuntimeCommandValidationResult.Invalid("Skill level cannot exceed max level.");
            }

            return RuntimeCommandValidationResult.Valid();
        }

        public RuntimeCommandValidationResult ValidateItemSlot(
            string heroInstanceId,
            int itemSlotIndex,
            RuntimeItemReference item)
        {
            RuntimeCommandValidationResult heroResult = ValidateHeroExists(heroInstanceId);
            if (!heroResult.IsValid)
            {
                return heroResult;
            }

            if (item == null)
            {
                return RuntimeCommandValidationResult.Invalid("Item reference cannot be null.");
            }

            RuntimeHeroState heroState = query.GetHeroState(heroInstanceId);
            bool itemSlotExists = heroState.ItemSlots.Any(slot => slot.SlotIndex == itemSlotIndex);
            return itemSlotExists
                ? RuntimeCommandValidationResult.Valid()
                : RuntimeCommandValidationResult.Invalid($"Item slot \"{itemSlotIndex}\" was not found on hero state \"{heroInstanceId}\".");
        }
    }

    public static class RuntimeCommandValidatorFactory
    {
        public static RuntimeCommandValidator Create(RuntimeQueryService query)
        {
            return new RuntimeCommandValidator(query);
        }
    }
}
