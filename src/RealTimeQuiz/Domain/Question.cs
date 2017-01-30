using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeQuiz.Domain
{
    public class Question
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public int TimeLimitSeconds { get; set; }

        public bool MultipleCorrectAnswers { get; set; }

        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<Choice> Choices { get; set; }
    }
}
