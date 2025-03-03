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
        public static string ExamNotFoundError => Properties.ValidationMessages.ExamNotFoundError;
        public static string ExamVersionNotFoundError => Properties.ValidationMessages.ExamVersionNotFoundError;
        public static string WrongValidationModelError => Properties.ValidationMessages.WrongValidationModelError;
        public static string DataAlreadyExists => Properties.ValidationMessages.DataAlreadyExists;
        public static string DataNotFound => Properties.ValidationMessages.DataNotFoundError;

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
        public static string TrainingSectionExistsError => Properties.ValidationMessages.TrainingSectionExistsError;
        public static string TrainingSectionNotExistsError => Properties.ValidationMessages.TrainingSectionNotExistsError;
        public static string TrainingContentTypeNotExistsError => Properties.ValidationMessages.TrainingContentTypeNotExistsError;
        public static string TrainingContentNotExistsError => Properties.ValidationMessages.TrainingContentNotExistsError;
        public static string TrainingContentTypeExistsError => Properties.ValidationMessages.TrainingContentTypeExistsError;
        public static string PreconditionNotFound => Properties.ValidationMessages.PreconditionNotFound;
        public static string ForWhomNotFound => Properties.ValidationMessages.ForWhomNotFound;
        public static string EducationLevelNotFound => Properties.ValidationMessages.EducationLevelNotFound;
        public static string PriceTierNotFound => Properties.ValidationMessages.PriceTierNotFound;
        public static string CategoryExistsError => Properties.ValidationMessages.CategoryExistsError;
        public static string MasterCategoryNotFoundError => Properties.ValidationMessages.MasterCategoryNotFoundError;
        public static string RecursiveCategoryAlreadyExistError => Properties.ValidationMessages.RecursiveCategoryAlreadyExistError;
        public static string MasterCategoryAlreadyExists => Properties.ValidationMessages.MasterCategoryAlreadyExists;


        public static string ContentCommenExistsError => Properties.ValidationMessages.ContentCommenAlreadyExists;
        public static string ContentCommentNotFoundError => Properties.ValidationMessages.ContentCommentNotFoundError;
        public static string ContentCommenNotTrainingUserError => Properties.ValidationMessages.ContentCommenNotTrainingUserError;

        public static string CommentVoteExistsError => Properties.ValidationMessages.CommentVoteExistsError;
        public static string CommentVoteNotFoundError => Properties.ValidationMessages.CommentVoteNotFoundError;



        public static string CurrAccNotExistsError => Properties.ValidationMessages.CurrAccNotExistsError;
        public static string TrainingStatusNotExistsError => Properties.ValidationMessages.TrainingStatusNotExistsError;
        public static string ConfirmUserNotExistsError => Properties.ValidationMessages.ConfirmUserNotExistsError;
        public static string TrainingAlreadyAssignedError => Properties.ValidationMessages.TrainingAlreadyAssignedError;


        public static string TrainingAnnouncementNotFoundError => Properties.ValidationMessages.TrainingAnnouncementNotFoundError; 
        public static string TrainingAnnouncementExistsError => Properties.ValidationMessages.TrainingAnnouncementExistsError;
        public static string TrainingAnnouncementNotTrainingUserError => Properties.ValidationMessages.TrainingAnnouncementNotTrainingUserError;

        

    }
}
