using LogApp;
using LogWrangler.LogApp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LogWrangler.LogApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            LogWindowViewModel viewModel = new LogWindowViewModel(new LogReader.LogReader());
            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}
