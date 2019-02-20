using System;
using System.Collections.Generic;

namespace RecieveEPHClient.Model
{
    public partial class UserDevices
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DeviceId { get; set; }
        public string Pac { get; set; }
        public string DeviceType { get; set; }
        public string Webhook { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string Alias { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDtm { get; set; }
        public string CreatedBy { get; set; }
        public string ModicatedBy { get; set; }
        public DateTime? ModificationDtm { get; set; }
        public string TwitterAccount { get; set; }
    }
}
