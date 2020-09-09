using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace VavilichevGD.Tools.Time {
	public class TimeLoader {

		#region CONSTANTS

		private const bool LOADED_FROM_LOCAL = false;
		private const bool LOADED_FROM_INTERNET = true;
		private const bool HAS_ERROR = true;
		private const bool NO_ERROR = false;
		private const int BREAK_TIME_DEFAULT = 2;
		private const string SERVER_URL = "https://www.microsoft.com";

		#endregion

		#region DELEGATES

		public delegate void DownloadTimeHandler(TimeLoader timeLoader, DownloadedTimeArgs e);
		public event DownloadTimeHandler OnTimeDownloadedEvent;

		#endregion


		public bool isLoading => this.routineLoadTime.isActive;

		private RoutineWithArg<int> routineLoadTime;

		
		public TimeLoader() {
			this.routineLoadTime = new RoutineWithArg<int>(this.LoadTimeRoutine);
		}
		
		public Coroutine LoadTime(int breakTime = BREAK_TIME_DEFAULT) {
			if (!this.isLoading)
				return this.routineLoadTime.Start(breakTime);
			return null;
		}

		private IEnumerator LoadTimeRoutine(int breakTime) {
			
			var request = new UnityWebRequest(SERVER_URL);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.timeout = breakTime;

			yield return request.SendWebRequest();
			if (this.NotValidResponse(request))
				yield break;
			
			var todaysDates = request.GetResponseHeaders()["date"];
			DateTime downloadedTime = DateTime.ParseExact(todaysDates,
									   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
									   CultureInfo.InvariantCulture.DateTimeFormat,
									   DateTimeStyles.AdjustToUniversal);

			this.NotifyAboutDownloadedTime(downloadedTime, NO_ERROR, null, LOADED_FROM_INTERNET);
		}

		private bool NotValidResponse(UnityWebRequest request) {
			string errorText = "";

			if (request.isNetworkError)
				errorText = $"Downloading time stopped: {request.error}";
			else if (request.downloadHandler == null)
				errorText = $"Downloading time stopped: DownloadHandler is NULL";
			else if (string.IsNullOrEmpty(request.downloadHandler.text))
				errorText = $"Downloading time stopped: Downloaded string is empty or NULL";

			if (string.IsNullOrEmpty(errorText))
				return false;

			this.NotifyAboutDownloadedTime(new DateTime(), HAS_ERROR, errorText, LOADED_FROM_LOCAL);
			return true;
		}

		private void NotifyAboutDownloadedTime(DateTime downloadedTime, bool error, string errorText, bool downloadedFromServer) {
			DownloadedTimeArgs downloadedTimeArgs = new DownloadedTimeArgs(downloadedTime, error, errorText, downloadedFromServer);
			OnTimeDownloadedEvent?.Invoke(this, downloadedTimeArgs);
		}
	}
}