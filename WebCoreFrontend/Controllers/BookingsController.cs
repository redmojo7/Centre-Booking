using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebCoreFrontend.Models;

namespace WebCoreFrontend.Controllers
{
    public class BookingsController : Controller
    {
        string URL = "http://localhost:64519/";

        private List<Centre> centres;
        List<Booking> bookings;

        public BookingsController()
        {
            var c1 = new Centre();
            c1.Id = 1;
            c1.Name = "perth";
            var c2 = new Centre();
            c2.Id = 2;
            c2.Name = "sydney";
            bookings = new List<Booking>();
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
            bookings.Add(b2);
            bookings.Add(b3);
            bookings.Add(b4);
            bookings.Add(b1);
            centres = new List<Centre>();
            centres.Add(c1);
            centres.Add(c2);
        }

        

        public IActionResult Index()
        {
            ViewBag.Title = "Bookings";
            /*
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/centres", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Centre> centres = JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content);
            */
            return View(centres);
        }


        [HttpGet]
        public IActionResult Users()
        {
            ViewBag.Title = "Bookings";
            /*
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/centres", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Centre> centres = JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content);
            */
            return View(centres);
        }

        [HttpGet]
        public IActionResult inCentre(int centreId)
        {
            if (centreId == 0)
            {
                return BadRequest("id cannot be null.");
            }
            /*
            RestClient client = new RestClient(URL);
            // send http request to delete
            RestRequest restRequest = new RestRequest("api/bookings/incentre/{centreId}", Method.Get);
            restRequest.AddUrlSegment("centreId", centreId);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                List<Booking> bookings = JsonConvert.DeserializeObject<List<Booking>>(restResponse.Content);
                return Ok(bookings);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Centre with id = {0} was not found", centreId));
            }
            else
            {
                Console.WriteLine(restResponse.Content);
                return StatusCode(500, $"Internal server error.");
            }
            */
            return Ok(bookings);
        }

        
        [HttpGet]
        public IActionResult NextAvaliableDate(int centreId)
        {
            if (centreId == 0)
            {
                return BadRequest("id cannot be null.");
            }
            /*
            RestClient client = new RestClient(URL);
            // send http request to delete
            RestRequest restRequest = new RestRequest("api/Bookings/nextAvaliableDate/{centreId}", Method.Get);
            restRequest.AddUrlSegment("centreId", centreId);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                DateTime avaliableDate = JsonConvert.DeserializeObject<DateTime>(restResponse.Content);
                return Ok(avaliableDate);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Centre with id = {0} was not found", centreId));
            }
            else
            {
                Console.WriteLine(restResponse.Content);
                return StatusCode(500, $"Internal server error.");
            }*/
            
            return Ok(DateTime.Now);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Booking booking)
        {
            if (booking == null || booking.EndDate == null || booking.CentreId == null
                || booking.StartDate == null || booking.UserName == null)
            {
                return BadRequest("StartDate/EndDate/CentreId/UserName is null");
            }
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/bookings", Method.Post);
            restRequest.AddBody(booking);
            RestResponse restResponse = restClient.Execute(restRequest);

            if (restResponse.IsSuccessful)
            {
                return Ok();
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return Conflict(string.Format("The the time periods({0} to {1}) overlaps", booking.StartDate, booking.EndDate));
            }
            return Ok();
        }
    }
}
