using System.Collections.Concurrent;

namespace Directory_Scanner.Servants
{
	public class DirectoryQuque
	{
		private ConcurrentQueue<Task> _taskQueue;
		private readonly Semaphore _taskSemaphore;

		public DirectoryQuque(int maxThreadCount)
		{
			_taskQueue = new ConcurrentQueue<Task>();
			_taskSemaphore = new Semaphore(maxThreadCount, maxThreadCount);

		}
	}
}
