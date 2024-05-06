using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using MauiController;

namespace MauiController.Classes.Modules
{
    public abstract class Modules
    {
        private static int numOfModules;
        public static int NumOfModules { get { return numOfModules; } }

        private int moduleIndex;

        private string moduleType { get; set; }
        public string ModuleType { get { return moduleType; } }

        private string moduleFriendlyName;
        public string ModuleFrienlyName { get { return moduleFriendlyName; } }

        private int tabIndex;
        public int TabIndex { get { return tabIndex; } }

        private decimal midiChannel;
        private decimal minMidiChannel = 1;
        private decimal maxMidiChannel = 16;

        public ContentPage ModulePage = new ContentPage();
        public MainPage MainPage = new MainPage();

        //costruttore
        public Modules(decimal midiChannel, string moduleType, string moduleFriendlyName, MainPage mainPage)
        {   //TODO: pensare a come gestire la possibilità di rimuovere blocchi che sono in "metà"...switcho il resto?
            if (numOfModules < 10)
            {
                moduleIndex = numOfModules;
                numOfModules++;

                if (midiChannel >= minMidiChannel && midiChannel <= maxMidiChannel)
                {
                    this.midiChannel = midiChannel;
                }

                this.moduleType = moduleType;
                this.moduleFriendlyName = moduleFriendlyName;
            }

            MainPage = mainPage;


        }


    }
}
