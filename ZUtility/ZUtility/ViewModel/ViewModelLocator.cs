using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZUtility.ViewModel
{
    public class ViewModelLocator
    {
        private static MainWindowViewModel _mainWindowViewModel;

        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get
            {
                if (_mainWindowViewModel == null)
                    _mainWindowViewModel = new MainWindowViewModel();

                return _mainWindowViewModel;
            }
        }

        public static FileViewerViewModel GetFileViewerViewModel()
        {
            return new FileViewerViewModel();
        }

        public static DataViewerViewModel GetDataViewerViewModel()
        {
            return new DataViewerViewModel();
        }

        public static ThreadingViewModel GetThreadingViewModel()
        {
            return new ThreadingViewModel();
        }

        public static AboutViewModel GetAboutViewModel()
        {
            return new AboutViewModel();
        }
    }
}
