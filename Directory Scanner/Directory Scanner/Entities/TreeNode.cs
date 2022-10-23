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
		public long AbsoluteSize {get; private set;} = 0;
		public float RelativeSizeN {get; private set;} = 100;
		public ConcurrentBag<TreeNode>? InnerNodes {get; set;} = null;
	
		public TreeNode(string path, string name, NodeType nodeType)
		{
			Path = path;
			Name = name;
			NodeType = nodeType;
			AbsoluteSize = 0;
		} 

		public TreeNode(string path, string name, NodeType nodeType, long absoluteSize) : this(path, name, nodeType)
		{
			AbsoluteSize = absoluteSize;
		}

		
		public void CalculateParameters()
		{
			this.GetAbsoluteSize();
			this.GetRelativeSize();
		}
		private long GetAbsoluteSize()
		{
			if (InnerNodes != null && NodeType == NodeType.Dir)
			{
				foreach (var node in InnerNodes)
				{
					AbsoluteSize += node.GetAbsoluteSize();
				}
			}
			return AbsoluteSize;
		}

		private float GetRelativeSize()
		{
			if (InnerNodes != null && NodeType == NodeType.Dir)
			{
				foreach (var node in InnerNodes)
				{
					node.RelativeSizeN = (float)node.AbsoluteSize / (float)this.AbsoluteSize; 
					node.GetRelativeSize();
				}
			}
			return this.RelativeSizeN;
		}


	}
}
