using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NMEA_Parser
{
    public abstract class Sentence
    {
        const int MAX_SENTENCE_LENGTH = 83; // 80 + $ + <CR><LF>
        const int MIN_SENTENCE_LENGTH = 8; // $ttsss<CR><LF>
        protected const char VALID_CHAR = 'A';
        protected const char INVALID_CHAR = 'V';

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
            if (sentence[0] != '$' || (strict && sentence.Substring(sentence.Length - 2) != "\r\n"))
                return false;

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

            // if the sentence is malformed and causes parsing to fail, just return false
            try
            {
                return ParsePayload(sentence.Substring(headerMatch.Length + 1, dataLength).Split(new char[] { ',' }));
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

        public string GetQuerySentence(string requester, string listener)
        {
            if (requester.Length != 2 || listener.Length != 2)
                throw new ArgumentException("Invalid length for requester/listener identifiers. Use the TalkerIdentifier class!");

            return string.Format("${0}{1}Q,{2}\r\n", requester, listener, SentenceIdentifier);
        }

        protected abstract bool ParsePayload(string[] data);

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
            return true;
            //return ComputeChecksum(s) == 0;
        }
    }
}
