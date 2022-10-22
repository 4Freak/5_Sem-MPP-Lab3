using Directory_Scanner.Servants;

namespace DerictoryScannerTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test1()
		{
			var dirScan = new DirectoryScanner("D:\\∆ругое");
			var tree = dirScan.StartScan(3);
			Assert.Pass();
		}
	}
}