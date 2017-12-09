using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TranscriptGenerator.PageSwitcher
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception has occurred:\n" + e.Exception.Message, "TCP/IP-Proxy - Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;

            if (this.MainWindow != null)
            {
                this.MainWindow.Close();
            }
        }
    }
}
