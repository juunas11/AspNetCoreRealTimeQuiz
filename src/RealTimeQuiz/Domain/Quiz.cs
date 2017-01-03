using System.Collections.Generic;

namespace RealTimeQuiz.Domain
{
    public class Quiz
    {
        public ICollection<Question> Questions { get; set; }
    }
}
