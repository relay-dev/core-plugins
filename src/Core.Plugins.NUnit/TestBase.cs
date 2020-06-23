using Core.Plugins.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core.Plugins.NUnit
{
    public class TestBase
    {
        protected string TestUsername;
        protected IPropertyBag CurrentTestProperties => TestExecutionContext.CurrentContext.CurrentTest.Properties;

        public TestBase()
        {
            TestUsername = "UnitTest";
        }

        protected virtual void WriteLine(string s)
        {
            Debug.WriteLine(s);
            Console.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            Debug.WriteLine(s, args);
            Console.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            WriteLine(d.ToPrintFriendly());
        }

        protected virtual void WriteLine(object o)
        {
            WriteLine(JsonSerializer.Serialize(o));
        }

        protected virtual void WriteLineWithOptions(object o, JsonSerializerOptions options)
        {
            WriteLine(JsonSerializer.Serialize(o, options));
        }

        protected JObject ToJObject(object o)
        {
            if (o == null)
            {
                throw new ArgumentException("o cannot be null", "o");
            }

            return JsonConvert.DeserializeObject<JObject>(o.ToString());
        }
    }
}
