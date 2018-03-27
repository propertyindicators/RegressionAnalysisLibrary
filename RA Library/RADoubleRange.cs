using Accord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    /// <summary>
    /// класс - обёртка для структуры Accord.DoubleRange
    /// </summary>
    public class RADoubleRange
    {
        DoubleRange r;
        public double Min { get { return r.Min; } set { r.Min = value; } }
        public double Max { get { return r.Max; } set { r.Max = value; } }
        public RADoubleRange(double min, double max) { Min = min; Max = max; }
        public bool IsInside(double v) { return r.IsInside(v); }
    }
}
