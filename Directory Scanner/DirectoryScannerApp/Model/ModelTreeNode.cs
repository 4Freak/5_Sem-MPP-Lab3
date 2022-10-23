using Directory_Scanner.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DirectoryScannerApp.Model
{
	public class ModelTreeNode
	{
		public NodeType NodeType { get; set; }
		public string Name {get; set;}
		public string Path {get; set;}
		public long AbsoluteSize {get; set;} = 0;
		public float RelativeSize {get; set;} = 100;
		public string ImagePath {get; set;}
		public ObservableCollection<ModelTreeNode>? InnerNodes {get; set;} = null;

		public string GetImgagePath()
		{
			return _imgPath[this.NodeType];
		}

		private static Dictionary<NodeType, string> _imgPath = new Dictionary<NodeType, string>()
		{
			{NodeType.Dir, "Resources/Dir.png"},
			{NodeType.File, "Resources/File.png"} 
		};
	}
}
