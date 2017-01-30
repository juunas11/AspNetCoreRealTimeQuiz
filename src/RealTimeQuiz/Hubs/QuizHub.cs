using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealTimeQuiz.Models;

namespace RealTimeQuiz.Hubs
{
    public class QuizHub : Hub<IQuizHub>
    {
        // Clients must be able to send their answers
        // Server must be able to tell clients answering a certain quiz
        //  the next question
        // Server must be able to tell clients answering a certain quiz
        //  that the quiz is over, and pass the user's results

        public Task Subscribe(Guid quizId)
        {
            Groups.Add(Context.ConnectionId, quizId.ToString());
            return Task.CompletedTask;
        }
    }

    public interface IQuizHub
    {
        Task ChangeQuestion(QuestionModel question);
        Task QuizOver();
    }
}
