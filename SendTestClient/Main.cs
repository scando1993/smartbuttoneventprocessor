using System;
using System.Collections.Generic;
using System.Text;

namespace SendTestClient
{
    public class MainClient
    {
        public static List<string> DummyData = new List<string> {
            "4101C4","41C132","41C20E","41C777","41CCFF","41FF5A","421157","421345","422816","43B23C","43B397","43B42A","43C74E","43C7A0","43C890","43C896","43C959","43CAA8","43CB1B","43CB1F","43CB75","43CD7C","43CD95","43CEF8","43CF18","43CF77","43CF81","43D049","45A01C","45A248"
        };

        public static void Main(string[] args)
        {
            new SendClient().MainAsync(DummyData).GetAwaiter().GetResult();
        }
    }
}
