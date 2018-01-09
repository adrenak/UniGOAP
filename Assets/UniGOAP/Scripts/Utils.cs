using System.Collections.Generic;

using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public static class Utils {
        /// <summary>
        /// Removes the action from a HashSet of actions
        /// </summary>
        /// <param name="pActions">THe HashSet of Actions from which an Action is to be removed</param>
        /// <param name="pActionToRemove">The Action to be removed</param>
        /// <returns></returns>
        public static HashSet<Action> RemoveFromActions(HashSet<Action> pActions, Action pActionToRemove) {
            HashSet<Action> lresult = new HashSet<Action>();
            foreach (Action a in pActions)
                if (!a.Equals(pActionToRemove))
                    lresult.Add(a);
            return lresult;
        }

        /// <summary>
        /// Takes two HashSets of Actions and returns if the second is a subset of the first
        /// </summary>
        /// <param name="pActionSet">The Actions against which the subset is to be checked</param>
        /// <param name="pActionSubSet">Actions that are to be checked</param>
        /// <returns></returns>
        public static bool InState(HashSet<StateFlag> pActionSet, HashSet<StateFlag> pActionSubSet) {
            foreach (var a in pActionSet) {
                bool _match = false;
                foreach (var b in pActionSubSet) {
                    if (a.Equals(b)) {
                        _match = true;
                        break;
                    }
                }
                if (!_match)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Ensures that the an extire State exists in another state. Does this by modifying the existing 
        /// State flags of adding new ones
        /// </summary>
        /// <param name="pCurrentState">The State in which the subset should exist</param>
        /// <param name="pSubsetState">The subset state that must exist in the State provided as the first parameter</param>
        /// <returns></returns>
        public static HashSet<StateFlag> EnsureSubset(HashSet<StateFlag> pCurrentState, HashSet<StateFlag> pSubsetState) {
            var lState = new HashSet<StateFlag>();

            // Create a copy of current state
            foreach (var p in pCurrentState)
                lState.Add(new StateFlag(p.Key, p.Value));

            // Go through each StateFlag in StateB and see if it exists in StateA by comparing keys
            // If it does, then remove the StateFlag
            // Finally, add the StateFlag from StateB to StateA
            foreach (StateFlag _effect in pSubsetState) {
                bool _exists = false;
                foreach (StateFlag s in lState) {
                    if (s.Key.Equals(_effect.Key)) {
                        _exists = true;
                        break;
                    }
                }

                if (_exists) {
                    lState.RemoveWhere((StateFlag s) => { return s.Key.Equals(_effect.Key); });
                    StateFlag _updated = new StateFlag(_effect.Key, _effect.Value);
                    lState.Add(_updated);
                }
            }
            return lState;
        }
    }
}