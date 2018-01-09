using UnityEngine;
using UniFSM.ComponentBased;

namespace Miner {
	public class MinerAgent : MonoBehaviour, IAgent {
        FSM pFSM;
        StateBase pGlobalState = new GlobalState();
        StateBase pDigState = new DigState();
        StateBase pSleepState = new SleepState();

        void Start() {
            InitFSM();
        }

        private void OnGUI() {
            GUI.Button(new Rect(0, 0, 200, 50), "See console on clicking");
            if (GUI.Button(new Rect(0, 75, 200, 50), "Sleep"))
                pFSM.ChangeState(pSleepState);
        }

        public void ChangeState(StateBase pNextState) {
            pFSM.ChangeState(pNextState);
        }

        public void InitFSM() {
            pFSM = new FSM(this, pGlobalState, pDigState);
        }
    }
}