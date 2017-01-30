using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RealTimeQuiz.Domain;

namespace RealTimeQuiz.Migrations
{
    [DbContext(typeof(QuizContext))]
    partial class QuizContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RealTimeQuiz.Domain.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("QuestionId");

                    b.Property<Guid>("SessionId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SessionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Choice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Correct");

                    b.Property<Guid>("QuestionId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("MultipleCorrectAnswers");

                    b.Property<Guid>("QuizId");

                    b.Property<string>("Text");

                    b.Property<int>("TimeLimitSeconds");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("State");

                    b.Property<string>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.QuizSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CurrentQuestionId");

                    b.Property<DateTimeOffset?>("EndedAt");

                    b.Property<DateTimeOffset?>("QuestionTimeEndsAt");

                    b.Property<Guid>("QuizId");

                    b.Property<DateTimeOffset>("StartedAt");

                    b.HasKey("Id");

                    b.HasIndex("CurrentQuestionId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizSessions");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.SelectedAnswer", b =>
                {
                    b.Property<Guid>("ChoiceId");

                    b.Property<Guid>("AnswerId");

                    b.HasKey("ChoiceId", "AnswerId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("ChoiceId");

                    b.ToTable("SelectedAnswer");
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Answer", b =>
                {
                    b.HasOne("RealTimeQuiz.Domain.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RealTimeQuiz.Domain.QuizSession", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Choice", b =>
                {
                    b.HasOne("RealTimeQuiz.Domain.Question", "Question")
                        .WithMany("Choices")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.Question", b =>
                {
                    b.HasOne("RealTimeQuiz.Domain.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.QuizSession", b =>
                {
                    b.HasOne("RealTimeQuiz.Domain.Question", "CurrentQuestion")
                        .WithMany()
                        .HasForeignKey("CurrentQuestionId");

                    b.HasOne("RealTimeQuiz.Domain.Quiz", "Quiz")
                        .WithMany()
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RealTimeQuiz.Domain.SelectedAnswer", b =>
                {
                    b.HasOne("RealTimeQuiz.Domain.Answer", "Answer")
                        .WithMany("SelectedChoices")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RealTimeQuiz.Domain.Choice", "Choice")
                        .WithMany("AnswersThatSelectedThis")
                        .HasForeignKey("ChoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
