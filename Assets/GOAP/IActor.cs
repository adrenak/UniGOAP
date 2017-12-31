using System.Collections.Generic;
using State = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public interface IActor {
        HashSet<State> GetWorldState();
        HashSet<State> GetGoalState();

        // Commands
        bool MoveAgent(Action pNextAction);

        // Events
        void OnPlanFailed(HashSet<State> pFailedGoal);
        void OnPlanFound(HashSet<State> pGoal, Queue<Action> pPlan);
        void OnActionsFinished();
        void OnPlanAborted(Action pAbortingAction);
    }
}
