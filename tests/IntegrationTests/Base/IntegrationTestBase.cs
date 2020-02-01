using IntegrationTests.Common;
using System;
using System.IO;
using System.Xml.Serialization;

namespace IntegrationTests.Base
{
    public class IntegrationTestBase<T> : IntegrationTestBase where T : new()
    {
        public T CUT => new T();
    }

    public class IntegrationTestBase
    {
        private readonly object _lockObject = new object();

        public IntegrationTestBase()
        {
            if (!TestFramework.IsInitialized)
            {
                lock (_lockObject)
                {
                    if (!TestFramework.IsInitialized)
                    {
                        new IntegrationTestBootstrapper().Boostrap();

                        TestFramework.IsInitialized = true;
                    }
                }
            }
        }

        protected virtual void Output(string s)
        {
            Console.WriteLine(s);
        }

        protected virtual void Output(object o)
        {
            Console.Write(SerializeToString(o));
        }

        protected static string SerializeToString(object obj, string startEachLineWith = "")
        {
            try
            {
                var serializer = new XmlSerializer(obj.GetType());

                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj);

                    return writer.ToString().Replace("\n", "\n" + startEachLineWith);
                }
            }
            catch (Exception e)
            {
                return String.Format("<SerializeToString() failed>{0}Exception: {1}", Environment.NewLine, e.Message);
            }
        }
    }
}
