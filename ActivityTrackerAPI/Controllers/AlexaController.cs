using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerAPI.Data;
using TrackerAPI.Models;

namespace ActivityTrackerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlexaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Alexa
        [HttpGet]
        public string Get()
        {
            Activity LatestActivity = _context.Activity.OrderBy(x => x.StartTime).Last();
            double Time = (LatestActivity.Distance / LatestActivity.Pace) * 60;
            int NumberOfMinutes = (int)Math.Floor(Time);
            int NumberOfSeconds = (int)Math.Round((Time - Math.Truncate(Time))*60);
            return $"Chris' last run was on {LatestActivity.StartTime.ToString("MMMM dd, yyyy")}, was {LatestActivity.Distance.ToString("#.##")} miles and took {NumberOfMinutes} minutes and {NumberOfSeconds} seconds";
        }

    }
}
