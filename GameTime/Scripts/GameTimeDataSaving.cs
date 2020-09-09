using System;

namespace VavilichevGD.Tools.Time {
    [Serializable]
    public class GameTimeDataSaving {
        public GameSessionTimeData gameSessionPreviousTimeData;
        public DateTimeSerialized firstPlayDateTimeSerialized;

        public override string ToString() {
            string text = $"First play time: {this.firstPlayDateTimeSerialized}\n{gameSessionPreviousTimeData}";
            return text;
        }
    }
}