using kadmium_sacn_core;
using System;
using ReactiveUI;
using Avalonia.Controls;
using System.Reactive.Linq;
using System.Windows.Input;
using LFO_Tests.Models;
using System.Collections.ObjectModel;
using LFO_Tests.Views;
using System.Collections.Generic;

namespace LFO_Tests.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Dictionary<ControllableRange, OscillatorPropertiesViewModel> oscillatorVms = new Dictionary<ControllableRange, OscillatorPropertiesViewModel>();

        public ICommand DefaultValuesCommand { get; private set; }
        public ICommand StartOutputCommand { get; private set; }


        private ControllableRange selectedItem;
        public ControllableRange SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }


        public ObservableCollection<ControllableRange> Controllables { get; private set; } = new ObservableCollection<ControllableRange>();


        private bool oscillatorPropertiesIsVisible;
        public bool OscillatorPropertiesIsVisible
        {
            get => oscillatorPropertiesIsVisible;
            set => this.RaiseAndSetIfChanged(ref oscillatorPropertiesIsVisible, value);
        }


        private OscillatorPropertiesViewModel oscillatorPropertiesViewModel;
        public OscillatorPropertiesViewModel OscillatorPropertiesViewModel
        {
            get => oscillatorPropertiesViewModel;
            set => this.RaiseAndSetIfChanged(ref oscillatorPropertiesViewModel, value);
        }


        private byte rateSliderValue;
        public byte RateSliderValue
        {
            get => rateSliderValue;
            set => this.RaiseAndSetIfChanged(ref rateSliderValue, value);
        }


        public MainWindowViewModel()
        {            
            DefaultValuesCommand = ReactiveCommand.Create(ToDefault);
            StartOutputCommand = ReactiveCommand.Create(StartOutput);
            this.WhenAnyValue(x => x.SelectedItem).Where(x => x != null).Subscribe(x =>
            {
                OscillatorPropertiesViewModel vm;
                if (oscillatorVms.ContainsKey(x))
                {
                    vm = oscillatorVms[x];
                }
                else
                {
                    vm = new OscillatorPropertiesViewModel();
                    oscillatorVms.Add(x, vm);
                }
                OscillatorPropertiesViewModel = vm;
            });

            this.WhenAnyValue(x => x.SelectedItem).Where(x => x != null).Select(x => x.Oscillator != null).Subscribe(x => OscillatorPropertiesIsVisible = x);
 

            this.WhenAnyValue(x => x.OscillatorPropertiesViewModel.SelectedOscillator).Where(x => SelectedItem != null).Subscribe(x => SelectedItem.Oscillator = x);

            ToDefault();

            RateSliderValue = 5;
            var valueChangeObs = PopulateSliders();
            valueChangeObs.Where(x => started == true).Sample(TimeSpan.FromMilliseconds(20)).Do(x => Log($"Value did change: {x}")).Subscribe(_ => RecalculateOutput());           
        }

        void Log(string message)
        {
            Console.WriteLine(message);
        }

        bool started;
        void StartOutput()
        {
            if (sender == null)
                sender = new SACNSender(Guid.NewGuid(), "sacn");

            data = new byte[Controllables.Count];

            started = true;
            var ticker = Observable.Interval(TimeSpan.FromMilliseconds(20)).Where(_ => started == true).Subscribe(_ => SendOutput());
        }



        IObservable<double> PopulateSliders()
        {
            IObservable<double> obserable;

            var shutter = new ControllableRange("Shutter", 0.9);
            obserable = shutter.ValueSubject.AsObservable();
            Controllables.Add(shutter);

            var dimmer = new ControllableRange("Dimmer", 1.0);
            obserable = obserable.Merge(dimmer.ValueSubject.AsObservable());
            Controllables.Add(dimmer);

            var color = new ControllableRange("Color");
            obserable = obserable.Merge(color.ValueSubject.AsObservable());
            Controllables.Add(color);

            var goboS = new ControllableRange("Gobo S");
            obserable = obserable.Merge(goboS.ValueSubject.AsObservable());
            Controllables.Add(goboS);

            var goboR = new ControllableRange("Gobo R");
            obserable = obserable.Merge(goboR.ValueSubject.AsObservable());
            Controllables.Add(goboR);

            var focus = new ControllableRange("Focus");
            obserable = obserable.Merge(focus.ValueSubject.AsObservable());
            Controllables.Add(focus);

            var prism = new ControllableRange("Prism");
            obserable = obserable.Merge(prism.ValueSubject.AsObservable());
            Controllables.Add(prism);

            var pan = new ControllableRange("Pan", 0.5);
            obserable = obserable.Merge(pan.ValueSubject.AsObservable());
            Controllables.Add(pan);

            var tilt = new ControllableRange("Tilt", 0.5);
            obserable = obserable.Merge(tilt.ValueSubject.AsObservable());
            Controllables.Add(tilt);
                    
            return obserable;
        }

      

        byte ToByte(double d)
        {
            //can be -1 to 1. 0 is center 
            var scaled = Helpers.NumericHelpers.Scale(d, 0, 255);
            return (byte)scaled;
        }

   

        byte[] data;
        SACNSender sender;

        void RecalculateOutput()
        {         
            if (Design.IsDesignMode == false)
            {
                data = new byte[Controllables.Count];
                for (int i = 0; i < Controllables.Count; i++)
                {
                    data[i] = Helpers.NumericHelpers.ScaleTo8Bit(Controllables[i].Value);
                }
            }
        }

        void SendOutput()
        {
            sender.Send(1, data);
        }


        void ToDefault()
        {
            foreach (var c in Controllables)
            {
                c.ResetValueCommand.Execute();
            }
        }

       
    }
}
