using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LIFFileViewerDesktop.Data;
using RaceRegAssistant;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LIFFileViewerDesktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        ///// <summary>
        ///// Initializes a new instance of the MainViewModel class.
        ///// </summary>
        //public MainViewModel()
        //{
        //    ////if (IsInDesignMode)
        //    ////{
        //    ////    // Code runs in Blend --> create design time data.
        //    ////}
        //    ////else
        //    ////{
        //    ////    // Code runs "for real"
        //    ////}
        //    ///
        //}

        //Default Constructor
        public MainViewModel() : this(new DefaultDataService(), true) { }

        public MainViewModel(IDataService data, bool shouldReturnDirectoryFiles)
        {
            this.data = data;
            this.shouldReturnDirectoryFiles = shouldReturnDirectoryFiles;
            FilesInCurrentDirectory = new ObservableCollection<string>();
            CurrentDirectory = "";
            SelectedFile = "";
            LoadLIFDirectory.RaiseCanExecuteChanged();
        }

        private bool shouldReturnDirectoryFiles;

        private string selectedFile;
        public string SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                Set(ref selectedFile, value);
            }
        }

        private string currentDirectory;
        public string CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
            set
            {
                Set(ref currentDirectory, value);
            }
        }

        private ObservableCollection<string> filesInCurrentDirectory;
        public ObservableCollection<string> FilesInCurrentDirectory
        {
            get
            {
                return filesInCurrentDirectory;
            }
            set
            {
                Set(ref filesInCurrentDirectory, value);
            }
        }

        private readonly IDataService data;
        private string LIFFilePath;

        private LIF _lif;
        public LIF Lif
        {
            get
            {
                return _lif;
            }
            set
            {
                Set(ref _lif, value);
            }
        }

        private RelayCommand findAndLoadLIFFile;
        public RelayCommand FindAndLoadLIFFile => findAndLoadLIFFile ?? (findAndLoadLIFFile = new RelayCommand(
            () => !IsBusy,
            async () =>
            {
                LIFFilePath = await data.FindFileAsync();
                CurrentDirectory = Path.GetDirectoryName(LIFFilePath);

                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
                if (shouldReturnDirectoryFiles)
                {
                    LoadDirectoryContents();
                }

                if (!IsBusy && data.FileExists(LIFFilePath))
                {
                    IsBusy = true;
                    Lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

                    IsBusy = false;
                }
            }
            ));

        private RelayCommand findLIFFile;
        public RelayCommand FindLIFFile => findLIFFile ?? (findLIFFile = new RelayCommand(
            () => !IsBusy,
            async () =>
            {
                LIFFilePath = await data.FindFileAsync();
                CurrentDirectory = Path.GetDirectoryName(LIFFilePath);
                if (shouldReturnDirectoryFiles)
                {
                    LoadDirectoryContents();
                }
                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
            ));

        private void LoadDirectoryContents()
        {
            var ext = new List<string> { ".lif", ".LIF" };
            try
            {
                var files = Directory.GetFiles(CurrentDirectory, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s))); //uses LINQ to get the results we want

                FilesInCurrentDirectory.Clear();
                foreach (string file in files)
                {
                    FilesInCurrentDirectory.Add(Path.GetFileName(file));

                    if (string.Equals(file, LIFFilePath))
                    {
                        SelectedFile = FilesInCurrentDirectory.ElementAt<string>(FilesInCurrentDirectory.Count - 1);
                    }
                }
            }
            catch (System.ArgumentNullException e)
            {
                //We are okay if the directory is null, we simply will do nothing. In the future, we could change the view to say "No directory."
            }


        }

        private RelayCommand loadLIFDirectory;
        public RelayCommand LoadLIFDirectory => loadLIFDirectory ?? (loadLIFDirectory = new RelayCommand(
            () => !IsBusy,
            async () =>
            {
                CurrentDirectory = await data.FindDirectoryAsync();
                if (shouldReturnDirectoryFiles)
                {
                    LoadDirectoryContents();
                }
                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
            ));

        private RelayCommand loadLIFFile;
        public RelayCommand LoadLIFFile => loadLIFFile ?? (loadLIFFile = new RelayCommand(
            () => !IsBusy, //sees if we can read in a LIF file
            async () =>
            {
                IsBusy = true;
                LIFFilePath = Path.Combine(CurrentDirectory, SelectedFile);
                Lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

                IsBusy = false;
            }
            ));

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                Set(ref isBusy, value);
                LoadLIFFile.RaiseCanExecuteChanged();
                FindLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
        }
    }
}