namespace UniFSM.ComponentBased {
    public class FSM {
        IAgent pOwner;
        StateBase pCurrState;
        StateBase prevState;
        StateBase pGlobalState;

        public FSM(IAgent pOwner, StateBase pGlobalState, StateBase pCurrState) {
            this.pOwner = pOwner;
            this.pGlobalState = pGlobalState;
            this.pCurrState = pCurrState;
            FSMUpdater.Create();
            FSMUpdater.onUpdate += FSMUpdater_HandleOnUpdate;
        }

        ~FSM() {
            FSMUpdater.onUpdate -= FSMUpdater_HandleOnUpdate;
        }

        void FSMUpdater_HandleOnUpdate() {
            if (pCurrState != null) pCurrState.Execute(pOwner);
            if (pGlobalState != null) pGlobalState.Execute(pOwner);
        }
        
        public void ChangeState(StateBase pNewState) {
            pCurrState.Exit(pOwner);
            prevState = pCurrState;
            pCurrState = pNewState;
            pCurrState.Enter(pOwner);
        }

        public bool IsInState(StateBase aState) {
            return pCurrState == aState;
        }

        public bool WasInState(StateBase aState) {
            return prevState == aState;
        }
    }
}
