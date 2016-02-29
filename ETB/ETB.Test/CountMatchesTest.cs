using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;
namespace ETB.Test
{
    
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void TestCountMatches()
        {
            var str = @"220 Microsoft FTP Service

331 Password required for ftpuser.

230 User logged in.
prompt
ascii
200 Type set to A.
put ""d:\dojo.js""
200 EPRT command successful.
125 Data connection already open; Transfer starting.
226 Transfer complete.
0.007452000.00put ""d:\dojo (1).js""
200 EPRT command successful.
125 Data connection already open; Transfer starting.
226 Transfer complete.
0.007452000.00bye
221 Goodbye.";
            Assert.That(str.CountMatches("226 Transfer complete"), Is.EqualTo(2));
        }

        [Test]
        public void TestCountMatches2()
        {
            var str = @"hoge foo hoge bar";
            Assert.That(str.CountMatches("hoge"), Is.EqualTo(2));
            Assert.That(str.CountMatches("^hoge"), Is.EqualTo(1));

        }
    }
}
