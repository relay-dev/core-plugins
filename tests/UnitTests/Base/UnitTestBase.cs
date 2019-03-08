using System;
using System.IO;
using System.Xml.Serialization;

namespace UnitTests.Base
{
    public class UnitTestBase<T> : UnitTestBase where T : new()
    {
        public T CUT => new T();
    }

    public class UnitTestBase
    {
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
