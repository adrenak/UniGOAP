using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentBase : MonoBehaviour {
	public StateBase currState;
	protected virtual void StartImpl(){}

	void Start(){
		StartImpl ();
	}

	void Update(){
		if (currState != null)
			currState.Execute (this);
	}

	public void ChangeState(StateBase nextState){
		if(currState != null)
			currState.Exit (this);
		currState = nextState;
		nextState.Enter (this);
	}
}
