using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Domain;
using RealTimeQuiz.Models;

namespace RealTimeQuiz.Controllers
{
    [Route("api/quiz")]
    public class QuizController : Controller
    {
        private readonly QuizContext _db;

        public QuizController(QuizContext db)
        {
            _db = db;
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetQuizStatus(Guid id)
        {
            string activeQuizId = HttpContext.Session.GetString("activeQuizId");
            if (activeQuizId == null)
            {
                HttpContext.Session.SetString("activeQuizId", id.ToString());
            }

            Quiz quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            if (quiz.State == "Stopped")
            {
                return Ok(new QuizStatusModel
                {
                    Status = "Stopped",
                    CurrentQuestion = null
                });
            }

            QuizSession session = await _db.QuizSessions
                                            .Include(s => s.CurrentQuestion)
                                                .ThenInclude(q => q.Choices)
                                            .Where(s => s.EndedAt == null && s.QuizId == id)
                                            .SingleAsync();
            Question question = session.CurrentQuestion;
            int? timeLeftSeconds = null;
            if (session.QuestionTimeEndsAt.HasValue)
            {
                TimeSpan remainingTime = session.QuestionTimeEndsAt.Value.Subtract(DateTimeOffset.UtcNow);
                timeLeftSeconds = (int)remainingTime.TotalSeconds;
                if (timeLeftSeconds < 0)
                {
                    timeLeftSeconds = 0;
                }
            }
            return Ok(new QuizStatusModel()
            {
                Status = quiz.State,
                CurrentQuestion = new QuestionModel()
                {
                    Id = question.Id,
                    MultipleCorrectAnswers = question.MultipleCorrectAnswers,
                    Text = question.Text,
                    TimeLeftSeconds = timeLeftSeconds,
                    Choices = question.Choices.Select(c => new ChoiceModel()
                    {
                        Id = c.Id,
                        Text = c.Text
                    }).ToList()
                }
            });
        }

        [HttpPost("{id}/submitAnswer")]
        public async Task<IActionResult> SubmitAnswer(Guid id, [FromBody] IList<Guid> chosenAnswers)
        {
            //Find the current question in the given quiz
            QuizSession session = await _db.QuizSessions
                                            .Where(s => s.EndedAt == null && s.QuizId == id)
                                            .SingleAsync();

            if (session.CurrentQuestionId.HasValue == false)
            {
                return BadRequest();
            }

            string userId = HttpContext.Session.Id;
            //Assign a new answer for this session id on that question
            var answer = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = session.CurrentQuestionId.Value,
                SessionId = session.Id,
                UserId = userId,
                SelectedChoices = chosenAnswers.Select(choiceId => new SelectedAnswer()
                {
                    ChoiceId = choiceId
                }).ToList()
            };
            _db.Answers.Add(answer);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}/results")]
        public IActionResult GetResults(Guid id)
        {
            //Find the latest session with user id == session id
            //Compile results and return

            return Ok();
        }
    }
}
