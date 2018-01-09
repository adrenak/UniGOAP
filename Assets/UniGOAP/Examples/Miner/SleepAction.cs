using UnityEngine;
using System;

namespace UniGOAP {
    public class SleepAction : Action {
        public float sleepDuration;

        float mStartTime;

        protected override void Init() {
            mStartTime = 0;
        }

        public override void SetupStateFlags() {
            AddPrecondition(StateFlagNames.IS_TIRED, true);

            AddEffect(StateFlagNames.IS_TIRED, false);
            AddEffect(StateFlagNames.HAS_ENOUGH_MONEY, false);
        }

        public override bool IsRanged() {
            return true;
        }

        public override bool Perform(GameObject pAgent) {
            if (mStartTime == 0)
                mStartTime = Time.time;
            if (Time.time - mStartTime > sleepDuration) {
                Logger.Log("Sleeping done");
                isDone = true;
            }
            return true;
        }


        public override bool CheckProceduralPrecondition(Agent pAgent) {
            target = GameObject.Find("Bed");
            return target != null;
        }

    }
}
