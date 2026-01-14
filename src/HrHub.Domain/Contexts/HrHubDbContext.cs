using HrHub.Abstraction.Data.Context;
using HrHub.Core.Interceptors;
using HrHub.Domain.Entities.SqlDbEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Contexts
{
    public partial class HrHubDbContext : DbContextBase
    {
        private readonly AuditInterceptor auditInterceptor;
        public HrHubDbContext()
        {

        }

        public HrHubDbContext([NotNull] DbContextOptions<HrHubDbContext> options, AuditInterceptor _auditInterceptor) : base(options)
        {
            this.auditInterceptor = _auditInterceptor;
        }
        #region DbSets
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<CartStatus> CartStatus { get; set; }
        public virtual DbSet<CertificateTemplate> CertificateTemplates { get; set; }
        public virtual DbSet<CertificateType> CertificateTypes { get; set; }
        public virtual DbSet<CommentVote> CommentVotes { get; set; }
        public virtual DbSet<ContentComment> ContentComments { get; set; }
        public virtual DbSet<ContentNote> ContentNotes { get; set; }
        public virtual DbSet<ContentType> ContentTypes { get; set; }
        public virtual DbSet<CurrAcc> CurrAccs { get; set; }
        public virtual DbSet<CurrAccTraining> CurrAccTrainings { get; set; }
        public virtual DbSet<CurrAccTrainingStatus> CurrAccTrainingStatuses { get; set; }
        public virtual DbSet<CurrAccTrainingUser> CurrAccTrainingUsers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamVersion> ExamVersions { get; set; }

        public virtual DbSet<ExamVersionStatus> ExamVersionStatuses { get; set; }
        public virtual DbSet<QuestionOption> ExamOptions { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<ExamTopic> ExamTopics { get; set; }
        public virtual DbSet<ExamResult> ExamResults { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<InstructorType> InstructorTypes { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<TimeUnit> TimeUnits { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<TrainingAmount> TrainingAmounts { get; set; }
        public virtual DbSet<TrainingAnnouncement> TrainingAnnouncements { get; set; }
        public virtual DbSet<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; }
        public virtual DbSet<TrainingCategory> TrainingCategories { get; set; }
        public virtual DbSet<TrainingContent> TrainingContents { get; set; }
        public virtual DbSet<TrainingLevel> TrainingLevels { get; set; }
        public virtual DbSet<TrainingSection> TrainingSections { get; set; }
        public virtual DbSet<TrainingStatus> TrainingStatuses { get; set; }
        public virtual DbSet<TrainingType> TrainingTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<UserCertificate> UserCertificates { get; set; }
        public virtual DbSet<UserContentsViewLog> UserContentsViewLogs { get; set; }
        public virtual DbSet<UserContentsViewLogDetail> UserContentsViewLogDetails { get; set; }
        public virtual DbSet<UserExam> UserExams { get; set; }
        public virtual DbSet<WhatYouWillLearn> WhatYouWillLearns { get; set; }
        public virtual DbSet<ContentLibrary> ContentLibraries { get; set; }
        public virtual DbSet<Precondition> Preconditions { get; set; }
        public virtual DbSet<ForWhom> ForWhoms { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<PriceTier> PriceTiers { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<TrainingLanguage> TrainingLanguages { get; set; }
        public virtual DbSet<TrainingProcessRequest> TrainingProcessRequests { get; set; }
        public virtual DbSet<SysMenu> SysMenus { get; set; }
        public virtual DbSet<SysMenuRole> SysMenuRoles { get; set; }
        public virtual DbSet<SysMenuPolicy> SysMenuPolicies { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=188.132.128.38;Port=5432;Database=HrHubDb;User Id=hrhub_user;Password=SoOm2024*;");
            optionsBuilder.AddInterceptors(auditInterceptor);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Cart>().HasOne(cart => cart.AddCartUser)
                                      .WithMany(user => user.AddCartUser)
                                      .HasForeignKey(cart => cart.AddCartUserId)
                                      .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>().HasOne(cart => cart.ConfirmUser)
                                    .WithMany(user => user.ConfirmCarts)
                                    .HasForeignKey(cart => cart.ConfirmUserId)
                                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().ToTable("AspNetUsers")
                .HasOne(u => u.Instructor).WithOne(i => i.User)
                .HasForeignKey<Instructor>(i => i.UserId);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
