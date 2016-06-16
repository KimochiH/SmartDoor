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
    [RoutePrefix("api/data")]
    public class DataTransferController : ApiController
    {
        //protected ApplicationDbContext db = new ApplicationDbContext();

        private static List<DataTransfer> _datas = new List<DataTransfer>();


        public DataTransferController()
        {
        }

        [Route("")]
        [HttpGet]
        public string GetDatas()
        {
            return "1";
        }

        [Route("{id}")]
        [HttpGet]
        public DataTransfer GetDataTransfer(string id)
        {
            DataTransfer pro = _datas.Find(p => p.Id == id);

            if (pro == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            else
                return pro;
        }

        [Route("up")]
        [HttpPost]
        public async Task<IHttpActionResult> PostData(DataTransfer p)
        {
            if (p == null)
                return BadRequest();

            p.Result = "false";
            var pos = _datas.Count();
            p.Id = pos++.ToString();
            var door = DoorPasswordController._doors.Find(o => o.DoorId == p.DoorId);
            if (door.Code == p.Code) p.Result = "success";
            p.Date = DateTime.Now;

            return Ok(p);
        }



    }
}
