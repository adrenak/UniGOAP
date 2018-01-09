using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP.MinerExample {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Miner : MonoBehaviour, IActor {
        NavMeshAgent mNavMeshAgent;

        private void Start() {
            mNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public HashSet<StateFlag> GetGoalState() {
            HashSet<StateFlag> lGoal = new HashSet<StateFlag>();
            lGoal.Add(new StateFlag(StateFlagNames.HAS_ORE, true));
            return lGoal;
        }

        public HashSet<StateFlag> GetCurrentState() {
            HashSet<StateFlag> lWorldData = new HashSet<StateFlag>();

            lWorldData.Add(new StateFlag(StateFlagNames.HAS_ORE, false));
            lWorldData.Add(new StateFlag(StateFlagNames.HAS_TOOL, false));
            lWorldData.Add(new StateFlag(StateFlagNames.IS_TIRED, false));

            return lWorldData;
        }

        public bool MoveAgent(Action pNextAction) {
            mNavMeshAgent.destination = pNextAction.target.transform.position;
            if(Vector3.Distance(transform.position, mNavMeshAgent.destination) < pNextAction.range) {
                pNextAction.SetInRange(true);
                return true;
            }
            return false;
        }

        public void OnActionsFinished() {
            Debug.Log("Finished executing actions");
        }

        public void OnPlanAborted(Action pAbortingAction) {
            Debug.Log("Plan aborted. Aborting action : " + pAbortingAction.ToString());
        }

        public void OnPlanFailed(HashSet<StateFlag> pFailedGoal) {
            Debug.Log("Goal planmning failed : " + pFailedGoal.PrettyPrint());
        }

        public void OnPlanFound(HashSet<StateFlag> pGoal, Queue<Action> pPlan) {
            Debug.Log("Plan found for goal " + pGoal.PrettyPrint() + ". Plan : " + pPlan.PrettyPrint());
        }
    }
}
