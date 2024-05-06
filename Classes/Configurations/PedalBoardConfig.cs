using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiController.Classes.Configurations
{
    public class PedalBoardConfig
    {
        public string PedalBoardConfigName { get; set; }

        public string[] ModuleType = new string[10];

        public string[] ModuleFriendlyName = new string[10];

        public decimal[] ModuleMidiChannel;
    }
}
