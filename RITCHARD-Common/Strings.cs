using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Common
{
    public static class Strings
    {
        /* PageMaps */
        public static string PageMapDictionaryCheckString = "DictionaryCheckString"; // Can be used to check capitalisation
        public static string PageMapDictionaryDescription = "PageMapDictionaryDescription";
        public static string PageMapDictionaryLikelySpelling = "PageMapDictionaryLikelySpelling";
        public static string PageMapDictionaryPartsOfSpeech = "DictionaryPartsOfSpeech"; // Includes pluralisation data
        public static string PageMapDictionarySpellingSuggestions = "DictionarySpellingSuggestions";
        public static string PageMapDictionarySingularFromPlural = "DictionarySingularFromPlural";

        public static string PageMapDictionaryPartOfSpeechStopCharacters = ",(";
        public static string PageMapDictionaryPartOfSpeechIgnoreList = "Idioms,Verb phrases";

        public static string PageMapThesaurusAntonym = "ThesaurusAntonym";
        public static string PageMapThesaurusSynonym = "ThesaurusSynonym";
        

        /* RITCHARD-Processing */

        public static string WordLookupReady = "Ready";
        public static string WordLookupSearching = "Searching";
        public static string WordLookupProcessing = "Processing";
        public static string WordLookupFinished = "Done";

        public static string DictionaryURL = "http://dictionary.reference.com/browse/";
        public static string DictionarySearching = "Searching dictionary...";
        public static string DictionaryWordFound = "Word found. Checking...";

        public static string ThesaurusURL = "http://thesaurus.com/browse/";
        public static string ThesaurusSearching = "Searching thesaurus...";
        public static string ThesaurusWordFound = "Word found. Checking...";

        public static string ConjugatorURL = "http://conjugator.reverso.net/default.aspx?lang=en&language=en&verb=";
        public static string ConjugatorSearching = "Searching conjugator...";
        public static string ConjugatorVerbFound = "Word found. Checking...";
    }
}
