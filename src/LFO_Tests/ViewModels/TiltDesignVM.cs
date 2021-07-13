using LFO_Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.ViewModels
{
    public class TiltDesignVM : ControllableRange
    {
        public TiltDesignVM()
        {
            Title = "Tilt";
            Minimum = 0;
            Maximum = 1;
            Value = 0.5d;
            DefaultValue = 0.5d;
            DisplayUnit = DisplayUnit.Hex16;
        }
    }
}
