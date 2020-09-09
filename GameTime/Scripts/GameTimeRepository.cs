using System;
using System.Collections;
using VavilichevGD.Architecture;

namespace VavilichevGD.Tools.Time {
    public class GameTimeRepository : Repository {

        private GameTimeDataSaving gameTimeDataSaving;

        public GameSessionTimeData gameSessionPreviousTimeData => this.gameTimeDataSaving != null
            ? this.gameTimeDataSaving.gameSessionPreviousTimeData
            : null;

        public DateTime firstPlayTime => this.gameTimeDataSaving != null
            ? this.gameTimeDataSaving.firstPlayDateTimeSerialized.GetDateTime()
            : GameTime.gameSessionCurrenctTimeData.sessionStartTime;
        
        private const string PREF_KEY_GAME_TIME_DATA = "PREF_KEY_GAME_TIME_DATA";


        protected override void Initialize() {
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            this.gameTimeDataSaving = Storage.GetCustom<GameTimeDataSaving>(PREF_KEY_GAME_TIME_DATA);
            Logging.Log($"GAME TIME REPOSITORY: Loaded last data from the Storage. \n{this.gameTimeDataSaving}");
        }

        public void SetGameSessionPreviousTimeData(GameSessionTimeData newData) {
            if (this.gameTimeDataSaving == null) {
                this.gameTimeDataSaving = new GameTimeDataSaving();
                this.gameTimeDataSaving.firstPlayDateTimeSerialized = new DateTimeSerialized(newData.sessionStartTime);
            }
            
            this.gameTimeDataSaving.gameSessionPreviousTimeData = newData;
        }

        public override void Save() {
            Storage.SetCustom(PREF_KEY_GAME_TIME_DATA, this.gameTimeDataSaving);
            Logging.Log($"GAME TIME REPOSITORY: Saved current data in the Storage. \n{this.gameTimeDataSaving}");
        }

    }
}