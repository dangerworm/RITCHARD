using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Data
{
    public class Mood
    {
        public Dictionary<string, GrammaticalPersonTense> GrammaticalPersonTenses;
        public Dictionary<string, string> StaticTenses;

        public Mood()
        {
            GrammaticalPersonTenses = new Dictionary<string, GrammaticalPersonTense>();
            StaticTenses = new Dictionary<string, string>();
        }

        public void AddGrammaticalPersonTense(string tense)
        {
            GrammaticalPersonTenses.Add(tense, new GrammaticalPersonTense());
        }

        public void AddStaticTense(string tense)
        {
            StaticTenses.Add(tense, "");
        }
    }
}
