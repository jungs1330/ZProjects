using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace ZUtility.ViewModel
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        #region Data

        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        private readonly ObservableCollection<TreeViewItemViewModel> _children;
        private readonly TreeViewItemViewModel _parent;
        private readonly DirectoryInfo _directoryInfo;

        private bool _isExpanded;
        private bool _isSelected;

        #endregion // Data

        #region Constructors

        public TreeViewItemViewModel(DirectoryInfo dirInfo, TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            _parent = parent;
            _directoryInfo = dirInfo;

            _children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
                _children.Add(DummyChild);
        }

        // This is used to create the DummyChild instance.
        private TreeViewItemViewModel()
        {
        }

        #endregion // Constructors

        #region Presentation Members

        public string Name
        {
            get 
            {
                string name = _directoryInfo.ToString();
                int fileCount = 0;
                int directoryCount = 0;

                try
                {
                    fileCount = _directoryInfo.GetFiles().Length;
                    directoryCount = _directoryInfo.GetDirectories().Length;
                }
                catch (Exception e) { }

                if (directoryCount == 0)
                    this.Children.Remove(DummyChild);

                return name + string.Format("\t[directories:{0} files:{1}]", directoryCount, fileCount);
            }
        }

        public string FullPath
        {
            get { return _directoryInfo.FullName; }
        }

        #region Children

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return _children; }
        }

        #endregion // Children

        #region HasLoadedChildren

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        #endregion // HasLoadedChildren

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region LoadChildren

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        public void LoadChildren()
        {
            try
            {
                DirectoryInfo[] directories = this._directoryInfo.GetDirectories();
                foreach(DirectoryInfo di in this._directoryInfo.GetDirectories())
                    Children.Add(new TreeViewItemViewModel(di, this, true));
            }
            catch (Exception e) { }
        }

        #endregion // LoadChildren

        #region Parent

        public TreeViewItemViewModel Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members
    }
}
