using System;
using NMEA_Parser.Sentences;
using NMEA_Parser.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMEA_Parser_Tests
{
    /// <summary>
    /// Summary description for RecMinSpecificGnssDataTest
    /// </summary>
    [TestClass]
    public class RecMinSpecificGnssDataTest
    {
        const double POSITION_THRESHOLD = 1;
        const double MAG_VAR_THRESHOLD = 0.1;

        [TestMethod]
        public void InvalidRMCTest()
        {
            string sentence = "$GPRMC,000548.2,V,4304.32642,N,08924.64855,W,,,220899,001.8,W,N*18";
            RecMinSpecificGnssData rmcMsg = new RecMinSpecificGnssData();
            Assert.IsTrue(rmcMsg.Parse(sentence));
            Assert.IsFalse(rmcMsg.ModeIndicator.Valid);
            Assert.AreEqual(rmcMsg.ModeIndicator.Indicator, PositionModeIndicator.INVALID);
            Assert.IsTrue(rmcMsg.NavigationReceiverWarning);
            Assert.IsTrue(Math.Abs(rmcMsg.Position.Latitude - 43.0432462) < POSITION_THRESHOLD);
            Assert.IsTrue(Math.Abs(rmcMsg.Position.Longitude - 89.2464855) < POSITION_THRESHOLD);
            Assert.AreEqual(rmcMsg.Position.LatitudeDirection, Direction.NORTH);
            Assert.AreEqual(rmcMsg.Position.LongitudeDirection, Direction.WEST);
            Assert.AreEqual(rmcMsg.TimeCollected, new DateTime(1999, 08, 22, 00, 05, 48, 200));
            Assert.IsTrue((rmcMsg.MagneticVariation > 0) && (Math.Abs(rmcMsg.MagneticVariation - 1.8) < MAG_VAR_THRESHOLD));
        }
    }
}
