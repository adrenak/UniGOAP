using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.State;

namespace Miner {
	public class MinerAgent : AgentBase {
		public float health = 1;
		public float money = 0;
		public float hunger = 0;
		public float exhaustion = 0;

		protected override void StartImpl (){
			health = 1F;
			ChangeState (Dig.Instance);
		}
	}
}