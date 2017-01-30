using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RealTimeQuiz.Controllers
{
    public class HomeController : Controller
    {
        //[Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("a", 2);
            string sessionId = HttpContext.Session.Id;
            ViewBag.SessionId = sessionId;
            return View();
        }
    }
}
