using UniLife;
using UnityEngine;

namespace Miner.States {
	public class Dig : StateBase {
		static Dig instance;
		public static Dig Instance {
			get { if (instance == null) instance = new Dig(); return instance; }
		}

        MetricDelta moneyDelta = new MetricDelta(0.1F, Metric.Status.None);
        MetricDelta energyDelta = new MetricDelta(-0.03F, Metric.Status.Empty);
        MetricDelta hungerDelta = new MetricDelta(0.06F, Metric.Status.Full);

		public override void Enter (AgentBase agent){
			Debug.Log ("Lets do some digging");
		}

		public override void Execute (AgentBase agent){
            MinerAgent miner = (MinerAgent)agent;

            if (miner.energy.Update(energyDelta))
                miner.GetFSM().ChangeState(Rest.Instance);

            if (miner.hunger.Update(hungerDelta))
                miner.GetFSM().ChangeState(Eat.Instance);

            miner.money.Update(moneyDelta);
		}

		public override void Exit (AgentBase agent){
			Debug.Log ("That's enough digging");
		}
	}
}
