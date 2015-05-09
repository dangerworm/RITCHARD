using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using RITCHARD_Common;
using RITCHARD_Data;

namespace RITCHARD_Processing
{
    public class InputProcessor
    {
        private RitchardDataContext _db;
        private Queue queue;

        private OrderedDictionary odWords;
        private OrderedDictionary odVerbs;

        private List<string> words;
        private List<string> tenses;
        private List<string> tenseMatches;
        private List<string> consoleOutput;
        private string newInput;

        private Dictionary<string, string> specialApostropheWords = new Dictionary<string, string>()
        {
            {"cont'd", "continued" },
            {"e'er", "ever" },
            {"I'm", "I am" },
            {"ma'am", "madam" },
            {"o'er", "over" },
        };

        private char[] splitSymbols = "/|\\\"^*!?£$%<>().,_".ToCharArray();

        public InputProcessor()
        {
            _db = new RitchardDataContext();
            odWords = new OrderedDictionary();
            odVerbs = new OrderedDictionary();
        }

        public void ProcessInput(string input)
        {
            /*
             * Creates new queue.
             * Adds each new word to a list to be searched for, removing duplicates.
             */

            // Initialise queue to hold calls to dictionary, thesaurus and conjugator
            queue = new Queue();

            if (words == null)
            {
                words = GetWordsFromInput(input);
            }

            // Queue words we haven't seen before
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    // Check whether the word is in the definitions table
                    Definition def = RitchardDataHelper.GetDefinitionCaseSensitive(word); // Full def needed for 'else'

                    if (def == null)
                    {
                        // If not, see if we have previously seen a misspelling of this word
                        CorrectedSpelling cs = _db.CorrectedSpellings.SingleOrDefault(crs => crs.OriginalSpelling == word);
                        
                        Guid correctedDefGuid = Guid.Empty ;
                        if (cs != null)
                        {
                            correctedDefGuid = cs.ResultingDefinition;
                        }

                        // If not...
                        if (correctedDefGuid == Guid.Empty)
                        {
                            // ...check whether search has already been requested for that word
                            if (!odWords.Contains(word))
                            {
                                // If not, queue it
                                odWords.Add(word, Strings.WordLookupReady);
                                queue.AddDictionaryEntry(word);
                            }
                        }
                    }
                    // If it IS in the definitions table, see if we have part of speech and verb info.
                    else
                    {
                        if (!_db.PartsOfSpeeches.Any(pos => pos.DefinitionID == def.DefinitionID))
                        {
                            // If not, queue it
                            odWords.Add(word, Strings.WordLookupReady);
                            queue.AddDictionaryEntry(word);
                        }
                    }
                }
            }

            CorrectSpellings();
            GetPartsOfSpeech();

