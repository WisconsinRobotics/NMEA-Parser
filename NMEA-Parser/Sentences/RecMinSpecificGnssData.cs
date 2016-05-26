using System;
using System.Globalization;
using NMEA_Parser.Util;

namespace NMEA_Parser.Sentences
{
    public class RecMinSpecificGnssData : Sentence
    {
        DateTime timeDataCollected;
        float groundSpeed;
        float courseOverGround;
        float magneticVariation;

        readonly string[] DATE_FORMATS = {
            "ddMMyyhhmmss.f", "hhmmss.f"
        };

        public RecMinSpecificGnssData()
        {
            NavigationReceiverWarning = false;
            Position = new GeographicPosition();
            groundSpeed = 0;
            courseOverGround = 0;
            magneticVariation = 0;
        }

        public override string SentenceIdentifier
        {
            get { return "RMC"; }
        }

        protected override int NumberFields
        {
            get { return 12; }
        }

        #region Sentence Data
        public DateTime TimeCollected
        {
            get { return timeDataCollected; }
        }

        public bool NavigationReceiverWarning
        {
            get; private set;
        }

        public GeographicPosition Position
        {
            get; private set;
        }

        public float GroundSpeed
        {
            get { return groundSpeed; }
        }

        public float CourseOverGround
        {
            get { return courseOverGround; }
        }

        // < 0 => Easterly variation, subtract from True course
        // > 0 => Westerly variation, add to True course
        public float MagneticVariation
        {
            get { return magneticVariation; }
        }

        public PositioningSystemIndicator ModeIndicator
        {
            get; private set;
        }
        #endregion

        protected override bool ParsePayload(string[] data)
        {
            // data[8] -> mmddyy, data[0] -> time
            DateTime.TryParseExact(data[8] + data[0], DATE_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeDataCollected);
            NavigationReceiverWarning = data[1] == CharacterSymbol.WarningFlagSet;

            // latitude
            if (!double.TryParse(data[2], out Position.Latitude))
                return false;

            if (!GeographicPosition.LatitudeMap.ContainsKey(data[3]))
                return false;

            Position.LatitudeDirection = GeographicPosition.LatitudeMap[data[3]];
            Position.LatitudeValid = true;

            // longitude
            if (!double.TryParse(data[4], out Position.Longitude))
                return false;

            if (!GeographicPosition.LongitudeMap.ContainsKey(data[5]))
                return false;

            Position.LongitudeDirection = GeographicPosition.LongitudeMap[data[5]];
            Position.LongitudeValid= true;

            float.TryParse(data[6], out groundSpeed);
            float.TryParse(data[7], out courseOverGround);
            float.TryParse(data[9], out magneticVariation);

            if (data[10] == CharacterSymbol.East)
                magneticVariation = -magneticVariation;

            // mode indicator cannot be a null field
            if (data[11] == string.Empty)
                return false;

            ModeIndicator = new PositioningSystemIndicator(data[1], data[11]);

            return true;
        }
    }
}
