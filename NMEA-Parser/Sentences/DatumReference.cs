using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Parser.Sentences
{
    public class DatumReference : Sentence
    {
        public override string SentenceIdentifier
        {
            get { return "DTM"; }
        }

        protected override int NumberFields
        {
            get { return 8; }
        }

        protected override bool ParsePayload(string[] data)
        {
            // TODO
            return false;
        }
    }
}
