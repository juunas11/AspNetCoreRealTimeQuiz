using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Domain;
using RealTimeQuiz.Hubs;
using RealTimeQuiz.Models;

namespace RealTimeQuiz.Controllers
{
    [Route("/api/admin/quiz")]
    public class QuizAdminController : Controller
    {
        private readonly IHubContext<QuizHub, IQuizHub> _hub;
        private readonly QuizContext _db;

        public QuizAdminController(IConnectionManager connectionManager,
            QuizContext db)
        {
            _hub = connectionManager.GetHubContext<QuizHub, IQuizHub>();
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(User.Claims.Select(c => $"{c.Type} : {c.Value}").ToList());
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartQuiz(Guid id)
        {
            //Check the quiz is one owned by the user's tenant
            Quiz quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            if (User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value != quiz.TenantId)
            {
                return NotFound();
            }

            //Change status to "Started"
            quiz.State = "Started";

            Question firstQuestion = quiz.Questions.OrderBy(q => q.Id).First();

            //Create the session, assign the first question
            var session = new QuizSession()
            {
                Id = Guid.NewGuid(),
                CurrentQuestionId = firstQuestion.Id,
                StartedAt = DateTimeOffset.UtcNow,
                QuizId = id
            };

            //Setup the ending time in the session and fire off a delayed message
            if (firstQuestion.TimeLimitSeconds > 0)
            {
                session.QuestionTimeEndsAt =
                    DateTimeOffset.UtcNow.AddSeconds(firstQuestion.TimeLimitSeconds);
            }

            _db.QuizSessions.Add(session);
            await _db.SaveChangesAsync();

            //Notify the group of clients subscribed to the quiz of the question
            await NotifyNextQuestionAsync(id, firstQuestion);

            return Ok();
        }

        private async Task NotifyNextQuestionAsync(Guid quizId, Question question)
        {
            await _hub.Clients.Group(quizId.ToString())
               .ChangeQuestion(new QuestionModel()
               {
                   Id = question.Id,
                   Text = question.Text,
                   MultipleCorrectAnswers = question.MultipleCorrectAnswers,
                   TimeLeftSeconds = question.TimeLimitSeconds,
                   Choices = question.Choices.Select(c => new ChoiceModel()
                   {
                       Id = c.Id,
                       Text = c.Text
                   }).ToList()
               });
        }

        [HttpPost("{id}/goToNext")]
        //[Authorize("goToNextQuestion")]
        public async Task<IActionResult> GoToNextQuestion(Guid id)
        {
            //Get the quiz, check it is running
            Quiz quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            if (User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value != quiz.TenantId)
            {
                return NotFound();
            }

            if (quiz.State != "Started")
            {
                return BadRequest();
            }

            //Get next question
            QuizSession session = await _db.QuizSessions
                .Where(s => s.EndedAt == null && s.QuizId == id && s.CurrentQuestionId != null)
                .SingleAsync();
            Question nextQuestion = quiz.Questions
                .OrderBy(q => q.Id)
                .FirstOrDefault(q => q.Id.CompareTo(session.CurrentQuestionId.Value) > 0);

            //If there is one
            if (nextQuestion != null)
            {
                //  Do same things as when quiz was started, update the session
                //  and signal the clients
                session.CurrentQuestionId = nextQuestion.Id;
                if (nextQuestion.TimeLimitSeconds > 0)
                {
                    session.QuestionTimeEndsAt = DateTimeOffset.UtcNow.AddSeconds(nextQuestion.TimeLimitSeconds);
                }
                await NotifyNextQuestionAsync(id, nextQuestion);
                await _db.SaveChangesAsync();
            }
            else
            {
                //  quiz is over
                //  update status to stopped, end the session
                quiz.State = "Stopped";
                session.EndedAt = DateTimeOffset.UtcNow;
                session.CurrentQuestionId = null;
                await _db.SaveChangesAsync();
                //  send a message to all clients
                await _hub.Clients.Group(id.ToString()).QuizOver();

                //  erase the group created for the quiz?
            }

            return Ok();
        }
    }
}