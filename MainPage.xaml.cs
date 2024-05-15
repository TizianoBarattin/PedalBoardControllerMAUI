using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json;
using NAudio;
using NAudio.Midi;
using Commons.Music.Midi;
using CommunityToolkit.Maui.Views;
using MauiController.Pages.ModulesPages;
using MauiController.Classes;
using MauiController.Classes.Configurations;
using MauiController.Classes.Modules;
using System.Windows.Input;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui.Controls;
using Color = Microsoft.Maui.Graphics.Color;


namespace MauiController
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        string ProjectName = "MauiController";

        MidiInterface midiInterface = new MidiInterface();
        public List<Modules> pedalboardModules { get; private set; } = new List<Modules>();
        public Config config { get; private set; } = new Config();

        public List<string> MidiInputs = new List<string>();
        public List<string> MidiOutputs = new List<string>();
        public bool startupDone = false;
        const string generalconfigfile = @"\PedalBoardControllerFiles\settings\generalsettings.json";
        const string pedalboardFile = @"\PedalBoardControllerFiles\pedalboards\";
        public List<int> midiChannels = new List<int>();
        
        private bool IsAndroid() => DeviceInfo.Current.Platform == DevicePlatform.Android;
        private bool IsMac() => DeviceInfo.Current.Platform == DevicePlatform.macOS;
        private bool IsWindows() => DeviceInfo.Current.Platform == DevicePlatform.WinUI;

        //TODO: definire costante per dare max numero di moduli
        public PedalBoardConfig pedalBoardConfig { get; private set; } = new PedalBoardConfig();

        public ObservableCollection<Modules> PedalboardModules { get; private set; }  = new ObservableCollection<Modules>();


        public MainPage()
        {            
            InitializeComponent();
            InitializeMidiChannels();
            LoadConfig();
            InitializeMidiInputs();
            InitializeMidiOutputs();

            startupDone = true;
        }
        public MainPage(bool ExternalCall)//per evitare loop con chiamata di Load Config che quando chiama i moduli crea nuove MainPage temporanee
        {
            InitializeComponent();
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

        public void OnMidiInputSelectionChange(object sender, EventArgs e)
        {
            if (startupDone)
            {
                config.MidiInput = (string)cbMidiInputs.SelectedItem;
                SaveConfig();
            }
        }
        public void OnMidiOutputSelectionChange(object sender, EventArgs e)
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

        public bool SavePedalboard(string pedalboardName)
        {
            var dir = GetApplicationDirectoryPath();
            var configfile = dir + pedalboardFile;

            if (!Directory.Exists(Path.GetDirectoryName(configfile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configfile));
            }

            configfile = $"{configfile}{pedalboardName}.json";

            if (!File.Exists(configfile))
            {
                //SAVE COMMON SETTINGS
                JsonSerializerSettings pedalboardFile = new JsonSerializerSettings();
                pedalboardFile.Formatting = Formatting.Indented;
                pedalboardFile.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                var stringpedalboarddefault = JsonConvert.SerializeObject(pedalBoardConfig, pedalboardFile);
                using (var writer = new StreamWriter(configfile))
                {
                    writer.Write(stringpedalboarddefault);
                }

                config.PedalBoardFile = configfile;

                return true;
            }
            else
            {
                //TODO: pop up cambia nome o sovrascrivi
                return false;
            }

        }

        public string GetBackupFileName(string fileName)
        {
            return fileName.Replace(".json", ".bkp.json");
        }

        public void OpenPedalboard(string pedalboardFile)
        {
            string backupFile = GetBackupFileName(pedalboardFile);
            if (File.Exists(backupFile))
            {
                //TODO: popup apri backup
                //Popup popup = new Popup();
                //this.ShowPopup(popup);
            }

            string jsonFromFile;
            try
            {
                using (var reader = new StreamReader(pedalboardFile))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                pedalBoardConfig = null;
                pedalBoardConfig = JsonConvert.DeserializeObject<PedalBoardConfig>(jsonFromFile);

                LabelPedalboardName.Text = pedalBoardConfig.PedalBoardName;
            }
            catch (Exception)
            {
                //TODO: messaggio errore
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

            if (config.PedalBoardFile != null && config.PedalBoardFile != "" && File.Exists(config.PedalBoardFile))
            {
                OpenPedalboard(config.PedalBoardFile);

                if (pedalBoardConfig.ModuleType.Count > 0)
                {
                    for (int i = 0; i < pedalBoardConfig.ModuleType.Count; i++)
                    {
                        AddModule(pedalBoardConfig.ModuleType[i], pedalBoardConfig.ModuleMidiChannel[i], pedalBoardConfig.ModuleFriendlyName[i]);
                    }
                }


            }

        }

        public void AddModule(string moduleType, decimal channel, string friendlyName)
        {
            //TODO: capire come legare tab a classe e richiamare in base a quello

            string moduleClassName = $"{ProjectName}.Classes.Modules.{moduleType}";
            Type classType = Type.GetType(moduleClassName);

            //creo modulo in base a tipo modulo passato da form creazione
            object[] inputs = { this, channel, friendlyName };
            object newModule = Activator.CreateInstance(classType, inputs);
        }

        public void AddTabPassedByModule(ContentPage newPage, string ModuleName)
        {
            ShellContent shellContent = new ShellContent();
            shellContent = newPage;

            AppShell shellApp = Application.Current.MainPage as AppShell;
            shellApp.AddMainFlyoutTab(newPage, ModuleName);
        }

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
                    if (module.ModuleFriendlyName == inputModuleName)
                    {
                        ModuleNameFree = false;
                    }
                }
            }

            return ModuleNameFree;
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

        public void OnCreateNewPedalboardClicked(object sender, EventArgs e)
        {
            //TODO
        }
        public void OnLoadPedalboardClicked(object sender, EventArgs e)
        {
            //TODO
        }
        public void OnSavePedalboardClicked(object sender, EventArgs e)
        {
            pedalBoardConfig.PedalBoardName = LabelPedalboardName.Text;
            if (SavePedalboard(LabelPedalboardName.Text))
            {
                SaveConfig();
                PedalboardIsNotChanging();
            } 
        }

        public void OnPedalboardLabelChanged(object sender, EventArgs e)
        {
            if (startupDone)
            {
                PedalBoardIsChanging();
            }
        }

        public void PedalBoardIsChanging()
        {
            //LabelPedalboardName.TextColor = Color.FromRgba(0, 255, 255, 255); ;

            //SaveBackup
            SavePedalboard($"{LabelPedalboardName.Text}.bkp");
        }
        public void PedalboardIsNotChanging()
        {
            if (File.Exists(GetBackupFileName(pedalBoardConfig.PedalBoardName)))
            {
                File.Delete(GetBackupFileName(pedalBoardConfig.PedalBoardName));
            }
            //LabelPedalboardName.TextColor = Color.FromRgba(0, 255, 255, 255); ;
        }

    }

}
