using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Cryptography;

using RITCHARD_Common;
using RITCHARD_Data;
using RITCHARD_Processing;

namespace RITCHARD_Windows
{
    public partial class Interface : Form
    {
        private RitchardDataContext db;
        private RITCHARDDataController ritchard;

        private InputProcessor processor;

        private OrderedDictionary odWords;
        private OrderedDictionary odVerbs;

        private Guid languageID = Guids.EnglishLanguage;
        private string words;

        public Interface()
        {
            InitializeComponent();
        }

        private void ClearForm()
        {
            ClearSentenceFields();
            ClearThesaurusFields();
            ClearVerbFields();
        }

        private void ClearSentenceFields()
        {
            lstWords.Items.Clear();
            lstResults.Items.Clear();
        }

        private void ClearThesaurusFields()
        {
            txtDescription.Clear();
            txtPOS.Clear();
            lstSynonyms.Items.Clear();
            lstAntonyms.Items.Clear();
        }

        private void ClearVerbFields()
        {
            lstVerbs.Items.Clear();
            lstMatches.Items.Clear();
            lstTenses.Items.Clear();
        }

        private void ConjugateVerbs()
        {
            processor.ConjugateVerbs();

            lstVerbs.Width = 246;
            lstVerbs.Enabled = false;
            tmrCheckVerbStatus.Enabled = true;
        }


        private void FindConjugationMatches(string verb)
        {
            // List all tenses for selected verb
            foreach (var conjugation in processor.GetVerbConjugations())
            {
                string infinitive = conjugation.GetVerb().GetInfinitiveFromTense(verb);

                if (!string.IsNullOrEmpty(infinitive))
                {
                    DisplayVerb(conjugation);
                    break;
                }

                string sTense = "";
                int iConjugation = -1;
                foreach (string tense in lstTenses.Items)
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
                                            lstMatches.Items.Contains(shortMatch))
                                        {
                                            lstMatches.Items.Remove(shortMatch);

                                            int index = 0;
                                            foreach (string line in lstConsole.Items)
                                            {
                                                if (line == shortMatch)
                                                {
                                                    index = lstConsole.Items.IndexOf(line);
                                                }
                                            }
                                            lstConsole.Items.RemoveAt(index);
                                        }
                                    }
                                }

                                if (!lstMatches.Items.Contains(sMatchingWords))
                                {
                                    lstMatches.Items.Add(sMatchingWords);

                                    string gpt = "";
                                    if (iConjugation > -1)
                                    {
                                        gpt = " " + Verb.GrammaticalPeople[iConjugation] + " ";
                                    }

                                    // Remove items from list that have been replaced by better matches
                                    List<string> errorLines = new List<string>();
                                    foreach (string line in lstConsole.Items)
                                    {
                                        if (line.EndsWith(sMatchingWords.Split(" ".ToCharArray()).Last()))
                                        {
                                            errorLines.Add(line);
                                        }
                                    }
                                    foreach (string err in errorLines)
                                    {
                                        lstConsole.Items.Remove(err);
                                    }

                                    lstConsole.Items.Add(sMatchingWords + " <-- " + gpt + sTense);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DisplayVerb(VerbConjugation conjugation)
        {
            lstTenses.Items.Clear();

            foreach (var tense in conjugation.GetStaticTenses())
            {
                if (tense.Key == "Past participle")
                {
                    lstTenses.Items.Add("*Compound " + tense.Key);
                }
                else
                {
                    lstTenses.Items.Add("*Simple " + tense.Key);
                }
                lstTenses.Items.Add(tense.Value);
                lstTenses.Items.Add("");
            }

            foreach (var tense in conjugation.GetGrammaticalPersonTenses())
            {
                lstTenses.Items.Add("*" + tense.Key);
                for (int i = 0; i < 6; i++)
                {
                    string sConjugation = GrammaticalPersonTense.TenseNames[i] + " " + tense.Value.GetConjugation(i).Trim();
                    lstTenses.Items.Add(sConjugation);
                }

                lstTenses.Items.Add("");
            }
        }

        private void ShowError(Exception e)
        {
            if (e != null)
            {
                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(ref ListBox list, ref OrderedDictionary od, bool includeStatus)
        {
            list.Items.Clear();

            string[] keys = new string[od.Count];
            string[] vals = new string[od.Count];
            od.Keys.CopyTo(keys, 0);
            od.Values.CopyTo(vals, 0);

            for (int i = 0; i < od.Count; i++)
            {
                if (includeStatus)
                {
                    list.Items.Add(keys[i] + " (" + vals[i] + ")");
                }
                else
                {
                    list.Items.Add(keys[i]);
                }
            }
        }

        private void lstResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThesaurusResult result = thesaurusEntries.Single(t => t.Query == (string)lstWords.SelectedItem).GetResults()[lstResults.SelectedIndex];

            foreach (ThesaurusDefinition def in result.Definitions)
            {
                txtDescription.Text = def.Description;

                if (!txtPOS.Text.Split("; ".ToCharArray()).Contains(def.ListPOS()))
                {
                    if (txtPOS.Text != "")
                    {
                        txtPOS.Text += "; ";
                    }

                    txtPOS.Text += def.ListPOS();
                }

                lstSynonyms.Items.Clear();
                foreach (string syn in def.Synonyms)
                {
                    lstSynonyms.Items.Add(syn);
                }

                lstAntonyms.Items.Clear();
                foreach (string ant in def.Antonyms)
                {
                    lstAntonyms.Items.Add(ant);
                }
            }
        }

        private void lstVerbs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVerbs.SelectedIndex > -1)
            {
                string query = (string)lstVerbs.SelectedItem;
                DisplayVerb(processor.GetVerbConjugation(query));
            }
        }

