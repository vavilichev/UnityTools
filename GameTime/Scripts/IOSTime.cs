using System.Runtime.InteropServices;

namespace VavilichevGD.Timing {
	public static class IOSTime {
		[DllImport("__Internal")]
		public static extern long GetSystemUpTime();
	}
}