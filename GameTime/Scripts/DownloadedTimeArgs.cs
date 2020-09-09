using System;

namespace VavilichevGD.Tools.Time {
	public struct DownloadedTimeArgs {
		public DateTime downloadedTime { get; }
		public bool error { get; }
		public string errorText { get; }
		public bool loadedFromServer { get; }

		public DownloadedTimeArgs(DateTime dowloadedTime, bool error, string errorText, bool loadedFromServer) {
			this.downloadedTime = dowloadedTime;
			this.error = error;
			this.errorText = errorText;
			this.loadedFromServer = loadedFromServer;
		}
	}
}