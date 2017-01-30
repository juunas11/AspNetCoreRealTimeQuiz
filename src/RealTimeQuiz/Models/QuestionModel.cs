using System;
using System.Collections.Generic;

namespace RealTimeQuiz.Models
{
    public class QuestionModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public ICollection<ChoiceModel> Choices { get; set; }
        public bool MultipleCorrectAnswers { get; set; }
        public int? TimeLeftSeconds { get; set; }
    }
}