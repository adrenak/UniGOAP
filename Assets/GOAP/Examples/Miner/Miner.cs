using UnityEngine;
using System.Collections.Generic;
using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP.MinerExample {
    public class Miner : MonoBehaviour, IActor {
        public HashSet<StateFlag> GetGoalState() {
            HashSet<StateFlag> lGoal = new HashSet<StateFlag>();
            lGoal.Add(new StateFlag(StateFlagNames.HAS_ORE, true));
            return lGoal;
        }

        public HashSet<StateFlag> GetWorldState() {
            HashSet<StateFlag> lWorldData = new HashSet<StateFlag>();

            lWorldData.Add(new StateFlag(StateFlagNames.HAS_ORE, false));
            lWorldData.Add(new StateFlag(StateFlagNames.HAS_TOOL, true));
            lWorldData.Add(new StateFlag(StateFlagNames.IS_TIRED, true));

            return lWorldData;
        }

        public bool MoveAgent(Action pNextAction) {
            Debug.Log("Moving to " + pNextAction.target.transform.position);
            transform.position = pNextAction.target.transform.position;
            pNextAction.SetInRange(true);
            return true;
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
