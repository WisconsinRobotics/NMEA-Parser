using System;

namespace NMEA_Parser.Sentences
{
    public class GeographicPositionLatLong : Sentence
    {

        public override string SentenceIdentifier
        {
            get { return "GLL"; }
        }

        protected override bool ParsePayload(string[] data)
        {
            throw new NotImplementedException();
        }
    }
}
