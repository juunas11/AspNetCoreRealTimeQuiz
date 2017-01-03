using Microsoft.AspNetCore.Mvc;

namespace RealTimeQuiz.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Index() => View();
    }
}
