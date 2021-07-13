using LFO_Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive.Linq;

namespace LFO_Tests.ViewModels
{

    public class OscillatorPropertiesViewModel : ViewModelBase
    {
        int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); AssignOscillator(); }
        }

        void AssignOscillator()
        {
            switch (SelectedIndex)
            {
                case (0): //Linear
                    return;
                case (1): //Sqaure
                    return;
                case (2): //Triangle
                    return;
                case (3): //Sawtooth
                    return;
                case (4): //Sine
                    SelectedOscillator = new Sine(Rate);
                    return;
                case (5): //Cosine
                    return;
                case (6): //Sine 2x
                    return;
                case (7): //Rainbow Cyan
                    return;
                case (8): //Rainbow Magenta
                    return;
                case (9): //Rainbow Yellow
                    return;
                case (10): //Sparkle 1
                    return;
                case (11): //Sparkle 2
                    return;
                case (12): //Sparkle 3
                    return;
                case (13): //Triangle Off Zero
                    return;
                case (14): //Sine Off Zero
                    return;
                case (15): //Inverted Cos Off Zero
                    return;
                case (16): //Ramp
                    return;
                case (17): //Random
                    return;
            }           
        }

        public OscillatorPropertiesViewModel()
        {
            Size = 1.0;
            Rate = 1;
            Offset = 0;
            Phase = 0;

            this.WhenAnyValue(x => x.IsRunning).Where(x => SelectedOscillator != null).Where(x => x == true).Subscribe(x => SelectedOscillator.Start());
            this.WhenAnyValue(x => x.IsRunning).Where(x => SelectedOscillator != null).Where(x => x == false).Subscribe(x => SelectedOscillator.Pause());

            this.WhenAnyValue(x => x.Rate).Where(x => SelectedOscillator != null).Subscribe(x => SelectedOscillator.Rate = x);
            this.WhenAnyValue(x => x.Offset).Where(x => SelectedOscillator != null).Subscribe(x => SelectedOscillator.Offset = x);
            this.WhenAnyValue(x => x.Phase).Where(x => SelectedOscillator != null).Subscribe(x => SelectedOscillator.Phase = x);
            this.WhenAnyValue(x => x.Size).Where(x => SelectedOscillator != null).Subscribe(x => SelectedOscillator.Size = x);

        }



        Oscillator selectedOscillator;
        public Oscillator SelectedOscillator
        {
            get => selectedOscillator;
            set => this.RaiseAndSetIfChanged(ref selectedOscillator, value);
        }


        private double rate;
        public double Rate
        {
            get => rate;
            set => this.RaiseAndSetIfChanged(ref rate, value);
        }

        private double offset;
        public double Offset
        {
            get => offset;
            set => this.RaiseAndSetIfChanged(ref offset, value);
        }

        private double phase;
        public double Phase
        {
            get => phase;
            set => this.RaiseAndSetIfChanged(ref phase, value);
        }

        private double size;
        public double Size
        {
            get => size;
            set => this.RaiseAndSetIfChanged(ref size, value);
        }

        private bool running;
        public bool IsRunning
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }
    }
}
