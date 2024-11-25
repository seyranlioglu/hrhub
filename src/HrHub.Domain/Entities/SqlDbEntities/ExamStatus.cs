using HrHub.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamStatus : TypeCardEntity<long>
    {
        public ExamStatus()
        {
            Exams = new HashSet<Exam>();
        }
        public virtual ICollection<Exam> Exams { get; set; } = null;
    }
}
