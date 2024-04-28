using PedalBoardController;
using PedalBoardController.Classes;
using PedalBoardController.Classes.Modules;
using System.Collections.ObjectModel;

namespace MauiController
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCekx0THxbf1x0ZFNMZFlbQX5PIiBoS35RckVgWn9feHBcQ2JeUUdy");
            InitializeComponent();

            MainPage = new AppShell();

            //LoadConfig();
            //Startup();
        }
    }
}
