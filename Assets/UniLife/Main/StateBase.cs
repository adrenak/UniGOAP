namespace UniLife {
    public abstract class StateBase {
	    public abstract void Enter(AgentBase agent);
	    public abstract void Execute(AgentBase agent);
	    public abstract void Exit(AgentBase agent);
    }
}