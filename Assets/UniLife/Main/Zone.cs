using UnityEngine;

namespace UniLife {
    public class Zone : MonoBehaviour {
        [SerializeField]
        ZoneType zoneType;
        public ZoneType GetZoneType() { return zoneType; }
    }
}
