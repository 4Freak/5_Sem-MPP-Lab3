using Directory_Scanner.Entities;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace Directory_Scanner.Servants
{
	public class DirectoryScanner
	{
		public readonly string DirectoryPath;
		public int ThreadCount;

		private readonly CancellationTokenSource _cts;
		private DirectoryScannerThreads _dst;

		private TreeNode _rootNode;

		public DirectoryScanner(string directoryPath)
		{
			DirectoryPath = directoryPath;
			_cts = new CancellationTokenSource();
		}
		
		public DirectoryTree StartScan(int threadCount)
		{	
			ThreadCount = threadCount;
			if (Directory.Exists(DirectoryPath))
			{
				var fileInfo = new FileInfo(DirectoryPath);
				_rootNode = new TreeNode(fileInfo.FullName, fileInfo.Name, NodeType.Dir);
				var newTask = new Task(() => ScanDirectory(_rootNode, DirectoryPath), _cts.Token);
				_dst = new DirectoryScannerThreads(threadCount, _cts);
				_dst.AddTask(newTask);
			}
			else
			{
				throw new DirectoryNotFoundException("Directory not found");
			}
			
			_dst.StartThreads(_cts.Token);
			_dst.WaitThreads();
			_rootNode.CalculateParameters();
			return new DirectoryTree(_rootNode);	
		}

		public DirectoryTree StopScan()
		{
			if (!_cts.IsCancellationRequested)
			{
				_cts.Cancel();
				if (_rootNode != null)
				{
					_rootNode.CalculateParameters();
				}
				else
				{
					var fileInfo = new FileInfo(DirectoryPath);
					_rootNode = _rootNode = new TreeNode(fileInfo.FullName, fileInfo.Name, NodeType.Dir);
				}
			}
			return new DirectoryTree(_rootNode);
		}

		private TreeNode ScanDirectory(TreeNode treeNode, string dirName)
		{
			if (treeNode.InnerNodes == null)
			{
				treeNode.InnerNodes = new ConcurrentBag<TreeNode>();
			}
			if (Directory.Exists(dirName))
			{	
				// Get files and save it to inner nodes
				try
				{
					var files = Directory.GetFiles(dirName);	
					foreach (var file in files)
					{
						var fileInfo = new FileInfo(file);
						if (fileInfo.LinkTarget == null)
						{
							var newNode = new TreeNode(fileInfo.FullName, fileInfo.Name, NodeType.File, fileInfo.Length);
							treeNode.InnerNodes.Add(newNode);
						}
					}				
					var directories = Directory.GetDirectories(dirName);
					foreach(var directory in directories)
					{
						var directoryInfo = new DirectoryInfo(directory);
						if (Directory.Exists(directoryInfo.FullName))
						{
							var newNode = new TreeNode(directoryInfo.FullName, directoryInfo.Name, NodeType.Dir);
							treeNode.InnerNodes.Add(newNode);
							var newTask = new Task(() => ScanDirectory(newNode, directoryInfo.FullName), _cts.Token); 
							_dst.AddTask(newTask);						
						}
					}
				}
				catch(UnauthorizedAccessException e)
				{
					throw;
				}

			}
			else
			{
				throw new DirectoryNotFoundException("Directory not found");
			}
			return treeNode;
		}

	}
}
