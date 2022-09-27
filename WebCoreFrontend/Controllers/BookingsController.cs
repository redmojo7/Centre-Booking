using Microsoft.AspNetCore.Mvc;
using WebCoreFrontend.Models;

namespace WebCoreFrontend.Controllers
{
    public class BookingsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Bookings";

            var c1 = new Centre();
            c1.Id = 1;
            c1.Name = "perth";
            var c2 = new Centre();
            c2.Id = 2;
            c2.Name = "sydney";
            List<Booking> bookings = new List<Booking>();
            var b1 = new Booking();
            b1.StartDate = new DateTime(2022, 01, 01, 0, 0, 0);
            b1.EndDate = new DateTime(2022, 01, 10, 0, 0, 0);
            b1.Id = 1;
            b1.UserName = "peng1";
            b1.Centre = c1;
            b1.CentreId = 1;
            var b2 = new Booking();
            b2.StartDate = new DateTime(2022, 01, 10, 0, 0, 0);
            b2.EndDate = new DateTime(2022, 01, 20, 0, 0, 0);
            b2.Id = 2;
            b2.UserName = "peng2";
            b2.Centre = c1;
            b2.CentreId = 1;
            var b3 = new Booking();
            b3.StartDate = new DateTime(2022, 09, 30, 0, 0, 0);
            b3.EndDate = new DateTime(2022, 10, 10, 0, 0, 0);
            b3.Id = 3;
            b3.UserName = "peng3";
            b3.Centre = c2;
            b3.CentreId = 2;
            var b4 = new Booking();
            b4.StartDate = new DateTime(2022, 09, 10, 0, 0, 0);
            b4.EndDate = new DateTime(2022, 09, 28, 0, 0, 0);
            b4.Id = 4;
            b4.UserName = "peng4";
            b4.Centre = c2;
            b4.CentreId = 2;

            bookings.Add(b1);
            bookings.Add(b2);
            bookings.Add(b3);
            bookings.Add(b4);
            return View(bookings);
        }
    }
}
