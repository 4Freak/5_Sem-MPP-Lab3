using Directory_Scanner.Servants;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;

namespace DerictoryScannerTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void CorrectName()
		{
			var dirScan = new DirectoryScanner(".");
			try {
				var tree = dirScan.StartScan(4);
				Assert.Pass();
			}
			catch (DirectoryNotFoundException e) {
				Assert.Fail(e.Message);
			}
		}

		[Test]
		public void IncorrectName()
		{
			var dirScan = new DirectoryScanner("#");
			try {
				var tree = dirScan.StartScan(4);
				Assert.Fail();
			}
			catch (DirectoryNotFoundException e) {
				Assert.Pass(e.Message);
			}
		}

		[Test]
		public void Directories()
		{
			string dirName = "test";
			string subDir = "subdir";
			var files = new List<string>()
			{
				"file0.txt",
				"file1.txt",
				"file2.txt",
				"file3.txt"
			};

			var dirScan = new DirectoryScanner(dirName);

			if (Directory.Exists(dirName) == false) 
			{
				Directory.CreateDirectory(dirName);
			}
			
			foreach (var file in files) 
			{
				var fs = new FileStream(dirName + "/" + file, FileMode.Create);
				fs.WriteByte(0);
				fs.Close();
			}

			if (Directory.Exists (dirName + "/" + subDir) == false)
			{
				Directory.CreateDirectory (dirName + "/" + subDir);
			}

			foreach (var file in files)
			{
				var fs = new FileStream(dirName + "/" + subDir + "/" + file, FileMode.Create);
				fs.WriteByte(0);
				fs.Close();
			}

			var tree = dirScan.StartScan(4);
			Assert.Multiple( () =>
			{
				Assert.That(tree.Root.Name, Is.EqualTo(dirName));
				
				var innerNodes = tree.Root.InnerNodes.ToArray();
				var innerDir = innerNodes[0];
				Assert.That(innerNodes.Length, Is.EqualTo(5));
				for (int i = 0; i < innerNodes.Length; i++)
				{
					if (files.Contains(innerNodes[i].Name) )
					{
						if (innerNodes[i].AbsoluteSize != 1)
						{
							Assert.Fail();
						}
					}
					else
					{
						if (innerNodes[i].Name == subDir)
						{
							innerDir = innerNodes[i];
						}
						else
						{
							Assert.Fail();
						}
					}
				}
				innerNodes = innerDir.InnerNodes.ToArray();
				Assert.That(innerNodes.Length, Is.EqualTo(4));
				for (int i = 0; i < innerNodes.Length; i++)
				{
					Assert.Contains(innerNodes[i].Name, files);
					Assert.AreEqual(innerNodes[i].AbsoluteSize, 1);
				}
			});

			if (Directory.Exists(dirName)) 
			{
				Directory.Delete(dirName, true);
			}
		}

		[Test]
		public void Cancel()
		{
			string dirName = "D:\\";
			var dirScanner = new DirectoryScanner(dirName);
			try
			{
				var scan = new Task(() => dirScanner.StartScan(4));
				Thread.Sleep(1000);
				dirScanner.StopScan();
			}
			catch (Exception e)
			{
				Assert.Fail(e.Message);
			}

		}
	}
}