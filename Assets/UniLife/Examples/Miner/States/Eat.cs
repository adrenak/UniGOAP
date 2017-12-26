using UniLife;
using UnityEngine;

namespace Miner.States {
    public class Eat : StateBase {
        static Eat instance;
        public static Eat Instance {
            get { if (instance == null) instance = new Eat(); return instance; }
        }

        MetricDelta hungerDelta = new MetricDelta(-0.3F, Metric.Status.Empty);

        public override void Enter(AgentBase agent) {
            Debug.Log("Time to eat something");
        }

        public override void Execute(AgentBase agent) {
            var miner = (MinerAgent)agent;
            if (miner.hunger.Update(hungerDelta))
                miner.GetFSM().ChangeState(Dig.Instance);
        }

        public override void Exit(AgentBase agent) {
            Debug.Log("That's enough eating");
        }
    }
}
