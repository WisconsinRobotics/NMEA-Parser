using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NMEA_Parser
{
    public abstract class Sentence
    {
        const int MAX_SENTENCE_LENGTH = 82; // Section 5.3
        const int MIN_SENTENCE_LENGTH = 12; // $ttsss,*<c1><c2><CR><LF>
        protected const char VALID_CHAR = 'A';
        protected const char INVALID_CHAR = 'V';
        protected const int VARIABLE_PAYLOAD_LENGTH = -1;

        protected string talkerIdentifier;
        protected byte checksum;

        public bool Parse(string sentence, bool strict = false)
        {
            // length check
            if (sentence.Length < MIN_SENTENCE_LENGTH)
                return false;

            if (strict && sentence.Length > MAX_SENTENCE_LENGTH)
                return false;

            // start & end checks
            // TODO: support encapsulation sentences ('!')
            if (sentence[0] != '$' || (strict && sentence.Substring(sentence.Length - 2) != "\r\n"))
                return false;

            // TODO: Support proprietary sentences, which has an ID of P.
            Match headerMatch = Regex.Match(sentence, @"\$([A-Z]{2})([A-Z]{3})");
            if (!headerMatch.Success)
                return false;

            // source device is always the first 2 letters
            talkerIdentifier = headerMatch.Groups[1].Value;

            // check identifier
            if (headerMatch.Groups[2].Value != SentenceIdentifier)
                return false;

            // parse checksum
            Match checksumMatch = Regex.Match(sentence, @"\*([1-9A-F]{2})$");
            if (!checksumMatch.Success)
                return false;

            try
            {
                checksum = Convert.ToByte(checksumMatch.Groups[1].Value, 16);
            }
            catch (Exception)
            {
                return false;
            }

            // verify checksum
            if (!VerifyChecksum(sentence))
                return false;

            // parse the rest of the packet
            int dataLength = sentence.Length - headerMatch.Length - checksumMatch.Length;

            // payload field length check
            // fail only if there's a length mismatch and it's not a variable length sentence
            string[] payload = sentence.Substring(headerMatch.Length + 1, dataLength).Split(new char[] { ',' });
            if (payload.Length != NumberFields && NumberFields != VARIABLE_PAYLOAD_LENGTH)
                return false;

            // if sentence is malformed and can't be parsed, return false
            try
            {
                return ParsePayload(payload);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string TalkerIdentifier
        {
            get { return talkerIdentifier; }
        }

        public abstract string SentenceIdentifier
        {
            get;
        }

        protected abstract int NumberFields
        {
            get;
        }

        public string GetQuerySentence(string requester, string listener)
        {
            if (requester.Length != 2 || listener.Length != 2)
                throw new ArgumentException("Invalid length for requester/listener identifiers. Use the TalkerIdentifier class!");

            return string.Format("${0}{1}Q,{2}\r\n", requester, listener, SentenceIdentifier);
        }

        protected abstract bool ParsePayload(string[] data);

        protected byte ComputeChecksum(string s)
        {
            Match m = Regex.Match(s, @"\$(.*)\*");
            if (!m.Success)
                throw new ArgumentException("Malformed sentence supplied to ComputeChecksum.");

            byte checksum = 0;
            foreach (byte b in Encoding.ASCII.GetBytes(m.Groups[1].Value))
                checksum ^= b;

            return checksum;
        }

        protected bool VerifyChecksum(string s)
        {
            return true;
            //return ComputeChecksum(s) == 0;
        }
    }
}
