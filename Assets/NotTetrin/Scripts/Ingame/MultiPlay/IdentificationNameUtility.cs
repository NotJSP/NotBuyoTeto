using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotTetrin.Ingame.MultiPlay {
    public static class IdentificationNameUtility {
        private static readonly char Separator = '_';

        public static string Create(string name, string id) {
            return name + Separator + id;
        }

        public static string ParseName(string identificationName) {
            var separator_pos = identificationName.IndexOf(Separator);
            if (separator_pos == -1) { return identificationName; }
            return identificationName.Substring(0, separator_pos);
        }

        public static string ParseId(string identificationName) {
            var separator_pos = identificationName.IndexOf(Separator);
            if (separator_pos == -1) { return identificationName; }
            return identificationName.Substring(separator_pos + 1);
        }
    }
}
