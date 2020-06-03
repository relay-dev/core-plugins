using Core.Plugins.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Text.Json;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core.Plugins.xUnit
{
    public class TestBase
    {
        private readonly ITestOutputHelper _output;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
            TestUsername = "UnitTest";
        }

        protected virtual void WriteLine(string s)
        {
            _output.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            _output.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            _output.WriteLine(d.ToPrintFriendly());
        }

        protected virtual void WriteLine(object o)
        {
            _output.WriteLine(JsonSerializer.Serialize(o));
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

        protected string TestUsername;
    }
}
