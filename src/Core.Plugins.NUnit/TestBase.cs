using Newtonsoft.Json;
using System;

namespace Common.Testing
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
