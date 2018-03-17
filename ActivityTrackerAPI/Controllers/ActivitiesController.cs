using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ActivityTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrackerAPI.Data;
using TrackerAPI.Models;
using System.Spatial;
using Geolocation;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ActivityTrackerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Activities")]
    public class ActivitiesController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public IEnumerable<Activity> GetActivity()
        {
            return _context.Activity;
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity([FromRoute] int id, [FromBody] Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activities/Load
        [HttpPost("{id}")]
        public async Task<IActionResult> PostActivity([FromRoute] string Id)
        {
            if(Id != "load")
            {
                return NotFound();
            }

            var response = await client.GetAsync("https://trackmyrun-41804.firebaseio.com/.json");
            var responseString = await response.Content.ReadAsStringAsync();

            List<FirebaseGeoCordObject> FBGeoCordResponse = Utility.ConvertFirebaseResponse(responseString);


            FBGeoCordResponse.ForEach(entry =>
            {
                GeoLocation[] geoLocations = entry.value;

                double distance = Utility.GetDistance(geoLocations);

                Activity activity = new Activity
                {
                    Distance = distance,
                    Pace = Utility.GetPace(geoLocations, distance),
                    StartTime = new DateTime(1970, 01, 01).AddMilliseconds((long)geoLocations[0].timestamp),
                    FirebaseLocation = entry.key
                };


                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                _context.Activity.Add(activity);
            });

            
            await _context.SaveChangesAsync();

            //return NoContent();
            return Ok();
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<IActionResult> PostActivity([FromBody] GeoLocation[] geoLocations)
        {
            string output = JsonConvert.SerializeObject(geoLocations);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://trackmyrun-41804.firebaseio.com/.json", content);
            var responseByteArray = await response.Content.ReadAsByteArrayAsync();

            double distance = Utility.GetDistance(geoLocations);

            Activity activity = new Activity
            {
                Distance = distance,
                Pace = Utility.GetPace(geoLocations, distance),
                StartTime = new DateTime(1970, 01, 01).AddMilliseconds((long)geoLocations[0].timestamp),
                FirebaseLocation = Utility.GetFirebaseLocation(responseByteArray)
            };


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Activity.Add(activity);
            await _context.SaveChangesAsync();

            //return NoContent();
            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok(activity);
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}