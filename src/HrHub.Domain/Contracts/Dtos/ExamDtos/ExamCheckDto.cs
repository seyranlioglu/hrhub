using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class ExamCheckDto
    {
        public long ExamId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string Description { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public List<ExamVersionCheckDto> ExamVersions { get; set; }
    }

    public class ExamVersionCheckDto
    {
        public long VersionId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string VersionDescription { get; set; }

        [ValidationRules(typeof(ZeroCheckRule))]
        public int VersionNumber { get; set; }

        public bool IsPublished { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public TimeSpan? ExamTime { get; set; }

        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal? PassingScore { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public List<ExamTopicCheckDto> ExamTopics { get; set; }
    }

    public class ExamTopicCheckDto
    {
        public long TopicId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }

        [ValidationRules(typeof(ZeroCheckRule))]
        public int? QuestionCount { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public List<ExamQuestionCheckDto> ExamQuestions { get; set; }
    }

    public class ExamQuestionCheckDto
    {
        public long QuestionId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string QuestionText { get; set; }

        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal Score { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public List<QuestionOptionCheckDto> QuestionOptions { get; set; }
    }

    public class QuestionOptionCheckDto
    {
        public long OptionId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }
    }
}
