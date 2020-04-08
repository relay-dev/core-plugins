using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;

namespace Core.Plugins.NUnit
{
    public class TestBase
    {
        public TestBase()
        {
            TestUsername = "UnitTest";
        }

        protected void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        protected void WriteLine(object o)
        {
            Console.WriteLine(JsonConvert.SerializeObject(o));
        }

        protected dynamic ToObject(object o)
        {
            if (o == null)
                throw new ArgumentException("o cannot be null", "o");

            return JsonConvert.DeserializeObject(o.ToString());
        }

        protected IPropertyBag CurrentTestProperties
        {
            get
            {
                return TestExecutionContext.CurrentContext.CurrentTest.Properties;
            }
        }

        protected string TestUsername;
    }
}
