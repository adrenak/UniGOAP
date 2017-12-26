namespace UniLife {
    public abstract class StateBase {
        public string enterSuccessMsg;
        public string executeMsg;
        public string exitMsg;
        public string enterFailMsg;

        public abstract void Enter(AgentBase agent);
	    public abstract void Execute(AgentBase agent);
	    public abstract void Exit(AgentBase agent);
    }
}