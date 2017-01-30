using System;

namespace RealTimeQuiz.Domain
{
    public class SelectedAnswer
    {
        public Guid ChoiceId { get; set; }
        public Choice Choice { get; set; }

        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}