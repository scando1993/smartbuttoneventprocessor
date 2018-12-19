using System;
using System.Collections.Generic;
using System.Text;

namespace RecieveEPHClient
{
    public class SmartButton
    {
        public string DeviceId { get; set; }
        public int Status { get; set; } //1 Pressed
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Data { get; set; }

    }
}
