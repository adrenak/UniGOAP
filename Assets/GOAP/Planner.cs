using System.Collections.Generic;
using State = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public class Planner {
        private class Node {
            public Node parent;
            public float cost;
            public HashSet<State> state;
            public Action action;

            public Node(Node pParent, float pCost, HashSet<State> pStates, Action pAction) {
                parent = pParent;
                cost = pCost;
                state = pStates;
                action = pAction;
            }
        }

        public Queue<Action> CreatePlan(Agent pAgent, HashSet<Action> pAvailableActions, HashSet<State> pWorldState, HashSet<State> pGoalState) {
            foreach (Action a in pAvailableActions)
                a.Reset();

            HashSet<Action> lUsableActions = new HashSet<Action>();
            foreach (Action a in pAvailableActions)
                if (a.CanRun())
                    lUsableActions.Add(a);

            List<Node> lLeaves = new List<Node>();
            Node lStart = new Node(null, 0, pWorldState, null);
            bool lSuccess = BuildGraph(lStart, lLeaves, lUsableActions, pGoalState);

            if (lSuccess)
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

        bool BuildGraph(Node pParent, List<Node> pLeaves, HashSet<Action> pUsableActions, HashSet<State> pGoalState) {
            bool lFoundOne = false;

            foreach(Action a in pUsableActions) {
                if (!InState(a.GetPreconditions(), pParent.state))
                    continue;

                var _currentState = ApplyEffects(pParent.state, a.GetEffects());
                Node _node = new Node(pParent, pParent.cost + a.cost, _currentState, a);

                if(InState(pGoalState, _currentState)) {
                    pLeaves.Add(_node);
                    lFoundOne = true;
                }
                else {
                    HashSet<Action> _subset = ActionSubset(pUsableActions, a);
                    if (BuildGraph(_node, pLeaves, _subset, pGoalState))
                        lFoundOne = true;
                }
            }
            return lFoundOne;
        }

        private HashSet<Action> ActionSubset(HashSet<Action> pActions, Action pActionToRemove) {
            HashSet<Action> lresult = new HashSet<Action>();
            foreach (Action a in pActions) 
                if (!a.Equals(pActionToRemove))
                    lresult.Add(a);
            return lresult;
        }

        bool InState(HashSet<State> pStateA, HashSet<State> pStateB) {
            foreach(var a in pStateA) {
                bool _match = false;
                foreach(var b in pStateB) {
                    if (!a.Equals(b)) {
                        _match = true;
                        break;
                    }
                }
                if (!_match)
                    return false;
            }
            return true;
        }

        HashSet<State> ApplyEffects(HashSet<State> pCurrentState, HashSet<State> pEffects) {
            var lState = new HashSet<State>();

            foreach (var p in pCurrentState)
                lState.Add(new State(p.Key, p.Value));

            foreach(State _effect in pEffects) {
                foreach(State s in lState) {
                    if (s.Equals(_effect)) {
                        lState.Add(new KeyValuePair<string, object>(_effect.Key, _effect.Value));
                        break;
                    }
                }

                lState.RemoveWhere((State s) => {
                    return s.Key.Equals(_effect.Key);
                });
                State _update = new State(_effect.Key, _effect.Value);
                lState.Add(_update);
            }
            return lState;
        }
    }
}
