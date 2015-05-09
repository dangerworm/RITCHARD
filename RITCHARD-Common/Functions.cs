using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RITCHARD_Common
{
    public static class Functions
    {
        public static string[] GetKeysFromOrderedDictionary(OrderedDictionary od)
        {
            string[] keys = new string[od.Count];
            od.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
}
