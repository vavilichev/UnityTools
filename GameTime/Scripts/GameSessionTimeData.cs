using System;

namespace VavilichevGD.Tools.Time {
	[Serializable]
	public class GameSessionTimeData {
		public DateTimeSerialized sessionStartSerializedFromServer;
		public DateTimeSerialized sessionStartSerializedFromDevice;
		public long timeValueActiveDeviceAtStart;
		public long timeValueActiveDeviceAtEnd;
		public double sessionDuration;

		public bool timeReceivedFromServer => this.sessionStartSerializedFromServer.GetDateTime() != new DateTime();

		public DateTime sessionStartTime => this.GetSessionStartTime();
		public DateTime sessionOverTime => this.GetSessionOverTime();

		public GameSessionTimeData() {
			this.sessionStartSerializedFromServer = new DateTimeSerialized();
			this.sessionStartSerializedFromDevice = new DateTimeSerialized();
		}

		private DateTime GetSessionStartTime() {
			if (this.timeReceivedFromServer)
				return this.sessionStartSerializedFromServer.GetDateTime();
			return this.sessionStartSerializedFromDevice.GetDateTime();
		}

		private DateTime GetSessionOverTime() {
			DateTime start = this.sessionStartTime;
			return start.AddSeconds(this.sessionDuration);
		}

		public override string ToString() {
			return $"Time start from server: {this.sessionStartSerializedFromServer}\n" +
			       $"Time start from device: {this.sessionStartSerializedFromDevice}\n" +
			       $"Active device time at start: {this.timeValueActiveDeviceAtStart}\n" +
			       $"Active device time at end: {this.timeValueActiveDeviceAtEnd}\n" +
			       $"Session duration: {this.sessionDuration}\n" +
			       $"Time received from server: {this.sessionStartTime}\n" +
			       $"Session start: {this.sessionStartTime}\n" +
			       $"Session over: {this.sessionOverTime}";
		}
	}
}