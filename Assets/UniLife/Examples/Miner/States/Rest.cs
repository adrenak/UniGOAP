using UniLife;
using UnityEngine;

namespace Miner.States {
    public class Rest : StateBase {
        static Rest instance;
        public static Rest Instance {
            get { if (instance == null) instance = new Rest(); return instance; }
        }

        public Rest() {
            enterSuccessMsg = "Lets rest";
            enterFailMsg = "Cant rest here";
            executeMsg = "Rest away";
            exitMsg = "Enough resting";
        }

        MetricDelta energyDelta = new MetricDelta(0.06F, Metric.Status.Full);

        public override void Enter(AgentBase agent) {
            Debug.Log(enterSuccessMsg);
        }

        public override void Execute(AgentBase agent) {
            //Debug.Log(executeMsg);
            MinerAgent miner = (MinerAgent)agent;

            if (miner.energy.UpdateFrame(energyDelta))
                miner.SetDesiredState(Dig.Instance);
        }

        public override void Exit(AgentBase agent) {
            Debug.Log(exitMsg);
        }
    }
}
