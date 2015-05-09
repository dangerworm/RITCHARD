using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Data
{
    public class Verb
    {
        public static Dictionary<int, string> GrammaticalPeople;

        public Dictionary<string, VerbForm> VerbForms;
        public Dictionary<string, string> VerbTenses;

        bool tensesSet;

        public Verb()
        {
            GrammaticalPeople = new Dictionary<int, string>();
            GrammaticalPeople.Add(0, "First Person Singular");
            GrammaticalPeople.Add(1, "Second Person Singular");
            GrammaticalPeople.Add(2, "Third Person Singular");
            GrammaticalPeople.Add(3, "First Person Plural");
            GrammaticalPeople.Add(4, "Second Person Plural");
            GrammaticalPeople.Add(5, "Third Person Plural");

            VerbForms = new Dictionary<string, VerbForm>();
            VerbTenses = new Dictionary<string, string>();

            VerbForms.Add("Simple", new VerbForm());

            VerbForms["Simple"].AddTense("Infinitive");
            VerbForms["Simple"].AddTense("Imperative");

            VerbForms["Simple"].AddMood("Indicative");
            VerbForms["Simple"].Moods["Indicative"].AddGrammaticalPersonTense("Present");
            VerbForms["Simple"].Moods["Indicative"].AddGrammaticalPersonTense("Preterite");

            VerbForms["Simple"].AddMood("Participle");
            VerbForms["Simple"].Moods["Participle"].AddStaticTense("Present");
            VerbForms["Simple"].Moods["Participle"].AddStaticTense("Past");

            VerbForms.Add("Compound", new VerbForm());

            VerbForms["Compound"].AddTense("Past participle");

            VerbForms["Compound"].AddMood("Indicative");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Present continuous");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Present perfect");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Future");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Future perfect");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Past continuous");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Past perfect");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Future continuous");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Present perfect continuous");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Past perfect continuous");
            VerbForms["Compound"].Moods["Indicative"].AddGrammaticalPersonTense("Future perfect continuous");

            tensesSet = false;
        }

        public GrammaticalPersonTense GetGPTense(string form, string mood, string tense)
        {
            return VerbForms[form].Moods[mood].GrammaticalPersonTenses[tense];
        }

        public string GetStaticTense(string form, string tense)
        {
            return VerbForms[form].Tenses[tense];
        }

        public string GetStaticTense(string form, string mood, string tense)
        {
            return VerbForms[form].Moods[mood].StaticTenses[tense];
        }

        public string GetInfinitiveFromTense(string tense)
        {
            foreach (var vTense in VerbTenses)
            {
                if (vTense.Value == tense)
                {
                    return VerbForms["Simple"].Tenses["Infinitive"];
                }
            }

            return null;
        }

        public void SetVerbTenses()
        {
            if (!tensesSet)
            {
                foreach (var form in VerbForms)
                {
                    foreach (var tense in form.Value.Tenses)
                    {
                        VerbTenses.Add(form.Key + " " + tense.Key, tense.Value);
                    }

                    foreach (var mood in form.Value.Moods)
                    {
                        foreach (var tense in mood.Value.GrammaticalPersonTenses)
                        {
                            int counter = 0;
                            foreach (var conjugation in tense.Value.GetConjugations())
                            {
                                VerbTenses.Add(form.Key + " " + mood.Key + " " + tense.Key + " " +
                                               Verb.GrammaticalPeople[counter++], conjugation);
                            }
                        }

                        foreach (var tense in mood.Value.StaticTenses)
                        {
                            VerbTenses.Add(form.Key + " " + mood.Key + " " + tense.Key + " ", tense.Value);
                        }
                    }
                }

                tensesSet = true;
            }
        }
    }
}
