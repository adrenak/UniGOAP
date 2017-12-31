using UniLife;
using UnityEngine;
using Miner.States;

namespace Miner {
	public class MinerAgent : AgentBase {
        public Metric health = new Metric(1, 0, 1);
        public Metric money = new Metric(0, 0, Mathf.Infinity);
        public Metric hunger = new Metric(0, 0, 1);
        public Metric energy = new Metric(1, 0, 1);

		protected override void StartImpl (){
            bindings.Add(ZoneType.Mess, Eat.Instance);

            stateMachine = new StateMachine(this, Global.Instance, Dig.Instance);
		}
	}
}