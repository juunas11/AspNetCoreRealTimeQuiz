using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeQuiz.Domain
{
    public class QuizSession
    {
        public Guid Id { get; set; }

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? EndedAt { get; set; }

        public DateTimeOffset? QuestionTimeEndsAt { get; set; }

        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [ForeignKey(nameof(CurrentQuestion))]
        public Guid? CurrentQuestionId { get; set; }
        public Question CurrentQuestion { get; set; }
    }
}
