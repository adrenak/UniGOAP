using UnityEngine;

namespace UniLife.SDP {
    public class Zone : MonoBehaviour {
        [SerializeField]
        ZoneType zoneType;
        public ZoneType GetZoneType() { return zoneType; }
    }
}
