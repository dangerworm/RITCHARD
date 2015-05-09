using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Data
{
    public class VerbForm
    {
        public Dictionary<string, Mood> Moods;
        public Dictionary<string, string> Tenses;

        public VerbForm()
        {
            Moods = new Dictionary<string, Mood>();
            Tenses = new Dictionary<string, string>();
        }

        public void AddMood(string mood)
        {
            Moods.Add(mood, new Mood());
        }

        public void AddTense(string tense)
        {
            Tenses.Add(tense, "");
        }
    }
}
