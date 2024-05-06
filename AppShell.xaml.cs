using Melanchall.DryWetMidi.Core;
using MauiController.Classes;
using System.Reflection;
using System.Timers;
using System.Xml;
using MauiController.Pages.ModulesPages;

namespace MauiController
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            //MainPageTab.AddLogicalChild(shellContent);

        }

        public void AddMainFlyoutTab(ContentPage page, string PageName)
        {
            ShellContent shellContent = new ShellContent();
            shellContent.Title = PageName;
            shellContent.Content = page;
            MainPageTab.Items.Add(shellContent);
        }
    }
}
