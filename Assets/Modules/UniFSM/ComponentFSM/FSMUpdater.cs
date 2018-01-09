using UnityEngine;

namespace UniFSM {
    public class FSMUpdater : MonoBehaviour {
        static FSMUpdater instance;

        public delegate void UpdateEvent();
        public static event UpdateEvent onUpdate;

        public static void Create() {
            if (instance != null)
                return;
            GameObject go = new GameObject("UniFSM.FSMUpdater");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<FSMUpdater>();
        }

        void Update() {
            onUpdate();
        }
    }
}
