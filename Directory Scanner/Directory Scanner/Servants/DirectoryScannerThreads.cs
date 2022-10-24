using System.Collections.Concurrent;
using System.Diagnostics;

namespace Directory_Scanner.Servants
{
	public class DirectoryScannerThreads
	{
		private ConcurrentQueue<Task> _taskQueue;
		private ConcurrentQueue<Task> _waitQueue;
		private readonly Semaphore _taskSemaphore;

		public Task _addNextTask;
		public Task _waitNextTask;

		private readonly CancellationToken _token;
		public DirectoryScannerThreads(int maxThreadCount, CancellationTokenSource cts)
		{
			_taskQueue = new ConcurrentQueue<Task>();
			_waitQueue = new ConcurrentQueue<Task>();
			_taskSemaphore = new Semaphore(maxThreadCount, maxThreadCount);
			_token = cts.Token;			
		}

		public void StartThreads(CancellationToken _token)
		{
			// Start tasks to add and wait another tasks
			_waitNextTask = new Task( () => WaitNextTasks(), _token);
			_waitNextTask.Start(); // TODO: Add custom exception
			_addNextTask = new Task( () => StartNextTask(), _token);
			_addNextTask.Start();	
		}

		public void WaitThreads()
		{
			_waitNextTask?.Wait();
			_addNextTask?.Wait(); // TODO: Add custo, exception
		}

		public void AddTask(Task task)
		{
			_waitQueue.Enqueue(task);
			_taskQueue.Enqueue(task);
		}

		private void StartNextTask()
		{
			Task? task;
			while(_waitNextTask.IsCompleted == false && _token.IsCancellationRequested == false)
			{
				bool res = _taskQueue.TryDequeue(out task);
				if (res != false && task != null)
				{
					_taskSemaphore.WaitOne();
					task.Start(); // TODO: Add custom exception
				}
			}
		}

		private void WaitNextTasks()
		{
			Task? task;
			while (_waitQueue.IsEmpty == false  && _token.IsCancellationRequested == false)
			{
				bool res = _waitQueue.TryDequeue(out task);
				if (res == true && task != null)
				{
					task.Wait();
					_taskSemaphore.Release();
				}
			}
		}
	}
}
