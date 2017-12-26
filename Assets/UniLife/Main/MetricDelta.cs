using UnityEngine;
using System.Collections;

namespace UniLife {
    public class MetricDelta {
        float mDelta;
        public float Delta { get { return mDelta; } }

        Metric.Status mEndStatus;
        public Metric.Status EndStatus { get { return mEndStatus; } }

        public MetricDelta(float pDelta, Metric.Status pEndStatus) {
            mDelta = pDelta;
            mEndStatus = pEndStatus;
        }
    }
}
