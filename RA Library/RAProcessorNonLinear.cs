using Accord.Math.Optimization;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;

namespace RALibrary
{
	/// <summary>
	/// Implement functionality of processing nonlinear regression analysis
	/// </summary>
	public class RAProcessorNonLinear : RAData//, IRAEngine
	{
		private RAResultNonLinear RAResult;

		// Accord tools
		private NonlinearRegression Regression;
		private NonlinearLeastSquares LeastSquares = new NonlinearLeastSquares()
		{
			Algorithm = new LevenbergMarquardt()
			{
				MaxIterations = 10000,
				Tolerance = 0
			}
		};

		/// <summary>
		/// Create new RAData object with prefilled RAData object
		/// </summary>
		/// <param name="init">Initial RAData object</param>
		public RAProcessorNonLinear(RAData init) : base(init) { }

		public void ComputeRAResult()
		{
			Regression = LeastSquares.Learn(Inputs, Outputs);
			RAResult = new RAResultNonLinear(this);
			RAResult.ComputeValuations();
		}

		/// <summary>
		/// Set model and learning adjustment for nonlinear regression analysis from already existing RA processor
		/// </summary>
		public void SetModel(RAProcessorNonLinear prototype)
		{
			LeastSquares = prototype.LeastSquares;
		}

		public void SetNumberOfParametrs(int n)
		{
			LeastSquares.NumberOfParameters = n;
		}

		/// <summary>
		/// Set model and learning adjustment for nonlinear regression analysis 
		/// </summary>
		/// <param name="f">model (w,x)=> </param>
		/// <param name="g">gradient (w,x,r)=> </param>
		/// <param name="n">Number of regression coefficients (parameters)</param>
		/// <param name="v">Array of start values of learning for every regression coefficient</param>
		public void SetModel(RARegressionFunction f, RARegressionGradientFunction g, int n, double[] v = null)
		{
			LeastSquares.Function = (k,x) => f(k,x);
			LeastSquares.Gradient = (k,x,r)=> g(k,x,r);
			LeastSquares.NumberOfParameters = n;
			if (v != null)
			{
				LeastSquares.StartValues = v;
			}
		}

		// Getters of fields that could not be changed outside
		public NonlinearRegression GetTempResult()
		{
			return Regression;
		}

		public NonlinearLeastSquares GetLeastSquares()
		{
			return LeastSquares;
		}

		public RAResultNonLinear GetRAresult()
		{
			return RAResult;
		}
	}
 }
