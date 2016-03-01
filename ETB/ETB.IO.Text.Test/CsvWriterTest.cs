using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
namespace ETB.IO.Text.Test
{
    public class MyClass
    {
        public DateTime Hoge { get; set; }
        private int Foo { get; set; }
        public double Data { get; set; }
        public bool Is { get; set; }
        public string Muu { get; set; }
    }
    [TestFixture]
    public class CsvWriterTest
    {
        private string _filepath = TempFileHelper.CreateTempFileName();
        [SetUp]
        public void Setup()
        {
            if(File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [TearDown]
        public void Teardown()
        {
            if(File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [Test]
        public void TestCreateCsvFile()
        {
            var data = new[]
            {
                new MyClass { Data = 0.1, Hoge = DateTime.Today, Is = false, Muu = @"it, was.
                OK" },
                new MyClass { Data = 0.1, Hoge = DateTime.Today, Is = true, Muu = null },
            };
            IEnumerable<MyClass> rdata = null;
            using (var writer = new System.IO.StreamWriter(_filepath))
            {
                var csv = new ETB.IO.Text.Csv.CSVWriter<MyClass>(writer);
                csv.Save(data);
                writer.Close();
                writer.Dispose();
            }
            FileAssert.Exists(_filepath);
            using (var reader = new System.IO.StreamReader(_filepath))
            {
                var rcsv = new Csv.CSVLoader<MyClass>(reader);
                rdata = rcsv.Load();
            }
            Assert.That(rdata.Count, Is.EqualTo(2));
            var d1 = rdata.ElementAt(0);
            var d2 = data[0];
            Assert.That(d1.Data, Is.EqualTo(d2.Data));
            Assert.That(d1.Hoge, Is.EqualTo(d2.Hoge));
            Assert.That(d1.Is, Is.EqualTo(d2.Is));
            Assert.That(d1.Muu, Is.EqualTo(d2.Muu));
        }
    }
}
