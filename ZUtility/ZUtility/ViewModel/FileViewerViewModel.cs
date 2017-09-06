using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ZUtility.Properties;
using ZUtility.Command;
using ZUtility.Common;
using ZUtility.DataModel;

namespace ZUtility.ViewModel
{
    public class FileViewerViewModel : WorkspaceViewModel
    {
        #region fields
        private bool isGroupBy;
        private bool includeSubDir;
        private bool isRunning;
        private string filterFileName;
        private bool isFilterFileName;
        private bool isFilterSize;
        private string filterSizeBy;
        private int? filterSize;
        private FileActionTypeOptions actionType;
        private string fileCopyTo;
        private string fileMoveTo;
        private string fileRenameTo;
        private OrderByDirectionOptions orderByDirection;
        private OrderByOptions orderBy;
        private ObservableCollection<TreeViewItemViewModel> folders;
        private TreeViewItemViewModel selectedFolder;
        private ListCollectionView files;
        private ObservableCollection<string> fileSizeByOptions;
        private string duration;
        #endregion

        #region Constructor
        public FileViewerViewModel()
        {
            base.DisplayName = Resources.MainWindowViewModel_Command_File_Utility;
            isGroupBy = false;
            isFilterFileName = false;
            isFilterSize = false;
            actionType = FileActionTypeOptions.Nothing;
            orderByDirection = OrderByDirectionOptions.Ascending;
            orderBy = OrderByOptions.Filename;

            this.PreviewCommand = new RelayCommand(this.OnPreview, this.IsFolderSelected);
            this.RunCommand = new RelayCommand(this.OnRun, this.IsActionSelected);
            this.BrowseCopyToCommand = new RelayCommand(this.OnBrowseCopyTo, this.IsBrowseEnabled);
            this.BrowseMoveToCommand = new RelayCommand(this.OnBrowseMoveTo, this.IsBrowseEnabled);

            folders = new ObservableCollection<TreeViewItemViewModel>();

            foreach (DriveInfo di in DriveInfo.GetDrives())
                folders.Add(new TreeViewItemViewModel(di.RootDirectory, null, true));

            this.FileSizeByOptions = new ObservableCollection<string>() { "<", ">" };
        }
        #endregion

        #region Properties
        public ObservableCollection<TreeViewItemViewModel> Folders
        {
            get { return folders; }
        }

        public ListCollectionView Files
        {
            get { return files; }
            set 
            { 
                files = value;
                base.OnPropertyChanged("Files");
                base.OnPropertyChanged("FileCount");
            }
        }

        public int FileCount
        {
            get 
            {
                if (files == null)
                    return 0;
                else
                    return files.Count; 
            }
        }

        public string Duration
        {
            get { return duration; }
            set
            {
                if (value == duration)
                    return;

                duration = value;

                base.OnPropertyChanged("Duration");
            }
        }

        public ObservableCollection<string> FileSizeByOptions
        {
            get { return fileSizeByOptions; }
            set
            {
                fileSizeByOptions = value;
                base.OnPropertyChanged("FileSizeByOptions");
            }
        }

        public TreeViewItemViewModel SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (value == selectedFolder)
                    return;

                selectedFolder = value;

                base.OnPropertyChanged("SelectedFolder");
                this.PreviewCommand.RaiseCanExecuteChanged();
                this.RunCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IncludeSubDir
        {
            get { return includeSubDir; }
            set
            {
                if (value == includeSubDir)
                    return;

                includeSubDir = value;

                base.OnPropertyChanged("IncludeSubDir");
            }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (value == isRunning)
                    return;

                isRunning = value;

                base.OnPropertyChanged("IsRunning");
            }
        }

        public bool IsGroupBy
        {
            get { return isGroupBy; }
            set
            {
                if (value == isGroupBy)
                    return;

                isGroupBy = value;

                if (isGroupBy)
                    this.Files.GroupDescriptions.Add(new PropertyGroupDescription("DirectoryName"));
                else
                    this.Files.GroupDescriptions.Clear();

                base.OnPropertyChanged("IsGroupBy");
            }
        }
        public bool IsFilterFileName
        {
            get { return isFilterFileName; }
            set
            {
                if (value == isFilterFileName)
                    return;

                isFilterFileName = value;

                if (value == false)
                    this.FilterFileName = "";

                base.OnPropertyChanged("IsFilterFileName");
            }
        }
        public string FilterFileName
        {
            get { return filterFileName; }
            set
            {
                if (value == filterFileName)
                    return;

                filterFileName = value;

                if (filterFileName.Length > 0 && !IsFilterFileName)
                    this.IsFilterFileName = true;

                base.OnPropertyChanged("FilterFileName");
            }
        }

