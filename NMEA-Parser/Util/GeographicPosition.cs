using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NMEA_Parser.Util
{
    public enum Direction
    {
        NORTH, WEST, SOUTH, EAST
    }

    public class GeographicPosition
    {
        public static readonly Dictionary<string, Direction> DirectionMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.North, Direction.NORTH },
            { CharacterSymbol.South, Direction.SOUTH },
            { CharacterSymbol.West, Direction.WEST },
            { CharacterSymbol.East, Direction.EAST }
        };

        public static readonly Dictionary<string, Direction> LatitudeMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.North, Direction.NORTH },
            { CharacterSymbol.South, Direction.SOUTH },
        };

        public static readonly Dictionary<string, Direction> LongitudeMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.West, Direction.WEST },
            { CharacterSymbol.East, Direction.EAST }
        };

        static readonly Regex LatRegex = new Regex(@"(\d{2})(\d{2}.\d+)", RegexOptions.Compiled);
        static readonly Regex LongRegex = new Regex(@"(\d{3})(\d{2}.\d+)", RegexOptions.Compiled);

        public static bool SetLatitudeFromString(string latStr, string dirString, out double latitude)
        {
            Direction dir;

            // initialize out parameter
            latitude = 0;

            if (!LatitudeMap.ContainsKey(dirString))
                return false;

            Match m = LatRegex.Match(latStr);
            if (!m.Success)
                return false;

            latitude = int.Parse(m.Groups[1].Value) + double.Parse(m.Groups[2].Value) / 60.0;

            dir = LatitudeMap[dirString];
            if (dir == Direction.SOUTH)
                latitude *= -1;

            return true;
        }

        public static bool SetLongitudeFromString(string longStr, string dirString, out double longitude)
        {
            Direction dir;

            // initialize out parameter
            longitude = 0;

            if (!LongitudeMap.ContainsKey(dirString))
                return false;

            Match m = LongRegex.Match(longStr);
            if (!m.Success)
                return false;

            longitude = int.Parse(m.Groups[1].Value) + double.Parse(m.Groups[2].Value) / 60.0;

            dir = LongitudeMap[dirString];
            if (dir == Direction.WEST)
                longitude *= -1;

            return true;
        }

        public double Latitude
        {
            get; set;
        }

        public bool LatitudeValid
        {
            get; set;
        }

        public double Longitude
        {
            get; set;
        }

        public bool LongitudeValid
        {
            get; set;
        }

        public bool SetLatitude(string latStr, string dir)
        {
            double parsedLat;
            bool success;

            if ((success = SetLatitudeFromString(latStr, dir, out parsedLat)))
                Latitude = parsedLat;

            return success;
        }

        public bool SetLongitude(string longStr, string dir)
        {
            double parsedLong;
            bool success;

            if ((success = SetLongitudeFromString(longStr, dir, out parsedLong)))
                Longitude = parsedLong;

            return success;
        }

    }
}
