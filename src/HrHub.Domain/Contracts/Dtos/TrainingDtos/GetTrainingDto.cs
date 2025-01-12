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
        public string InstructorEmail { get; set; }
        public string InstructorTitle { get; set; }
        public string InstructorAddress { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorPicturePath { get; set; }

        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }

        // TimeUnit Details
        public string? CompletionTimeUnitCode { get; set; }
        public string? CompletionTimeUnitDescription { get; set; }

        // Training Level Details
        public string? TrainingLevelCode { get; set; }
        public string? TrainingLevelDescription { get; set; }

       
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        //Training Status
        public string? TrainingStatusLangCode { get; set; }
        public string? TrainingStatusCode { get; set; }
        public string? TrainingStatusTitle { get; set; }
        public string? TrainingStatusDescription { get; set; }


        public string? CongratulationMessage { get; set; }
        public string? CourseImage { get; set; }

        //Education Level
        public string EducationLevelCode { get; set; }
        public string EducationLevelTitle { get; set; }
        public string EducationLevelDescription { get; set; }


        //ForWhom
        public string ForWhomCode { get; set; }
        public string ForWhomTitle { get; set; }
        public string ForWhomDescription { get; set; }

        public string Labels { get; set; }

        //Precondition
        public string PreconditionCode { get; set; }
        public string PreconditionTitle { get; set; }
        public string PreconditionDescription { get; set; }


        //PriceTier
        public string PriceTierCode { get; set; }
        public string PriceTierTitle { get; set; }
        public string PriceTierDescription { get; set; }

        public string SubTitle { get; set; }
        public string Trailer { get; set; }
        public string WelcomeMessage { get; set; }


        //TrainingContent
        public TimeSpan? TrainingContentTime { get; set; }
        public int? TrainingContentPageCount { get; set; }
        public decimal TrainingContentCompletedRate { get; set; }
        public string? TrainingContentFilePath { get; set; }
        public bool? TrainingContentMandatory { get; set; }
        public long? TrainingContentOrderId { get; set; }
        public bool? TrainingContentAllowSeeking { get; set; }
        public int? TrainingContentPartCount { get; set; }
        public int? TrainingContentMinReadTimeThreshold { get; set; }

        //TrainingType
        public string TrainingTypeCode { get; set; }
        public string TrainingTypeTitle { get; set; }
        public string TrainingTypeDescription { get; set; }

    }

}
