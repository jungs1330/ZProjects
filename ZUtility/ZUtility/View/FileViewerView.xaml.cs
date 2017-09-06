using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.IO;
using System.Collections.ObjectModel;
using ZUtility.ViewModel;

namespace ZUtility.View
{
    /// <summary>
    /// Interaction logic for FileViewerView.xaml
    /// </summary>
    public partial class FileViewerView : UserControl
    {
        public FileViewerView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FileViewerViewModel vm = this.DataContext as FileViewerViewModel;
            if (e.NewValue != null)
                vm.SelectedFolder = e.NewValue as TreeViewItemViewModel;
        }
    }
}
