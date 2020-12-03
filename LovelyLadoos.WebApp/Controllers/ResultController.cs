using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LovelyLadoos.WebApp.Models;
using Newtonsoft.Json;

namespace LovelyLadoos.WebApp.Controllers
{
    public class ResultController : Controller
    {
        public IActionResult Result(string jsonBody)
        {
            var json = jsonBody;
            var objJson = JsonConvert.DeserializeObject<ResultModel>(json); //here we will map the Json to C# class
            //here we will return this model to view
            return View(objJson);  //you have to pass model to view
        }
    }
}
