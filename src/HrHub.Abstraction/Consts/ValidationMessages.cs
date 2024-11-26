using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Consts
{
    public static class ValidationMessages
    {
        public static string NullError => Properties.ValidationMessages.NullCheck;
        public static string InvalidEmailError => Properties.ValidationMessages.InvalidEmail;
        public static string ZeroCheckError => Properties.ValidationMessages.ZeroCheck;
        public static string PositiveTimeSpanError => Properties.ValidationMessages.PositiveTimeSpanError;
        public static string ExamQuestionCountError => Properties.ValidationMessages.ExamQuestionOptionsCountError;
        public static string ExamUserExistsError => Properties.ValidationMessages.ExamUserExistsError;
        public static string TrainingExistsError => Properties.ValidationMessages.TrainingExistsError;
        public static string InstructorNotFound => Properties.ValidationMessages.InstructorNotFound;
        public static string CategoryNotFound => Properties.ValidationMessages.CategoryNotFound;
        public static string TrainingLevelNotFound => Properties.ValidationMessages.TrainingLevelNotFound;
        public static string TimeUnitNotFound => Properties.ValidationMessages.TimeUnitNotFound;
        public static string AmountMustBeGreaterThanZero => Properties.ValidationMessages.AmountMustBeGreaterThanZero;
        public static string CurrentAmountMustBeGreaterThanZero => Properties.ValidationMessages.CurrentAmountMustBeGreaterThanZero;
        public static string CompletionTimeAndUnitMustBeProvided => Properties.ValidationMessages.CompletionTimeAndUnitMustBeProvided;
        public static string TrainingStatusNotFound => Properties.ValidationMessages.TrainingStatusNotFound;
        public static string WhatYouWillLearnExistsError => Properties.ValidationMessages.WhatYouWillLearnExistsError;
        public static string TrainingNotExistsError => Properties.ValidationMessages.TrainingNotExistsError;
        public static string WhatYouWillLearnNotExistsError => Properties.ValidationMessages.WhatYouWillLearnNotExistsError;
    }
}
