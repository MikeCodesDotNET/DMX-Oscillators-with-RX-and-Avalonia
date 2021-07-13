using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.Models.SignalGenerators
{
    public interface ISignalGenerator 
    {
        /// <summary>
        /// A signal value ranges from 0 - 1
        /// </summary>
        ISubject<double> ValueSubject { get; }

    }
}
