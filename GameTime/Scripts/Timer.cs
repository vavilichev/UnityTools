using System;

namespace VavilichevGD.Tools.Time {
    public class Timer {

        #region DEDLEGATES

        public delegate void TimerHandler(Timer timer);
        public event TimerHandler OnTimerValueChangedEvent;
        public event TimerHandler OnTimerStartedEvent;
        public event TimerHandler OnTimerFinishedEvent;

        #endregion


        public bool paused { get; private set; }
        public bool isActive { get; private set; }
        public int remainingSeconds { get; private set; }


        public Timer(int seconds) {
            this.remainingSeconds = seconds;
        }


        #region LIFETIME

        public void Start() {
            this.Stop();
            if (!GameTime.isInitialized)
                throw new Exception("Game time is not initialized yet");
            
            this.isActive = true;
            this.paused = false;
            GameTime.OnOneSecondTickEvent += OnOneSecondTick;
            this.OnTimerStartedEvent?.Invoke(this);
        }
        
        public void Start(int seconds) {
            this.Stop();
            this.remainingSeconds = seconds;
            this.Start();
        }
        
        public void Pause() {
            this.paused = true;
        }

        public void Unpause() {
            this.paused = false;
        }
        
        public void Stop() {
            this.isActive = false;
            this.remainingSeconds = 0;
            this.OnTimerFinishedEvent?.Invoke(this);
            GameTime.OnOneSecondTickEvent -= this.OnOneSecondTick;
        }
        
        #endregion
        

        public void SetTimer(int seconds) {
            this.remainingSeconds = seconds;
        }


        #region EVENTS

        private void OnOneSecondTick() {
            if (!this.isActive || this.paused)
                return;
            
            this.remainingSeconds -= 1;
            if (this.remainingSeconds <= 0)
                this.Stop();
            else
                this.OnTimerValueChangedEvent?.Invoke(this);
        }

        #endregion
       
       
        public override string ToString() {
            return TimeConverter.ToMinSecFormat(this.remainingSeconds);
        }
    }
}