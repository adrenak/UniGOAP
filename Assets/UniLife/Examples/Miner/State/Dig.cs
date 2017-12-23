using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.State {
	public class Dig : StateBase {
		static Dig instance;
		public static Dig Instance {
			get {
				if (instance == null)
					instance = new Dig();
				return instance;
			}
		}

		float moneyGrowthRate = 0.1F;
		float exhaustionGrowthRate = 0.03F;
		float hungerGrowthRate = .06F;

		public override void Enter (AgentBase agent){
			MinerAgent minerAgent = (MinerAgent)agent;
			Debug.Log ("Lets do some digging");
		}

		public override void Execute (AgentBase agent){
			MinerAgent minerAgent = (MinerAgent)agent;

			// Increase exhaustion
			minerAgent.exhaustion += Time.deltaTime * Time.timeScale * exhaustionGrowthRate;

			// Increase hunger
			minerAgent.hunger += Time.deltaTime * Time.timeScale * hungerGrowthRate;

			// Increase money
			minerAgent.money += Time.deltaTime * Time.timeScale * moneyGrowthRate;
		}

		public override void Exit (AgentBase agent){
			Debug.Log ("That's enough digging");
		}
	}
}
