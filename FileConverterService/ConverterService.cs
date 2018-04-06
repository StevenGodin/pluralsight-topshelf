using System;
using System.IO;
using System.Threading;
using Topshelf.Logging;

namespace FileConverterService
{
	public class ConverterService
	{
		private const string DIR = @"c:\temp\a";

		private FileSystemWatcher _watcher;
		private readonly LogWriter _log = HostLogger.Get<ConverterService>();

		public bool Start()
		{
			Directory.CreateDirectory(DIR);
			_watcher = new FileSystemWatcher(DIR, "*_in.txt")
			{
				IncludeSubdirectories = false,
				EnableRaisingEvents = true
			};

			_watcher.Created += FileCreated;

			return true;
		}

		private void FileCreated(object sender, FileSystemEventArgs e)
		{
			_log.Info($"Starting conversion of '{e.FullPath}'");

			// To simulate an unhandled exception
			if (e.FullPath.Contains("bad_in"))
				throw new NotSupportedException("Cannot convert");

			var content = ReadAllTextWithRetry(e.FullPath);
			if (content == null)
			{
				_log.Warn($"Failed to read file '{e.FullPath}'.");
				return;
			}

			var upperContent = content.ToUpperInvariant();
			var dir = Path.GetDirectoryName(e.FullPath);
			var convertedFileName = Path.GetFileName(e.FullPath) + ".converted.txt";
			var convertedPath = Path.Combine(dir, convertedFileName);

			File.WriteAllText(convertedPath, upperContent);
		}

		private string ReadAllTextWithRetry(string path, int maxTryCount = 10, int retryDelay = 2000)
		{
			for (var retry = 0; retry < maxTryCount; retry++)
			try
			{
				return File.ReadAllText(path);
			}
			catch (IOException)
			{
				if (retry < maxTryCount - 1)
					Thread.Sleep(retryDelay);
			}

			return null;
		}

		public bool Stop()
		{
			_watcher.Dispose();
			return true;
		}
	}
}