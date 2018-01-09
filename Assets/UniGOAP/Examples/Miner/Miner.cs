using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP.MinerExample {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Miner : MonoBehaviour, IActor {
        NavMeshAgent mNavMeshAgent;
        HashSet<StateFlag> lGoalState;
        HashSet<StateFlag> lCurrentState;

        private void Start() {
            mNavMeshAgent = GetComponent<NavMeshAgent>();

            // Default goal state
            lGoalState = new HashSet<StateFlag>();
            lGoalState.Add(new StateFlag(StateFlagNames.HAS_ENOUGH_MONEY, true));

            // Default current state
            lCurrentState = new HashSet<StateFlag>();
            lCurrentState.Add(new StateFlag(StateFlagNames.HAS_ENOUGH_MONEY, false));
            lCurrentState.Add(new StateFlag(StateFlagNames.HAS_TOOL, false));
            lCurrentState.Add(new StateFlag(StateFlagNames.IS_TIRED, false));
        }

        public HashSet<StateFlag> GetGoalState() {
            return lGoalState;
        }

        public HashSet<StateFlag> GetCurrentState() {
            return lCurrentState;
        }

        public bool MoveAgent(Action pNextAction) {
            mNavMeshAgent.destination = pNextAction.target.transform.position;

            bool isInRange = Vector3.Distance(transform.position, mNavMeshAgent.destination) < pNextAction.range;
            pNextAction.SetInRange(isInRange);
            return isInRange;
        }

        public void OnPlanFound(HashSet<StateFlag> pGoal, Queue<Action> pPlan) {
            Debug.Log("Plan found for goal " + pGoal.PrettyPrint() + ". Plan : " + pPlan.PrettyPrint());
        }

        public void OnPlanFailed(HashSet<StateFlag> pFailedGoal) {
            Debug.Log("Goal planmning failed : " + pFailedGoal.PrettyPrint());
        }

        public void OnActionFailed(Action pAbortingAction) {
            Debug.Log("Plan aborted. Aborting action : " + pAbortingAction.ToString());
        }

        public void OnMovingFailed(Action pAction) {
            Debug.Log("The action requires a target, which is not defined. Action : " + pAction.GetType().Name);
        }

        public void OnActionFinished(Action pAction) {
            Debug.Log("Action finished : " + pAction.GetType().Name);
            lCurrentState = Utils.EnsureSubset(lCurrentState, pAction.GetEffects());
        }

        public void OnActionsFinished() {
            Debug.Log("Finished executing actions");
        }

        public void OnMovingFinished(Action pAction) {
            Debug.Log("Moved to the target of action : " + pAction.GetType().Name);
        }
    }
}
