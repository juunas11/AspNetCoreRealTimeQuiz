using System.Collections.Generic;

namespace RealTimeQuiz.Domain
{
    public class Question
    {
        public string Text { get; set; }
        public int TimeToAnswerSeconds { get; set; }
        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }
    }
}