        private void lstWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            ClearThesaurusFields();
            PopulateLstResults();
        }

        private void PopulateLstResults()
        {
            if (lstWords.SelectedItem != null)
            {

                string query = ((string)lstWords.SelectedItem).Split(" ".ToCharArray())[0];
                ThesaurusEntry entry = thesaurusEntries.Single(ent => ent.Query == query);
                foreach (ThesaurusResult result in entry.GetResults())
                {
                    lstResults.Items.Add(result.Word);
                }

                if (entry.GetResults().Count > 0)
                {
                    lstResults.Enabled = true;
                    lstResults.SelectedIndex = 0;
                }
                else
                {
                    lstResults.Items.Add("<no results>");
                    lstResults.Enabled = false;
                }
            }
        }

        public void PopulateVerbList()
        {
            foreach (ThesaurusEntry entry in processor.GetThesaurusEntries())
            {
                lstResults.Items.Clear();

                if (entry.CanBeVerb)
                {
                    AddToVerbList(entry.Query, "");
                }
            }
        }

        private void tmrCheckWordStatus_Tick(object sender, EventArgs e)
        {
            processor.UpdateStatuses();

            pbrWords.Maximum = processor.GetDictionaryEntries().Count * WordLookup.DONE;
            int statusCounter = 0;

            foreach (DictionaryEntry entry in processor.GetDictionaryEntries())
            {
                entry.CheckTimeout();
                odWords[entry.Query] = entry.GetStatus();
                statusCounter += Math.Abs(entry.Status);
            }

            pbrWords.Value = statusCounter;
            UpdateStatus(ref lstWords, ref odWords, true);

            if (statusCounter == dictionaryEntries.Count * WordLookup.DONE)
            {
                tmrCheckDictionaryStatus.Enabled = false;
                CorrectSpelling();
            }

            Application.DoEvents();
        }

        private void tmrCheckThesaurusStatus_Tick(object sender, EventArgs e)
        {
            pbrWords.Maximum = thesaurusEntries.Count * WordLookup.DONE;
            int statusCounter = 0;
            foreach (ThesaurusEntry entry in thesaurusEntries)
            {
                entry.CheckTimeout();
                odWords[entry.Query] = entry.GetStatus();
                statusCounter += entry.Status;
            }
            if (statusCounter > -1)
            {
                pbrWords.Value = statusCounter;
            }
            else
            {
                pbrWords.Value = 0;
            }

            if (statusCounter == thesaurusEntries.Count * WordLookup.DONE)
            {
                tmrCheckWordStatus.Enabled = false;
                pbrWords.Value = 0;
                PopulateLstResults();

                bool spellCheckRequests = false;
                DisposeOfDictionaryEntries();

                foreach (ThesaurusEntry entry in thesaurusEntries)
                {
                    if (entry.CanBeVerb)
                    {
                        AddToVerbList(entry.Query, "");
                    }

                    if (entry.GetResults().Count == 0 && !spellCheckedWords.Contains(entry.Query) && !entry.SpellCheckRequested)
                    {
                        AddDictionaryEntry(entry.Query);

                        if (!dictionaryList.Contains(entry.Query))
                        {
                            dictionaryList.Add(entry.Query);
                            spellCheckedWords.Add(entry.Query);
                        }

                        entry.SpellCheckRequested = true;
                        spellCheckRequests = true;
                    }
                }

                if (spellCheckRequests)
                {
                    tmrCheckWordStatus.Enabled = false;
                    tmrCheckDictionaryStatus.Enabled = true;
                }
                else
                {
                    GetWordsFromSentence();
                    lstWords.Width = 120;
                    UpdateStatus(ref lstWords, ref odWords, false);
                    lstWords.Enabled = true;
                    lstWords.Focus();
                    lstWords.SelectedIndex = 0;

                    PopulateVerbList();
                    ConjugateVerbs();
                    return;
                }
            }

            UpdateStatus(ref lstWords, ref odWords, true);
            Application.DoEvents();
        }

        private void tmrCheckVerbStatus_Tick(object sender, EventArgs e)
        {
            pbrVerbs.Maximum = verbConjugations.Count * WordLookup.DONE;
            int statusCounter = 0;

            foreach (VerbConjugation conjugation in verbConjugations)
            {
                conjugation.CheckTimeout();
                odVerbs[conjugation.Query] = conjugation.GetStatus();
                statusCounter += conjugation.Status > 0 ? conjugation.Status : WordLookup.DONE;
            }

            pbrVerbs.Value = statusCounter;

            if (statusCounter == verbConjugations.Count * WordLookup.DONE)
            {
                tmrCheckVerbStatus.Enabled = false;

                // Find any VerbConjugations that aren't really for verbs ('I', for example)

                // Remove verbs that caused errors from odVerbs
                foreach (var con in verbConjugations)
                {
                    if (con.Status == -1)
                    {
                        odVerbs.Remove(con.Query);
                    }
                }

                // Look for any that have been removed and add them to badConjugations
                var badConjugations = verbConjugations.Where(vc => !GetODKeys(odVerbs).Contains(vc.Query)).ToList();
                foreach (var con in badConjugations)
                {
                    verbConjugations.Remove(con);
                }

                pbrVerbs.Value = 0;
                lstVerbs.Width = 120;
                UpdateStatus(ref lstVerbs, ref odVerbs, false);
                lstVerbs.Enabled = true;

                ParseSentences();
                return;
            }

            UpdateStatus(ref lstVerbs, ref odVerbs, true);
            Application.DoEvents();
        }

        private void txtSentence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSentence.Text != "")
                {
                    ClearForm();
                    processor.ProcessInput(txtSentence.Text);

                    lstWords.Width = 246;
                    lstWords.Enabled = false;

                    // Enables timer to update word search status.
                    tmrCheckWordStatus.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please type a sentence and press <Enter>.", "I Have No Words", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                e.Handled = true;
            }
        }

        private void btnStoreEntry_Click(object sender, EventArgs e)
        {
            if (lstWords.SelectedIndex > -1)
            {
                ThesaurusEntry entry = thesaurusEntries.Single(te => te.Query == ((string)lstWords.SelectedItem));

                foreach (ThesaurusResult result in entry.GetResults((string)lstResults.SelectedItem))
                {
                    foreach (ThesaurusDefinition tDef in result.Definitions)
                    {
                        Definition def = DataConverter.GetDefinitionObject(tDef, languageID);
                        def = Definition.InsertDefinition(def);

                        if (lstSynonyms.Items.Count > 0)
                        {
                            foreach (string synonym in lstSynonyms.Items)
                            {
                                ritchard.InsertSynonym(new Synonym { DefinitionID = def.DefinitionID, SynonymID = Definition.GetDefinitionIDForString(synonym) });
                            }
                        }
                    }
                }
            }
        }

        private void btnStoreConjugation_Click(object sender, EventArgs e)
        {
            if (lstVerbs.SelectedIndex > -1)
            {
                VerbConjugation vCon = verbConjugations.Single(vc => vc.Query == ((string)lstVerbs.SelectedItem));

                Guid defID;
                lstWords.Focus();
                for (int i = 0; i < lstWords.Items.Count; i++)
                {
                    lstWords.SelectedIndex = i;

                    if (lstResults.Items.Contains(vCon.Query))
                    {
                        defID = Definition.GetDefinitionIDForString(vCon.Query);
                        List<Conjugation> cons = DataConverter.GetConjugationObjects(defID, vCon.GetVerb(), languageID);

                        foreach (Conjugation c in cons)
                        {
                            Conjugation.InsertConjugation(c);
                        }
                    }
                }
            }
        }

        private void btnInsertKnowlege_Click(object sender, EventArgs e)
        {
            string[] pos = { "adjective", "adverb", "conjunction", "interjection", "noun", "preposition", "pronoun", "proper noun", "verb" };

            foreach (string p in pos)
            {
                //ritchard.InsertDefinition
            }
        }
    }
}