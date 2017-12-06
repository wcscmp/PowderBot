using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class UserModelTest
    {
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(-3, true)]
        [DataRow(3, true)]
        [DataRow(-24, true)]
        [DataRow(24, true)]
        [DataRow(-4, true)]
        [DataRow(4, false)]
        [DataRow(-10, false)]
        [DataRow(10, false)]
        public void CanBeNotifiedTest(int gmt, bool expected)
        {
            var user = new UserModel("42")
            {
                Gmt = gmt,
                NotifyAfter = 10,
                NotifyBefore = 18
            };
            var now = new DateTimeOffset(2017, 10, 10, 14, 0, 0, new TimeSpan());
            Assert.AreEqual(expected, user.CanBeNotified(now));
        }
    }
}
