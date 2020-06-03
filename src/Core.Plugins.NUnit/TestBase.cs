using Core.Plugins.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Data;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core.Plugins.NUnit
{
    public class TestBase
    {
        public TestBase()
        {
            TestUsername = "UnitTest";
        }

        protected virtual void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            Console.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            Console.WriteLine(d.ToPrintFriendly());
        }

        protected virtual void WriteLine(object o)
        {
            Console.WriteLine(JsonSerializer.Serialize(o));
        }

        protected virtual void WriteLineWithOptions(object o, JsonSerializerOptions options)
        {
            Console.WriteLine(JsonSerializer.Serialize(o, options));
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
