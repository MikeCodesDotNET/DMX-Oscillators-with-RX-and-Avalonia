using Avalonia.Controls;
using LFO_Tests.Helpers;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LFO_Tests.Models
{
    public class ControllableRange : ReactiveObject
    {
        double minimum;
        public double Minimum
        {
            get => minimum;
            set
            {
                if (value.GreaterThan(Maximum))
                    return;

                this.RaiseAndSetIfChanged(ref minimum, value);
            }
        }

        double maximum;
        public double Maximum
        {
            get => maximum;
            set
            {
                if (value.LessThan(Minimum))
                    return;

                this.RaiseAndSetIfChanged(ref maximum, value);
            }
        }

        double clampedValue;
        public double Value
        {
            get => clampedValue;
            set
            {
                if (IsParked)
                    return;
                var cv = value.Clamp(Minimum, Maximum);
                this.RaiseAndSetIfChanged(ref clampedValue, cv);
                ReadOut = ToString();
                ValueSubject.OnNext(cv);
            }
        }

        double defaultValue;
        public double DefaultValue
        {
            get => defaultValue;
            set
            {               
                var clampedValue = value.Clamp(Minimum, Maximum);
                this.RaiseAndSetIfChanged(ref defaultValue, clampedValue);
            }
        }

        bool isParked;
        public bool IsParked
        {
            get => isParked;
            set => this.RaiseAndSetIfChanged(ref isParked, value);
        }

        string title;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        string readOut;
        public string ReadOut
        {
            get => readOut;
            private set => this.RaiseAndSetIfChanged(ref readOut, value);
        }

           
        DisplayUnit displayUnit;
        public DisplayUnit DisplayUnit
        {
            get => displayUnit;
            set
            {
                this.RaiseAndSetIfChanged(ref displayUnit, value);
                this.ReadOut = this.ToString();
            }
        }


        internal Subject<double> ValueSubject { get; private set; }

        public ControllableRange(string title, double defaultValue = 0, DisplayUnit du = DisplayUnit.Percentage, double min = 0, double max = 1)
        {
            ValueSubject = new Subject<double>();

            Minimum = min;
            Maximum = max;
            DefaultValue = defaultValue;
            Title = title;
            DisplayUnit = du;

            SetDisplayUnitCommand = ReactiveCommand.Create<string>(SetDisplayUnit);
            ResetValueCommand = ReactiveCommand.Create(ResetValue);
            AddOscillatorCommand = ReactiveCommand.Create(AddOscillator);
            RemoveOscillatorCommand = ReactiveCommand.Create(RemoveOscillator);

            this.WhenAnyValue(x => x.Oscillator.Value).Where(x => Oscillator != null).Subscribe(x => UpdateValueFromOscillator(x));

            ResetValue();
        }

        IObservable<double> d1 = null;
        IObservable<double> d2 = null;


        void UpdateValueFromOscillator(double oscillatorValue)
        {
            var scaled = NumericHelpers.Scale(oscillatorValue, 0.0, Oscillator.Size, -1.0, 1.0);
            System.Diagnostics.Debug.WriteLine($"Value: {oscillatorValue} - {scaled}");
            Value = scaled;
        }

        public ControllableRange() : this(string.Empty, 0)
        {
            
        }

        public ReactiveCommand<string, Unit> SetDisplayUnitCommand { get; private set; }

        public ReactiveCommand<Unit, Unit> ResetValueCommand { get; private set; }

        public ReactiveCommand<Unit, Unit> AddOscillatorCommand { get; private set; }

        public ReactiveCommand<Unit, Unit> RemoveOscillatorCommand { get; private set; }



        private Oscillator oscillator;
        public Oscillator Oscillator
        {
            get => oscillator;
            set => this.RaiseAndSetIfChanged(ref oscillator, value);
        }



        void AddOscillator()
        {
            Oscillator = new Sine(1.0);
        }

        void RemoveOscillator()
        {
            Oscillator = null;
        }



        void ResetValue()
        {
            Value = DefaultValue;
        }

        void SetDisplayUnit(string parameter)
        {
            Enum.TryParse(parameter, out DisplayUnit du);
            DisplayUnit = du;
        }

        public override string ToString()
        {
            switch (DisplayUnit)
            {
                case DisplayUnit.Percentage:
                    return Value.ToString("P0");
                    
                case DisplayUnit.Hex8:
                    return Helpers.NumericHelpers.ScaleTo8BitHex(Value);

                case DisplayUnit.Hex16:
                    return Helpers.NumericHelpers.ScaleTo16BitHex(Value);

                case DisplayUnit.Byte:
                    return Helpers.NumericHelpers.ScaleTo8Bit(Value).ToString();

                case DisplayUnit.Short:
                    return Helpers.NumericHelpers.ScaleTo16Bit(Value).ToString();
                case DisplayUnit.Original:
                    return Value.ToString();

                default:
                    return base.ToString();

            }
        }
    }

    public enum DisplayUnit
    {
        Percentage,
        Byte,
        Short,

        Hex8,
        Hex16,

        Hex8Bit,
        Hex16Bit, 

        Original,
    }
}
