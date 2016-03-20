using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using FsCheck;
namespace ETB.Fnc.Test
{
    [TestFixture]
    public class MonadTest
    {
        [Test]
        public void TestJust()
        {
            var just = Maybe.Just(1);
            int v;
            var couldtake = just.Take(out v);

            Assert.That(couldtake, Is.True);
            Assert.That(v, Is.EqualTo(1));
        }

        [Test]
        public void TestNothing()
        {
            var nothing = Maybe.Nothing<string>();
            string str;
            var couldtake = nothing.Take(out str);
            Assert.That(couldtake, Is.False);
            Assert.That(string.IsNullOrEmpty(str));
        }

        [Test]
        public void TestReturn()
        {
            var maybe = Maybe.Return(0.1d);
            double d;
            Assert.That(maybe.Take(out d));
            Assert.That(d, Is.EqualTo(0.1d));

            var nothing = Maybe.Return(default(DateTime));
            DateTime dt;
            Assert.That(!nothing.Take(out dt));
            Assert.That(dt, Is.EqualTo(default(DateTime)));
        }

        [Test]
        public void TestBind()
        {
            var nothing = Maybe.Return(default(int));
            var ret = nothing.Bind(x => (x + 1).ToMaybe());
            Assert.That(ret.IsNothing());

            var today = DateTime.Today;
            var just = Maybe.Return(today);
            var ret2 = just.Bind(d => d.AddYears(1).ToMaybe());
            DateTime dt;
            Assert.That(ret2.IsJust());
            Assert.That(ret2.Take(out dt));
            Assert.That(dt, Is.EqualTo(today.AddYears(1)));
        }

        [Test]
        public void TestMap()
        {
            var just = Maybe.Return(0.1);
            var pi = 3.14;
            var data = just.Map(x => x * x * pi);
            Assert.That(data.IsJust());
            double val;
            Assert.That(data.Take(out val));
            Assert.That(val, Is.EqualTo(0.1 * 0.1 * pi));
        }

        [Test]
        public void TestMapException()
        {
            var just = Maybe.Just(-0.1);
            var data = just.Map(d => {
                if (d < 0) throw new ArgumentException("Negative value");
                return Math.Sqrt(d);
            });
            Assert.That(data.IsNothing());
            Assert.That(data.Error, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void TestRandomValues()
        {
            Prop.ForAll<double>(x =>
            {
                var just = Maybe.Just(x);
                var data = just.Map(d => {
                    if (d <= 0d || double.IsNaN(d) || double.IsInfinity(d)) throw new ArgumentException("Negative value");
                    return Math.Sqrt(d);
                });
                if((x <= 0d || double.IsNaN(x) || double.IsInfinity(x)))
                {
                    Assert.That(data.IsNothing());
                    Assert.That(data.Error, Is.TypeOf<ArgumentException>());
                }
                else
                {
                    double x2;
                    Assert.That(data.IsJust(), "But {0} is not just".Format2(x));
                    Assert.That(data.Take(out x2));
                    Assert.That(x2, Is.EqualTo(Math.Sqrt(x)));
                }
                
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void TestLinq()
        {
            var data = from unwrapped in Maybe.Just(1.0)
                       select unwrapped + 2.0;
            double val;
            Assert.That(data.Take(out val));
            Assert.That(val, Is.EqualTo(3.0));

            var data2 = from val1 in Maybe.Just(2.0)
                        from val2 in Maybe.Nothing<double>()
                        select val1 + val2;
            Assert.That(data2.IsNothing());
            Assert.That(!data2.Take(out val));

        }

        [Test]
        public void TestTuples()
        {
            var data = Maybe.Merge(Maybe.Just(DateTime.Now), Maybe.Nothing<DateTime>());
            DateTime v1, v2;
            Assert.That(!data.Take(out v1, out v2));

            var data2 = Maybe.Merge(Maybe.Just(DateTime.Now), Maybe.Return(DateTime.Today));
            Assert.That(data2.Take(out v1, out v2));

            var data3 = Maybe.Merge(Maybe.Just(1.0), Maybe.Just("hoge"));
            string hoge;
            double val;
            Assert.That(data3.Take(out val, out hoge));
            Assert.That(val, Is.EqualTo(1.0));
            Assert.That(hoge, Is.EqualTo("hoge"));

            data.Do(d =>
            {
                Assert.That(d.Item1, Is.Not.Null);
                Assert.That(d.Item2, Is.EqualTo(default(DateTime)));
            });

            data3.Do(d =>
            {
                Assert.That(d.Item1, Is.EqualTo(1.0));
                Assert.That(d.Item2, Is.EqualTo("hoge"));
            });

            data2.Do((d1, d2) =>
            {
                d1.Should().NotBe(default(DateTime));
                d2.Should().NotBe(default(DateTime));
            });
        }

        [Test]
        public void TestTakedata()
        {
            var nothing = Maybe.Nothing<DateTime>();
            Assert.That(nothing.FromJust(), Is.EqualTo(default(DateTime)));

            var just = Maybe.Just(0.0);
            just.FromJust().Should().Be(0.0);

            var now = DateTime.Now;
            nothing.GetValueOrDefault(now).Should().Be(now);
            just.GetValueOrDefault(Double.NaN).Should().Be(0.0);

            var edata = just.ToEnumerable();
            edata.ShouldBeEquivalentTo(new[] { 0.0 });

            var e2 = nothing.ToEnumerable();
            e2.Should().HaveCount(0);

        }
    }
}
