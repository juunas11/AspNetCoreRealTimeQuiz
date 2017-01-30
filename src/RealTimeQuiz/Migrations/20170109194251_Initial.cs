using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RealTimeQuiz.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MultipleCorrectAnswers = table.Column<bool>(nullable: false),
                    QuizId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    TimeLimitSeconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Correct = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CurrentQuestionId = table.Column<Guid>(nullable: true),
                    EndedAt = table.Column<DateTimeOffset>(nullable: true),
                    QuestionTimeEndsAt = table.Column<DateTimeOffset>(nullable: true),
                    QuizId = table.Column<Guid>(nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizSessions_Questions_CurrentQuestionId",
                        column: x => x.CurrentQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizSessions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_QuizSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "QuizSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SelectedAnswer",
                columns: table => new
                {
                    ChoiceId = table.Column<Guid>(nullable: false),
                    AnswerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedAnswer", x => new { x.ChoiceId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_SelectedAnswer_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedAnswer_Choices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SessionId",
                table: "Answers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizSessions_CurrentQuestionId",
                table: "QuizSessions",
                column: "CurrentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizSessions_QuizId",
                table: "QuizSessions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswer_AnswerId",
                table: "SelectedAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswer_ChoiceId",
                table: "SelectedAnswer",
                column: "ChoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedAnswer");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "QuizSessions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
