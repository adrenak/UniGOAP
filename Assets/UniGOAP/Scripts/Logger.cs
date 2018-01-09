using UnityEngine;

namespace UniLife.GOAP {
    public static class Logger {
        public static bool enabled
            = true;
            //= false;

        public static void Log(string pMessage) {
            if (!enabled) return;
            Debug.Log(pMessage);
        }

        public static void LogError(string pError) {
            if (!enabled) return;
            Debug.LogError(pError);
        }

        public static void LogWarning(string pWarning) {
            if (!enabled) return;
            Debug.LogWarning(pWarning);
        }
    }
}
