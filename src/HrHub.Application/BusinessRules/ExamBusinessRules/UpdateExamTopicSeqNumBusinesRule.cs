using HrHub.Abstraction.Consts;
using HrHub.Domain.Entities.SqlDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.ExamBusinessRules
{
    public class UpdateExamTopicSeqNumBusinesRule : IUpdateExamTopicSeqNumBusinesRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            (bool IsValid, string ErrorMessage) result = (true, string.Empty);
            if (value is Tuple<ExamTopic, int> examTopic)
            {
                return examTopic.Item1 is null ? (false, ValidationMessages.DataNotFound) :
                    (examTopic.Item2 < 1 || examTopic.Item2 > examTopic.Item1.ExamVersion.ExamTopics.Count) ?
                    (false, "Invalid Sequesnce Number!") :
                    result;
            }
            else
                result = (false, ValidationMessages.WrongValidationModelError);

            return result;
        }
    }
}
