using Directory_Scanner.Entities;

namespace Directory_Scanner.Servants
{
	public class DirectoryScanner
	{
		public readonly string DirectoryPath;
		public readonly string ThreadCount;


		private readonly CancellationTokenSource? _cancellationTokenSource;

		public DirectoryScanner(string directoryPath, string threadCount)
		{
			DirectoryPath = directoryPath;
			ThreadCount = threadCount;
			var _cancellationTokenSource = new CancellationTokenSource();
		}
		
		public DirectoryTree StartScan()
		{
			return null;
		}

		public DirectoryTree StopScan()
		{
			return null;
		}

		private TreeNode ScanDirectory()
		{
			return null;
		}

	}
}
