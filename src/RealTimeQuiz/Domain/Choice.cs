using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeQuiz.Domain
{
    public class Choice
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public bool Correct { get; set; }

        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<SelectedAnswer> AnswersThatSelectedThis { get; set; }
        //public ICollection<Answer> AnswersThatSelectedThis { get; set; }
    }
}
