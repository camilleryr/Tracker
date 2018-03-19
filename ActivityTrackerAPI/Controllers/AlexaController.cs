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

        private string FormattedResponse(Activity activity, string qualifier)
        {
            double Time = (activity.Distance / activity.Pace) * 60;
            int NumberOfMinutes = (int)Math.Floor(Time);
            int NumberOfSeconds = (int)Math.Round((Time - Math.Truncate(Time)) * 60);
            return $"Chris' {qualifier} run was on {activity.StartTime.ToString("MMMM dd, yyyy")}, was {activity.Distance.ToString("#.##")} miles and took {NumberOfMinutes} minutes and {NumberOfSeconds} seconds";
        }

        // GET: api/Alexa/{Id}
        [HttpGet("{Id}")]
        public string Get([FromRoute] string Id)
        {
            Activity SelectedActivity;
            
            switch(Id.ToLower())
            {
                case "latest":
                    SelectedActivity = _context.Activity.OrderBy(x => x.StartTime).Last();
                    break;
                case "fastest":
                    SelectedActivity = _context.Activity.OrderBy(x => x.Pace).Last();
                    break;
                case "longest":
                    SelectedActivity = _context.Activity.OrderBy(x => x.Pace).Last();
                    break;
                default:
                    return "That was not a valid endpoint";
            }

            return FormattedResponse(SelectedActivity, Id);
        }

    }
}
