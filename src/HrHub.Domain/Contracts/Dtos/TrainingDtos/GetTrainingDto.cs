using System.Diagnostics.CodeAnalysis;

namespace HrHub.Abstraction.Contracts.Dtos.TrainingDtos
{
    public class GetTrainingDto
    {
        public long Id { get; set; }
        public string? HeaderImage { get; set; }
        public string? LangCode { get; set; }

        // Category Details
        public string? CategoryCode { get; set; }
        public string? CategoryTitle { get; set; }
        public string? CategoryDescription { get; set; }


        // Instructor Details
        public string? InstructorEmail { get; set; }
        public string? InstructorTitle { get; set; }
        public string? InstructorAddress { get; set; }
        public string? InstructorPhone { get; set; }
        public string? InstructorPicturePath { get; set; }

        public decimal? CurrentAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? CertificateOfAchievementRate { get; set; }
        public decimal? CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }

        // TimeUnit Details
        public string? CompletionTimeUnitCode { get; set; }
        public string? CompletionTimeUnitDescription { get; set; }

        // Training Level Details
        public string? TrainingLevelCode { get; set; }
        public string? TrainingLevelDescription { get; set; }


        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        //Training Status
        public string? TrainingStatusLangCode { get; set; }
        public string? TrainingStatusCode { get; set; }
        public string? TrainingStatusTitle { get; set; }
        public string? TrainingStatusDescription { get; set; }


        public string? CongratulationMessage { get; set; }
        public string? CourseImage { get; set; }

        //Education Level
        public string? EducationLevelCode { get; set; }
        public string? EducationLevelTitle { get; set; }
        public string? EducationLevelDescription { get; set; }


        //ForWhom
        public string? ForWhomCode { get; set; }
        public string? ForWhomTitle { get; set; }
        public string? ForWhomDescription { get; set; }

        public string? Labels { get; set; }

        //Precondition
        public string? PreconditionCode { get; set; }
        public string? PreconditionTitle { get; set; }
        public string? PreconditionDescription { get; set; }


        //PriceTier
        public string? PriceTierCode { get; set; }
        public string? PriceTierTitle { get; set; }
        public string? PriceTierDescription { get; set; }

        public string? SubTitle { get; set; }
        public string? Trailer { get; set; }
        public string? WelcomeMessage { get; set; }


        //TrainingType
        public string? TrainingTypeCode { get; set; }
        public string? TrainingTypeTitle { get; set; }
        public string? TrainingTypeDescription { get; set; }

        //TrainingSection
        public List<TrainingSectionDto> TrainingSections { get; set; } = new();
    }

    public class TrainingSectionDto
    {
        public long? TrainingSectionId { get; set; }
        public string? TrainingSectionCode { get; set; }
        public string? TrainingSectionTitle { get; set; }
        public string? TrainingSectionDescription { get; set; }
        public long? TrainingSectionRowNumber { get; set; }
        public string? TrainingSectionLangCode { get; set; }
        public List<TrainingContentDto> TrainingContents { get; set; } = new();
    }

    public class TrainingContentDto
    {
        public long? Id { get; set; }
        public long? TrainingSectionId { get; set; }
        public long? ContentTypeId { get; set; }
        public TimeSpan? Time { get; set; }
        public int? PageCount { get; set; }
        public string? FilePath { get; set; }
        public bool? Mandatory { get; set; }
        public long? OrderId { get; set; }
        public bool? AllowSeeking { get; set; }
        public int? PartCount { get; set; }
        public int? MinReadTimeThreshold { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long? ExamId { get; set; }
        public TrainingContentTypeDto ContentType { get; set; } = new();


    }

    public class TrainingContentTypeDto
    {
        public long? Id { get; set; }
        public string? LangCode { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

}
