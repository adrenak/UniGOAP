using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinerStates{
	public class Dig : StateBase {
		static Dig instance;
		public static Dig Instance {
			get {
				if (instance == null)
					instance = new Dig();
				return instance;
			}
		}

		public override void Enter (AgentBase agent)
		{
			MinerAgent minerAgent = (MinerAgent)agent;

		}

		public override void Execute (AgentBase agent)
		{
			MinerAgent minerAgent = (MinerAgent)agent;
			minerAgent.health -= Time.deltaTime / 10;
		}

		public override void Exit (AgentBase agent)
		{
			throw new System.NotImplementedException ();
		}
	}
}
