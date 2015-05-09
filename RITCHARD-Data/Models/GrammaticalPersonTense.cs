using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Data
{
    public class GrammaticalPersonTense
    {
        public static string[] TensePronouns = { "I", "you", "he/she/it", "we", "you", "they" };

        public static string[] TenseNames = {
                                                "First Person Singular",
                                                "Second Person Singular",
                                                "Third Person Singular",
                                                "First Person Plural",
                                                "Second Person Plural",
                                                "Third Person Plural"
                                            };

        public enum Tenses {
            FirstPersonSingular,
            SecondPersonSingular,
            ThirdPersonSingular,
            FirstPersonPlural,
            SecondPersonPlural,
            ThirdPersonPlural
        };

        public static int FIRST_PERSON_SINGULAR = 0;
        public static int SECOND_PERSON_SINGULAR = 1;
        public static int THIRD_PERSON_SINGULAR = 2;
        public static int FIRST_PERSON_PLURAL = 3;
        public static int SECOND_PERSON_PLURAL = 4;
        public static int THIRD_PERSON_PLURAL = 5;

        private string[] conjugations;

        public GrammaticalPersonTense()
        {
            conjugations = new string[6];
        }

        public void AddConjugation(int grammaticalPerson, string conjugation)
        {
            conjugations[grammaticalPerson] = conjugation.Trim();
        }

        public string GetConjugation(int person)
        {
            return conjugations[person];
        }

        public string[] GetConjugations()
        {
            return conjugations;
        }
    }
}
