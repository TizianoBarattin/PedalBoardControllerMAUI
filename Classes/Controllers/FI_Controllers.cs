using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiController.Classes.Controllers
{
    public class FI_Controllers
    {
        public string ControllerName;
        public int ActualValue;
        public int MinValue;
        public int MaxValue;
        public int CC;
        public int DefaultValue;
        public int MinCmd;
        public int ActualOutput;
        public bool EnumPresence;
        public string EnumsText;
        public int field;
        public int poti;
        public int sNumber;
        public int BlockNum;
        public int OffsetMidi;
        public int MinValueCmd;

        static int ControllerIndex;

        //Fi controller generico
        public FI_Controllers(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue)
        {
            GestioneGenericaPubblicazioneControllo(controllerName, field, poti, sNumber, blocknum, cc, minValue, maxValue, defaultValue);
            MinCmd = 0;
            EnumPresence = false;
            ControllerIndex++;
            OffsetMidi = 0;
        }

        //Fi controller generico + enumeratore
        public FI_Controllers(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue, string valueEnums)
        {
            //TODO: gestisci i valori con descrizione
            GestioneGenericaPubblicazioneControllo(controllerName, field, poti, sNumber, blocknum, cc, minValue, maxValue, defaultValue);
            MinCmd = 0;
            EnumPresence = true;
            EnumsText = valueEnums;
            ControllerIndex++;
            OffsetMidi = 0;
        }

        //Fi controller generico + enumeratore + inviano il messagio midi con un offset
        public FI_Controllers(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue, string valueEnums, int offsetToMidi)
        {
            //TODO: gestisci i valori con descrizione
            GestioneGenericaPubblicazioneControllo(controllerName, field, poti, sNumber, blocknum, cc, minValue, maxValue, defaultValue);
            MinCmd = 0;
            EnumPresence = true;
            EnumsText = valueEnums;
            ControllerIndex++;
            OffsetMidi = offsetToMidi;
        }

        //Fi controller che con valore minimo non comandano al CC "0" ma un altro numero
        public FI_Controllers(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue, int minValueCmd)
        {
            //TODO: gestisci il valore 0 con comando <> da 0
            GestioneGenericaPubblicazioneControllo(controllerName, field, poti, sNumber, blocknum, cc, minValue, maxValue, defaultValue);
            MinCmd = minValueCmd;
            EnumPresence |= false;
            ControllerIndex++;
        }

        //Fi controller che con valore minimo non comandano al CC "0" ma un altro numero e inviano il messagio midi con un offset
        public FI_Controllers(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue, int minValueCmd, int offsetToMidi)
        {
            //TODO: gestisci il valore 0 con comando <> da 0
            GestioneGenericaPubblicazioneControllo(controllerName, field, poti, sNumber, blocknum, cc, minValue, maxValue, defaultValue);
            MinCmd = minValueCmd;
            EnumPresence |= false;
            ControllerIndex++;
            OffsetMidi = offsetToMidi;
            MinValueCmd = minValueCmd;
        }

        public void GestioneGenericaPubblicazioneControllo(string controllerName, int field, int poti, int sNumber, int blocknum, int cc, int minValue, int maxValue, int defaultValue)
        {
            ControllerName = controllerName;
            ActualValue = MinValue; //this.DefaultValue = defaultValue; x debug
            MinValue = minValue;
            MaxValue = maxValue;
            CC = cc;
            this.field = field;
            this.poti = poti;
            this.sNumber = sNumber;
            BlockNum = blocknum;

        }

    }
}
