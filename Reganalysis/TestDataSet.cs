using RALibrary;

namespace Reganalysis
{
	static class TestSet
	{
		/// <summary>
		/// The data set displays 30 observations of approximation of a function y=k0*k1^ln(x0)*k2^x1*k3^x2 
		/// (coefficient function with logarithm of the first parameter) 
		/// with some slight random error.
		/// </summary>
		public static RAData Data = new RAData()
		{
			Outputs = new double[]
				{27,33,106,113,173,58,195,20,254,225,7,150,31,47,40,38,104,33,32,6,20,154,65,134,29,33,70,395,15,191},

			Inputs = new[]
			{
				new double[] {9,11,6},
				new double[] {12,20,12},
				new double[] {7,16,15},
				new double[] {16,18,19},
				new double[] {17,2,12},
				new double[] {1,7,1},
				new double[] {3,18,17},
				new double[] {17,14,7},
				new double[] {19,3,15},
				new double[] {14,2,13},
				new double[] {20,19,2},
				new double[] {5,9,12},
				new double[] {5,16,8},
				new double[] {12,15,12},
				new double[] {17,15,11},
				new double[] {6,17,10},
				new double[] {2,19,13},
				new double[] {7,20,11},
				new double[] {15,10,7},
				new double[] {13,18,1},
				new double[] {7,18,7},
				new double[] {19,11,17},
				new double[] {6,7,7},
				new double[] {13,4,11},
				new double[] {15,10,7},
				new double[] {4,8,3},
				new double[] {16,4,8},
				new double[] {11,3,16},
				new double[] {10,12,1},
				new double[] {7,5,12}
			}
		};
	}
}
