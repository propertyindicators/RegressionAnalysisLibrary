using Accord;
using Accord.Math.Optimization.Losses;

namespace RALibrary
{
	/// <summary>
	/// Contains structure of linear regression analysis result and implements quality evaluation environment
	/// </summary>
	public class RAResultLinear : RAResultBase
	{
		private readonly RAProcessorLinear RAProcessor;

		public RAResultLinear(RAProcessorLinear initProcessor) : base(initProcessor)
		{
			RAProcessor = initProcessor;
			var tempResult = RAProcessor.GetRegression();
			double[] temp = new double[RAProcessor.GetRegression().Weights.Length + 1];
			for (int i = 1; i < RAProcessor.GetRegression().Weights.Length + 1; i++)
			{
				temp[i] = RAProcessor.GetRegression().Weights[i - 1];
			}
			temp[0] = RAProcessor.GetRegression().Intercept;
			Coefficients = temp;
			// Standard linear regression model
			Model = (coef, x) =>
			{
				var t = coef[0];
				for (int i = 0; i < x.Length; i++)
				{
					t += coef[i + 1] * x[i];
				}
				return t;
			};
		}

		/// <summary>
		/// Computes valuations (predictions based on defined model) for every observation sample input data set -- using Accord algorithm for linear models
		/// </summary>
		public override void ComputeValuations()
		{
			RAProcessor.Valuations = RAProcessor
				.GetRegression()
				.Transform(RAProcessor.Inputs);
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
		/// Computes R2 and general standard error using Accord methods for linear regression model
		/// </summary>
		protected override void ComputeR2AndStandardError()
		{
			StandardError = RAProcessor
				.GetRegression()
				.GetStandardError(RAProcessor.Inputs, RAProcessor.Outputs);
			R2 = RAProcessor
				.GetRegression()
				.CoefficientOfDetermination(RAProcessor.Inputs, RAProcessor.Outputs);
		}

		/// <summary>
		///  Computes prediction intervals for every observation sample data set -- using Accord methods for linear regression model
		/// </summary>
		protected override void ComputeStandardErrors()
		{
			StandartErrors = RAProcessor
				.GetRegression()
				.GetStandardErrors(MSE, RAProcessor.GetLeastSquares()
				.GetInformationMatrix());
		}

		/// <summary>
		/// Prediction intervals for every observation data set
		/// </summary>
		public RADoubleRange[] PredictionIntervals;

		/// <summary>
		/// Computes prediction intervals for every observation sample data set -- using Accord methods for linear regression model
		/// </summary>
		/// <param name="percentage">Range chance percentage</param>
		public void ComputePredictionIntervals(double percentage)
		{
			if (MSE == -1)
			{
				ComputeSseAndMse(); // Computing of prediction intervals needs MSE counter
			}
			PredictionIntervals = new RADoubleRange[NumberOfObservations];
			for (int i = 0; i < NumberOfObservations; i++)
			{
				DoubleRange r = RAProcessor.GetRegression().GetPredictionInterval(
					input: RAProcessor.Inputs[i],
					mse: MSE,
					numberOfSamples: NumberOfObservations,
					informationMatrix: RAProcessor.GetLeastSquares().GetInformationMatrix(),
					percent: percentage);
				PredictionIntervals[i] = new RADoubleRange(r.Min, r.Max);
			}
		}
	}
}
