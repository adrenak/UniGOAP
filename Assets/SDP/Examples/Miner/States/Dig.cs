using UniLife;
using UnityEngine;

namespace Miner.States {
	public class Dig : StateBase {
		static Dig instance;
		public static Dig Instance {
			get { if (instance == null) instance = new Dig(); return instance; }
		}

        public Dig() {
            enterSuccessMsg = "Lets do some digging";
            enterFailMsg = "Cant do digging here";
            executeMsg = "Dig away";
            exitMsg = "Enough digging";
        }

        MetricDelta moneyDelta = new MetricDelta(0.1F, Metric.Status.None);
        MetricDelta energyDelta = new MetricDelta(-0.03F, Metric.Status.Empty);
        MetricDelta hungerDelta = new MetricDelta(0.06F, Metric.Status.Full);

		public override void Enter (AgentBase agent){
			Debug.Log (enterSuccessMsg);
		}

		public override void Execute (AgentBase agent){
            //Debug.Log(executeMsg);
            MinerAgent miner = (MinerAgent)agent;

            if (miner.energy.UpdateFrame(energyDelta))
                miner.SetDesiredState(Rest.Instance);

            if (miner.hunger.UpdateFrame(hungerDelta))
                miner.SetDesiredState(Eat.Instance);

            miner.money.UpdateFrame(moneyDelta);
		}

		public override void Exit (AgentBase agent){
            Debug.Log(exitMsg);
        }
	}
}
