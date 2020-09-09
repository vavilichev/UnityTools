extern "C" 
{
	double GetSystemUpTime()
	{
		return [[NSProcessInfo processInfo] systemUptime];
	}
}