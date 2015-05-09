using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using PageMapper;
using RITCHARD_Common; 
using RITCHARD_Data;

namespace RITCHARD_Processing
{
    public class DictionaryEntry : WordLookup
    {
        public bool IsSpelledIncorrectly;

        public DictionaryEntry()
            : base()
        {
            Initialise();
        }

        public DictionaryEntry(string query) : base()
        {
            Initialise();
            Browse(query);
        }

        public void Initialise()
        {
            IsSpelledIncorrectly = false;

            Url = "http://dictionary.reference.com/browse/";
            StatusText[1] = Strings.DictionarySearching;
            StatusText[2] = Strings.DictionaryWordFound;
        }

        public bool IsSpellingCorrect()
        {
            CurrentMap = Mapper.RetrieveMapFromDatabase(Strings.PageMapDictionaryCheckString);
            List<string> textOutput = Mapper.GetRelevantTextFromDocumentUsingMap(Document, CurrentMap, "");
            return (Query == textOutput[0]);
        }

        public string GetLikelySpelling()
        {
            CurrentMap = Mapper.RetrieveMapFromDatabase(Strings.PageMapDictionaryLikelySpelling);
            List<string> textOutput = Mapper.GetRelevantTextFromDocumentUsingMap(Document, CurrentMap, "");
            return textOutput[0];
        }

        public List<string> GetPartsOfSpeech()
        {
            CurrentMap = Mapper.RetrieveMapFromDatabase(Strings.PageMapDictionaryPartsOfSpeech);
            List<string> textOutput = Mapper.GetRelevantTextFromDocumentUsingMap(Document, CurrentMap, Strings.PageMapDictionaryPartOfSpeechStopCharacters);
            return textOutput;
        }

        public List<string> GetSpellingSuggestions()
        {
            CurrentMap = Mapper.RetrieveMapFromDatabase(Strings.PageMapDictionarySpellingSuggestions);
            List<string> textOutput = Mapper.GetRelevantTextFromDocumentUsingMap(Document, CurrentMap, "");
            return textOutput;
        }
    }
}
