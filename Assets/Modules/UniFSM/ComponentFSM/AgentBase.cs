namespace UniFSM.ComponentBased {
    public interface IAgent {
        void InitFSM();
        void ChangeState(StateBase pNextState);
    }
}
