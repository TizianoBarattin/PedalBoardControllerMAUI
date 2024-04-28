using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiController;
//using PedalBoardController.Classes.TabPages;

namespace PedalBoardController.Classes.Modules
{
    public class BossES5 : Modules
    {
        private static int numOfThisModules;

        public const string moduleType = "ES-5 by BOSS";
        public const string relativeFormTabName = "tpBossES5";

        private MainPage MainForm = null;

        public BossES5(MainPage _mainForm, decimal midiChannel, string moduleFriendlyName)
            : base(midiChannel, moduleType, moduleFriendlyName)
        {
            numOfThisModules++;

            MainForm = _mainForm;
            //MainForm.AddTabPassedByModule(new tpageBossES5(moduleFriendlyName, Modules.NumOfModules, midiChannel), this);
        }
    }
}
