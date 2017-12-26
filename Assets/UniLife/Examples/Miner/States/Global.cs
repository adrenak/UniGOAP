using UniLife;
using System;
using UnityEngine;

namespace Miner.States {
    public class Global : StateBase {
        static Global instance;
        public static Global Instance {
            get { if (instance == null) instance = new Global(); return instance; }
        }

        MetricDelta accidentalDamage = new MetricDelta(-2000, Metric.Status.Empty);

        public override void Enter(AgentBase agent) { }

        public override void Execute(AgentBase agent) {
            if(UnityEngine.Random.Range(0, 100) == 0) {
                MinerAgent miner = (MinerAgent)agent;
                if (miner.health.Update(accidentalDamage))
                    Debug.Log("Oh shit");
            }
        }

        public override void Exit(AgentBase agent) {
            throw new NotImplementedException();
        }
    }
}
