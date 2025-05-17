using Microsoft.AspNetCore.Mvc;
using MeetingApp.Models;


namespace MeetingApp.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            int saat = DateTime.Now.Hour;
            ViewData["Selamlama"] = saat > 12 ? "İyi Günler" : "Günaydın";
            int UserCount = Repository.Users.Where(info => info.WillAttend == true).Count();

            var meetingInfo = new MeetingInfo()
            {
                Id = 1,
                Location = "İstanbul",
                Date = new DateTime(2024, 01, 01, 20, 0, 0),
                NumberOfPeople = UserCount
            };

            return View(meetingInfo);
        }
    }
}
