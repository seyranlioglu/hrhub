using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Enums
{
    public enum TrainingRequestType
    {
        PublishApproval = 1,      // Eğitmen -> Admin (Eğitimi yayınla)
        AccessExtension = 2,      // Personel -> Yönetici (Süre uzat)
        CourseAssignment = 3      // Personel -> Yönetici (Eğitim ata)
    }
}
