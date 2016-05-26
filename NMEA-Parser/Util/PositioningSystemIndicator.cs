using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Parser.Util
{
    public enum PositionModeIndicator
    {
        AUTONOMOUS, DIFFERENTIAL, DEAD_RECKONING,
        MANUAL_INPUT, SIMULATOR, INVALID 
    }

    public class PositioningSystemIndicator
    {
        static readonly Dictionary<string, PositionModeIndicator> pmiMap = new Dictionary<string, PositionModeIndicator>()
        {
            { CharacterSymbol.Auto, PositionModeIndicator.AUTONOMOUS },
            { "D", PositionModeIndicator.DIFFERENTIAL },
            { "E", PositionModeIndicator.DEAD_RECKONING },
            { CharacterSymbol.Manual, PositionModeIndicator.MANUAL_INPUT },
            { "S", PositionModeIndicator.SIMULATOR }
        };

        public PositioningSystemIndicator(string status, string modeIndicator)
        {
            Indicator = (pmiMap.ContainsKey(modeIndicator)) ? pmiMap[modeIndicator] : PositionModeIndicator.INVALID;

            if (Indicator != PositionModeIndicator.AUTONOMOUS && Indicator != PositionModeIndicator.DIFFERENTIAL)
                Valid = false;
            else
                Valid = status == CharacterSymbol.DataValid;
        }

        public bool Valid
        {
            get; private set;
        }

        public PositionModeIndicator Indicator
        {
            get; private set;
        }

    }
}
