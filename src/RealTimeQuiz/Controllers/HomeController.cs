using Microsoft.AspNetCore.Mvc;

namespace RealTimeQuiz.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
