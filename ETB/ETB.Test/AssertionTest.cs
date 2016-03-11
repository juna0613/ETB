using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace ETB.Test
{
    [TestFixture]
    public class AssertionTest
    {
        [Test]
        public void TestSingleAssertionFails_ThrowsException()
        {
            Assert.Throws<MultiException>(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    a.Add(false);
                });
            });
        }

        [Test]
        public void TestSingleAssertionSuccess_DoNotThrowException()
        {
            Assert.DoesNotThrow(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    a.Add(true);
                });
            });
        }

        [Test]
        public void TestNoAssertion_DoNotThrowException()
        {
            Assert.DoesNotThrow(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    // no exception
                });
            });
        }

        [Test]
        public void TestMultiAssertionsSuccess_DoNotThrowException()
        {
            Assert.DoesNotThrow(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    a.Add(true);
                    a.Add(() => true);
                });
            });
        }

        [Test]
        public void TestAnyOfMultiAssertionsFails_ThrowsException()
        {
            Assert.Throws<MultiException>(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    a.Add(true);
                    a.Add(() => false);
                });
            });
        }

        [Test]
        public void TestAnyOfMultiAssertionsFails_ButNoThrowFlag_DoNoThrowException()
        {
            Assert.DoesNotThrow(() =>
            {
                AssertionHelper.DoAssert(a =>
                {
                    a.Add(true);
                    a.Add(() => false);
                }, false);
            });
        }
    }
}
