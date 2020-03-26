using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
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

        protected IPropertyBag CurrentTestProperties
        {
            get
            {
                return TestExecutionContext.CurrentContext.CurrentTest.Properties;
            }
        }

        protected string TestUsername = "UnitTest";
    }
}
