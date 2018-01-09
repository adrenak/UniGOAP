using System;
using UnityEngine;
using System.Collections.Generic;

namespace UniLife.GOAP {
    public class Agent : MonoBehaviour {
        public GameObject actionContainer;

        FSM mStateMachine;
        FSM.State mIdleState;
        FSM.State mMovingState;
        FSM.State mPerformState;

        HashSet<Action> mAvailableActions;
        Queue<Action> mCurrentActions;

        IActor mActor;
        Planner mPlanner;
        
        void Start() {
            if(actionContainer == null) {
                Debug.LogError("GOAP Agent named " + gameObject.name + " doesn't have an action container. The agent will disable itself.");
                enabled = false;
                return;
            }
                
            mStateMachine = new FSM();
            mAvailableActions = new HashSet<Action>();
            mCurrentActions = new Queue<Action>();
            mPlanner = new Planner();

            LoadActorFromComponent();
            LoadActionsFromComponents();

            CreateIdleState();
            CreateMovingState();
            CreatePerformState();
            mStateMachine.PushState(mIdleState);
        }

        private void LoadActorFromComponent() {
            foreach (Component comp in gameObject.GetComponents(typeof(Component))) {
                if (typeof(IActor).IsAssignableFrom(comp.GetType())) {
                    mActor = (IActor)comp;
                    return;
                }
            }
        }

        void LoadActionsFromComponents() {
            Action[] actions = actionContainer.GetComponents<Action>();
            foreach (Action a in actions)
                AddAction(a);
        }

        void Update() {
            mStateMachine.Update(gameObject);
        }

        public void AddAction(Action a) {
            mAvailableActions.Add(a);
        }

        public Action GetAction(Type action) {
            foreach (var a in mAvailableActions) 
                if (a.GetType().Equals(action))
                    return a;
            return null;
        }

        public void RemoveAction(Action action) {
            mAvailableActions.Remove(action);
        }

        private bool HasActionPlan() {
            return mCurrentActions.Count > 0;
        }


        // Agent States
        void CreateIdleState() {
            mIdleState = (pFSM, pGameObject) => {
                var _currentState = mActor.GetCurrentState();
                var _goalState = mActor.GetGoalState();

                Queue<Action> _plan = mPlanner.CreatePlan(this, mAvailableActions, _currentState, _goalState);
                if (_plan != null) {
                    mCurrentActions = _plan;
                    mActor.OnPlanFound(_goalState, _plan);

                    mStateMachine.ReplaceState(mPerformState);
                }
                else {
                    mActor.OnPlanFailed(_goalState);
                    mStateMachine.ReplaceState(mIdleState);
                }
            };
        }

        void CreateMovingState() {
            mMovingState = (pFSM, pGameObject) => {
                Action action = mCurrentActions.Peek();
                if (action.IsRanged() && action.target == null) {
                    Debug.Log("ERROR : Action requires a target but has none. Planning failed.");
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
                if (!HasActionPlan()) {
                    Debug.Log("Done actions.");
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
    }
}
