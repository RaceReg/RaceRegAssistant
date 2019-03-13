using LIFFileViewer.Data;
using LIFFileViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LIFFileViewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
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
                selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
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
                currentDirectory = value;
                OnPropertyChanged(nameof(CurrentDirectory));
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
                filesInCurrentDirectory = value;
                OnPropertyChanged(nameof(FilesInCurrentDirectory));
            }
        }

        private readonly IDataService data;
        private string LIFFilePath;

        private LIF _lif;
        public LIF lif
        {
            get
            {
                return _lif;
            }
            set
            {
                _lif = value;
                OnPropertyChanged(nameof(lif));
            }
        }

        //Default Constructor
        public MainWindowViewModel() : this(new DefaultDataService(), true) { }

        public MainWindowViewModel(IDataService data, bool shouldReturnDirectoryFiles)
        {
            this.data = data;
            this.shouldReturnDirectoryFiles = shouldReturnDirectoryFiles;
            FilesInCurrentDirectory = new ObservableCollection<string>();
            CurrentDirectory = "";
            SelectedFile = "";
            LoadLIFDirectory.RaiseCanExecuteChanged();
        }

        private RelayCommand findAndLoadLIFFile;
        public RelayCommand FindAndLoadLIFFile => findAndLoadLIFFile ?? (findAndLoadLIFFile = new SimpleCommand(
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
                    lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

                    IsBusy = false;
                }
            }
            ));

        private SimpleCommand findLIFFile;
        public SimpleCommand FindLIFFile => findLIFFile ?? (findLIFFile = new SimpleCommand(
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

        private SimpleCommand loadLIFDirectory;
        public SimpleCommand LoadLIFDirectory => loadLIFDirectory ?? (loadLIFDirectory = new SimpleCommand(
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

        private SimpleCommand loadLIFFile;
        public SimpleCommand LoadLIFFile => loadLIFFile ?? (loadLIFFile = new SimpleCommand(
            () => !IsBusy, //sees if we can read in a LIF file
            async () =>
            {
                IsBusy = true;
                LIFFilePath = Path.Combine(CurrentDirectory, SelectedFile);
                lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

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
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                LoadLIFFile.RaiseCanExecuteChanged();
                FindLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
