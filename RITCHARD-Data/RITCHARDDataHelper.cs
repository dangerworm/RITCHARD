using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RITCHARD_Common;

namespace RITCHARD_Data
{
    public static class RitchardDataHelper
    {
        private static RitchardDataContext _db;

        #region Definitions

        public static Definition GetDefinitionCaseSensitive(string word)
        {
            return GetDefinitionCaseSensitive(Guids.EnglishLanguage, word);
        }

        public static Definition GetDefinitionCaseSensitive(Guid languageID, string word)
        {
            _db = new RitchardDataContext();

            List<Definition> definitions = _db.Definitions.Where(d => d.String == word).ToList();

            /*
             * 1) SQL is not case sensitive.
             * 2) Language has different meanings when things are capitalised.
             * 3) People generally don't give two shits about grammar.
             * 
             * The following is my attempt to work my way around this.
             * 
             * Examples:
             * Tom is a name, but not a word. This will only return one definition. (Easy)
             * 
             * only is a word, but not a name. (Probably. Hopefully, even - that parent deserves a chair to the face.)
             * 
             * 'Drew' is my name, 'drew' is the past tense of a verb and, if grammar is ignored, 'Drew'
             * can appear as the first word of a sentence meaning the past tense.
             * e.g. "What did you do?" "Drew the curtain." (Harder)
             */

            if (definitions.Count == 0)
            {
                // We have never seen this word before. Return null so that the InputProcessor knows to go and look for it.
                return null;
            }
            if (definitions.Count == 1)
            {
                // That was lucky.
                return definitions.First();
            }
            // Find out if there's a capitalised and a lowercase version (e.g. 'Drew' the name and 'drew' the verb tense.
            // If so, put both in separate definitions.
            else if (definitions.Count == 2)
            {
                Definition definition = null;

                if (definitions.Where(d => d.IsCapitalised).Count() == 1 &&
                    definitions.Where(d => !d.IsCapitalised).Count() == 1)
                {
                    Definition dCap = definitions.Single(d => d.IsCapitalised);
                    Definition dLwr = definitions.Single(d => !d.IsCapitalised);

                    string firstLetter = word[0].ToString(); // Prevents ugly inline code: word[0].ToString() == word[0].ToString().ToUpper()

                    // If word is capitalised and exists in Definitions with IsCapitalised = 1 then
                    // chances are that it's important we use the capitalised version.
                    if (firstLetter == firstLetter.ToUpper())
                    {
                        definition = dCap;
                    }
                    // If word is lowercase and exists with IsCapitalised = 0 then, having done the above first,
                    // we can assume that the lowercase word is the one they meant.
                    else 
                    {
                        definition = dLwr;
                    }
                }

                return definition;
            }
            else
            {
                throw new Exception("There are three or more definitions in the database with the same string. " +
                                    "Even allowing for capitalisation, this shouldn't happen. Check it.");
            }
        }

        public static Guid GetDefinitionIDForString(string word)
        {
            return GetDefinitionIDForString(Guids.EnglishLanguage, word);
        }

