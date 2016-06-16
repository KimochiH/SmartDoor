using System;
using Newtonsoft.Json;

namespace SmartDoorAPI.Models
{
    public class DataTransfer
    {
        [JsonIgnore]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string DoorId { get; set; }
        public string Result { get; set; }
    }
}