using System;
using System.IO;
using NMEA_Parser.Sentences;
using NMEA_Parser.Util;

namespace NMEA_Parser_Sample
{
    class Program
    {
        static readonly string BANNER_DELIMITER = "=================================";

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage: {0} <file>", AppDomain.CurrentDomain.FriendlyName);
                return;
            }

            using (StreamReader sr = new StreamReader(args[0]))
            {
                string sentence;

                while ((sentence = sr.ReadLine()) != null)
                {
                    RecMinSpecificGnssData rmc = new RecMinSpecificGnssData();
                    bool success = rmc.Parse(sentence);

                    Console.WriteLine(BANNER_DELIMITER);
                    Console.WriteLine("Sentence: {0}", sentence);
                    Console.WriteLine("Parse Success: {0}", success);
                    if (!success)
                        continue;

                    Console.WriteLine("Time: {0}", rmc.TimeCollected);
                    Console.WriteLine("Valid: {0}", rmc.ModeIndicator.Valid);
                    Console.WriteLine("Mode: {0}", rmc.ModeIndicator.Indicator);
                    Console.WriteLine("Navigation Receiver Warning: {0}", rmc.NavigationReceiverWarning);
                    Console.WriteLine("Latitude: {0} | Direction: {1}", rmc.Position.Latitude, rmc.Position.LatitudeDirection);
                    Console.WriteLine("Longitude: {0} | Direction: {1}", rmc.Position.Longitude, rmc.Position.LongitudeDirection);
                    Console.WriteLine("Magnetic Variation: {0}", rmc.MagneticVariation);
                }
            }
        }
    }
}
