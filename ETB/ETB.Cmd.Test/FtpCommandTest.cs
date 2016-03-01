using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace ETB.Cmd.Test
{
    [TestFixture]
    public class FtpCommandTest
    {
        private const string FtpPath = @"D:\ftp\root2";
        private const string FtpSite = "localhost";
        private const string FtpUser = "ftpuser";
        private const string FtpPass = "juna0613";

        
        [Test]
        public void TestFtp()
        {
            var ftpCommand = new FtpPutCommand(FtpSite, FtpUser, FtpPass, FtpMode.ASCII, @"d:\dojo.js", @"d:\dojo (1).js");
            var stat = ftpCommand.DoCommand();
            Assert.That(stat.Status, Is.EqualTo(0), stat.Error);
            Console.WriteLine("---output---");
            Console.WriteLine(stat.Output);
            Console.WriteLine("---Error---");
            Console.WriteLine(stat.Error);
        }

        [Test]
        public void TestFtpError()
        {
            var ftpCommand = new FtpPutCommand(FtpSite, FtpUser, FtpPass, FtpMode.ASCII, @"d:\dojo.js", @"d:\dojo (2).js");
            var stat = ftpCommand.DoCommand();
            Assert.That(stat.Status, Is.EqualTo(-1), stat.Error);
            Console.WriteLine("---output---");
            Console.WriteLine(stat.Output);
            Console.WriteLine("---Error---");
            Console.WriteLine(stat.Error);
        }
    }
}
