using UnityEngine;
using UniFSM.ComponentBased;

namespace Miner {
    public class SleepState : StateBase {
        public override void Enter(IAgent agent) {
            Debug.Log("EnterS");
        }

        public override void Execute(IAgent agent) {
            Debug.Log("ExecS");
        }

        public override void Exit(IAgent agent) {
            Debug.Log("ExitS");
        }
    }
}