        public static Guid GetDefinitionIDForString(Guid languageID, string word)
        {
            Definition d = GetDefinitionCaseSensitive(languageID, word);

            if (d != null)
            {
                return d.DefinitionID;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #endregion

        #region PartsOfSpeech

        public static List<PartsOfSpeech> GetPartOfSpechCaseSensitive(string word)
        {
            return GetPartsOfSpeechCaseSensitive(Guids.EnglishLanguage, word);
        }

        public static List<PartsOfSpeech> GetPartsOfSpeechCaseSensitive(Guid languageID, string word)
        {
            _db = new RitchardDataContext();

            Definition def = GetDefinitionCaseSensitive(languageID, word);
            List<PartsOfSpeech> partsOfSpeech = _db.PartsOfSpeeches.Where(ps => ps.DefinitionID == def.DefinitionID).ToList();

            return partsOfSpeech;
        }

        public static bool InsertDefinition(Definition definition)
        {
            _db = new RitchardDataContext();

            Definition d = GetDefinitionCaseSensitive(definition.LanguageDefID, definition.String);

            if (d != null)
            {
                _db.Definitions.InsertOnSubmit(d);

                try
                {
                    _db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return false;
        }

        #endregion

        #region Conjugations

        public static List<Conjugation> GetConjugationObjects(Guid languageID, Guid definitionID, Verb verb)
        {
            RitchardDataContext db = new RitchardDataContext();
            List<Conjugation> conjugations = new List<Conjugation>();

            string form = "Simple";

            foreach (var tense in verb.VerbForms[form].Tenses)
            {
                conjugations.Add(new Conjugation()
                {
                    ConjugationID = Guid.NewGuid(),
                    InfinitiveDefinitionID = definitionID,
                    LanguageID = languageID,
                    Form = form,
                    Tense = tense.Key,
                    ConjugatedFormDefinitionID = RitchardDataHelper.GetDefinitionIDForString(tense.Value)
                });
            }

            string mood = "Indicative";

            foreach (var tense in verb.VerbForms[form].Moods[mood].GrammaticalPersonTenses)
            {
                for (int p = 0; p < 6; p++)
                {
                    conjugations.Add(new Conjugation()
                    {
                        ConjugationID = Guid.NewGuid(),
                        InfinitiveDefinitionID = definitionID,
                        LanguageID = languageID,
                        Form = form,
                        Mood = mood,
                        Person = GrammaticalPersonTense.TenseNames[p],
                        Tense = tense.Key,
                        ConjugatedFormDefinitionID = RitchardDataHelper.GetDefinitionIDForString(tense.Value.GetConjugation(p))
                    });
                }
            }

            mood = "Participle";

            foreach (var tense in verb.VerbForms[form].Moods[mood].StaticTenses)
            {
                conjugations.Add(new Conjugation()
                {
                    ConjugationID = Guid.NewGuid(),
                    InfinitiveDefinitionID = definitionID,
                    LanguageID = languageID,
                    Form = form,
                    Mood = mood,
                    Tense = tense.Key,
                    ConjugatedFormDefinitionID = RitchardDataHelper.GetDefinitionIDForString(tense.Value)
                });
            }

            form = "Compound";
            string ppTense = "Past participle";

            conjugations.Add(new Conjugation()
            {
                ConjugationID = Guid.NewGuid(),
                InfinitiveDefinitionID = definitionID,
                LanguageID = languageID,
                Form = form,
                Tense = ppTense,
                ConjugatedFormDefinitionID = RitchardDataHelper.GetDefinitionIDForString(verb.VerbForms[form].Tenses[ppTense])
            });

            mood = "Indicative";

            foreach (var tense in verb.VerbForms[form].Moods[mood].GrammaticalPersonTenses)
            {
                for (int p = 0; p < 6; p++)
                {
                    conjugations.Add(new Conjugation()
                    {
                        ConjugationID = Guid.NewGuid(),
                        InfinitiveDefinitionID = definitionID,
                        LanguageID = languageID,
                        Form = form,
                        Mood = mood,
                        Person = GrammaticalPersonTense.TenseNames[p],
                        Tense = tense.Key,
                        ConjugatedFormDefinitionID = RitchardDataHelper.GetDefinitionIDForString(tense.Value.GetConjugation(p))
                    });
                }
            }

            return conjugations;
        }

        public static bool InsertConjugation(Conjugation con)
        {
            _db = new RitchardDataContext();

            if (!_db.Conjugations.Any(c => c.LanguageID == con.LanguageID && c.ConjugatedFormDefinitionID == con.ConjugatedFormDefinitionID))
            {
                _db.Conjugations.InsertOnSubmit(con);

                try
                {
                    _db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return false;
        }

        #endregion

        #region Relationships

        public static bool InsertRelationship(Relationship relationship)
        {
            _db = new RitchardDataContext();

            if (!_db.Relationships.Any(r => r.Concept1ID == relationship.Concept1ID && r.VerbDefinitionID == relationship.VerbDefinitionID && r.Concept2ID == relationship.Concept2ID))
            {
                _db.Relationships.InsertOnSubmit(relationship);

                try
                {
                    _db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return false;
        }

        #endregion
    }
}
