using System;

namespace MBEasyMod.Interfaces
{
	public interface ILogger : IDisposable
	{
		void Log(string msg);
		void LogException(Exception exception);
	}
}
