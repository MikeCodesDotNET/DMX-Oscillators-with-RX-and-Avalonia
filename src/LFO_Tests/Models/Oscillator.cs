using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LFO_Tests.Models
{
    public abstract class Oscillator : ReactiveObject
    {
		protected internal int currentStep;

		private volatile bool running;
		protected float[] table;
		private IObservable<long> tick = Observable.Interval(TimeSpan.FromMilliseconds(20));

		public Oscillator()
		{
			tick.SampleEvery(() => Rate).Subscribe(_ =>
			{
				if (running)
				{					
					StepForward();
					CalcValue();	
				}
			});
		}

		public void Start()
		{
			running = true;
		}

		private double _value;
		public double Value
		{
			get => _value;
			protected set => this.RaiseAndSetIfChanged(ref _value, value);
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


		private double size;
		public double Size
		{
			get => size;
			set => this.RaiseAndSetIfChanged(ref size, value);
		}



		private double phase;
		public double Phase
		{
			get => phase;
			set
			{
				var nextStep = (int)(table.Length * value);
				if (nextStep > 128)
					currentStep = 128 - nextStep;
				else if (nextStep < 0)
					currentStep = -nextStep;
				else
					currentStep = nextStep;

				this.RaiseAndSetIfChanged(ref phase, value);
			}
		}


		public abstract void CalcValue();

		protected int StepForward(int skip = 0)
		{
			var ns = currentStep + 1;
			ns += skip;

			if (ns > table.Length -1)
				ns = 0;

			currentStep = ns;
			return ns;
		}

		public virtual void Pause()
		{
			running = false;
		}

		public virtual bool Paused
		{
			get
			{
				return !running;
			}
		}
	}
}

public static class Ext
{
	public static IObservable<T> SampleEvery<T>(this IObservable<T> source, Func<double> multipleProvider)
	{
		double counter = 0;
		Func<T, bool> predicate = ignored => {
			counter++;
			if (counter >= multipleProvider())
			{
				counter = 0;
			}
			return counter == 0;
		};
		return source.Where(predicate);
	}
}