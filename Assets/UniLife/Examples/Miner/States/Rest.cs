using UniLife;
using UnityEngine;

namespace Miner.States {
    public class Rest : StateBase {
        static Rest instance;
        public static Rest Instance {
            get { if (instance == null) instance = new Rest(); return instance; }
        }

        MetricDelta energyDelta = new MetricDelta(0.06F, Metric.Status.Full);

        public override void Enter(AgentBase agent) {
            Debug.Log("Going to bed");
        }

        public override void Execute(AgentBase agent) {
            MinerAgent miner = (MinerAgent)agent;

            if (miner.energy.UpdateFrame(energyDelta))
                miner.GetFSM().ChangeState(Dig.Instance);
        }

        public override void Exit(AgentBase agent) {
            Debug.Log("That was a good sleep");
        }
    }
}
