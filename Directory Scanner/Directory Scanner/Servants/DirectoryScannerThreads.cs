using System.Collections.Concurrent;
using System.Diagnostics;

namespace Directory_Scanner.Servants
{
	public class DirectoryScannerThreads
	{
		private ConcurrentBag<Task> _taskBag;
		private ConcurrentBag<Task> _waitBag;
		private readonly Semaphore _taskSemaphore;

		public Task? _addNextTask;
		public Task? _waitNextTask;

		private readonly CancellationToken _token;
		public DirectoryScannerThreads(int maxThreadCount, CancellationTokenSource cts)
		{
			_taskBag = new ConcurrentBag<Task>();
			_waitBag = new ConcurrentBag<Task>();
			_taskSemaphore = new Semaphore(maxThreadCount, maxThreadCount);
			_token = cts.Token;			
		}

		public void StartThreads(CancellationToken _token)
		{
			// Start tasks to add and wait another tasks
			_waitNextTask = new Task( () => WaitNextTasks(), _token);
			_waitNextTask.Start();
			_addNextTask = new Task( () => StartNextTask(), _token);
			_addNextTask.Start();	
		}

		public void WaitThreads()
		{
			_waitNextTask?.Wait();
			_addNextTask?.Wait();
		}

		public void AddTask(Task task)
		{
			_waitBag.Add(task);
			_taskBag.Add(task);
		}

		private void StartNextTask()
		{
			Task? task;
			while(_waitNextTask.IsCompleted == false && _token.IsCancellationRequested == false)
			{
				bool res = _taskBag.TryTake(out task);
				if (res != false && task != null)
				{
					_taskSemaphore.WaitOne();
					task.Start();
				}
			}
		}

		private void WaitNextTasks()
		{
			Task? task;
			Debug.WriteLine("WaitTask");
			while (_waitBag.IsEmpty == false  && _token.IsCancellationRequested == false)
			{
				Debug.WriteLine("Bag not empty");
				bool res = _waitBag.TryTake(out task);
				if (res == true && task != null)
				{
					task.Wait();
					_taskSemaphore.Release();
				}
			}
		}
	}
}
