using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using HtmlAgilityPack;
using PageMapper;
using RITCHARD_Common;
using RITCHARD_Data;

namespace RITCHARD_Processing
{
    public abstract class WordLookup
    {
        public const int FAILED = -3;
        public const int COULD_NOT_PROCESS = -2;
        public const int EMPTY = -1;
        public const int READY = 0;
        public const int SEARCHING = 1;
        public const int PROCESSING = 2;
        public const int DONE = 3;

        protected DateTime TimeCreated;
        protected IEnumerable<HtmlNode> Elements;
        protected TimeSpan Timeout;

        public HtmlWeb Browser;
        public HtmlDocument Document;

        public string[] StatusText;
        public string Query;
        public string ResponseString;
        public int Status;

        protected PageMap CurrentMap;
        protected string Url;

        public WordLookup()
        {
            TimeCreated = DateTime.Now;
            Timeout = new TimeSpan(0, 1, 0);

            StatusText = new string[4];
            StatusText[0] = Strings.WordLookupReady;
            StatusText[1] = Strings.WordLookupSearching;
            StatusText[2] = Strings.WordLookupProcessing;
            StatusText[3] = Strings.WordLookupFinished;
            Status = 0;
        }

        public virtual void Browse(string query)
        {
            Query = query;
            Browser = new HtmlWeb();
            Document = Browser.Load(Url + Query);
            Status = PROCESSING;
        }

        public void CheckTimeout()
        {
            if (DateTime.Now - TimeCreated > Timeout)
            {
                Status = FAILED;
            }
        }

        public virtual string GetStatus()
        {
            if (Status >= 0)
            {
                return StatusText[Status];
            }
            else
            {
                return "Error";
            }
        }
    }
}
