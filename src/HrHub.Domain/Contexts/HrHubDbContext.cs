using HrHub.Abstraction.Data.Context;
using HrHub.Domain.Entities.SqlDbEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Contexts
{
    public partial class HrHubDbContext : DbContextBase
    {
        public HrHubDbContext()
        {

        }

        public HrHubDbContext([NotNull] DbContextOptions<HrHubDbContext> options) : base(options)
        {
        }
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
        public virtual DbSet<ExamAnswer> ExamAnswers { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<ExamTopic> ExamTopics { get; set; }
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Cart>()
                                      .HasOne(cart => cart.AddCartUser)
                                      .WithMany(user => user.AddCartUser)
                                      .HasForeignKey(cart => cart.AddCartUserId)
                                      .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                                    .HasOne(cart => cart.ConfirmUser)
                                    .WithMany(user => user.ConfirmCarts)
                                    .HasForeignKey(cart => cart.ConfirmUserId)
                                    .OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
