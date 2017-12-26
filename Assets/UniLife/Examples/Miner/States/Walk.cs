using UnityEngine;
using System.Collections;
using System;

namespace UniLife {
    public class Walk : StateBase {
        static Walk instance;
        public static Walk Instance {
            get { if (instance == null) instance = new Walk(); return instance; }
        }

        public override void Enter(AgentBase agent) {
            Debug.Log("Need to go somewhere else.");
        }

        public override void Execute(AgentBase agent) {
            
        }

        public override void Exit(AgentBase agent) {
            Debug.Log("Could this be it?");
        }
    }
}
