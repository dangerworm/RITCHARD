using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Processing
{
    public class Queue
    {
        private List<DictionaryEntry> dictionaryEntries;
        private List<VerbConjugation> verbConjugations;

        private List<string> dictionaryList;
        private List<string> spellCheckedWords;

        public Queue()
        {
            dictionaryEntries = new List<DictionaryEntry>();
            verbConjugations = new List<VerbConjugation>();

            dictionaryList = new List<string>();
            spellCheckedWords = new List<string>();
        }

        public void AddDictionaryEntry(string word)
        {
            if (!dictionaryEntries.Any(de => de.Query == word))
            {
                dictionaryEntries.Add(new DictionaryEntry(word));
            }
        }

        public void AddVerbConjugation(string word)
        {
            if (!verbConjugations.Any(vc => vc.Query == word))
            {
                verbConjugations.Add(new VerbConjugation(word));
            }
        }

        public void DisposeOfDictionaryEntries()
        {
            if (dictionaryEntries != null)
            {
                dictionaryEntries.Clear();
            }

            dictionaryEntries = new List<DictionaryEntry>();
        }

        public void DisposeOfVerbConjugations()
        {
            if (verbConjugations != null)
            {
                verbConjugations.Clear();
            }

            verbConjugations = new List<VerbConjugation>();
        }

        public DictionaryEntry GetDictionaryEntry(string word)
        {
            if (dictionaryEntries != null && dictionaryEntries.Any(de => de.Query == word))
            {
                return dictionaryEntries.Single(de => de.Query == word);
            }

            return null;
        }

        public List<DictionaryEntry> GetDictionaryEntries()
        {
            return dictionaryEntries;
        }

        public List<VerbConjugation> GetVerbConjugations()
        {
            return verbConjugations;
        }
    }
}