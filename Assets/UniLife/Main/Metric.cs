using UnityEngine;

namespace UniLife {
    [System.Serializable]
    public class Metric {
        public enum Status {
            Empty,
            Full,
            Midway,
            None
        }

        [SerializeField]
        Status mStatus;

        [SerializeField]
        float mValue;

        [SerializeField]
        float min;

        [SerializeField]
        float max;

        public Metric(float pValue, float pMin, float pMax) {
            min = pMin;
            max = pMax;
            mValue = pValue;
        }
       
        public Status GetStatus() {
            return mStatus;
        }

        public bool IsStatus(Status pStatus) {
            return mStatus == pStatus;
        }

        public bool UpdateFrame(MetricDelta pMetricDelta) {
            mValue += pMetricDelta.Delta * Time.timeScale * Time.deltaTime;
            UpdateStatus();
            return mStatus == pMetricDelta.EndStatus;
        }

        public bool UpdateAbsolute(MetricDelta pMetricDelta) {
            mValue += pMetricDelta.Delta;
            UpdateStatus();
            return mStatus == pMetricDelta.EndStatus;
        }
        
        void UpdateStatus() {
            mValue = Mathf.Clamp(mValue, min, max);

            if (mValue == min)
                mStatus = Status.Empty;
            else if (mValue == max)
                mStatus = Status.Full;
            else
                mStatus = Status.Midway;
        }
    }
}
