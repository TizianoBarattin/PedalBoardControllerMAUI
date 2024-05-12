using MauiController;
using MauiController.Classes;
using MauiController.Classes.Modules;
using System.Collections.ObjectModel;

namespace MauiController
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            //LoadConfig();
            //Startup();
        }
    }
}
