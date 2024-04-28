using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiController;
//using PedalBoardController.Classes.TabPages;
using PedalBoardController.Controllers;

namespace PedalBoardController.Classes.Modules
{
    public class FutureImpactV3: Modules
    {   //TODO: deve diventare una proprietà di modules

        private static int numOfThisModules;
        public int NumOfThisModules { get { return numOfThisModules; } }

        private MainPage MainForm = null;

        public List<FI_Controllers> fiParams = new List<FI_Controllers>();
        public List<bool> paramChanging = new List<bool>(new bool[200]);
        public string currentFileName = null;
        public decimal Channel = 1;
        public decimal LastBlock = 0;

        public const string moduleType = "FUTURE IMPACT V3 by Panda Audio";
        public const string relativeFormTabName = "tpFutureImpactV3";

        //PROGRAMS FOLDER NAME
        //public string defaultFolderProgramsName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "FutureImpactPrograms"; TODO:scommenta

        public FutureImpactV3(MainPage _mainForm, decimal midiChannel, string moduleFriendlyName) 
            : base(midiChannel, moduleType, moduleFriendlyName)
        {
            numOfThisModules++;

            fiParams = PublicControllers();
            currentFileName = "INIT";

            //CREATE FOLDER
            //if (!Directory.Exists(defaultFolderProgramsName))
            //{
            //    Directory.CreateDirectory(defaultFolderProgramsName);
            //}
            //
            //Channel = midiChannel;
            //MainForm = _mainForm;
            //MainForm.AddTabPassedByModule(new tpageFutureImpactV3(moduleFriendlyName, Modules.NumOfModules, midiChannel, this), this); TODO: scommenta

        }

