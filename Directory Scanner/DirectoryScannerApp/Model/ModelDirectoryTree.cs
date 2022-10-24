using System.Collections.ObjectModel;

namespace DirectoryScannerApp.Model
{
	public class ModelDirectoryTree
	{
		public ModelTreeNode? Root {get; set;} = null;
		public ObservableCollection<ModelTreeNode> InnerNodes {get; set;} = null;

		public ModelDirectoryTree(ModelTreeNode modelTreeNode)
		{
			Root = modelTreeNode;
			InnerNodes = new ObservableCollection<ModelTreeNode>();
			InnerNodes.Add(modelTreeNode);
		}
	}
}
