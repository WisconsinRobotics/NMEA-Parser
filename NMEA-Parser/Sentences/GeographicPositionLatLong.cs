using System;

namespace NMEA_Parser.Sentences
{
    public class GeographicPositionLatLong : Sentence
    {

        public override string Identifier
        {
            get { return "GLL"; }
        }

        protected override void ParsePayload(string[] data)
        {
            throw new NotImplementedException();
        }
    }
}
