using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI.Extentions;

namespace VavilichevGD.Tools.Time.Example {
    public class GameTimeExample : MonoBehaviour {

        [SerializeField] private Text textGameSessionInfoPrevous;
        [SerializeField] private Text textGameSessionInfoCurrent;
        [SerializeField] private Text textGameSessionTime;
        [SerializeField] private Text textLifeTimeHours;
        [SerializeField] private Text textUnscaledDeltaTime;
        [SerializeField] private Text textTimeBtwSessions;
        [Space] 
        [SerializeField] private Button btnPause;
        [SerializeField] private Text textBtnPause;
        [Space] 
        [SerializeField] private Text textFirstPlayTime;

        private GameTimeInteractor interactor;
        
        private IEnumerator Start() {
            this.interactor = new GameTimeInteractor();
            yield return interactor.InitializeAsync();
        }

        private void OnEnable() {
            GameTime.OnGameTimeInitializedEvent += this.OnGameTimeInitialized;
            btnPause.AddListener(OnPauseBtnClicked);
        }

        private void OnGameTimeInitialized() {
            this.UpdateView();
            this.textFirstPlayTime.text = GameTime.firstPlayTime.ToString();
        }

        private void UpdateView() {
            this.textGameSessionInfoPrevous.text =
                GameTime.isInitialized ?  $"{GameTime.gameSessionPreviousTimeData}" : "None";
            
            this.textGameSessionInfoCurrent.text =
                GameTime.isInitialized ? GameTime.gameSessionCurrenctTimeData.ToString() : "None";

            this.textTimeBtwSessions.text = GameTime.isInitialized
                ? GameTime.timeBetweenSessionsSec.ToString()
                : "None";

            this.UpdateBtnPauseView();
        }

        private void UpdateBtnPauseView() {
            this.btnPause.interactable = GameTime.isInitialized;
            this.textBtnPause.text = GameTime.isPaused ? "Unpause" : "Pause";
        }
        

        private void OnPauseBtnClicked() {
            GameTime.SwitchPauseState();
            this.UpdateBtnPauseView();
        }

        private void Update() {
            this.textUnscaledDeltaTime.text = GameTime.unscaledDeltaTime.ToString(CultureInfo.InvariantCulture);
            this.textGameSessionTime.text = GameTime.timeSinceGameStarted.ToString(CultureInfo.InvariantCulture);
            this.textLifeTimeHours.text = GameTime.lifeTimeHourse.ToString();
        }

        private void OnDisable() {
            GameTime.OnGameTimeInitializedEvent -= this.OnGameTimeInitialized;
            this.btnPause.RemoveListener(OnPauseBtnClicked);
            this.interactor.Save();
        }
    }
}