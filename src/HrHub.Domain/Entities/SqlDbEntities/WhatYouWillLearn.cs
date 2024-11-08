using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class WhatYouWillLearn : TypeCardEntity<long>
    {
        public long TrainingId { get; set; }
    }
}
