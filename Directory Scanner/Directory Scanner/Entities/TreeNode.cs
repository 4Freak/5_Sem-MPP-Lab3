using System.Collections.Concurrent;

namespace Directory_Scanner.Entities
{
	public class TreeNode
	{
		public string Name {get; private set;}

		public string Path {get; private set;}
		public long Size {get; private set;} = 0;
		public ConcurrentBag<TreeNode>? InnerNodes {get; private set;} = null;
	
		public TreeNode(string path, string name)
		{
			Path = path;
			Name = name;
		} 

		public TreeNode(string path, string name, long size) : this(path, name)
		{
			Size = size;
		}
	}
}
