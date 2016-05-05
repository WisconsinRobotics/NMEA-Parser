using System.Collections.Generic;

namespace NMEA_Parser.Sentences
{
    public struct SatelliteData
    {
        public int satellitesInView;
        public int satelliteNumber;
        public int azimuth; // degrees to true
        public int elevation; // degrees
        public int signalNoiseRatio; // dB
    }

    public class SatellitesInView : Sentence
    {
        int totalNumberMessages;
        int messageNumber;
        int numSatellites;
        SatelliteData[] satellites;

        const int TOTAL_NUM_MSG_INDEX = 0;
        const int CUR_MSG_NUM_INDEX = 1;
        const int NUM_SAT_INDEX = 2;

        public override string SentenceIdentifier
        {
            get { return "GSV"; }
        }

        public IReadOnlyCollection<SatelliteData> SatelliteData
        {
            get { return satellites; }
        }

        protected override bool ParsePayload(string[] data)
        {
            // attempt to parse total # of messages
            if (!int.TryParse(data[TOTAL_NUM_MSG_INDEX], out totalNumberMessages))
                return false;

            // attempt to parse current message index
            if (!int.TryParse(data[CUR_MSG_NUM_INDEX], out messageNumber))
                return false;

            // attempt to parse # of satellites in view
            if (!int.TryParse(data[NUM_SAT_INDEX], out numSatellites))
                return false;

            // initialize satellite data
            satellites = new SatelliteData[numSatellites];

            int satDataOffset = 3;
            int satDataSize = 4;
            for (int i = 0; i < numSatellites; i++)
            {
                SatelliteData satData = new SatelliteData();

                // if we can't parse the satellite number, then this data is useless
                // however, as long as satellite number and at least one other piece of data is OK,
                // then it can be used (and spec compliant). However, it is up to user to be to 
                // discern if data is valid or not.
                if (!int.TryParse(data[(satDataSize * i) + satDataOffset], out satData.satelliteNumber))
                    continue;

                int.TryParse(data[(satDataSize * i) + satDataOffset + 1], out satData.elevation);
                int.TryParse(data[(satDataSize * i) + satDataOffset + 2], out satData.azimuth);
                int.TryParse(data[(satDataSize * i) + satDataOffset + 3], out satData.signalNoiseRatio);

                satellites[i] = satData;
            }

            return true;
        }
    }
}
