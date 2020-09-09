using System;
using UnityEngine;

namespace VavilichevGD.Tools.Time {
	public class GameTime : MonoBehaviour {

		#region DELEGATES

		public delegate void GamePauseHandler(bool paused);
		public static event GamePauseHandler OnGamePausedEvent;
		
		public delegate void GameTimeEventHandler();
		public static event GameTimeEventHandler OnGameTimeInitializedEvent;
		public delegate void GameTimeTickHandler();
		public static event GameTimeTickHandler OnOneSecondTickEvent;

		#endregion
		
		public static bool isInitialized => instance != null && instance.interactor != null;
		public static float unscaledDeltaTime => UnityEngine.Time.unscaledDeltaTime;
		public static float deltaTime => UnityEngine.Time.deltaTime;
		public static DateTime now => isInitialized ? instance.interactor.now : DateTime.MinValue;
		public static DateTime nowDevice => isInitialized ? instance.interactor.nowDevice : DateTime.MinValue;
		public static DateTime firstPlayTime => isInitialized ? instance.interactor.firstPlayTime : DateTime.MinValue;
		public static double lifeTimeHourse => isInitialized ? instance.interactor.lifeTimeHours : 0;
		public static GameSessionTimeData gameSessionCurrenctTimeData => isInitialized ? instance.interactor.gameSessionCurrentTimeData : null;
		public static GameSessionTimeData gameSessionPreviousTimeData => isInitialized ? instance.interactor.gameSessionPreviousTimeData : null;
		public static double timeBetweenSessionsSec => isInitialized ? instance.interactor.timeBetweenSessionsSec : 0;
		public static double timeSinceGameStarted => isInitialized ? instance.interactor.timeSinceGameStartedSec : 0;
		public static bool isPaused => deltaTime == 0f;

		private static GameTime instance { get; set; }
		
		private GameTimeInteractor interactor;
		private float timer;


		#region INITIALIZING

		public static void Initialize(GameTimeInteractor interactor) {
			if (isInitialized)
				return;

			CreateSingleton();
			instance.interactor = interactor;
			Logging.Log("GAME TIME: is initialized.");
			OnGameTimeInitializedEvent?.Invoke();
		}

		private static void CreateSingleton() {
			GameObject gameTimeGO = new GameObject("[GAME TIME]");
			instance = gameTimeGO.AddComponent<GameTime>();
			DontDestroyOnLoad(gameTimeGO);
		}

		#endregion
		

		
		private void Update() {
			float deltaTimeUnscaled = unscaledDeltaTime;
			interactor.Update(deltaTimeUnscaled);
			this.TimerWork(deltaTimeUnscaled);
		}

		private void TimerWork(float deltaTimeUnscaled) {
			this.timer += deltaTimeUnscaled;
			if (this.timer >= 1f) {
				this.timer = Mathf.Max(1f - this.timer, 0f);
				OnOneSecondTickEvent?.Invoke();
			}
		}

		
		public static void Pause() {
			UnityEngine.Time.timeScale = 0f;
			NotifyAboutGamePauseStateChanged();
		}
		
		public static void Unpause() {
			UnityEngine.Time.timeScale = 1f;
			NotifyAboutGamePauseStateChanged();
		}
		
		public static void SwitchPauseState() {
			if (isPaused)
				Unpause();
			else
				Pause();
		}

		
		private static void NotifyAboutGamePauseStateChanged() {
			OnGamePausedEvent?.Invoke(isPaused);
		}
	}
}