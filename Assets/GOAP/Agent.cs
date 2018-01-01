using System;
using UnityEngine;
using System.Collections.Generic;

namespace UniLife.GOAP {
    public class Agent : MonoBehaviour {
        FSM mStateMachine;
        FSM.State mIdleState;
        FSM.State mMovingState;
        FSM.State mPerformState;

        HashSet<Action> mAvailableActions;
        Queue<Action> mCurrentActions;

        IActor mActor;
        Planner mPlanner;

        void Start() {
            mStateMachine = new FSM();
            mAvailableActions = new HashSet<Action>();
            mCurrentActions = new Queue<Action>();
            mPlanner = new Planner();

            GetActorReference();
            CreateIdleState();
            CreateMovingState();
            CreatePerformState();

            mStateMachine.PushState(mIdleState);
            // Add Action implementations here
            //mAvailableActions.Add()
        }

        void Update() {
            mStateMachine.Update(gameObject);
        }


        public void AddAction(Action a) {
            mAvailableActions.Add(a);
        }

        public Action GetAction(Type action) {
            foreach (var a in mAvailableActions) {
                if (a.GetType().Equals(action))
                    return a;
            }
            return null;
        }

        public void RemoveAction(Action action) {
            mAvailableActions.Remove(action);
        }

        private bool HasActionPlan() {
            return mCurrentActions.Count > 0;
        }

        private void GetActorReference() {
            foreach (Component comp in gameObject.GetComponents(typeof(Component))) {
                if (typeof(IActor).IsAssignableFrom(comp.GetType())) {
                    mActor = (IActor)comp;
                    return;
                }
            }
        }

        void CreateIdleState() {
            mIdleState = (fsm, gameObj) => {
                var _worldState = mActor.GetWorldState();
                var _goalState = mActor.GetGoalState();

                Queue<Action> _plan = mPlanner.CreatePlan(this, mAvailableActions, _worldState, _goalState);
                if (_plan != null) {
                    mCurrentActions = _plan;
                    mActor.OnPlanFound(_goalState, _plan);

                    mStateMachine.ReplaceState(mPerformState);
                }
                else {
                    //Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
                    mActor.OnPlanFailed(_goalState);
                    mStateMachine.ReplaceState(mIdleState);
                }
            };
        }

        void CreateMovingState() {
            mMovingState = (pFSM, pGameObject) => {
                Action action = mCurrentActions.Peek();
                if (action.IsRanged() && action.target == null) {
                    Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    mStateMachine.PopState();
                    mStateMachine.ReplaceState(mIdleState);
                    return;
                }

                if (mActor.MoveAgent(action))
                    mStateMachine.PopState();
            };
        }

        void CreatePerformState() {
            mPerformState = (pFSM, pGameObject) => {
                // perform the action

                if (!HasActionPlan()) {
                    Debug.Log("<color=red>Done actions</color>");
                    mStateMachine.ReplaceState(mIdleState);
                    mActor.OnActionsFinished();
                    return;
                }

                Action action = mCurrentActions.Peek();
                if (action.IsDone()) 
                    mCurrentActions.Dequeue();

                if (HasActionPlan()) {
                    action = mCurrentActions.Peek();
                    bool inRange = action.IsRanged() ? action.IsInRange() : true;

                    if (inRange) {
                        bool success = action.Perform(pGameObject);

                        if (!success) {
                            mStateMachine.ReplaceState(mIdleState);
                            mActor.OnPlanAborted(action);
                        }
                    }
                    else 
                        mStateMachine.PushState(mMovingState);

                }
                else {
                    mStateMachine.ReplaceState(mIdleState);
                    mActor.OnActionsFinished();
                }

            };
        }

        public static string PrettyPrint(HashSet<KeyValuePair<string, object>> state) {
            String s = "";
            foreach (KeyValuePair<string, object> kvp in state) {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }

        public static string PrettyPrint(Queue<Action> actions) {
            String s = "";
            foreach (Action a in actions) {
                s += a.GetType().Name;
                s += "-> ";
            }
            s += "GOAL";
            return s;
        }

        public static string PrettyPrint(Action[] actions) {
            String s = "";
            foreach (Action a in actions) {
                s += a.GetType().Name;
                s += ", ";
            }
            return s;
        }

        public static string PrettyPrint(Action action) {
            String s = action.GetType().Name;
            return s;
        }
    }
}
