using UnityEngine;

namespace VavilichevGD.Tools.Time {
    public static class TimeConverter {
        public static string ToMinSecFormat(int seconds) {
            int min = Mathf.FloorToInt(seconds / 60);
            int sec = Mathf.FloorToInt(seconds % 60);
            string strMin = min < 10 ? $"0{min}" : min.ToString();
            string strSec = sec < 10 ? $"0{sec}" : sec.ToString();
            return $"{strMin}:{strSec}";
        }
    }
}