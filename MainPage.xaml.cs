using PedalBoardController;
using PedalBoardController.Classes;
using PedalBoardController.Classes.Modules;
using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json;
using NAudio;
using NAudio.Midi;
using Commons.Music.Midi;
using CommunityToolkit.Maui.Views;


namespace MauiController
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        MidiInterface midiInterface = new MidiInterface();
        public List<Modules> pedalboardModules { get; private set; } = new List<Modules>();
        public Config config { get; private set; } = new Config();

        public List<string> MidiInputs = new List<string>();
        public List<string> MidiOutputs = new List<string>();
        public bool startupDone = false;
        const string generalconfigfile = @"\settings\generalsettings.json";
        public List<int> midiChannels = new List<int>();
        private bool IsAndroid() => DeviceInfo.Current.Platform == DevicePlatform.Android;
        private bool IsMac() => DeviceInfo.Current.Platform == DevicePlatform.macOS;
        private bool IsWindows() => DeviceInfo.Current.Platform == DevicePlatform.WinUI;

        //TODO: definire costante per dare max numero di moduli
        public PedalBoardConfig pedalBoardConfig { get; private set; } = new PedalBoardConfig();


        public MainPage()
        {            
            InitializeComponent();
            InitializeMidiChannels();

            LoadConfig();
            InitializeMidiInputs();
            InitializeMidiOutputs();

            startupDone = true;
        }

        public void InitializeMidiChannels()
        {
            for (int i = 0; i < 16; i++)
            {
                midiChannels.Add(i+1);
            }
        }


        //public bool timeElapsed = false;
        //public byte[] midiReceived { get; private set; }



        public void InitializeMidiInputs()
        {
            MidiInputs.Clear();
            MidiInputs = midiInterface.GetMidiInputs();
            cbMidiInputs.ItemsSource = MidiInputs;

            if ((config.MidiInput != "") && MidiInputs.Contains(config.MidiInput))
            {
                cbMidiInputs.SelectedItem = config.MidiInput;
            }

            config.MidiInput = (string)cbMidiInputs.SelectedItem;
        }
        public void InitializeMidiOutputs()
        {
            MidiOutputs.Clear();
            MidiOutputs = midiInterface.GetMidiOutputs();
            cbMidiOutputs.ItemsSource = MidiOutputs;

            if ((config.MidiOutput != "") && MidiOutputs.Contains(config.MidiOutput))
            {
                cbMidiOutputs.SelectedItem = config.MidiOutput;
            }

            config.MidiOutput = (string)cbMidiOutputs.SelectedItem;
        }

        public void OnMidiInputSelectionChange(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            if (startupDone)
            {
                config.MidiInput = (string)cbMidiInputs.SelectedItem;
                SaveConfig();
            }
        }
        public void OnMidiOutputSelectionChange(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            if (startupDone)
            {
                config.MidiOutput = (string)cbMidiOutputs.SelectedItem;
                SaveConfig();
            }
        }

        public void SaveConfig()
        {
            var dir = GetApplicationDirectoryPath();
            var configfile = dir + generalconfigfile;

            if (!Directory.Exists(Path.GetDirectoryName(configfile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configfile));
            }

            //SAVE COMMON SETTINGS
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var stringconfigdefault = JsonConvert.SerializeObject(config, settings);
            using (var writer = new StreamWriter(configfile))
            {
                writer.Write(stringconfigdefault);
            }
        }

        public string GetApplicationDirectoryPath()
        {
            if (IsWindows())
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                //TODO: aggiungi Andorid/Ios/Mac
            }
            {
                return "";
            }

        }
        public void AddModuleClicked(object sender, EventArgs e)
        {
            GetAllPossibleModules();

            if (pedalboardModules.Count < 10)
            {
                ModulesSelectionPopup popup = new ModulesSelectionPopup();
                popup.setParentForm(this);
                this.ShowPopup(popup);
            }
            else
            {   //TODO: far diventare tutte le descrizioni traducibili? O tutto maiuscolo
                //MessageBox.Show("You can not add more than 10 modules in your pedalboard!");
            }
        }
        public Type[] GetAllPossibleModules()
        {
            Type[] modules = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in modules)
            {
                //remove types that are not devices modules
                if (!type.FullName.Contains("Classes.Modules.") || type.IsAbstract || type.IsEnum)

                {
                    int numIndex = Array.IndexOf(modules, type);
                    //questa funzione toglie dall'array il numero selezionato dal foreach se if è vero
                    modules = modules.Where((val, idx) => idx != numIndex).ToArray();
                }
            }
            return modules;
        }

        //public bool SendControlChange(decimal channel, int controlNumber, int value)
        //{
        //    bool res = midiInterface.SendControlChange(channel, controlNumber, value, config.MidiOutput, true);
        //    if (!res)
        //    {
        //        MessageBox.Show("Some problems sending the midi message!"); //TODO: se problemi non segnare come ben letto/scrivere "error" sul nome programma
        //    }
        //    return res;
        //}

        //public bool SendSysEx(byte[] data)
        //{
        //    bool res = midiInterface.SendSysEx(config.MidiOutput, true, data);
        //    if (!res)
        //    {
        //        MessageBox.Show("Some problems sending the program!"); //TODO: se problemi non segnare come ben letto/scrivere "error" sul nome programma
        //    }
        //    return res;
        //}

        //public byte[] ReceiveSysEx()
        //{
        //    byte[] result = new byte[] { };
        //    Task<MidiEvent> midiEvent = midiInterface.ReceiveMidiData(config.MidiInput, false);
        //    SysExEvent sysexEvent = (SysExEvent?)midiEvent.Result;

        //    if (sysexEvent == null)
        //    {
        //        result = null;
        //    }
        //    else
        //    {
        //        result = sysexEvent.Data;
        //    }

        //    return result;
        //}

        //public void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    timeElapsed = true;
        //}

        public void LoadConfig()
        {
            var dir = GetApplicationDirectoryPath();
            var configfile = dir + generalconfigfile;

            string jsonFromFile;
            try
            {
                using (var reader = new StreamReader(configfile))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                config = JsonConvert.DeserializeObject<Config>(jsonFromFile);
            }
            catch (Exception)
            {
                SaveConfig();
            }

        }

        public void AddModule(Button senderButton, string moduleType, decimal channel, string friendlyName)
        {
            //TODO: capire come legare tab a classe e richiamare in base a quello
            //TODO: serve fare dei controlli che esistano le classi o le tab utilizzate?

            //string moduleClassName = $"PedalBoardController.Classes.Modules.{senderButton.Name}";
            //Type classType = Type.GetType(moduleClassName);

            ////creo modulo in base a tipo modulo passato da form creazione
            //object[] inputs = { this, channel, friendlyName };
            //object newModule = Activator.CreateInstance(classType, inputs);
        }

        //public void AddTabPassedByModule(TabPage newTab, Modules newModule)
        //{
        //    tcMain.TabPages.Add(newTab);
        //    pedalboardModules.Add(newModule);
        //}

        //private void bRemoveSelected_Click(object sender, EventArgs e)
        //{   //TODO: legare l'index del form all'index della classe modulo

        //    if (tcMain.TabCount > 0)
        //    {
        //        tcMain.TabPages.RemoveAt(tcMain.SelectedIndex);
        //    }
        //    else
        //    {
        //        MessageBox.Show("There are no modules to remove in this pedalboard");
        //    }
        //}

        public int ReturnNumOfSelectedTypeModules(string moduleType)
        {
            int numberOfThisModules = 0;

            if (pedalboardModules.Count > 0)
            {
                foreach (Modules module in pedalboardModules)
                {
                    if (module.ModuleType == moduleType)
                    {   //TODO: si potrebbe leggere direttamente l'index ma per farlo devo far diventare this module index una proprieta di modules che devo cambiare per forza nei figli
                        numberOfThisModules++;
                    }
                }
            }

            return numberOfThisModules;
        }
        public bool IsTheModuleNameFree(string inputModuleName)
        {
            bool ModuleNameFree = true;

            if (Modules.NumOfModules > 0)
            {
                foreach (Modules module in pedalboardModules)
                {
                    if (module.ModuleFrienlyName == inputModuleName)
                    {
                        ModuleNameFree = false;
                    }
                }
            }

            return ModuleNameFree;
        }
    }

}
