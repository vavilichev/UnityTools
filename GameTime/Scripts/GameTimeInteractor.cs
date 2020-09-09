using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Tools.Time {
    public class GameTimeInteractor : Interactor {

        #region DELEGATES

        public delegate void GameTimeInitializeHandler();
        public static event GameTimeInitializeHandler OnGameTimeInitialized;

        #endregion
       

        public GameSessionTimeData gameSessionPreviousTimeData => this.gameTimeRepository.gameSessionPreviousTimeData;
        public GameSessionTimeData gameSessionCurrentTimeData { get; set; }
        public double timeBetweenSessionsSec { get; private set; }
        public double timeSinceGameStartedSec { get; private set; }
        public DateTime nowDevice => DateTime.Now.ToUniversalTime();
        public DateTime now => this.GetNowDateTime();
        public DateTime firstPlayTime => this.gameTimeRepository.firstPlayTime;
        public double lifeTimeHours => (now - firstPlayTime).TotalHours;
        
        private GameTimeRepository gameTimeRepository;

        #region INITIALIZING

        protected override void Initialize() {
            this.gameTimeRepository = this.GetRepository<GameTimeRepository>();
        }

        protected override IEnumerator InitializeRoutine() {
            TimeLoader timeLoader = new TimeLoader();
            timeLoader.OnTimeDownloadedEvent += this.OnTimeDownloaded;
            yield return timeLoader.LoadTime();

            GameTime.Initialize(this);
            Logging.Log($"GAME TIME INTERACTOR: Initialized successful. \n{this}");
        }
        
        private void OnTimeDownloaded(TimeLoader timeLoader, DownloadedTimeArgs e) {
            timeLoader.OnTimeDownloadedEvent -= OnTimeDownloaded;

            this.gameSessionCurrentTimeData = this.InitGameTimeSessionCurrent(e.downloadedTime);
            this.CalculateTimeBetweenSessions(this.gameSessionPreviousTimeData, this.gameSessionCurrentTimeData);
            OnGameTimeInitialized?.Invoke();
        }

        private GameSessionTimeData InitGameTimeSessionCurrent(DateTime downloadedTime) {
            var currentSessionTimeData = new GameSessionTimeData();
            currentSessionTimeData.sessionStartSerializedFromServer.SetDateTime(downloadedTime);
			
            DateTime deviceTime = this.nowDevice;
            currentSessionTimeData.sessionStartSerializedFromDevice.SetDateTime(deviceTime);
            currentSessionTimeData.timeValueActiveDeviceAtStart = this.GetDeviceWorkTimeInSeconds();
            return currentSessionTimeData;
        }
        
        private DateTime GetNowDateTime() {
            DateTime gameStartTime = this.gameSessionCurrentTimeData.sessionStartTime;
            DateTime curerntTime = gameStartTime.AddSeconds(timeSinceGameStartedSec);
            return curerntTime;
        }
        
        private long GetDeviceWorkTimeInSeconds() {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass systemClock = new AndroidJavaClass("android.os.SystemClock");
			return Mathf.FloorToInt(systemClock.CallStatic<long>("elapsedRealtime") / 1000f);
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSTime.GetSystemUpTime() / 1000;
#else
            int deviceRunTimeTicks = Environment.TickCount & Int32.MaxValue;
            int totalSeconds = Mathf.FloorToInt(deviceRunTimeTicks / 1000f);
            return totalSeconds;
#endif
        }
        
        private void CalculateTimeBetweenSessions(GameSessionTimeData timeDataPreviousSession, GameSessionTimeData timeDataCurrentSession) {
            if (timeDataPreviousSession == null) {
                timeBetweenSessionsSec = 0;
                return;
            }

            timeBetweenSessionsSec = timeDataCurrentSession.timeValueActiveDeviceAtStart - timeDataPreviousSession.timeValueActiveDeviceAtEnd;
            if (timeBetweenSessionsSec < 0f) {
                timeBetweenSessionsSec = Mathf.FloorToInt((float)(timeDataCurrentSession.sessionStartTime - timeDataPreviousSession.sessionOverTime).TotalSeconds);
                timeBetweenSessionsSec = Mathf.Max((float)timeBetweenSessionsSec, 0f);
            }
        }

        #endregion
        
        
        public void Save() {
            this.gameSessionCurrentTimeData.sessionDuration = this.timeSinceGameStartedSec;
            this.gameSessionCurrentTimeData.timeValueActiveDeviceAtEnd = this.GetDeviceWorkTimeInSeconds();
            this.gameTimeRepository.SetGameSessionPreviousTimeData(this.gameSessionCurrentTimeData);
            this.gameTimeRepository.Save();
        }

        public void Update(float unscaledDeltaTime) {
            this.timeSinceGameStartedSec += unscaledDeltaTime;
        }
        
        public override string ToString() {
            if (this.isInitialized)
               return $"Last session: {gameSessionPreviousTimeData}\n\n" +
                   $"Current session: {gameSessionCurrentTimeData}\n\n" +
                   $"Time between sessions: {timeBetweenSessionsSec}";
            return this.GetType().ToString();
        }
    }
}