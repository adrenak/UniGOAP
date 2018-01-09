using System;
using UnityEngine;
using UniFSM.DelegateBased;
using System.Collections.Generic;

using Plan = System.Collections.Generic.Queue<UniLife.GOAP.Action>;

namespace UniLife.GOAP {
    public class Agent : MonoBehaviour {
        public GameObject actionContainer;

        FSM mStateMachine;
        FSM.State mIdleState;
        FSM.State mMovingState;
        FSM.State mPerformState;

        HashSet<Action> mAvailableActions;
        Plan mPlan;

        IActor mActor;
        Planner mPlanner;
        
        void Start() {
            if(actionContainer == null) {
                Debug.LogError("GOAP Agent named " + gameObject.name + " doesn't have an action container. The agent will disable itself.");
                enabled = false;
                return;
            }
                
            mStateMachine = new FSM(this);
            mAvailableActions = new HashSet<Action>();
            mPlan = new Plan();
            mPlanner = new Planner();

            LoadActorFromComponent();
            LoadActionsFromContainer();

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

        void LoadActionsFromContainer() {
            Action[] actions = actionContainer.GetComponents<Action>();
            foreach (Action a in actions)
                AddAction(a);
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
            return mPlan.Count > 0;
        }

        // Agent States
        // The agent only has three states:
        // Idle : During which the agent is looking for something to do
        // Moving : When the agent is travelling to a location where the next action can be executed
        // Perform : When the agent is actually performing the action
        void CreateIdleState() {
            mIdleState = (pFSM, pGameObject) => {
                var _currentState = mActor.GetCurrentState();
                var _goalState = mActor.GetGoalState();

                Plan _plan = mPlanner.CreatePlan(this, mAvailableActions, _currentState, _goalState);
                if (_plan != null) {
                    mPlan = _plan;
                    mActor.OnPlanFound(_goalState, _plan);

                    mStateMachine.PopAndPush(mPerformState);
                }
                else {
                    mActor.OnPlanFailed(_goalState);
                    mStateMachine.PopAndPush(mIdleState);
                }
            };
        }

        void CreateMovingState() {
            mMovingState = (pFSM, pGameObject) => {
                Action _action = mPlan.Peek();
                if (_action.target == null) {
                    mActor.OnMovingFailed(_action);
                    mStateMachine.PopAndPush(mIdleState);
                    return;
                }

                if (mActor.MoveAgent(_action)) {
                    mActor.OnMovingFinished(_action);
                    mStateMachine.PopState();
                }
            };
        }

        void CreatePerformState() {
            mPerformState = (pFSM, pGameObject) => {
                Action _action = mPlan.Peek();

                if (_action.IsDone()) {
                    mActor.OnActionFinished(_action);
                    mPlan.Dequeue();

                    if (!HasActionPlan()) {
                        mStateMachine.PopAndPush(mIdleState);
                        mActor.OnActionsFinished();
                        return;
                    }
                }

                _action = mPlan.Peek();
                bool inRange = _action.IsRanged() ? _action.IsInRange() : true;

                if (inRange) {
                    bool success = _action.Perform(gameObject);

                    if (!success) {
                        mStateMachine.PopAndPush(mIdleState);
                        mActor.OnActionFailed(_action);
                    }
                }
                else 
                    mStateMachine.PushState(mMovingState);
            };
        }
    }
}