            //ParseSentences();
        }

        public List<string> GetWordsFromInput(string input)
        {
            /*
             * Removes extraneous characters (e.g. apostrophes) from around and within words
             * e.g. it's -> it is, doesn't -> does not, etc.
             * and stores words to a string, separated by spaces.
             */

            string originalInput = input.Trim();
            string replacement;

            queue = new Queue();
            words = new List<string>();

            List<int[]> windowMap = new List<int[]>();
            List<int[]> newInputMap = new List<int[]>();

            newInput = "";

            int windowStart = 0;
            int windowLength = -1;

            while (windowLength <= originalInput.Length - (windowLength + 1))
            {
                windowStart = windowLength + 1;

                int charIndex = originalInput.Substring(windowStart).IndexOfAny(splitSymbols);
                windowLength = charIndex >= 0 ? charIndex : originalInput.Length - windowStart;

                string window = originalInput.Substring(windowStart, windowLength);

                // Create a map of where window starts and finishes in current input
                newInputMap.Add(new int[] { windowStart, windowLength });

                // Split into 'lumps' - individual words (including 'odd' characters) from input - and process

                string[] lumps = window.Split(" ".ToCharArray());

                // Used for windowMap
                int lumpStart = 0;
                int lumpLength = -1;

                foreach (string lump in lumps)
                {
                    if (lump.Trim() == "")
                    {
                        continue;
                    }

                    lumpStart = lumpLength + 1;
                    lumpLength = lump.Length;

                    // Create a map of where words start and finish in current window
                    windowMap.Add(new int[] { window.Substring(lumpStart).IndexOf(lump), lumpLength });

                    string currentWord = replacement = lump;

                    if (!_db.Definitions.Any(d => d.String == currentWord))
                    {
                        queue.AddDictionaryEntry(currentWord);
                    }

                    // Count apostrophes
                    int numApostrophes = 0;

                    foreach (char c in currentWord)
                    {
                        if (c == '\'')
                        {
                            numApostrophes++;
                        }
                    }

                    // Process apostrophes, including converting contractions to their meanings

                    bool apostrophesSurrounding = false;

                    if (numApostrophes == 1)
                    {

                        if (specialApostropheWords.Keys.Contains(currentWord))
                        {
                            replacement = specialApostropheWords[currentWord];
                        }
                        else
                        {
                            // TODO: Special case of possession (e.g. "Drew's") not dealt with yet

                            // To cope with capitalisation, add extra to whatever comes before the apostrophe
                            if (currentWord.EndsWith("'s"))
                            {
                                replacement = currentWord.Split("'".ToCharArray())[0] + " is";
                            }

                            switch (currentWord.Substring(currentWord.Length - 3, 3))
                            {
                                case "'ll":
                                    replacement = currentWord.Split("'".ToCharArray())[0] + " will";
                                    break;
                                case "'re":
                                    replacement = currentWord.Split("'".ToCharArray())[0] + " are";
                                    break;
                                case "'ve":
                                    replacement = currentWord.Split("'".ToCharArray())[0] + " have";
                                    break;
                            }

                            if (currentWord.EndsWith("n't"))
                            {
                                if (currentWord == "can't")
                                {
                                    replacement = "cannot";
                                }
                                else
                                {
                                    replacement = currentWord.Substring(0, currentWord.IndexOf("n't")).Trim() + " not";
                                }
                            }
                        }
                    }
                    else if (numApostrophes == 2 && currentWord.StartsWith("'") && currentWord.EndsWith("'"))
                    {
                        apostrophesSurrounding = true; // For use later to correct capitalisation
                        // Remove apostrophes from around words
                        replacement = currentWord.Replace("'", "").Trim();
                    }
                    else
                    {
                        replacement = currentWord;
                    }

                    // Manage capitalisation where apostrophesSurrouding == true

                    Regex capsRegex = new Regex("[A-Z]");
                    int lumpCharIndex = apostrophesSurrounding ? 1 : 0;

                    if (capsRegex.IsMatch(lump[lumpCharIndex].ToString()))
                    {
                        newInput += replacement[0].ToString().ToUpper() + replacement.Substring(1) + " ";
                    }
                    else
                    {
                        newInput += replacement + " ";
                    }

                    // Finalise

                    foreach (string word in replacement.Split(" ".ToCharArray()))
                    {
                        words.Add(word.Trim());
                    }
                }

                // Put punctuation back on the end of sentences

                if (charIndex >= 0)
                {
                    // Take off the space that was added after the final word
                    newInput = newInput.Trim();

                    newInput += originalInput.Substring(windowStart)[charIndex] + " ";
                }
                else
                {
                    newInput += " ";
                }
            }

            newInput = newInput.Trim();

            return words;
        }

        public void CorrectSpellings()
        {
            /*
             * Applies any changes in spelling to the list of dictionary entries.
             */

            foreach (var entry in queue.GetDictionaryEntries())
            {
                if (entry.IsSpelledIncorrectly)
                {
                    // Find in OrderedDictionary and replace the bad spelling with the likely correct one
                    string[] keys = Functions.GetKeysFromOrderedDictionary(odWords);

                    for (int i = 0; i < odWords.Count; i++)
                    {
                        // We need to know the index of the entry in odWords so that we can
                        // insert the correct spelling back in the right place.
                        //
                        // Hmm...is it just OCD to keep odWords in the right order?
                        // Not bothering to change as it's a minor efficiency thing.

                        if (keys[i] == entry.Query)
                        {
                            int replacementIndex = words.IndexOf(entry.Query);
                            words.RemoveAt(replacementIndex);
                            words.Insert(replacementIndex, entry.GetLikelySpelling());

                            odWords.Remove(entry.Query);
                            odWords.Insert(i, entry.GetLikelySpelling(), "");

                            break;
                        }
                    }
                }
            }
        }

        public void GetPartsOfSpeech()
        {
            foreach (var entry in queue.GetDictionaryEntries())
            {
                if (!_db.PartsOfSpeeches.Any(pos => pos.DefinitionString == entry.Query))
                {
                    List<string> partsOfSpeech = entry.GetPartsOfSpeech();

                    if (partsOfSpeech.Count > 0)
                    {
                        Definition def = RitchardDataHelper.GetDefinitionCaseSensitive(entry.Query);

                        foreach (string part in partsOfSpeech)
                        {
                            if (!Strings.PageMapDictionaryPartOfSpeechIgnoreList.Contains(part))
                            {
                                PartsOfSpeech pos = new PartsOfSpeech
                                {
                                    PartOfSpeechID = Guid.NewGuid(),
                                    DefinitionID = def.DefinitionID,
                                    DefinitionString = def.String,
                                    PartOfSpeechDefID = Guids.GuidDictionary[part],
                                    PartOfSpeechString = part
                                };

                                _db.PartsOfSpeeches.InsertOnSubmit(pos);
                            }
                        }

                        _db.SubmitChanges();
                    }
                }
            }
        }

        public void ConjugateVerbs()
        {
            if (odVerbs != null)
            {
                // Create new VerbConjugations

                string[] keys = Functions.GetKeysFromOrderedDictionary(odVerbs);
                foreach (string verb in keys)
                {
                    queue.AddVerbConjugation(verb);
                }
            }
        }

        public void GetConjugationMatches(string verb)
        {
            foreach (var conjugation in queue.GetVerbConjugations())
            {
                string infinitive = conjugation.GetVerb().GetInfinitiveFromTense(verb);

                string sTense = "";
                int iConjugation = -1;

                tenses = new List<string>();

                foreach (var tense in conjugation.GetStaticTenses())
                {
                    if (tense.Key == "Past participle")
                    {
                        tenses.Add("*Compound " + tense.Key);
                    }
                    else
                    {
                        tenses.Add("*Simple " + tense.Key);
                    }
                    tenses.Add(tense.Value);
                    tenses.Add("");
                }

                foreach (var tense in conjugation.GetGrammaticalPersonTenses())
                {
                    tenses.Add("*" + tense.Key);
                    for (int i = 0; i < 6; i++)
                    {
                        string sConjugation = GrammaticalPersonTense.TenseNames[i] + " " + tense.Value.GetConjugation(i).Trim();
                        tenses.Add(sConjugation);
                    }

                    tenses.Add("");
                }

                foreach (string tense in tenses)
                {
                    if (tense == "")
                    {
                        // Ignore blank lines
                    }
                    else if (tense.StartsWith("*"))
                    {
                        // Found the name of a tense, so store it
                        sTense = tense.Replace("*", "");
                    }
                    else
                    {
                        // This is a useful line, so set up the 
                        string[] conjugationWords = tense.ToLower().Split(" ".ToCharArray());
                        bool[] bWindow = new bool[conjugationWords.Length];

                        // Find out which person is being addressed
                        for (int i = 0; i < GrammaticalPersonTense.TenseNames.Count(); i++)
                        {
                            if (GrammaticalPersonTense.TenseNames[i] == conjugationWords[0])
                            {
                                iConjugation = i;
                            }
                        }

                        string[] keys = Functions.GetKeysFromOrderedDictionary(odWords);
                        for (int i = 0; i <= odWords.Count - conjugationWords.Length; i++)
                        {
                            string[] sWindow = new string[conjugationWords.Length];
                            int nMatchingWords = 0;
                            string sMatchingWords = "";

                            // Initialise window settings
                            for (int j = 0; j < conjugationWords.Length; j++)
                            {
                                bWindow[j] = false;
                                sWindow[j] = keys[i + j];
                            }

                            for (int j = 0; j < sWindow.Length; j++)
                            {
                                // Add spaces between matching words
                                if (sMatchingWords != "")
                                {
                                    sMatchingWords += " ";
                                }

                                // Look for words that appear in both the window and the current conjugation
                                for (int k = 0; k < conjugationWords.Length; k++)
                                {
                                    if (sWindow[j] == conjugationWords[k])
                                    {
                                        bWindow[j] = true;
                                        sMatchingWords += sWindow[j];
                                    }
                                    else if (conjugationWords[k] == "he/she/it")
                                    {
                                        // Split the different people and look at them individually
                                        foreach (string person in conjugationWords[k].Split("/".ToCharArray()))
                                        {
                                            if (sWindow[j] == person)
                                            {
                                                bWindow[j] = true;
                                                sMatchingWords += sWindow[j];
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (bool b in bWindow)
                            {
                                if (b)
                                {
                                    nMatchingWords++;
                                }
                            }

                            // God only knows what I was doing here
                            if (nMatchingWords == conjugationWords.Length)
                            {
                                for (int j = 1; j < conjugationWords.Length; j++)
                                {
                                    for (int k = 0; k <= conjugationWords.Length - j; k++)
                                    {
                                        string shortMatch = "";
                                        for (int l = 0; l < j; l++)
                                        {
                                            if (shortMatch != "")
                                            {
                                                shortMatch += " ";
                                            }

                                            shortMatch += keys[k + l];
                                        }

                                        if ((sMatchingWords.StartsWith(shortMatch) ||
                                             sMatchingWords.EndsWith(shortMatch)) &&
                                            tenseMatches.Contains(shortMatch))
                                        {
                                            tenseMatches.Remove(shortMatch);

                                            int index = 0;
                                            foreach (string line in consoleOutput)
                                            {
                                                if (line == shortMatch)
                                                {
                                                    index = consoleOutput.IndexOf(line);
                                                }
                                            }
                                            consoleOutput.RemoveAt(index);
                                        }
                                    }
                                }

                                if (!tenseMatches.Contains(sMatchingWords))
                                {
                                    tenseMatches.Add(sMatchingWords);

                                    string gpt = "";
                                    if (iConjugation > -1)
                                    {
                                        gpt = " " + Verb.GrammaticalPeople[iConjugation] + " ";
                                    }

                                    // Remove items from list that have been replaced by better matches
                                    List<string> errorLines = new List<string>();
                                    foreach (string line in consoleOutput)
                                    {
                                        if (line.EndsWith(sMatchingWords.Split(" ".ToCharArray()).Last()))
                                        {
                                            errorLines.Add(line);
                                        }
                                    }
                                    foreach (string err in errorLines)
                                    {
                                        consoleOutput.Remove(err);
                                    }

                                    consoleOutput.Add(sMatchingWords + " <-- " + gpt + sTense);
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetNewInput()
        {
            return newInput;
        }

        private void ParseSentences()
        {
            string[] sentences = newInput.Split("?!,.".ToCharArray());

            foreach (string sentence in sentences)
            {
                if (sentence != "")
                {
                    string[] sentenceWords = sentence.Trim().Split(" ".ToCharArray());

                    for (int i = 0; i < sentenceWords.Length; i++)
                    {
                        string word = sentenceWords[i];

                        Definition wordDefinition = _db.Definitions.Single(d => d.String == word);

                        if (wordDefinition == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (_db.PartsOfSpeeches.Any(pos => pos.DefinitionID == wordDefinition.DefinitionID && pos.PartOfSpeechDefID == Guids.Verb))
                            {
                                // Find conjugation matches for verbs
                                //FindConjugationMatches(wordDefinition.String);
                            }

                            // Find nouns
                            if (wordDefinition.CanBePartOfSpeech(Guids.Noun))
                            {
                                string description = wordDefinition.Description;

                                // Look for adjectives before noun
                                for (int j = i - 1; j >= 0; j--)
                                {
                                    Definition prevWordDefinition = _db.Definitions.Single(d => d.String == sentenceWords[j]);
                                    if (prevWordDefinition.CanBePartOfSpeech(Guids.Adjective))
                                    {
                                        description = prevWordDefinition.String + " " + description;

                                        // Remove items from list that have been replaced by better matches

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateStatuses()
        {
            foreach (var entry in queue.GetDictionaryEntries())
            {
                entry.CheckTimeout();
                odWords[entry.Query] = entry.GetStatus();
            }
        }

        #region Accessors

        public DictionaryEntry GetDictionaryEntry(string query)
        {
            return queue.GetDictionaryEntries().Single(e => e.Query == query);
        }

        public List<DictionaryEntry> GetDictionaryEntries()
        {
            return queue.GetDictionaryEntries();
        }


        public VerbConjugation GetVerbConjugation(string query)
        {
            return queue.GetVerbConjugations().Single(e => e.Query == query);
        }

        public List<VerbConjugation> GetVerbConjugations()
        {
            return queue.GetVerbConjugations();
        }

        #endregion
    }
}
