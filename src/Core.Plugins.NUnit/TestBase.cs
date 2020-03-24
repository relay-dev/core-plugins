using Newtonsoft.Json;
using System;

namespace Core.Plugins.NUnit
{
    public class TestBase
    {
        protected void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        protected void WriteLine(object o)
        {
            Console.WriteLine(JsonConvert.SerializeObject(o));
        }

        protected string TestUsername = "UnitTest";
    }
}
