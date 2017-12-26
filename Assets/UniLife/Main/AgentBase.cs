using UnityEngine;

namespace UniLife {
    public class AgentBase : MonoBehaviour {
        protected StateMachine stateMachine;
	    protected virtual void StartImpl(){}

	    void Start(){
		    StartImpl ();
	    }

	    void Update(){
            if (stateMachine != null) stateMachine.ManualUpdate();
	    }

	    public StateMachine GetFSM() {
            return stateMachine;
        }
    }
}
