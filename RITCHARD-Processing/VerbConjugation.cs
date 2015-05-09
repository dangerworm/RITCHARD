using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using HtmlAgilityPack;
using RITCHARD_Data;
using RITCHARD_Common;

namespace RITCHARD_Processing
{
    public class VerbConjugation : WordLookup
    {
        public List<string> Tags;
        private Verb verb;

        public VerbConjugation()
            : base()
        {
            Initialise();
        }

        public VerbConjugation(string query)
            : base()
        {
            Initialise();
            Browse(query);
        }

        public void Initialise()
        {
            Url = Strings.ConjugatorURL;
            StatusText[1] = Strings.ConjugatorSearching;
            StatusText[2] = Strings.ConjugatorVerbFound;
        }

        public override void Browse(string query)
        {
            verb = new Verb();
            base.Browse(query);

            if (Document.DocumentNode != null)
            {
                Tags = new List<string>();

                CheckNodes(Document.DocumentNode);
            }

            if (verb.VerbForms["Compound"].Moods["Indicative"].GrammaticalPersonTenses["Future perfect continuous"].GetConjugation(5) != null)
            {
                verb.SetVerbTenses();
                Status = DONE;
            }
            else
            {
                Status = EMPTY;
            }
        }

        public Verb GetVerb()
        {
            return verb;
        }

        public Dictionary<string, GrammaticalPersonTense> GetGrammaticalPersonTenses()
        {
            Dictionary<string, GrammaticalPersonTense> gpTenses = new Dictionary<string, GrammaticalPersonTense>();
            foreach (var form in verb.VerbForms)
            {
                foreach (var mood in form.Value.Moods)
                {
                    string tense = form.Key + " " + mood.Key + " ";
                    foreach (var gpTense in mood.Value.GrammaticalPersonTenses)
                    {
                        gpTenses.Add(tense + gpTense.Key, gpTense.Value);
                    }
                }
            }

            return gpTenses;
        }

        public Dictionary<string, string> GetStaticTenses()
        {
            Dictionary<string, string> tenses = new Dictionary<string, string>();

            foreach (var tense in verb.VerbForms["Simple"].Tenses)
            {
                tenses.Add(tense.Key, tense.Value);
            }

            KeyValuePair<string, string> cTense = verb.VerbForms["Compound"].Tenses.First();

            tenses.Add("Participle Past", verb.VerbForms["Simple"].Moods["Participle"].StaticTenses["Past"]);
            tenses.Add("Participle Present", verb.VerbForms["Simple"].Moods["Participle"].StaticTenses["Present"]);

            if (!tenses.ContainsKey(cTense.Key))
            {
                tenses.Add(cTense.Key, cTense.Value);
            }
            else
            {
                tenses[cTense.Key] = cTense.Value;
            }
            return tenses;
        }

        private void CheckNodes(HtmlNode node)
        {
            HtmlNode contentElement;

            foreach (HtmlNode e in node.SelectNodes("//h3"))
            {
                contentElement = e.ParentNode.ParentNode.ParentNode;

                string[] splitContent = Regex.Split(contentElement.InnerHtml, "</?[A-z]>");
                List<string> content = new List<string>();
                foreach (string s in splitContent)
                {
                    if (!string.IsNullOrEmpty(s.Trim()))
                    {
                        if (s.Contains(">"))
                        {
                            string newString = s.Substring(s.LastIndexOf(">") + 1).Trim();
                            if (!string.IsNullOrEmpty(newString))
                            {
                                content.Add(newString);
                            }
                        }
                        else
                        {
                            content.Add(s.Trim());
                        }
                    }
                }

                if (content.Count > 0)
                {
                    if (e.InnerHtml == "Indicative")
                    {
                        contentElement = e.ParentNode.ParentNode.ParentNode;
                        if (contentElement.InnerText.Contains("Present continuous"))
                        {
                            // Compound form

                            int tenseCounter = 0;
                            for (int i = 0; i < 10; i++)
                            {
                                int whileCounter = 0;
                                int wordCounter = 0;
                                while (!GrammaticalPersonTense.TensePronouns.Contains(content[tenseCounter + whileCounter++])) ;
                                while (!GrammaticalPersonTense.TensePronouns.Contains(content[tenseCounter + whileCounter++])) wordCounter++;

                                for (int j = 0; j < 6; j++)
                                {
                                    string conjugation = "";
                                    for (int k = 0; k < wordCounter; k++)
                                    {
                                        conjugation += content[tenseCounter + 2 + ((wordCounter + 1) * j) + k] + " ";
                                    }

                                    verb.VerbForms["Compound"].Moods[e.InnerHtml].GrammaticalPersonTenses[content[tenseCounter]].AddConjugation(j, conjugation.Trim());
                                }

                                tenseCounter += ((wordCounter + 1) * 6) + 1;
                            }

                            // Then find past participle, hidden in a 'B' header somewhere above

                            contentElement = contentElement.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;

                            splitContent = contentElement.InnerText.Trim().Split("\n\r".ToCharArray());
                            content = new List<string>();
                            foreach (string s in splitContent)
                            {
                                if (!string.IsNullOrEmpty(s.Trim()))
                                {
                                    content.Add(s.Trim());
                                }
                            }
                            verb.VerbForms["Compound"].Tenses[content[0]] = content[1];
                        }
                        else
                        {
                            // Simple form

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 6; j++)
                                {
                                    verb.VerbForms["Simple"].Moods[e.InnerHtml].GrammaticalPersonTenses[content[13 * i]].AddConjugation(j, content[(13 * i) + (2 * (j + 1))]);
                                }
                            }
                        }
                    }
                }

                if (e.InnerHtml == "Infinitive")
                {
                    verb.VerbForms["Simple"].Tenses[e.InnerHtml] = content[1];
                }
                else if (e.InnerHtml == "Imperative")
                {
                    verb.VerbForms["Simple"].Tenses[e.InnerHtml] = content[2];
                }
                else if (e.InnerHtml == "Participle")
                {
                    verb.VerbForms["Simple"].Moods[e.InnerHtml].StaticTenses[content[0]] = content[1];
                    verb.VerbForms["Simple"].Moods[e.InnerHtml].StaticTenses[content[2]] = content[3];
                }
            }
        }

        private static List<string> SplitConjugations(HtmlNode contentNode)
        {
            List<string> splitStrings = new List<string>();
            string[] splitContent = Regex.Split(contentNode.InnerHtml, "</?[A-z]>");

            foreach (string s in splitContent)
            {
                if (!string.IsNullOrEmpty(s.Trim()))
                {
                    if (s.Contains(">"))
                    {
                        string newString = s.Substring(s.LastIndexOf(">") + 1).Trim();
                        if (!string.IsNullOrEmpty(newString))
                        {
                            splitStrings.Add(newString);
                        }
                    }
                    else
                    {
                        splitStrings.Add(s.Trim());
                    }
                }
            }

            return splitStrings;
        }
    }
}
