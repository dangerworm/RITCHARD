using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Common
{
    public static class Guids
    {
        public static Guid EnglishLanguage = Guid.Parse("608576D6-087F-496C-9716-583F17471094");
        public static Guid Me = Guid.Parse("18F7879F-C6CF-45C7-9ADE-77C89C406D84");

        #region PartsOfSpeech

        public static Guid Adjective = Guid.Parse("FC8020E2-B6E5-440D-87D9-F71EBC8CEE9E");
        public static Guid Adverb = Guid.Parse("93458673-516B-49C4-809C-3D961204A2FD");
        public static Guid AuxiliaryVerb = Guid.Parse("19A62F28-03C9-4D6C-AB50-55DBA18A7097");
        public static Guid Conjunction = Guid.Parse("1254FAC6-DED9-45A1-B39D-9D3DC46C6100");
        public static Guid Interjection = Guid.Parse("DC068EFD-5008-438D-A5A7-C9140C020F4E");
        public static Guid Noun = Guid.Parse("CC46DA2A-DE49-4D6C-A326-30AF06C44B51");
        public static Guid Preposition = Guid.Parse("B8B5E517-A98D-41E7-A989-2685CF462D80");
        public static Guid Pronoun = Guid.Parse("77A4034A-327D-4B60-8F04-A0928BCB2DF4");
        public static Guid Verb = Guid.Parse("ABC6BBD6-3BDD-4DDE-8BA0-44AA1595F803");

        public static Dictionary<string, Guid> GuidDictionary = new Dictionary<string, Guid>
        {
            { "adjective", Adjective },
            { "adverb", Adverb },
            { "auxiliary verb", AuxiliaryVerb },
            { "conjunction", Conjunction  },
            { "interjection", Interjection  },
            { "noun", Noun },
            { "preposition", Preposition  },
            { "pronoun", Pronoun  },
            { "verb", Verb }
        };

        #endregion 
    }
}

