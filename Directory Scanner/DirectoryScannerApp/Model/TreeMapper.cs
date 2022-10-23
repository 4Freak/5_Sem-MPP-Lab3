using Directory_Scanner.Entities;
using System.Collections.ObjectModel;

namespace DirectoryScannerApp.Model
{
	internal static class TreeMapper
	{
		public static ModelDirectoryTree ToViewTree(DirectoryTree tree)
		{
			var root = TreeMapper.ToViewNode(tree.Root);
			return new ModelDirectoryTree(root);
		}

		private static ModelTreeNode ToViewNode(TreeNode treeNode)
		{
			var node = new ModelTreeNode();
			node.NodeType = treeNode.NodeType;
			node.Path = treeNode.Path;
			node.Name = treeNode.Name;
			node.AbsoluteSize = treeNode.AbsoluteSize;
			node.RelativeSize = treeNode.RelativeSizeN * 100;
			node.ImagePath = node.GetImgagePath();
			
			if (treeNode.InnerNodes != null)
			{
				node.InnerNodes = new ObservableCollection<ModelTreeNode>();
				foreach(var innerNode in treeNode.InnerNodes)
				{
					var newInnerNode = TreeMapper.ToViewNode(innerNode);
					node.InnerNodes.Add(newInnerNode);
				}
			}
			return node;
		}
	}
}
