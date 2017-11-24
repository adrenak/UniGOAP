using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinerStates;

public class MinerAgent : AgentBase {
	public float health = 1;

	protected override void StartImpl (){
		health = 1F;
		currState = Dig.Instance;
	}
}
