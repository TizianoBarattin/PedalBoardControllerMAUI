using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;

namespace PedalBoardController.Classes
{
    public class MidiInterface
    { 
        OutputDevice outputDevice;
        private InputDevice inputDevice;
        bool outputDisposed = true;
        string lastOutputDevice = "";
        bool inputDisposed = true;
        string lastInputDevice = "";
        MidiEvent dataFromMidi = null;
        public MidiEvent _midiEvent { get; set; }

        public MidiInterface()
        {

        }

        public List<string> GetMidiInputs()
        {
            List<string> midiInputs = new List<string>();

            foreach (var inputDevice in InputDevice.GetAll())
            {
                midiInputs.Add(inputDevice.Name);
            }

            return midiInputs;
        }
        public List<string> GetMidiOutputs()
        {
            List<string> midiOutputs = new List<string>();

            foreach (var outputDevice in OutputDevice.GetAll())
            {
                midiOutputs.Add(outputDevice.Name);
            }

            return midiOutputs;
        }
        public bool SendSysEx(string outputMidiDevice, bool keepOutputToDispose, byte[] data)
        {
            bool res = true;

            if (outputDisposed || lastOutputDevice != outputMidiDevice)
            {
                if (lastOutputDevice != outputMidiDevice && lastOutputDevice != "")
                {
                    outputDevice.Dispose();
                    outputDisposed = true;
                    lastOutputDevice = "";
                }

                outputDevice = OutputDevice.GetByName(outputMidiDevice);
                lastOutputDevice = outputMidiDevice;
                outputDisposed = false;
            }

            try
            {
                NormalSysExEvent message = new NormalSysExEvent(data)
                {
                };

                outputDevice.SendEvent(message);
            }
            catch (Exception ex)
            {
                res = false;
            }

            if (!keepOutputToDispose)
            {
                outputDevice.Dispose();
                outputDisposed = true;
                lastOutputDevice = "";
            }

            return res;
        }

        public async Task<MidiEvent> ReceiveMidiData(string inputMidiDevice, bool keepInputToDispose)
        {
            dataFromMidi = null;

            if (inputDisposed || lastInputDevice != inputMidiDevice)
            {
                if (lastInputDevice != inputMidiDevice && lastInputDevice != "")
                {
                    inputDevice.Dispose();
                    inputDisposed = true;
                    lastInputDevice = "";
                }

                inputDevice = InputDevice.GetByName(inputMidiDevice);
                lastInputDevice = inputMidiDevice;
                inputDisposed = false;
            }

            inputDevice.EventReceived+= OnEventReceived;

            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));

            Task task = Task.Run(() => StartListen(source.Token));

            while (!source.Token.IsCancellationRequested && dataFromMidi == null)
            {

            }

            source.Cancel();        //=> when timeout or data written go on
            return dataFromMidi;
        }

        private async Task StartListen(CancellationToken cancellationToken)
        {
            Task task = Task.Run(() => inputDevice.StartEventsListening());
            await task;
        }

        private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            dataFromMidi = e.Event;
        }

        public bool SendControlChange(decimal channel, int controlNumber, int value, string outputMidiDevice, bool keepOutputToDispose)
        {
            if (channel > 0 && channel <= 16 && controlNumber >= 0 && controlNumber <= 127 && value >= 0 && value <= 127 && outputMidiDevice != "")
            {

                //TODO: differenziare i vari casi di problema? 
                //TODO: capire se il dispostivo fa parte dei presenti e se ancora connesso

                try
                {
                    ///You must always take care about disposing an OutputDevice, so use it inside 
                    ///using block or call Dispose manually.Without it all resources taken by the 
                    ///device will live until GC collect them via finalizer of the OutputDevice.
                    ///It means that sometimes you will not be able to use different instances of 
                    ///the same device across multiple applications or different pieces of a program.
                    ///

                    //using (var outputDevice = OutputDevice.GetByName(outputMidiDevice))
                    //{
                    //var outputDevice = OutputDevice.GetByName(outputMidiDevice);

                    if (outputDisposed || lastOutputDevice != outputMidiDevice)
                    {
                        if (lastOutputDevice != outputMidiDevice && lastOutputDevice != "")
                        {
                            outputDevice.Dispose();
                            outputDisposed = true;
                            lastOutputDevice = "";
                        }

                        outputDevice = OutputDevice.GetByName(outputMidiDevice);
                        lastOutputDevice = outputMidiDevice;
                        outputDisposed = false;
                    }

                    ControlChangeEvent message = new ControlChangeEvent((SevenBitNumber)controlNumber, (SevenBitNumber)value) 
                    {
                        Channel = (FourBitNumber)(channel-1)    //-1 perché se do come canale 0 la libreria butta fuori messagio su canale 1
                    };
                    
                    outputDevice.SendEvent(message);


                    if (!keepOutputToDispose)
                    {
                        outputDevice.Dispose();
                        outputDisposed = true;
                        lastOutputDevice= "";
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
            }
            else
            {
                return false;
            }
        }
        
    }
}
