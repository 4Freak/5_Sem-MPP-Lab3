using Directory_Scanner.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace DirectoryScannerApp.Model
{
	internal static class TreeMapper
	{
		public static ModelDirectoryTree ToViewTree(DirectoryTree tree)
		{
			var root = TreeMapper.ToViewNode(tree.Root);
			root.RelativeSize = root.RelativeSize/100;
			return new ModelDirectoryTree(root);
		}

		private static ModelTreeNode ToViewNode(TreeNode treeNode)
		{
			var node = new ModelTreeNode();
			node.NodeType = treeNode.NodeType;
			node.Path = treeNode.Path;
			node.Name = treeNode.Name;
			if (node.Name == "")
			{
				node.Name = node.Path;
			}
			node.AbsoluteSize = treeNode.AbsoluteSize;
			node.RelativeSize = treeNode.RelativeSizeN * 100;
			node.ImagePath = node.GetImgagePath();
			
			if (treeNode.InnerNodes != null)
			{
				//node.InnerNodes = new ObservableCollection<ModelTreeNode>();
				var innerNodes = new ObservableCollection<ModelTreeNode>();
				foreach(var innerNode in treeNode.InnerNodes)
				{
					var newInnerNode = TreeMapper.ToViewNode(innerNode);
					//node.InnerNodes.Add(newInnerNode);
					innerNodes.Add(newInnerNode);
				}
				node.InnerNodes = new ObservableCollection<ModelTreeNode>(innerNodes.OrderBy(node => node.Name));
			}
			return node;
		}
	}
}
