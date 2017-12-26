using UniLife;
using UnityEngine;
using Miner.States;-------`23\

namespace Miner {
	public class MinerAgent : AgentBase {
        public Metric health;
        public Metric money;
        public Metric hunger;
        public Metric energy;

		protected override void StartImpl (){
            stateMachine = new StateMachine(this, Global.Instance, Dig.Instance);
		}
	}
}