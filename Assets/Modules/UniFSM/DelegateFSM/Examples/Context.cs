using UnityEngine;
using UniFSM;
using UniFSM.DelegateBased;

public class Context : MonoBehaviour {
    FSM machine;
    FSM.State stateA;
    FSM.State stateB;

    private void Start() {
        FSMUpdater.Create();
        machine = new FSM(this);

        stateA = (pFSM, owner) => {
            Debug.Log("StateA");
        };

        stateB = (pFSM, owner) => {
            Debug.Log("StateB");
        };
        machine.PushState(stateA);
    }

    private void OnGUI() {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Switch to state B")) 
            machine.PopAndPush(stateB);
    }
}
