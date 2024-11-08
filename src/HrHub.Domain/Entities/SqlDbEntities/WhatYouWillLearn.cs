using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class WhatYouWillLearn : TypeCardEntity<int>
    {
        public int TrainingId { get; set; }
    }
}
