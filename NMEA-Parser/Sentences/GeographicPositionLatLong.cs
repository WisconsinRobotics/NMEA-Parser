using System;
using System.Globalization;
using NMEA_Parser.Util;

namespace NMEA_Parser.Sentences
{
    public class GeographicPositionLatLong : Sentence
    {
        GeographicPosition position;
        DateTime timeDataCollected;

        public GeographicPositionLatLong()
        {
            position = new GeographicPosition();
        }

        public override string SentenceIdentifier
        {
            get { return "GLL"; }
        }

        protected override int NumberFields
        {
            get { return 6; }
        }

        public GeographicPosition Position
        {
            get { return position; }
        }

        protected override bool ParsePayload(string[] data)
        {
            // parse latitude value
            if (!position.SetLatitude(data[0], data[1]))
                return false;

            // Technically: A => valid, V => invalid.
            // It's probably okay to treat as invalid even if "malformed" and this field 
            // is some random character
            position.LatitudeValid = data[2] == CharacterSymbol.DataValid;

            // parse longitude value
            if (!position.SetLongitude(data[3], data[4]))
                return false;

            position.LongitudeValid = data[5] == CharacterSymbol.DataValid;

            // parse date the data was collected
            return DateTime.TryParseExact(data[7], "hhmmss.f", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeDataCollected);
        }
    }
}
