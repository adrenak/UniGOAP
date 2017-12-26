using UniLife;
using System;
using UnityEngine;

namespace Miner.States {
    public class Global : StateBase {
        static Global instance;
        public static Global Instance {
            get { if (instance == null) instance = new Global(); return instance; }
        }

        MetricDelta accidentalDamage = new MetricDelta(-.15F, Metric.Status.Empty);

        public override void Enter(AgentBase agent) { }

        public override void Execute(AgentBase agent) {
            if(UnityEngine.Random.Range(0, 100) == 0) {
                MinerAgent miner = (MinerAgent)agent;
                Debug.Log("Oh shit that hurtz");
                if (miner.health.UpdateAbsolute(accidentalDamage)) {
                    Debug.Log("Ok I'm just gonna die now");
                    miner.StopAgent();
                }
            }
        }

        public override void Exit(AgentBase agent) {
            throw new NotImplementedException();
        }
    }
}
