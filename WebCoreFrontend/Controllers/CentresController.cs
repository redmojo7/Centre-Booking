﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Web.Http.Description;
using System.Web.Http.Results;
using WebCoreFrontend.Models;

namespace WebCoreFrontend.Controllers
{
    public class CentresController : Controller
    {
        string URL = "http://localhost:64519/";

        // GET: api/Centres
        public IActionResult Index()
        {
            ViewBag.Title = "Centre";
            List<Centre> centres = new List<Centre>();
            var c1 = new Centre();
            c1.Id = 1;
            c1.Name = "perth";
            var c2 = new Centre();
            c2.Id = 2;
            c2.Name = "sydney";
            centres.Add(c1);
            centres.Add(c2);
            return View(centres);
        }


        // POST: api/Centres
        [HttpPost]
        public IActionResult CreateCentre([FromBody] Centre centre)
        {
            if (centre == null)
            {
                return BadRequest("centre cannot be null.");
            }

            if (centre.Id is not null)
            {
                return BadRequest();
            }
            RestClient client = new RestClient(URL);
            // send http request to create a NEW Centre
            RestRequest restRequest = new RestRequest("api/centres/", Method.Post);
            restRequest.AddBody(centre);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                return Ok();
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return Conflict(string.Format("Centre with name = {0} already exist.", centre.Name));
            }
            else 
            {
                Console.WriteLine(restResponse.Content);
                return StatusCode(500, $"Internal server error.");
            }
        }

        // DELETE: api/Centres/5
        [HttpDelete]
        public IActionResult DeleteCentre(int id)
        {
            if (id == 0)
            {
                return BadRequest("id cannot be null.");
            }
            RestClient client = new RestClient(URL);
            // send http request to delete
            RestRequest restRequest = new RestRequest("api/centres/{id}", Method.Delete);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Delete Successful!");
                return Ok();
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Centre with id = {0} was not found", id));
            }
            else
            {
                Console.WriteLine(restResponse.Content);
                return StatusCode(500, $"Internal server error.");
            }
        }

    }
}