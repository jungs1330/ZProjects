using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using ZUtility.ViewModel;
using ZUtility.View;

namespace ZUtility.Common
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessageBox(string content,
        string title, MessageBoxButton buttons);
        void ShowDialog(ViewModelBase viewModel);
        string OpenFolderDialog();
    }

    public class DialogService : IDialogService
    {
        static IDialogService _instance;
        public static IDialogService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DialogService();
                }
                return _instance;
            }
            internal set
            {
                _instance = value;
            }
        }
        public MessageBoxResult ShowMessageBox(string content, string title, MessageBoxButton buttons)
        {
            return System.Windows.MessageBox.Show(content, title, buttons);
        }
        public void ShowDialog(ViewModelBase viewModel)
        {
            var dialog = new DialogView() { DataContext = viewModel };
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.ShowInTaskbar = false;
            dialog.ShowDialog();
        }

        public string OpenFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = "C:\\";

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                return dialog.SelectedPath;
            else
                return string.Empty;
        }
    }
}
