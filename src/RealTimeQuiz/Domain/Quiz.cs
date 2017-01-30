using System;
using System.Collections.Generic;

namespace RealTimeQuiz.Domain
{
    public class Quiz
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public string TenantId { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
