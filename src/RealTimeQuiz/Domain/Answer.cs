using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeQuiz.Domain
{
    public class Answer
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(Session))]
        public Guid SessionId { get; set; }
        public QuizSession Session { get; set; }

        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<SelectedAnswer> SelectedChoices { get; set; }
        //public ICollection<Choice> SelectedChoices { get; set; }
    }
}
