using UnityEngine;
using System.Collections.Generic;

namespace UniLife {
    //[RequireComponent(typeof(NavMeshAgent))]
    public class AgentBase : MonoBehaviour {
        protected StateMachine stateMachine;
        protected StateBase desiredState;
        protected ZoneType currentZoneType = ZoneType.None;
        protected Dictionary<ZoneType, StateBase> bindings = new Dictionary<ZoneType, StateBase>();
        protected NavMeshAgent navMeshAgent;

        public bool isActive;
        // ================================================
        // LIFECYCLE
        // ================================================
        protected virtual void StartImpl(){}
        void Start(){
            isActive = true;
            //navMeshAgent = GetComponent<NavMeshAgent>();
		    StartImpl ();
	    }

	    void Update(){
            if (!isActive) return;
            if (stateMachine != null) stateMachine.ManualUpdate();
	    }


        private void OnTriggerEnter(Collider other) {
            CheckColliderForNewZone(other);
        }
        // ================================================
        // ZONES AND STATES
        // ================================================
        public void SetDesiredState(StateBase pState) {
            desiredState = pState;
            if (!TryChangeState()) {
                stateMachine.ChangeState(Walk.Instance);
                Debug.Log(pState.enterFailMsg);
            }
        }

        void CheckColliderForNewZone(Collider other) {
            var lZone = other.GetComponent<Zone>();
            if (lZone == null)
                return;
            currentZoneType = lZone.GetZoneType();
            TryChangeState();
        }
        
        bool TryChangeState() {
            if (bindings.ContainsKey(currentZoneType) && bindings[currentZoneType] == desiredState) {
                stateMachine.ChangeState(desiredState);
                return true;
            }
            return false;
        }

        // ================================================
        // MOVEMENT
        // ================================================
    }
}
