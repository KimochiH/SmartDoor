using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SmartDoorAPI.Models;

namespace SmartDoorAPI.Controllers
{
    [RoutePrefix("api/door")]
    public class DoorPasswordController : ApiController
    {

        //protected ApplicationDbContext db = new ApplicationDbContext();

        public static List<DoorPassword> _doors = new List<DoorPassword>()
        {

        };
 

        public DoorPasswordController()
        {
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<DoorPassword> GetDoors()
        {
            return _doors;
        }

        [Route("{id}")]
        [HttpGet]
        public DoorPassword GetDoorId(string id)
        {
            DoorPassword door = _doors.Find(p => p.DoorId == id);

            if (door == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            else
                return door;
        }

        [Route("up")]
        [HttpPost]
        public HttpResponseMessage RegisterDoor(DoorPassword p)
        {
            if (p == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _doors.Add(p);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [Route("code/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCode(string id, DoorPassword door)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != door.DoorId)
            {
                return BadRequest();
            }

            var tempDoor = _doors.Find(p => p.DoorId == id);

            tempDoor.Code = door.Code;
            tempDoor.LastModified = DateTime.Now;

            return Ok(door);
        }

    }

}
