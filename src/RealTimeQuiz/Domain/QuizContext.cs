using Microsoft.EntityFrameworkCore;

namespace RealTimeQuiz.Domain
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options)
            :base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizSession> QuizSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SelectedAnswer>()
                .HasKey(sa => new {sa.ChoiceId, sa.AnswerId});

            modelBuilder.Entity<SelectedAnswer>()
                .HasOne(sa => sa.Choice)
                .WithMany(c => c.AnswersThatSelectedThis)
                .HasForeignKey(sa => sa.ChoiceId);

            modelBuilder.Entity<SelectedAnswer>()
                .HasOne(sa => sa.Answer)
                .WithMany(a => a.SelectedChoices)
                .HasForeignKey(sa => sa.AnswerId);
        }
    }
}