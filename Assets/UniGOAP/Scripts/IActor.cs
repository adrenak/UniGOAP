using System.Collections.Generic;

using StateFlag = System.Collections.Generic.KeyValuePair<string, object>;
using Plan = System.Collections.Generic.Queue<UniLife.GOAP.Action>;

namespace UniLife.GOAP {
    public interface IActor {
        HashSet<StateFlag> GetCurrentState();
        HashSet<StateFlag> GetGoalState();

        // Commands
        bool MoveAgent(Action pNextAction);

        // ================================================
        // EVENTS
        // ================================================
        /// <summary>
        /// When the Planner is unable to find a path of actions to reach the goal state
        /// </summary>
        /// <param name="pFailedGoal">THe unreachable goal state</param>
        void OnPlanFailed(HashSet<StateFlag> pFailedGoal);

        /// <summary>
        /// When the Planner finds an action sequence that would end at the goal state
        /// </summary>
        /// <param name="pGoal">The goal state to be reached</param>
        /// <param name="pPlan">THe sequences of actions that should be executed to reach the goal state</param>
        void OnPlanFound(HashSet<StateFlag> pGoal, Plan pPlan);

        void OnMovingFinished(Action pAction);
        void OnMovingFailed(Action pAction);

        void OnActionFinished(Action pAction);
        void OnActionFailed(Action pAbortingAction);
        void OnActionsFinished();
    }
}
