using Microsoft.VisualStudio.TestTools.UnitTesting;

using NMEA_Parser.Util;
using NMEA_Parser.Sentences;

namespace NMEA_Parser_Tests
{
    [TestClass]
    public class SatellitesInViewTest
    {
        [TestMethod]
        public void OneSatelliteTest()
        {
            string msg = @"$GPGSV,1,1,1,1,12,32,21*AB";
            SatellitesInView s = new SatellitesInView();
            Assert.IsTrue(s.Parse(msg));
            Assert.AreEqual(s.TalkerIdentifier, TalkerIdentifier.GP);
            Assert.AreEqual(s.SentenceIdentifier, "GSV");
        }
    }
}
