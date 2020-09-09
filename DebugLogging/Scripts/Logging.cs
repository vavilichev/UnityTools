﻿using UnityEngine;

namespace VavilichevGD.Tools {
    public static class Logging {

        public static bool enabled => IsEnabled();

        private static bool IsEnabled() {
        #if DEBUG
            return true;
        #endif
            return false;
        }
        
        public static void Log(string text, GameObject gameObject = null) {
#if DEBUG
            Debug.Log(text, gameObject);
#endif
        }

        public static void LogError(string text, GameObject gameObject = null) {
#if DEBUG
            Debug.LogError(text, gameObject);
#endif
        }
    }
}