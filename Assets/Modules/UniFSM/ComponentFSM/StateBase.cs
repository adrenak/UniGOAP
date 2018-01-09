namespace UniFSM.ComponentBased {
    public abstract class StateBase {
        public abstract void Enter(IAgent agent);
	    public abstract void Execute(IAgent agent);
	    public abstract void Exit(IAgent agent);
    }
}