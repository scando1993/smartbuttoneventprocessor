using System;
using System.Collections.Generic;
using System.Text;

namespace RecieveEPHClient
{
    public class SmartButton
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; } = "";
        public int IdClient { get; set; }
    }
}
