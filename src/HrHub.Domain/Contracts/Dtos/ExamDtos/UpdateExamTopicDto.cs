using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamTopicDto
    {
        public long Id { get; set; }
        [AllowNull]
        public int? QuestionCount { get; set; }
        public string Title { get; set; }
        [AllowNull]
        public string? ImgPath { get; set; }
        /// <summary>
        /// Sıra Numarası. UI da açılan sıraya göre verilecek.
        /// </summary>
        public int SeqNumber { get; set; }
    }
}
