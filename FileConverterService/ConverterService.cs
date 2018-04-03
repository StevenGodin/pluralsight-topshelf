using System.IO;

namespace FileConverterService
{
	public class ConverterService
	{
		private FileSystemWatcher _watcher;

		public bool Start()
		{
			_watcher = new FileSystemWatcher(@"c:\temp\a", "*_in.txt")
			{
				IncludeSubdirectories = false,
				EnableRaisingEvents = true
			};

			_watcher.Created += FileCreated;

			return true;
		}

		private static void FileCreated(object sender, FileSystemEventArgs e)
		{
			var content = File.ReadAllText(e.FullPath);

			var upperContent = content.ToUpperInvariant();
			var dir = Path.GetDirectoryName(e.FullPath);
			var convertedFileName = Path.GetFileName(e.FullPath) + ".converted.txt";
			var convertedPath = Path.Combine(dir, convertedFileName);

			File.WriteAllText(convertedPath, upperContent);
		}

		public bool Stop()
		{
			_watcher.Dispose();
			return true;
		}
	}
}