namespace Directory_Scanner.Entities
{
	public class DirectoryTree
	{
		public TreeNode? Root {get; private set;} = null;

		public DirectoryTree(TreeNode rootNode)
		{
			
			Root = rootNode;
		}

	}
}
