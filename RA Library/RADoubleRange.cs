using Accord;

namespace RALibrary
{
	// This class is needed in order to get rid reference to Accord library in projects that use RA library

	/// <summary>
	/// Wrap Accord.DoubleRange structure
	/// </summary>
	public class RADoubleRange
	{
		private DoubleRange r;
		public double Min { get { return r.Min; } set { r.Min = value; } }
		public double Max { get { return r.Max; } set { r.Max = value; } }

		public RADoubleRange(double min, double max)
		{
			Min = min;
			Max = max;
		}

		public bool IsInside(double v)
		{
			return r.IsInside(v);
		}
	}
}