        public bool IsFilterSize
        {
            get { return isFilterSize; }
            set
            {
                if (value == isFilterSize)
                    return;

                isFilterSize = value;

                if (value == false)
                {
                    this.FilterSizeBy = "";
                    this.FilterSize = null;
                }

                base.OnPropertyChanged("IsFilterSize");
            }
        }
        public string FilterSizeBy
        {
            get { return filterSizeBy; }
            set
            {
                if (value == filterSizeBy)
                    return;

                filterSizeBy = value;

                if (filterSizeBy.Length > 0 && !IsFilterSize)
                    this.IsFilterSize = true;

                base.OnPropertyChanged("FilterSizeBy");
            }
        }
        public int? FilterSize
        {
            get { return filterSize; }
            set
            {
                if (value == filterSize)
                    return;

                filterSize = value;

                if (filterSize != null && !IsFilterSize)
                    this.IsFilterSize = true;

                base.OnPropertyChanged("FilterSize");
            }
        }
        public FileActionTypeOptions ActionType
        {
            get { return actionType; }
            set
            {
                if (value == actionType)
                    return;

                actionType = value;

                if (actionType == FileActionTypeOptions.Nothing)
                {
                    this.FileCopyTo = "";
                    this.FileMoveTo = "";
                    this.FileRenameTo = "";
                }

                base.OnPropertyChanged("ActionType");
                this.PreviewCommand.RaiseCanExecuteChanged();
                this.RunCommand.RaiseCanExecuteChanged();
            }
        }

        public string FileCopyTo
        {
            get { return fileCopyTo; }
            set
            {
                if (value == fileCopyTo)
                    return;

                fileCopyTo = value;

                if (fileCopyTo.Length > 0 && ActionType != FileActionTypeOptions.CopyTo)
                    this.ActionType = FileActionTypeOptions.CopyTo;

                base.OnPropertyChanged("FileCopyTo");
            }
        }

        public string FileMoveTo
        {
            get { return fileMoveTo; }
            set
            {
                if (value == fileMoveTo)
                    return;

                fileMoveTo = value;

                if (fileMoveTo.Length > 0 && ActionType != FileActionTypeOptions.MoveTo)
                    this.ActionType = FileActionTypeOptions.MoveTo;

                base.OnPropertyChanged("FileMoveTo");
            }
        }

        public string FileRenameTo
        {
            get { return fileRenameTo; }
            set
            {
                if (value == fileRenameTo)
                    return;

                fileRenameTo = value;

                if (fileRenameTo.Length > 0 && ActionType != FileActionTypeOptions.RenameTo)
                    this.ActionType = FileActionTypeOptions.RenameTo;

                base.OnPropertyChanged("FileRenameTo");
            }
        }

        public OrderByDirectionOptions OrderByDirection
        {
            get { return orderByDirection; }
            set
            {
                if (value == orderByDirection)
                    return;

                orderByDirection = value;

                base.OnPropertyChanged("OrderByDirection");
            }
        }

        public OrderByOptions OrderBy
        {
            get { return orderBy; }
            set
            {
                if (value == orderBy)
                    return;

                orderBy = value;

                base.OnPropertyChanged("OrderBy");
            }
        }

        public RelayCommand RunCommand
        {
            get;
            private set;
        }

        public RelayCommand PreviewCommand
        {
            get;
            private set;
        }

        public RelayCommand BrowseMoveToCommand
        {
            get;
            private set;
        }

        public RelayCommand BrowseCopyToCommand
        {
            get;
            private set;
        }
        #endregion

        private void OnPreview()
        {
            Run(true);
        }

        private void OnRun()
        {
            Run(false);
        }

