using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class TrainingFilterOptionsDto
    {
        public List<FilterItemDto> Categories { get; set; } = new();
        public List<FilterItemDto> Levels { get; set; } = new();
        public List<FilterItemDto> Languages { get; set; } = new();
        public List<FilterItemDto> Instructors { get; set; } = new();
    }

    public class FilterItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; } // YENİ: Hiyerarşi için gerekli
    }
}