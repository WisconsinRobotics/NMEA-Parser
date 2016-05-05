using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NMEA_Parser
{
    public abstract class Sentence
    {
        const int MAX_SENTENCE_LENGTH = 83; // 80 + $ + <CR><LF>
        const int MIN_SENTENCE_LENGTH = 8; // $ttsss<CR><LF>
        const char VALID_CHAR = 'A';
        const char INVALID_CHAR = 'V';

        protected string source;
        protected int checksum;

        public bool Parse(string sentence, bool strict = false)
        {
            // length check
            if (sentence.Length < MIN_SENTENCE_LENGTH)
                return false;

            if (strict && sentence.Length > MAX_SENTENCE_LENGTH)
                return false;

            // start & end checks
            if (sentence[0] != '$' || (sentence.Substring(sentence.Length - 2) != "\r\n"))
                return false;

            Match m = Regex.Match(sentence, @"\$([A-Z]{2})([A-Z]{3})");
            if (!m.Success)
                return false;

            // source device is always the first 2 letters
            source = m.Groups[0].Value;

            // check identifier
            if (m.Groups[1].Value != Identifier)
                return false;

            // parse the rest of the packet
            ParsePayload(sentence.Substring(m.Length + 1).Split(new char[] { ',' }));

            return true;
        }

        public string Source
        {
            get { return source; }
        }

        public abstract string Identifier
        {
            get;
        }

        public string GetQuerySentence(string requester, string listener)
        {
            if (requester.Length != 2 || listener.Length != 2)
                throw new ArgumentException("Invalid length for requester/listener identifiers. Use the Identifier class!");

            return string.Format("${0}{1}Q,{2}\r\n", requester, listener, Identifier);
        }

        protected abstract void ParsePayload(string[] data);

        protected byte ComputeChecksum(string s)
        {
        // TODO: NOT CORRECT
        // Between $ and * 
            byte[] bytes = Encoding.ASCII.GetBytes(s);

            byte checksum = 0;
            foreach (byte b in bytes)
                checksum ^= b;

            return checksum;
        }

        protected bool VerifyChecksum(string s)
        {
            return ComputeChecksum(s) == 0;
        }
    }
}
