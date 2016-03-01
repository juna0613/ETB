using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace ETB.Test
{
    public class MyClass
    {
        public string Data { get; set; }
        public int Val { get; set; }
        private DateTime PriDate { get; set; }
    }
    [TestFixture]
    public class TypeHelperTest
    {
        [Test]
        public void TestObj()
        {
            var dic = new Dictionary<string, object>();
            dic["hoge"] = 1;

        }
        
        [Test]
        public void TestTypeHelper()
        {
            var mapper = TypeHelper.CreateObjMapFunc<MyClass>();
            var data = new MyClass { Data = "soo", Val = 32 };
            var ret = mapper(data);

            Assert.That(ret.Keys.Count, Is.EqualTo(2));
            Assert.That(ret["Data"], Is.EqualTo("soo"));
            Assert.That(ret["Val"], Is.EqualTo(32));

            var mapper2 = TypeHelper.CreateMapFunc<MyClass>();
            var ret2 = mapper2(data);

            Assert.That(ret2.Keys.Count, Is.EqualTo(2));
            Assert.That(ret2["Data"], Is.EqualTo("soo"));
            Assert.That(ret2["Val"], Is.EqualTo("32"));

            var data3 = new MyClass { Data = "muuu", Val = -2 };
            var ret3 = mapper2(data3);
            Assert.That(ret3.Keys.Count, Is.EqualTo(2));
            Assert.That(ret3["Data"], Is.EqualTo("muuu"));
            Assert.That(ret3["Val"], Is.EqualTo("-2"));

        }

    }
}
