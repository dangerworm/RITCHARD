using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RITCHARD_Common;

namespace RITCHARD_Data
{
    partial class Definition
    {
        public Definition(Guid languageID, string word)
        {
            DefinitionID = Guid.NewGuid();
            String = word;
            IsCapitalised = char.IsUpper(word[0]);
            LanguageDefID = languageID;
            IsResearched = false;
        }

        public bool CanBePartOfSpeech(Guid partOfSpeech)
        {
            return this.PartsOfSpeech(partOfSpeech).Any();
        }

        public List<PartsOfSpeech> PartsOfSpeech(Guid partOfSpeech)
        {
            return this.PartsOfSpeeches.Where(p => p.PartOfSpeechID == partOfSpeech).ToList();
        }
    }
}
