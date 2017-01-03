namespace RealTimeQuiz.Domain
{
    public class SelectedAnswer
    {
        public string UserId { get; set; }
        public PossibleAnswer Answer { get; set; }
        public Quiz Quiz { get; set; }
    }
}
