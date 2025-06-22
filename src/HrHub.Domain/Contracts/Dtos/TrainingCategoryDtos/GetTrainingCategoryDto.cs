using System.Text.Json.Serialization;

namespace HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos
{
    public class GetTrainingCategoryDto{
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? MasterCategoryId { get; set; }
        public string MasterCategoryTitle { get; set; }
        public string MasterCategoryCode { get; set; }
        public string MasterCategoryDescription { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<GetTrainingCategoryDto> SubCategories { get; set; }
    }
}
