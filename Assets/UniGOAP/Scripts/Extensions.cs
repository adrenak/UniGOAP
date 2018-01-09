using System.Collections.Generic;
using State = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniGOAP {
    public static class Extensions {
        public static string PrettyPrint(this Queue<Action> pActions) {
            string s = "";
            foreach (Action a in pActions) {
                s += a.GetType().Name;
                s += "-> ";
            }
            s += "GOAL";
            return s;
        }

        public static string PrettyPrint(this HashSet<State> pState) {
            string s = "";
            foreach (KeyValuePair<string, object> kvp in pState) {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }

        public static string PrettyPrint(this HashSet<Action> pAction) {
            string s = "";
            foreach (Action a in pAction) {
                s += a.GetType().Name;
                s += ", ";
            }
            return s;
        }
    }
}