using System;
using System.Collections.Generic;
using System.Text;

namespace RecieveEPHClient
{
    class MainReciever
    {
        public static void Main(string[] args)
        {
            new EventProcessor().MainAsync().GetAwaiter().GetResult();
        }
    }
}
