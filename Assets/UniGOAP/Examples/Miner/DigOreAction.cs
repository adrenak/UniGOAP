using UnityEngine;

namespace UniGOAP.MinerExample {
    public class DigOreAction : Action {
        public float miningDuration = 0;
        float mStartTime = 0;
        
        protected override void Init() {
            mStartTime = 0;
        }

        public override void SetupStateFlags() {
            AddPrecondition(StateFlagNames.HAS_TOOL, true);
            AddPrecondition(StateFlagNames.HAS_ENOUGH_MONEY, false);
            AddPrecondition(StateFlagNames.IS_TIRED, false);

            AddEffect(StateFlagNames.HAS_TOOL, false);
            AddEffect(StateFlagNames.HAS_ENOUGH_MONEY, true);
            AddEffect(StateFlagNames.IS_TIRED, true);
        }

        public override bool Perform(GameObject pAgent) {
            if (mStartTime == 0)
                mStartTime = Time.time;
            if (Time.time - mStartTime > miningDuration) {
                Debug.Log("Mining done. Axe needs repair now.");
                isDone = true;
            }
            return true;
        }

        public override bool CheckProceduralPrecondition(Agent pAgent) {
            target = GameObject.Find("Mine");
            return target != null;
        }
        
        public override bool IsRanged() {
            return true;
        }
    }
}
