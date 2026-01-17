using System;
using System.IO;
using System.Text;
using MBEasyMod.Interfaces;

namespace MBEasyMod.Services
{
	public class Logger : ILogger, IDisposable
	{
		StreamWriter logFile = null;
		string logDirectory = "";

		public Logger(string logDirectory)
		{
			this.logDirectory = logDirectory;
			Init();
		}

		private void Init()
		{
			try
			{
				logFile = new StreamWriter($"{logDirectory}/{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt", false, Encoding.UTF8);
			}catch (Exception e)
			{
				// Just because we are a logger doesn't mean there may not be another already registered
				if(!ServiceManager.TryGetService(out ILogger logger))
					return;

				logger.LogException(e);
			}
		}

		public void Log(string msg)
		{
			if(logFile == null)
				Init();

			try
			{
				logFile.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}]: {msg}");
				logFile.Flush();
			}catch(Exception e)
			{
				LogException(e);
			}
		}

		public void LogException(Exception exception)
		{
			if(logFile == null)
				Init();

			try
			{
				logFile.WriteLine($"{exception.GetType()}\nMessage:\n{exception.Message}\n\tStacktrace:\n{exception.StackTrace}");
			}catch(Exception e)
			{
				// see constructor
				if(!ServiceManager.TryGetService(out ILogger logger))
					return;

				logger.LogException(e);
			}
		}

		public void Dispose()
		{
			logFile.Close();
			logFile = null;
		}
	}
}
