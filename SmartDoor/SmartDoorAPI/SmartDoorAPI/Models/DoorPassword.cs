using System;

namespace SmartDoorAPI.Models
{
    public class DoorPassword
    {
        public DateTime LastModified { get; set; }
        public string Code { get; set; }
        public string DoorId { get; set; }
    }
}