        private void Run(bool isPreview)
        {
            Task task1 = new Task(() => 
                {
                    this.IsRunning = true;
                    App.Current.Dispatcher.Invoke(() => {
                        this.PreviewCommand.RaiseCanExecuteChanged();
                        this.RunCommand.RaiseCanExecuteChanged();
                    });
                    
                });

            Stopwatch stopWatch = new Stopwatch();
            long elapsedTime1 = 0;
            long elapsedTime2 = 0;
            long elapsedTime3 = 0;
            IEnumerable<FileDirectory> list = null;

            Task continuationTask1 = task1.ContinueWith(task  =>
            {
                stopWatch.Start();
                list = FilterFiles();
                stopWatch.Stop();
                elapsedTime1 = stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            });

            Task continuationTask2 = continuationTask1.ContinueWith(task =>
            {
                stopWatch.Start();
                ModifyFile(list, isPreview);
                stopWatch.Stop();
                elapsedTime2 = stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            });

            Task continuationTask3 = continuationTask2.ContinueWith(task =>
            {
                stopWatch.Start();
                DisplayFiles(list);
                elapsedTime3 = stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            });

            Task continuationTask4 = continuationTask3.ContinueWith(task =>
            {
                Duration = string.Format("Filter files [{0}] Modify files [{1}] Display files [{2}]", elapsedTime1, elapsedTime2, elapsedTime3);
            });

            Task continuationTask5 = continuationTask4.ContinueWith(task =>
            {
                this.IsRunning = false;
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.PreviewCommand.RaiseCanExecuteChanged();
                    this.RunCommand.RaiseCanExecuteChanged();
                });
            });

            task1.Start();
        }

        private void ModifyFile(IEnumerable<FileDirectory> list, bool bPreview)
        {
            if (this.ActionType != FileActionTypeOptions.Nothing)
            {
                switch (this.ActionType)
                {
                    case FileActionTypeOptions.Delete:
                        Parallel.ForEach(list, file => {
                            file.NewName = "Deleted";

                            if (!bPreview)
                                File.Delete(file.DirectoryName + "\\" + file.Name);
                        });
                        break;
                    case FileActionTypeOptions.MoveTo:
                        Parallel.ForEach(list, file =>
                        {
                            file.NewName = this.FileMoveTo + "\\" + file.Name;

                            if (File.Exists(this.FileMoveTo + "\\" + file.Name))
                                File.Delete(this.FileMoveTo + "\\" + file.Name);

                            if (!bPreview)
                                File.Move(file.DirectoryName + "\\" + file.Name, this.FileMoveTo + "\\" + file.Name);
                        });
                        break;
                    case FileActionTypeOptions.CopyTo:
                        Parallel.ForEach(list, file =>
                        {
                            file.NewName = this.FileCopyTo + "\\" + file.Name;

                            if (!bPreview)
                                File.Copy(file.DirectoryName + "\\" + file.Name, this.FileCopyTo + "\\" + file.Name, true);
                        });
                        break;
                    case FileActionTypeOptions.RenameTo:
                        Dictionary<string, int> fileNameCounter = new Dictionary<string, int>();
                        Parallel.ForEach(list, file =>
                        {
                            file.NewName = this.FileRenameTo
                                    .Replace("[YYYY]", file.LastWriteTime.Year.ToString())
                                    .Replace("[MM]", file.LastWriteTime.Month.ToString("D2"))
                                    .Replace("[DD]", file.LastWriteTime.Day.ToString("D2")) + file.Extension;

                            int count = 1;

                            lock (fileNameCounter)
                            {
                                if (fileNameCounter.ContainsKey(file.NewName))
                                {
                                    count = fileNameCounter[file.NewName] + 1;
                                    fileNameCounter[file.NewName] = count;
                                }
                                else
                                {
                                    fileNameCounter.Add(file.NewName, count);
                                }
                            }

                            file.NewName = file.NewName
                                .Replace("[XXXXX]", count.ToString("D5"))
                                .Replace("[XXXX]", count.ToString("D4"))
                                .Replace("[XXX]", count.ToString("D3"))
                                .Replace("[XX]", count.ToString("D2"))
                                .Replace("[X]", count.ToString());

                            if (!bPreview)
                                File.Move(file.DirectoryName + "\\" + file.Name, file.DirectoryName + "\\" + file.NewName);
                        });
                        break;
                }
            }
        }

        private void DisplayFiles(IEnumerable<FileDirectory> list)
        {
            ListCollectionView collection = new ListCollectionView(list.ToList<FileDirectory>());
            if (IsGroupBy)
                collection.GroupDescriptions.Add(new PropertyGroupDescription("DirectoryName"));
            else
                collection.GroupDescriptions.Clear();
            this.Files = collection;
        }

        private IEnumerable<FileDirectory> FilterFiles()
        {
            List<FileDirectory> filesList = new List<FileDirectory>();
            DirectoryInfo directoryInfo = new DirectoryInfo(this.SelectedFolder.FullPath);

            GetFiles(directoryInfo, filesList);
            
            IEnumerable<FileDirectory> list = filesList.AsEnumerable();

            if (this.IsFilterFileName)
                list = list.Where(f => f.Name.Contains(this.FilterFileName));

            if (this.IsFilterSize)
            {
                if (FilterSizeBy == ">")
                    list = list.Where(f => (f.Length / 1048576) > this.FilterSize);
                else
                    list = list.Where(f => (f.Length / 1048576) < this.FilterSize);
            }

            if (this.OrderBy == OrderByOptions.Filename && this.OrderByDirection == OrderByDirectionOptions.Ascending)
                list = list.OrderBy(f => f.Name);
            else if (this.OrderBy == OrderByOptions.Filename && this.OrderByDirection == OrderByDirectionOptions.Descending)
                list = list.OrderByDescending(f => f.Name);
            else if (this.OrderBy == OrderByOptions.CreateDate && this.OrderByDirection == OrderByDirectionOptions.Ascending)
                list = list.OrderBy(f => f.CreationTime);
            else if (this.OrderBy == OrderByOptions.CreateDate && this.OrderByDirection == OrderByDirectionOptions.Descending)
                list = list.OrderByDescending(f => f.CreationTime);
            else if (this.OrderBy == OrderByOptions.ModifiedDate && this.OrderByDirection == OrderByDirectionOptions.Ascending)
                list = list.OrderBy(f => f.LastWriteTime);
            else if (this.OrderBy == OrderByOptions.ModifiedDate && this.OrderByDirection == OrderByDirectionOptions.Descending)
                list = list.OrderByDescending(f => f.LastWriteTime);
            else if (this.OrderBy == OrderByOptions.Size && this.OrderByDirection == OrderByDirectionOptions.Ascending)
                list = list.OrderBy(f => f.Length);
            else if (this.OrderBy == OrderByOptions.Size && this.OrderByDirection == OrderByDirectionOptions.Descending)
                list = list.OrderByDescending(f => f.Length);
            return list;
        }

        private void GetFiles(DirectoryInfo directory, List<FileDirectory> filesList)
        {
           
            if (this.IncludeSubDir)
            {
                DirectoryInfo[] directories = directory.GetDirectories();
                
                Parallel.ForEach(directories, item => { GetFiles(item, filesList);  });

                //foreach (DirectoryInfo dir in directories)
                //    GetFiles(dir, filesList);
            }

            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
                filesList.Add(new FileDirectory(file));
        }

        private void OnBrowseCopyTo()
        {
            string folder = DialogService.Instance.OpenFolderDialog();
            this.FileCopyTo = folder;
        }

        private void OnBrowseMoveTo()
        {
            string folder = DialogService.Instance.OpenFolderDialog();
            this.FileMoveTo = folder;
        }

        private bool IsBrowseEnabled()
        { return true; }

        private bool IsFolderSelected()
        {
            if (SelectedFolder != null && !IsRunning)
                return true;
            else
                return false;
        }

        private bool IsActionSelected()
        {
            if (SelectedFolder != null
                && this.ActionType != FileActionTypeOptions.Nothing
                && !IsRunning)
                return true;
            else
                return false;
        }
    }

    public enum FileActionTypeOptions
    {
        Nothing,
        Delete,
        CopyTo,
        MoveTo,
        RenameTo
    }

    public enum OrderByDirectionOptions
    {
        Ascending,
        Descending
    }

    public enum OrderByOptions
    {
        Filename,
        Size,
        CreateDate,
        ModifiedDate
    }
}
