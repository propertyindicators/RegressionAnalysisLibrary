using Accord.Math.Optimization.Losses;

namespace RALibrary
{
	/// <summary>
	/// Contains structure of non-linear regression analysis result and implements quality evaluation environment
	/// </summary>
	public class RAResultNonLinear : RAResultBase
	{
		private readonly RAProcessorNonLinear RAProcessor;

		public RAResultNonLinear(RAData initData) : base(initData)
		{
			RAProcessor = new RAProcessorNonLinear(initData);
		}

		public RAResultNonLinear(RAProcessorNonLinear initProcessorWithData) : base(initProcessorWithData)
		{
			RAProcessor = initProcessorWithData;
			Coefficients = RAProcessor.GetTempResult().Coefficients;
			Model = (x, y) => RAProcessor.GetTempResult().Function(x, y);
		}

		/// <summary>
		/// Computes valuations (predictions based on defined model) for every observation sample input data set -- using Accord algorithm for non-linear models
		/// </summary>
		public override void ComputeValuations()
		{
			RAProcessor.Valuations = RAProcessor.GetTempResult().Transform(RAProcessor.Inputs);
		}

		/// <summary>
		/// Computes SSE and MSE using Accord functionality
		/// </summary>
		protected override void ComputeSseAndMse()
		{
			SSE = (new SquareLoss(RAProcessor.Outputs) { Mean = false })
				.Loss(RAProcessor.Valuations);
			MSE = SSE / NumberOfObservations;
		}

		/// <summary>
		/// Computes prediction intervals for every observation sample data set -- using Accord methods for linear regression model
		/// </summary>
		protected override void ComputeStandardErrors()
		{
			StandartErrors = RAProcessor.GetTempResult().StandardErrors;
		}
	}
}
