using System.Collections.Concurrent;

namespace Directory_Scanner.Entities
{

	public enum NodeType
	{
		Dir,
		File
	}
	public class TreeNode
	{
		public NodeType NodeType { get; set; }
		public string Name {get; private set;}
		public string Path {get; private set;}
		public long Size {get; private set;} = 0;
		public ConcurrentBag<TreeNode>? InnerNodes {get; set;} = null;
	
		public TreeNode(string path, string name, NodeType nodeType)
		{
			Path = path;
			Name = name;
			NodeType = nodeType;
		} 

		public TreeNode(string path, string name, NodeType nodeType, long size) : this(path, name, nodeType)
		{
			Size = size;
		}
	}
}
