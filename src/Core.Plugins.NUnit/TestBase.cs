using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        protected JObject ToJObject(object o)
        {
            if (o == null)
            {
                throw new ArgumentException("o cannot be null", "o");
            }

            return JsonConvert.DeserializeObject<JObject>(o.ToString());
        }

        protected IPropertyBag CurrentTestProperties => TestExecutionContext.CurrentContext.CurrentTest.Properties;

        protected string TestUsername;
    }
}
