using Accord.Statistics.Models.Regression.Linear;

namespace RALibrary
{
	/// <summary>
	/// Implement functionality of processing linear regression analysis
	/// </summary>
	public class RAProcessorLinear : RAData
	{
		private RAResultLinear RAResult;

		// Accord tools
		private MultipleLinearRegression Regression;
		private readonly OrdinaryLeastSquares LeastSquares = new OrdinaryLeastSquares() { UseIntercept = true };

		/// <summary>
		/// Create new RAData object with prefilled RAData object
		/// </summary>
		/// <param name="init">Initial RAData object</param>
		public RAProcessorLinear(RAData init) : base(init) { }

		public void ComputeRAResult()
		{
			Regression = LeastSquares.Learn(Inputs, Outputs);
			RAResult = new RAResultLinear(this);
			RAResult.ComputeValuations();
		}

		// Getters of fields that could not be changed outside
		public RAResultLinear GetRAresult()
		{
			return RAResult;
		}

		public OrdinaryLeastSquares GetLeastSquares()
		{
			return LeastSquares;
		}

		public MultipleLinearRegression GetRegression()
		{
			return Regression;
		}
	}
}
