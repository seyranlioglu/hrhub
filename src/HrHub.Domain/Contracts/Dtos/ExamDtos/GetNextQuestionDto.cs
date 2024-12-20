using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class GetNextQuestionDto
    {
        public long UserExamId { get; set; } // Kullanıcının sınav kimliği
        public int CurrentQuestionSeqNum { get; set; } // Şu anda bulunan sorunun sıra numarası
    }
}
