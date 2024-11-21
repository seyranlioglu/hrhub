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
    }
}
