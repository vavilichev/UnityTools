using System;

namespace VavilichevGD.Tools.Time {
	[Serializable]
	public class DateTimeSerialized {
		public string dateTimeStr;

		public DateTimeSerialized(DateTime dateTime) {
			this.SetDateTime(dateTime);
		}

		public DateTimeSerialized() { }

		public DateTime GetDateTime() {
			if (!string.IsNullOrEmpty(this.dateTimeStr))
				return DateTime.Parse(dateTimeStr);
			return new DateTime();
		}

		public void SetDateTime(DateTime dateTime) {
			this.dateTimeStr = dateTime.ToString();
		}

		public override string ToString() {
			return this.dateTimeStr;
		}
	}
}