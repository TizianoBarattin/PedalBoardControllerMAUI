using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiController;

namespace MauiController.Classes.Modules
{
    public class BossES8 : Modules
    {
        private static int numOfThisModules;

        public const string moduleType = "ES-8 by BOSS";
        public const string relativeFormTabName = "tpBossES8";

        public BossES8(MainPage _mainPage, decimal midiChannel, string moduleFriendlyName)
            : base(midiChannel, moduleType, moduleFriendlyName, _mainPage)
        {
            numOfThisModules++;

            MainPage.PedalboardModules.Add(this);
            MainPage.AddTabPassedByModule(ModulePage, moduleFriendlyName);
        }
    }
}
