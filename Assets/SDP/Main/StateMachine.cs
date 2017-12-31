using UnityEngine;

namespace UniLife.SDP {
    public class StateMachine {
        AgentBase owner;
        StateBase currState;
        StateBase prevState;
        StateBase globalState;

        public StateMachine(AgentBase owner, StateBase globalState, StateBase currState) {
            this.owner = owner;
            this.globalState = globalState;
            this.currState = currState;
        }

        public void ManualUpdate() {
            if (currState != null) currState.Execute(owner);
            if (globalState != null) globalState.Execute(owner);
        }

        public void ChangeState(StateBase newState) {
            currState.Exit(owner);
            currState = newState;
            currState.Enter(owner);
        }

        public bool IsInState(StateBase aState) {
            return currState == aState;
        }

        public bool WasInState(StateBase aState) {
            return prevState == aState;
        }
    }
}
