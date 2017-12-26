using UnityEngine;

namespace UniLife {
    public class AgentBase : MonoBehaviour {
        protected StateMachine stateMachine;
	    protected virtual void StartImpl(){}
        protected bool isActive;

	    void Start(){
            isActive = true;
		    StartImpl ();
	    }

	    void Update(){
            if (isActive && stateMachine != null)
                stateMachine.ManualUpdate();
	    }

	    public StateMachine GetFSM() {
            return stateMachine;
        }

        public void StopAgent() {
            isActive = false;
        }
    }
}