        public List<FI_Controllers> PublicControllers()
        {
            List<FI_Controllers> Params = new List<FI_Controllers>();

            //param 0 doesn't exist
            Params.Add(new FI_Controllers("", 0, 0, 0, 0, 0, 0, 0, 0));       //Param N° 0 non esiste

            //VCOs
            for (int i = 0; i < 4; i++)
            {//TODO: valori iniziali da approfondire
                Params.Add(new FI_Controllers($"vco{i + 1}PitchCoarse",     i,  0,  (1 + (i * 12)),     5,  (0 + (i * 12)),   -24,   +48,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}PitchFine",       i,  1,  (2 + (i * 12)),     5,  (1 + (i * 12)),   -63,   +63,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}PitchBeat",       i,  2,  (3 + (i * 12)),     5,  (2 + (i * 12)),   -63,   +63,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}SawDecayTime",    i,  3,  (4 + (i * 12)),     5,  (3 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}SawDecay",        i,  4,  (5 + (i * 12)),     5,  (4 + (i * 12)),   -63,   +63,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}PulseOffset",     i,  5,  (6 + (i * 12)),     5,  (5 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}PulseLfoFreq",    i,  6,  (7 + (i * 12)),     5,  (6 + (i * 12)),    +1,  +127,   1,      1));
                Params.Add(new FI_Controllers($"vco{i + 1}PulseLfoDepth",   i,  7,  (8 + (i * 12)),     5,  (7 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}VolumeSaw",       i,  8,  (9 + (i * 12)),     5,  (8 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}VolumeSqr",       i,  9, (10 + (i * 12)),     5,  (9 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}VolumeTriang",    i, 10, (11 + (i * 12)),     5, (10 + (i * 12)),     0,  +127,   0));
                Params.Add(new FI_Controllers($"vco{i + 1}Volume",          i, 11, (12 + (i * 12)),     5, (11 + (i * 12)),     0,  +127,   0));
            }

            //First Value x VolumeSaw+Volume of VCO1
            Params[9].ActualValue = 42;
            Params[12].ActualValue = 32;      //TODO: confrontare che siano uguali i due programmi sia quando apro solo uno (e manco cc) sia quando apro tanti e mando sysex

            //HARMONIZER
            Params.Add(new FI_Controllers("harmVox2Shift",     4,      0,      49,       5,      48,  -2,      +5,   0,      "-12,-5,7,12,19,24"));
            Params.Add(new FI_Controllers("harmVox3Shift",     4,      1,      50,       5,      49,  -2,      +5,   0,      "-12,-5,7,12,19,24"));
            Params.Add(new FI_Controllers("harmInstrumVolume", 4,      2,      51,       5,      50,   0,    +127,   0));
            Params.Add(new FI_Controllers("harmVox+1octVoulme",4,      3,      52,       5,      51,   0,    +127,   0));
            Params.Add(new FI_Controllers("harmVox2Volume",    4,      4,      53,       5,      52,   0,    +127,   0));
            Params.Add(new FI_Controllers("harmVox3Volume",    4,      5,      54,       5,      53,   0,    +127,   0));

            //DISTORSION
            Params.Add(new FI_Controllers("distGrade",         4,      6,      55,       5,      54,   0,     +31,   0));
            Params.Add(new FI_Controllers("distTone",          4,      7,      56,       5,      55,   0,    +127,   0));

            //MIXER
            Params.Add(new FI_Controllers("mixerInstrument",   5,      0,      57,       5,      56,   0,    +127,  64));
            Params.Add(new FI_Controllers("mixerVcfLin",       5,      1,      58,       5,      57,   0,    +127,  64));
            Params.Add(new FI_Controllers("mixerVcfLog",       5,      2,      59,       5,      58,   0,    +127,  64));

            //VCA ADSR
            Params.Add(new FI_Controllers("vcaAttack",         6,      0,      60,       5,      59,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("vcaDecay",          6,      1,      61,       5,      60,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("vcaSustain",        6,      2,      62,       5,      61,   0,    +127, 127));
            Params.Add(new FI_Controllers("vcaRelease",        6,      3,      63,       5,      62,   1,    +127,   1,      1));
                                                                                                                              
            //VCF ADSR                                                                                                        
            Params.Add(new FI_Controllers("vcfAttack",         6,      4,      64,       5,      63,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("vcfDecay",          6,      5,      65,       5,      64,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("vcfSustain",        6,      6,      66,       5,      65,   0,    +127, 127));
            Params.Add(new FI_Controllers("vcfRelease",        6,      7,      67,       5,      66,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("vcaEnvelopeMode",   6,      8,      68,       5,      67,   0,      +2,   1));             //TODO:se su vintage D/S di vca e S/R di vcf sono bloccati

            //NOISE
            Params.Add(new FI_Controllers("noiseAttack",       6,      9,      69,       5,      68,   1,    +127,   1,      1));
            Params.Add(new FI_Controllers("noiseDecay",        6,     10,      70,       5,      69,   1,    +127,   1,      1));

            //VCF INPUT
            Params.Add(new FI_Controllers("vcfInInstrument",   7,      0,      71,       5,      70,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfInDist",         7,      1,      72,       5,      71,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfInAirDist",      7,      2,      73,       5,      72,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfInSynth",        7,      3,      74,       5,      73,   0,    +127,  64));
            Params.Add(new FI_Controllers("vcfInNoise",        7,      4,      75,       5,      74,   0,    +127,   0));

            //FILTER
            Params.Add(new FI_Controllers("vcfAge",            7,      5,      76,       5,      75,   0,      +1,   1,      "vintage,new"));            //TODO: cambia di vcfFreq vintage 0/80, new 36/122 e porta al valore minimo
            Params.Add(new FI_Controllers("vcfFreq",           7,      6,      77,       5,      76,   0,    +122, 122));
            Params.Add(new FI_Controllers("bpfFreq",           7,      7,      78,       5,      77,   0,     +24,   0));
            Params.Add(new FI_Controllers("vcfEnvFollow",      7,      8,      79,       5,      78,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfAccent",         7,      9,      80,       5,      79,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfAdAdsr",         7,     10,      81,       5,      80,   0,    +127,   0));
            Params.Add(new FI_Controllers("vcfPitchFollow",    7,     11,      82,       5,      81,   0,      +1,   0,      "OFF,ON"));
            Params.Add(new FI_Controllers("vcfType",           8,      0,      83,       5,      82,   0,      +4,   0,      "LPF,HPF,BPF,NTC,OFF"));
            Params.Add(new FI_Controllers("vcfReso",           8,      1,      84,       5,      83,  +7,    +127,   7,      7));
            Params.Add(new FI_Controllers("vcfSlope",          8,      2,      85,       5,      84,   0,      +1,   1,      "12db,24db"));

            //MIDI
            Params.Add(new FI_Controllers("midiPortatime",     9,      0,      86,       5,      85,   0,    +127,   0));
            Params.Add(new FI_Controllers("midiPortaSlope",    9,      1,      87,       5,      86,   0,      +1,   0,      "rate,time"));
            Params.Add(new FI_Controllers("midiPbdRange",      9,      2,      88,       5,      87,   0,     +24,   0));
            Params.Add(new FI_Controllers("vcosTranspose",     9,      3,      89,       5,      88, -24,     +48,   0));
            Params.Add(new FI_Controllers("midiKModePortaMode",9,      4,      90,       5,      89,   0,      +3,   0,      "K:gate/P:always,K:trigger/P:always,K:gate/P:legato,K:trigger/P:legato"));
            Params.Add(new FI_Controllers("midiPbdRange",      9,      5,      91,       5,      90,   0,      +3,   0,      "lower,upper,first,last"));

            //LFO
            Params.Add(new FI_Controllers("lfoFreq",          10,      0,      92,     5,      91,     1,    +127,   1,      1));
            Params.Add(new FI_Controllers("lfoDelay",         10,      1,      93,     5,      92,     0,    +127,   0));
            Params.Add(new FI_Controllers("lfoFVcf",          10,      2,      94,     5,      93,     0,    +127,   0));
            Params.Add(new FI_Controllers("lfoVco",           10,      3,      95,     5,      94,     0,    +127,   0));

            //FLEXIS
            for (int i = 0; i < 4; i++)
            {//TODO: valori iniziali da approfondire
                Params.Add(new FI_Controllers($"flexi{i + 1}Source",    (11 + i),   0,  (96 + (i * 7)),     5,  (95 + (i * 7)),     0,  +113,   0));    //TODO: no valore 6
                Params.Add(new FI_Controllers($"flexi{i + 1}Mode",      (11 + i),   1,  (97 + (i * 7)),     5,  (96 + (i * 7)),     0,    +3,   0,  "Quantize,Quantize,Continuous,Continuous"));       //TODO: Quant=0 1 Cont=2 (1° gruppo di param Learn), Quant=1 Cont=3 (2° gruppo)
                Params.Add(new FI_Controllers($"flexi{i + 1}Learn",     (11 + i),   2,  (98 + (i * 7)),     5,  (97 + (i * 7)),     0,   +94,   0));    //0/94 1° 0/41 2°...TODO: ci sono param non collegabili?                           
                Params.Add(new FI_Controllers($"flexi{i + 1}Polarity",  (11 + i),   3,  (99 + (i * 7)),     5,  (98 + (i * 7)),     0,    +3,   0));    //0:+, 2:-, 1/3 uguale ma con limit up verso basso
                Params.Add(new FI_Controllers($"flexi{i + 1}UpLimit",   (11 + i),   4, (100 + (i * 7)),     5,  (99 + (i * 7)),     0,  +128,   0));    //TODO: quando si muove limite sotto si muove della stessa distanza (limiti sopra e sotto variano in base a parametro..si stringe se si sposta il limite sotto (che è quello reale del parametro), valore default valore quando si inserisce controllo)
                Params.Add(new FI_Controllers($"flexi{i + 1}RangeCtrl", (11 + i),   5, (101 + (i * 7)),     5, (100 + (i * 7)),     0,  +113,   0));                                                    //TODO: no valore 6
                Params.Add(new FI_Controllers($"flexi{i + 1}Diagram",   (11 + i),   6, (102 + (i * 7)),     5, (101 + (i * 7)),     0,   +16,   0,      -1, 11));
            }                                                                                                                                           

            //FX ORDER
            Params.Add(new FI_Controllers("eqOdOrder",         15,      0,    124,    6,       0,      0,       +1,  0,      "Od -> Eq,Eq -> Od"));

            //CHORUS
            Params.Add(new FI_Controllers("chorusInstrLev",    15,      1,    125,    6,       1,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusVcfLev",      15,      2,    126,    6,       2,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusOnOff",       16,      0,    127,    6,       3,      0,      +1,   0,      "Off,On"));
            Params.Add(new FI_Controllers("chorusLfoAFrq",     16,      1,    128,    6,       4,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusLfoBFrq",     16,      2,    129,    6,       5,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusLevel1",      16,      3,    130,    6,       6,    -64,     +63,   0));
            Params.Add(new FI_Controllers("chorusDelay1",      16,      4,    131,    6,       7,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusMod1A",       16,      5,    132,    6,       8,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusMod1B",       16,      6,    133,    6,       9,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusLevel2",      16,      7,    134,    6,      10,    -64,     +63,   0));
            Params.Add(new FI_Controllers("chorusDelay2",      16,      8,    135,    6,      11,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusMod2A",       16,      9,    136,    6,      12,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusMod2B",       16,     10,    137,    6,      13,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusFeedbackLvl", 16,     11,    138,    6,      14,      0,    +127,   0));
            Params.Add(new FI_Controllers("chorusFeedbackDmp", 16,     12,    139,    6,      15,      0,    +127,   0));
                                                                                                       
            //OVERDRIVE                                                                                
            Params.Add(new FI_Controllers("odState",           17,      0,    140,    6,      16,      0,      +4,   0,      "Off,Instrument,Vcf,Instr+Vcf"));
            Params.Add(new FI_Controllers("odDrive",           17,      1,    141,    6,      17,      0,    +127,   0));
            Params.Add(new FI_Controllers("odLevel",           17,      2,    142,    6,      18,     -6,      +6,   0));

            for (int i = 142; i < 152; i++)
            {
                Params.Add(new FI_Controllers("", 17, (i-142+3), (i+1), 6, (i-142+19), 0, 0, 0));
            }

            //EQUALIZATOR
            Params.Add(new FI_Controllers("eqState",           18,      0,    153,    6,      29,      0,      +4,   0,      "Off,Instrument,Vcf,Instr+Vcf"));
            Params.Add(new FI_Controllers("eqBassFreq",        18,      1,    154,    6,      30,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqBassSlope",       18,      2,    155,    6,      31,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqBassBost",        18,      3,    156,    6,      32,    -20,     +20,   0));
            Params.Add(new FI_Controllers("eqMid1Frq",         18,      4,    157,    6,      33,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqMid1Q",           18,      5,    158,    6,      34,     10,    +100,  10,      10));    //10= 1,11=1.1,.... 100 = 10
            Params.Add(new FI_Controllers("eqMid1Boost",       18,      6,    159,    6,      35,    -20,     +20,   0));             //-20/+20 dB
            Params.Add(new FI_Controllers("eqMid2Frq",         18,      7,    160,    6,      36,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqMid2Q",           18,      8,    161,    6,      37,     10,    +100,  10,      10));
            Params.Add(new FI_Controllers("eqMid2Boost",       18,      9,    162,    6,      38,    -20,     +20,   0));
            Params.Add(new FI_Controllers("eqTrebleFrq",       18,     10,    163,    6,      39,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqTrebleSlope",     18,     11,    164,    6,      40,      0,    +127,   0));
            Params.Add(new FI_Controllers("eqTrebleBoost",     18,     12,    165,    6,      41,    -15,     +20,   0));


            return Params;
        }

        public enum FlexiSource
        {   //TODO
            //da 1 a 87 mettere CC 1 ecc... (no 6)
            Aftertouch = 96,
            EnvelopeFollower = 97,
            VcfEnvelope = 98,
            NoiseEnvelope = 99,
            Accent = 100,
            Lfo = 101,
            Pitch = 102,
            PitchBendWheel = 103,
            ChorusLfoA = 104,
            ChorusLfoB = 105,
            Vco3LfoTri = 106,
            Vco4LfoTri = 107,
            WhiteNoise = 108,
            PinkNoise = 109,
            Vco3LfoSqr = 110,
            Vco4LfoSqr = 111,
            Vco3LfoSaw = 112,
            Vco4LfoSaw = 113
        }

        public enum FlexiRange
        {   //TODO
            //da 1 a 87 mettere CC 1 ecc... (no 6)
            MidiCC1_LfoDepth = 1,
            MidiCC2_VcfVolume = 2,
            MidiCC5_PortamentoTime = 5,
            MidiCC7_Volume = 7,
            MidiCC11_Cutoff = 11,
            MidiCC65_PortamentoOnOff = 65,
            MidiCC66_FiOnOff = 66,
            MidiCC67_ProgramDecrement = 67,
            MidiCC69_FiOnOffToggle = 69,
            MidiCC74_FilterCutoff = 74,

            Aftertouch = 96,
            EnvelopeFollower = 97,
            Accent = 100,
            Pitch = 102,
            PitchBendWheel = 103,
        }

        //public List<FI_Controllers> OpenPrg3File(string file, bool sendingFiles)
        //{

        //    List<FI_Controllers> auxSendingParams = new List<FI_Controllers>(); //only if sending files (to not change actual fi params every time)
        //    if (sendingFiles)
        //    {
        //        auxSendingParams = PublicControllers();
        //    }

        //    var filestream = new System.IO.FileStream(file,
        //                                              System.IO.FileMode.Open,
        //                                              System.IO.FileAccess.Read,
        //                                              System.IO.FileShare.ReadWrite);

        //    try
        //    {
        //        string lineOftext = null;
        //        var linesReaded = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
        //        bool res = true;
        //        LastBlock = 0;

        //        while ((lineOftext = linesReaded.ReadLine()) != null && res)
        //        {
        //            //Esempio di stringa letta
        //            // 0    ; field:  0  poti:  0   ser.no.:  1
        //            //il primo numero indica il valore che deve prendere il parametro che si modifica
        //            //il numero dopo serial number indica il seriale del parametro da modificare

        //            bool lineOk = false;
        //            string[] appArray = null;
        //            string paramValueString = null;
        //            string paramSerNumString = null;
        //            int paramValue = 0;
        //            int paramSerNum = 0;

        //            if (lineOftext.Contains("; field:") && lineOftext.Contains("poti:") && lineOftext.Contains("ser.no.:"))
        //            {   //TODO: controllare meglio la formattazione?
        //                lineOk = true;
        //            }

        //            if (lineOk)
        //            {
        //                appArray = lineOftext.Split("; field:");
        //                paramValueString = appArray[0];

        //                try
        //                {

        //                    paramValue = Convert.ToInt32(paramValueString);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }

        //                appArray = appArray[1].Split("ser.no.:");
        //                paramSerNumString = appArray[1];


        //                try
        //                {

        //                    paramSerNum = Convert.ToInt32(paramSerNumString);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }

        //                if (!sendingFiles)
        //                {
        //                    res = ChangeParamValue(paramValue, paramSerNum, fiParams, true, false);
        //                }
        //                else
        //                {
        //                    res = ChangeParamValue(paramValue, paramSerNum, auxSendingParams, false, false);
        //                }

        //            }
        //            else
        //            {
        //                //TODO: avviso se file non ok
        //            }

        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
            

        //    if (!sendingFiles)
        //    {
        //        return fiParams;
        //    }
        //    else
        //    {
        //        return auxSendingParams;
        //    }
        //}

        //public string GetFileName(string file)
        //{
        //    //TODO: dire che il nome file non è corretto oppure se non ci sono numeri, chiedere a che numero da 1 a 99 asseggnare il programma

        //    string[] separatedFileName = file.Split('\\');
        //    currentFileName = separatedFileName.Last();

        //    if (currentFileName.Contains(".prg"))
        //    {
        //        separatedFileName = currentFileName.Split(".prg");
        //        currentFileName = separatedFileName.First();
        //    }
        //    else if (currentFileName.Contains(".pr3"))
        //    {
        //        separatedFileName = currentFileName.Split(".pr3");
        //        currentFileName = separatedFileName.First();
        //    }

        //    return currentFileName;//.ToUpper();
        //}

        //public void SavePrg3File(string file)
        //{
        //    using (StreamWriter writer = new StreamWriter(file))
        //    {
        //        writer.WriteLine(" 243 ");

        //        foreach (var item in fiParams)
        //        {
        //            if (item.sNumber != 0 && item.sNumber <= 123)   //s. number non indice array fiParams!!!
        //            {
        //                string valueString = GetStringWFixedLenght(item.ActualValue// + item.MinValueCmd
        //                                                                           , 5);
        //                string fieldString = GetStringWFixedLenght(item.field, 3);
        //                string potiString = GetStringWFixedLenght(item.poti, 4);
        //                string sNumberString = item.sNumber.ToString();

        //                if (sNumberString == "102")
        //                {
        //                    sNumberString = "102";
        //                }
                        

        //                if (valueString.Contains("-"))
        //                {
        //                    writer.WriteLine($"{valueString}; field:  {fieldString}poti:  {potiString}ser.no.:  {sNumberString} ");
        //                }
        //                else
        //                {
        //                    writer.WriteLine($" {valueString}; field:  {fieldString}poti:  {potiString}ser.no.:  {sNumberString} ");
        //                }
                        
        //            }
        //        }

        //        writer.WriteLine(" 0    ; dummy");
        //        writer.WriteLine(" 0    ; dummy");
        //        writer.WriteLine(" 0    ; dummy");
        //        writer.WriteLine(" 0    ; dummy");
        //        writer.WriteLine(" 0    ; dummy");

        //        foreach (var item in fiParams)
        //        {
        //            if (item.sNumber >=124 && item.sNumber <= 165)
        //            {
        //                string valueString = GetStringWFixedLenght(item.ActualValue, 5);
        //                string fieldString = GetStringWFixedLenght(item.field, 3);
        //                string potiString = GetStringWFixedLenght(item.poti, 4);
        //                string sNumberString = item.sNumber.ToString();


        //                if (valueString.Contains("-"))
        //                {
        //                    writer.WriteLine($"{valueString}; field:  {fieldString}poti:  {potiString}ser.no.:  {sNumberString} ");
        //                }
        //                else
        //                {
        //                    writer.WriteLine($" {valueString}; field:  {fieldString}poti:  {potiString}ser.no.:  {sNumberString} ");
        //                }
        //            }
        //        }
        //    }
        //}

        //public string GetStringWFixedLenght(int param, int stringLenght)
        //{
        //    string stringParam = param.ToString();
        //    int n = stringParam.Count();

        //    int k = stringLenght - n;
        //    string result = stringParam;

        //    if (stringParam.Contains("-"))
        //    {
        //        k++;
        //    }
        //    while (k>0)
        //    {
        //        result = result + " ";
        //        k = k - 1;
        //    }

        //    return result;

        //}

        //public bool ChangeParamValue(int paramValue, int paramSerNum, List<FI_Controllers> listParams, bool openingFile, bool slideChanging)
        //{
        //    //TODO: messaggi di valore parametro non valido
        //    //da 1 a 142, da 153 a 165

        //    bool res = true;

        //    if (paramValue > listParams[paramSerNum].MaxValue)
        //    {
        //        paramValue = listParams[paramSerNum].MaxValue;
        //    }
        //    if (paramValue < listParams[paramSerNum].MinValue)
        //    {
        //        paramValue = listParams[paramSerNum].MinValue;
        //    }

        //    listParams[paramSerNum].ActualValue = paramValue + listParams[paramSerNum].MinValueCmd;

        //    //if opening file don't send single midi message for cc changed
        //    if (openingFile)            
        //    {
        //        bool sendBlock = true;

        //        if (LastBlock == fiParams[paramSerNum].BlockNum)
        //        {
        //            sendBlock = false;
        //        }

        //        res = SendMidiParamChange(fiParams[paramSerNum].CC, fiParams[paramSerNum].BlockNum, paramSerNum, slideChanging, sendBlock);
        //        LastBlock = fiParams[paramSerNum].BlockNum;
        //    }

        //    return res;
        //}

        //public bool SendMidiParamChange(int cc, int blockNum, int paramSerNum, bool slideChange, bool sendBlock)
        //{
        //    bool res = false;
        //    int valueCmd;
        //    int distanceFromMinValue;

        //    distanceFromMinValue = fiParams[paramSerNum].ActualValue - fiParams[paramSerNum].MinValue;

        //    valueCmd = fiParams[paramSerNum].MinCmd + distanceFromMinValue + fiParams[paramSerNum].OffsetMidi;// + fiParams[paramSerNum].minValueCmd;

        //    //if slide changing of the cc, send only values after first declaration
        //    if (slideChange)
        //    {
        //        for (int i = 0; i < 200; i++)
        //        {
        //            if (i != paramSerNum || !paramChanging[paramSerNum])
        //            {
        //                paramChanging[i] = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        paramChanging[paramSerNum] = false;
        //    }

        //    if (!paramChanging[paramSerNum])
        //    {
        //        if (sendBlock)
        //        {
        //            res = MainForm.SendControlChange(Channel, 99, blockNum);        // Open command change
        //            res = MainForm.SendControlChange(Channel, 98, cc);              // Declare parameter number
        //            res = MainForm.SendControlChange(Channel, 6, valueCmd);         // Declare value of the parameter
        //        }
        //        else
        //        {
        //            res = MainForm.SendControlChange(Channel, 98, cc);              // Declare parameter number
        //            res = MainForm.SendControlChange(Channel, 6, valueCmd);         // Declare value of the parameter
        //        }
        //    }
        //    else
        //    {
        //        res = MainForm.SendControlChange(Channel, 6, valueCmd);      // Declare value of the parameter
        //    }

        //    paramChanging[paramSerNum] = true;
        //    return res;
        //}

        //public bool SendSingleFile(string file)
        //{
        //    //F0 00 21 11 03 20 03 01 x inviare file 1
        //    //F0 00 21 11 03 40 03 01 x ricevere file 1
        //    //F0 00 21 11 03 21       x risposta a messaggio invitato
        //    //F0 00 21 11 03 41 03 01 quando fi trasmette file 1

        //    bool res = true;

        //    //GET ALL PARAMS FROM FILE
        //    List<FI_Controllers> fileParams = OpenPrg3File(file, true);     //TODO: se file non selezionato non far niente

        //    if (fileParams != null)
        //    {
        //        //MANDARE FILE
        //        byte[] data = new byte[212];

        //        string fileName = GetFileName(file);

        //        int patchNum = GetPatchNum(fileName);

        //        if (!res || (patchNum < 1 || patchNum > 99))
        //        {
        //            res = false;
        //            MessageBox.Show("The number of the patch selected is not ok");
        //        }
        //        else
        //        {
        //            //ADD FIRST BYTES
        //            byte[] initialBytes = SetInitialSysExBytes(true, patchNum);

        //            for (int i = 0; i < initialBytes.Length; i++)
        //            {
        //                data[i] = initialBytes[i];
        //            }

        //            //ADD PARAMS BYTES
        //            byte[] paramsBytes = SetParamsBytes(fileParams);

        //            for (int i = 0; i < paramsBytes.Length; i++)
        //            {
        //                data[i + initialBytes.Length] = paramsBytes[i];
        //            }

        //            //ADD PROGRAM NAME BYTES
        //            byte[] programNameBytes = SetProgramNameBytes(fileName);

        //            for (int i = 0; i < programNameBytes.Length; i++)
        //            {
        //                data[i + initialBytes.Length + paramsBytes.Length] = programNameBytes[i];
        //            }

        //            //ADD 2 CHECK BYTES
        //            byte[] lastCheck = SetCheckBytes(data, initialBytes.Length);

        //            for (int i = 0; i < lastCheck.Length; i++)
        //            {
        //                data[i + initialBytes.Length + paramsBytes.Length + programNameBytes.Length] = lastCheck[i];
        //            }

        //            //ADD CLOSING MESSAGE
        //            data[initialBytes.Length + paramsBytes.Length + programNameBytes.Length + lastCheck.Length] = 247;

        //            res = MainForm.SendSysEx(data);

        //            if (res)
        //            {
        //                byte[] returnMessage = MainForm.ReceiveSysEx();

        //                //F0 00 21 11 03 21

        //                if (returnMessage != null)
        //                { 
        //                    if (returnMessage.Length >= 5)
        //                    {
        //                        if (returnMessage[0] == 0 && returnMessage[1] == 33 && returnMessage[2] == 17 && returnMessage[3] == 3 && returnMessage[4] == 33)
        //                        {
        //                            res = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        res = false;
        //                    }
        //                }
        //                else
        //                {
        //                    res = false;
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //        res = false;
        //    }


        //    return res;
        //}

        //public int GetPatchNum(string patchName)
        //{
        //    int patchNum = 0;

        //    //Get Number of the patch
        //    try
        //    {
        //        string[] nameParts = patchName.Split('_');
        //        patchNum = int.Parse(nameParts[0]);
        //    }
        //    catch (Exception)
        //    {
        //        patchNum = -1;
        //    }

        //    return patchNum;
        //}

        //public byte[] SetInitialSysExBytes(bool sending, int patchNum)
        //{
        //    byte[] initialBytes = new byte[7];

        //    initialBytes[0] = 0;
        //    initialBytes[1] = 33;
        //    initialBytes[2] = 17;
        //    initialBytes[3] = 3;

        //    if (sending)
        //    {
        //        initialBytes[4] = 32;   //sending files
        //    }
        //    else
        //    {
        //        initialBytes[4] = 64;   //ask to receive file
        //    }

        //    initialBytes[5] = 3;
        //    initialBytes[6] = (byte)patchNum;

        //    return initialBytes;
        //}

        //public byte[] SetParamsBytes(List<FI_Controllers> par)
        //{
        //    byte[] result = new byte[par.Count - 1 + 5]; //(5 sono i dummy)

        //    int k = 0;  //aux per saltare le pos dei dummy params

        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        //DUMMY
        //        if (i >= 123 && i <= 127)
        //        {
        //            result[i] = 0;
        //            k++;
        //        }
        //        else
        //        {
        //            result[i] = (byte)(par[i + 1 - k].ActualValue
        //                                - par[i + 1 - k].MinValue
        //                                + par[i + 1 - k].MinCmd
        //                                + par[i + 1 - k].OffsetMidi
        //                                - par[i + 1 - k].MinValueCmd);
        //        }

        //    }

        //    return result;
        //}

        //public byte[] SetProgramNameBytes(string programName)
        //{
        //    byte[] data = new byte[32];
        //    int i = 0;

        //    foreach (char c in programName)
        //    {
        //        if (i < 32)
        //        {
        //            data[i] = Convert.ToByte(c);
        //            i++;
        //        }
        //    }

        //    if (i < 32)
        //    {
        //        data[i] = 0;
        //        i++;
        //    }
            
        //    return data;
        //}

        //public byte[] SetCheckBytes(byte[] actualValues, int firstValuesToSkip)
        //{
        //    byte[] result = new byte[2];

        //    int valuesSum = 0;

        //    for (int i = firstValuesToSkip; i < actualValues.Length; i++)
        //    {
        //        valuesSum = valuesSum + actualValues[i];
        //    }

        //    decimal BigByte = Math.Floor(Convert.ToDecimal(valuesSum/128));  
        //    decimal SmallByte = valuesSum - (BigByte*128);

        //    result[0] = (byte)BigByte;
        //    result[1] = (byte)SmallByte;

        //    return result;
        //}

        //public int ReceiveSingleFile(int patchNum, bool askForFolder)
        //{
        //    byte[] dataToSend = new byte[8];
        //    int res = 0;

        //    //ADD FIRST BYTES
        //    byte[] initialBytes = SetInitialSysExBytes(false, patchNum);

        //    for (int i = 0; i < initialBytes.Length; i++)
        //    {
        //        dataToSend[i] = initialBytes[i];
        //    }

        //    dataToSend[initialBytes.Length] = 247;

        //    if (MainForm.SendSysEx(dataToSend))
        //    {
        //        res = 1;
        //    }
            

        //    if (res > 0)
        //    {
        //        byte[] returnMessage = MainForm.ReceiveSysEx();

        //        if (returnMessage != null)
        //        {
        //            if (returnMessage[0] == 0
        //                && returnMessage[1] == 33
        //                && returnMessage[2] == 17
        //                && returnMessage[3] == 3
        //                && returnMessage[4] == 65
        //                && returnMessage[5] == 3
        //                && returnMessage[6] == patchNum
        //                && returnMessage.Length == 212)
        //            {
        //                string programName = ConvertSysexToFiProgram(returnMessage);

        //                if (programName != "Error_-1")
        //                {
        //                    res = 1;
        //                    SaveFile(programName, "Future Impact V3 files (*.pr3)|*.pr3", askForFolder);
        //                }
        //                else
        //                {
        //                    res = 0;
        //                }
        //            }
        //            else
        //            {
        //                res =0; 
        //            }
        //        }                
        //        else
        //        {
        //            res = -1;
        //        }

        //    }

        //    return res;
        //}

        //public string ConvertSysexToFiProgram(byte[] message)
        //{
        //    int i = 0;
        //    int dSp = 0;    //dummys spaces (dummy non spediti via Sysex)
        //    string programName = "";

        //    try
        //    {

        //        foreach (var paramByte in message)
        //        {
        //            i++;    //salto primo indice di fiParams che è vuoto


        //            if (i > 7 && i <= 177)    //bypasso primi valori già controllati
        //            {
        //                if (i > 130 && i <= 135)
        //                {
        //                    dSp++;
        //                }
        //                else
        //                {
        //                    fiParams[i - 7 - dSp].ActualValue = paramByte
        //                                                      + fiParams[i - 7 - dSp].MinValue
        //                                                      - fiParams[i - 7 - dSp].MinCmd
        //                                                      - fiParams[i - 7 - dSp].OffsetMidi
        //                                                      + fiParams[i - 7 - dSp].MinValueCmd;

        //                }
        //            }
        //            else if (i > 177 && i <= (177 + 32))
        //            {
        //                if (paramByte != 0)
        //                {
        //                    programName = $"{programName}{Convert.ToChar(paramByte)}";
        //                }
        //            }
        //        }

        //        return programName;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error_-1";
        //    }
        //}

        //public void SaveFile(string defualtProgramName, string filter, bool askForFolder)
        //{
        //    SaveFileDialog saveFiSingleFile = new System.Windows.Forms.SaveFileDialog();
        //    saveFiSingleFile.InitialDirectory = this.defaultFolderProgramsName;
        //    saveFiSingleFile.Filter = filter;
        //    saveFiSingleFile.FileName = defualtProgramName;

        //    if (this.defaultFolderProgramsName == "" || askForFolder)
        //    {
        //        if (saveFiSingleFile.ShowDialog() == DialogResult.OK)
        //        {
        //            if (saveFiSingleFile.FileName != "")
        //            {
        //                SavePrg3File(saveFiSingleFile.FileName);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        SavePrg3File($"{this.defaultFolderProgramsName}\\{saveFiSingleFile.FileName}.pr3");
        //    }
        //}


    }
}
