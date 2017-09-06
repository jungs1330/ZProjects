using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Globalization;
using System.Windows.Markup;
using ZUtility.View;
using ZUtility.ViewModel;

/*
 * implement mvvm
 * implement prism
 * implement multiple exe
 * implement multiple exe communication through wcf/socket
 * implement entity framework
 * implement unit tests
*/
namespace ZUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool unsavedData = false;

        static App()
        {
            // This code is used to test the app when using other cultures.
            //
            //System.Threading.Thread.CurrentThread.CurrentCulture =
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //        new System.Globalization.CultureInfo("it-IT");


            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            //
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public bool UnsavedData
        {
            get { return unsavedData; }
            set { unsavedData = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UnsavedData = true;

            MainWindow window = new MainWindow();

            MainWindowViewModel viewModel = ViewModelLocator.MainWindowViewModelStatic;

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            window.DataContext = viewModel;

            window.Show();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);
            if (UnsavedData)
            {
                e.Cancel = true;
                string message = "The application attempted to be closed as a result of " +
                    e.ReasonSessionEnding.ToString() +
                    ". This is not allowed, as you have unsaved data.";

                Log4NetLogger.GetInstance.Error(message);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                string arg = e.Args[0];
                MessageBox.Show(arg);
            }

            Log4NetLogger.GetInstance.Info("Application startup.");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log4NetLogger.GetInstance.Info("Application exit.");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log4NetLogger.GetInstance.Error(e.ToString());
        }
    }
}
