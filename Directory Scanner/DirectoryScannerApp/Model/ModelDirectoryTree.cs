using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScannerApp.Model
{
	public class ModelDirectoryTree
	{
		public ModelTreeNode? Root {get; set;} = null;

		public ModelDirectoryTree(ModelTreeNode modelTreeNode)
		{
			Root = modelTreeNode;
		}
	}
}
