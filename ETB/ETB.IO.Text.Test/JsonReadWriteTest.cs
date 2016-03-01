using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
namespace ETB.IO.Text.Test
{
    [TestFixture]
    public class JsonReadWriteTest
    {
        private readonly string _filepath = TempFileHelper.CreateTempFileName();
        [SetUp]
        public void Setup()
        {
            if (File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [Test]
        public void TestJsonWriteRead()
        {
            TestHelper.Test(_filepath, w => new ETB.IO.Text.Json.JsonWriter<MyClass>(w), r => new ETB.IO.Text.Json.JsonLoader<MyClass>(r));

        }

    }
}
