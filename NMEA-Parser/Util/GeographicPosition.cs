using System.Collections.Generic;

namespace NMEA_Parser.Util
{
    public enum Direction
    {
        NORTH, WEST, SOUTH, EAST
    }

    public class GeographicPosition
    {
        public static Dictionary<string, Direction> DirectionMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.North, Direction.NORTH },
            { CharacterSymbol.South, Direction.SOUTH },
            { CharacterSymbol.West, Direction.WEST },
            { CharacterSymbol.East, Direction.EAST }
        };

        public static Dictionary<string, Direction> LatitudeMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.North, Direction.NORTH },
            { CharacterSymbol.South, Direction.SOUTH },
        };

        public static Dictionary<string, Direction> LongitudeMap = new Dictionary<string, Direction>
        {
            { CharacterSymbol.West, Direction.WEST },
            { CharacterSymbol.East, Direction.EAST }
        };

        public double Latitude;
        public Direction LatitudeDirection;
        public bool LatitudeValid;

        public double Longitude;
        public Direction LongitudeDirection;
        public bool LongitudeValid;

    }
}
