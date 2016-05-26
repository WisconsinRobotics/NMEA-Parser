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
            if (!double.TryParse(data[0], out position.Latitude))
                return false;

            // parse latitude direction
            if (!GeographicPosition.LatitudeMap.ContainsKey(data[1]))
                return false;

            position.Latitude /= 100;
            position.LatitudeDirection = GeographicPosition.LatitudeMap[data[1]];

            // Technically: A => valid, V => invalid.
            // It's probably okay to treat as invalid even if "malformed" and this field 
            // is some random character
            position.LatitudeValid = data[2] == CharacterSymbol.DataValid;

            // parse longitude value
            if (!double.TryParse(data[3], out position.Longitude))
                return false;

            // parse latitude direction
            if (!GeographicPosition.LongitudeMap.ContainsKey(data[4]))
                return false;

            position.Longitude /= 100;
            position.LongitudeDirection = GeographicPosition.LongitudeMap[data[4]];
            position.LongitudeValid = data[5] == CharacterSymbol.DataValid;

            // parse date the data was collected
            return DateTime.TryParseExact(data[7], "hhmmss.f", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeDataCollected);
        }
    }
}
