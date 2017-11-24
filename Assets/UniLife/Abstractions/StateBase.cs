using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase {
	public abstract void Enter(AgentBase agent);
	public abstract void Execute(AgentBase agent);
	public abstract void Exit(AgentBase agent);
}
