using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Directory_Scanner.Servants;
using Directory_Scanner.Entities;
using Directory_Scanner.Exceptions;
using Microsoft.Win32;
using System.Windows;

namespace DirectoryScannerApp.VievModel
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		private DirectoryScanner _directoryScanner;
		private string _directoryName;

		public ViewModel() {}

		private RelayCommand _setDirectory;
		public RelayCommand SetDirectory
		{
			get
			{
				if (_setDirectory == null)
				{
					_setDirectory = new RelayCommand(obj =>
					{
						var openFileDialog = new OpenFileDialog();
						if (openFileDialog.ShowDialog() == true)
						{
							_directoryName = openFileDialog.FileName;		
						}
					});
				}
				return _setDirectory;
			}
		}

		private RelayCommand _startSearch;
		public RelayCommand StartSearch
		{
			get
			{
				if (_startSearch == null)
				{
					_startSearch = new RelayCommand(obj => 
					{
						_directoryScanner =  new DirectoryScanner(_directoryName);
						Tree = _directoryScanner.StartScan(ThreadCount);
					});
				}
				return _startSearch;
			}
		}

		private RelayCommand _stopSearch;
		public RelayCommand StopSearch
		{
			get
			{
				if (_stopSearch == null)
				{
					_stopSearch = new RelayCommand(ong =>
					{
						if (IsScannerWorking)
						{
							Tree = _directoryScanner.StopScan();
						}
					});
				}
				return _stopSearch;
			}
		}

		private int _threadCount= 1;
		public int ThreadCount
		{
			get{ return _threadCount;}
			set
			{
				_threadCount = value;
				OnPropertyChanged("ThreadCount");
			}
		}

		private volatile bool _isScannerWorking;
		public bool IsScannerWorking
		{
			get { return _isScannerWorking;}
			set 
			{
				_isScannerWorking = value;
				OnPropertyChanged("IsScannerWorking");
			}
		}

		private DirectoryTree _tree;
		public DirectoryTree Tree
		{
			get {return _tree;}
			private set
			{
				_tree = value;
				OnPropertyChanged("Tree");
			}
		}
	}
}