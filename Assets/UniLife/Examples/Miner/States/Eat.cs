using UniLife;
using UnityEngine;

namespace Miner.States {
    public class Eat : StateBase {
        static Eat instance;
        public static Eat Instance {
            get { if (instance == null) instance = new Eat(); return instance; }
        }

        public Eat() {
            enterSuccessMsg = "Lets eat";
            enterFailMsg = "Cant eat here";
            executeMsg = "Eat away";
            exitMsg = "Enough eating";
        }

        MetricDelta hungerDelta = new MetricDelta(-0.3F, Metric.Status.Empty);

        public override void Enter(AgentBase agent) {
            Debug.Log(enterSuccessMsg);
        }

        public override void Execute(AgentBase agent) {
            //Debug.Log(executeMsg);
            var miner = (MinerAgent)agent;
            if (miner.hunger.UpdateFrame(hungerDelta))
                miner.SetDesiredState(Dig.Instance);
        }

        public override void Exit(AgentBase agent) {
            Debug.Log(exitMsg);
        }
    }
}
