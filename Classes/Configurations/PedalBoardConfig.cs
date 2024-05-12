using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiController.Classes.Configurations
{
    public class PedalBoardConfig
    {
        public string PedalBoardName { get; set; }

        public List<string> ModuleType = new List<string>();

        public List<string> ModuleFriendlyName = new List<string>();

        public List<decimal> ModuleMidiChannel = new List<decimal>();
    }
}
