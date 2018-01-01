using System.Collections.Generic;
using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public class Planner {
        private class Node {
            public Node parent;
            public float cost;
            public HashSet<StateFlag> state;
            public Action action;

            public Node(Node pParent, float pCost, HashSet<StateFlag> pState, Action pAction) {
                parent = pParent;
                cost = pCost;
                state = pState;
                action = pAction;
            }
        }

        public Queue<Action> CreatePlan(Agent pAgent, HashSet<Action> pAvailableActions, HashSet<StateFlag> pWorldState, HashSet<StateFlag> pGoalState) {
            Logger.Log("________________");
            Logger.Log("CREATING NEW PLAN");
            Logger.Log("Available actions : " + pAvailableActions.PrettyPrint());
            Logger.Log("World state : " + pWorldState.PrettyPrint());
            Logger.Log("Goal state : " + pGoalState.PrettyPrint());

            foreach (Action a in pAvailableActions)
                a.Reset();

            HashSet<Action> lUsableActions = new HashSet<Action>();
            foreach (Action a in pAvailableActions)
                if (a.CheckProceduralPrecondition(pAgent))
                    lUsableActions.Add(a);
            Logger.Log("Usable actions : " + lUsableActions.PrettyPrint());

            List<Node> lLeaves = new List<Node>();
            Node lStart = new Node(null, 0, pWorldState, null);
            bool lSuccess = BuildGraph(lStart, lLeaves, lUsableActions, pGoalState);
            
            if (!lSuccess) 
                return null;
            Node lCheapsetLeaf = null;
            foreach(Node l in lLeaves) {
                if (lCheapsetLeaf == null)
                    lCheapsetLeaf = l;
                else if (l.cost < lCheapsetLeaf.cost)
                    lCheapsetLeaf = l;
            }

            List<Action> _result = new List<Action>();
            Node n = lCheapsetLeaf;
            while(n != null) {
                if (n.action != null)
                    _result.Insert(0, n.action);
                n = n.parent;
            }

            Queue<Action> lQueue = new Queue<Action>();
            foreach (Action a in _result)
                lQueue.Enqueue(a);

            return lQueue;
        }

        bool BuildGraph(Node pStartNode, List<Node> pLeaves, HashSet<Action> pUsableActions, HashSet<StateFlag> pGoalState) {
            bool lFoundAnActionPath = false;

            foreach(Action a in pUsableActions) {
                // Check if the current state satisfies all the preconditions of the action we are considering 
                if (!InState(a.GetPreconditions(), pStartNode.state)) 
                    continue;

                // If yes, we merge onto the startnode all the effects of that action
                var _newState = MergeOntoState(pStartNode.state, a.GetEffects());
                Node _node = new Node(pStartNode, pStartNode.cost + a.cost, _newState, a);

                if(InState(pGoalState, _newState)) {
                    pLeaves.Add(_node);
                    lFoundAnActionPath = true;
                }
                else {
                    HashSet<Action> _subset = ActionSubset(pUsableActions, a);
                    if (BuildGraph(_node, pLeaves, _subset, pGoalState))
                        lFoundAnActionPath = true;
                }
            }
            return lFoundAnActionPath;
        }

        private HashSet<Action> ActionSubset(HashSet<Action> pActions, Action pActionToRemove) {
            HashSet<Action> lresult = new HashSet<Action>();
            foreach (Action a in pActions) 
                if (!a.Equals(pActionToRemove))
                    lresult.Add(a);
            return lresult;
        }

        // Returns if A is a subset of B
        bool InState(HashSet<StateFlag> pStateA, HashSet<StateFlag> pStateB) {
            foreach(var a in pStateA) {
                bool _match = false;
                foreach(var b in pStateB) {
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

        HashSet<StateFlag> MergeOntoState(HashSet<StateFlag> pCurrentState, HashSet<StateFlag> pEffects) {
            var lState = new HashSet<StateFlag>();

            // Create a copy of StateA
            foreach (var p in pCurrentState)
                lState.Add(new StateFlag(p.Key, p.Value));

            // Go through each StateFlag in StateB and see if it exists in StateA by comparing keys
            // If it does, then remove the StateFlag
            // Finally, add the StateFlag from StateB to StateA
            foreach(StateFlag _effect in pEffects) {
                bool _exists = false;
                foreach(StateFlag s in lState) {
                    if (s.Key.Equals(_effect.Key)) {
                        _exists = true;
                        break;
                    }
                }

                if (_exists) 
                    lState.RemoveWhere((StateFlag s) => { return s.Key.Equals(_effect.Key); });

                StateFlag _updated = new StateFlag(_effect.Key, _effect.Value);
                lState.Add(_updated);
            }
            return lState;
        }
    }
}
