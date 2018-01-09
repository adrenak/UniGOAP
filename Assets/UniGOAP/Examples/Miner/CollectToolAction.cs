using UnityEngine;
using System;

namespace UniGOAP {
    public class CollectToolAction : Action {
        protected override void Init() { }

        public override void SetupStateFlags() {
            AddPrecondition(StateFlagNames.HAS_TOOL, false);

            AddEffect(StateFlagNames.HAS_TOOL, true);
        }

        public override bool IsRanged() {
            return true;
        }

        public override bool Perform(GameObject pAgent) {
            isDone = true;
            return true;
        }

        public override bool CheckProceduralPrecondition(Agent pAgent) {
            target = GameObject.Find("Warehouse");
            return target != null;
        }
    }
}
