using System;
using System.Linq;

namespace RALibrary
{
	/// <summary>
	/// Contains general structure of regression analysis result and implements quality evaluation environment
	/// </summary>
	public class RAResultBase
	{
		// Input
		public readonly RAData Data;

		public RARegressionFunction Model;

		private double[] _coefficients;

		/// <summary>
		/// Coefficients used in regression function
		/// </summary>
		public double[] Coefficients
		{
			get { return _coefficients; }
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentException("Initial data set (array) could NOT be null or empty");
				_coefficients = value;
				NumberOfCoefficients = value.Length;
			}
		}

		// Output - standard metrics of regression analysis result
		/// <summary>
		/// Sum of squared errors
		/// </summary>
		public double SSE = -1;

		/// <summary>
		/// Mean squared error (dispersion)
		/// </summary>
		public double MSE = -1;

		/// <summary>
		/// R-squared metric
		/// </summary>
		public double R2 = -1;

		/// <summary>
		/// General regression standard error
		/// </summary>
		public double StandardError = -1;

		/// <summary>
		/// Standard errors for every coefficient
		/// </summary>
		public double[] StandartErrors;

		// 
		public int NumberOfCoefficients;
		public int NumberOfObservations;

		/// <summary>
		/// Creates RAResult object with prefilled input regression analysis data and validate initial data set
		/// </summary>
		/// <param name="initData">Prefilled regression analysis data</param>
		public RAResultBase(RAData initData)
		{
			if (initData == null)
				throw new ArgumentNullException("'initData' parameter could NOT be equals null");
			if (initData.Inputs == null || initData.Inputs.Length == 0)
				throw new ArgumentException("'initData' parameter could NOT contains null or empty Inputs array");
			if (initData.Outputs == null || initData.Outputs.Length == 0)
				throw new ArgumentException("'initData' parameter could NOT contains null or empty Outputs array");
			if (initData.Inputs.Length != initData.Outputs.Length)
				throw new ArgumentException("'initData' parameter could NOT contains Inputs and Outputs arrays with not equal length)");
			Data = initData;
			NumberOfObservations = initData.Inputs.Length;
		}

		/// <summary>
		/// Computes valuations (predictions based on defined model) for every observation sample input data set using standard algorithm
		/// </summary>
		public virtual void ComputeValuations()
		{
			if (Model == null)
				throw new ArgumentException("Regression model is not defined");
			if (Data == null || Data.Inputs.Length == 0)
				throw new ArgumentException("'Data' field should contains data for computing valuations");
			Data.Valuations = new double[Data.Inputs.Length];
			for (int i = 0; i < Data.Inputs.Length; i++)
			{
				Data.Valuations[i] = Model(Coefficients, Data.Inputs[i]);
			}
		}

		/// <summary>
		/// Computes all common metrics of regression analysis result
		/// </summary>
		public virtual void ComputeRAResultMetrics()
		{
			ComputeSseAndMse();
			ComputeR2AndStandardError();
			ComputeStandardErrors();
		}

		/// <summary>
		/// Implements standard algorithm of computing SSE and MSE
		/// </summary>
		protected virtual void ComputeSseAndMse()
		{
			if (NumberOfObservations == 0)
				throw new InvalidOperationException("'Data' field should contains data for computing metrics");
			if (Data.Valuations == null || Data.Valuations.Length == 0)
			{
				ComputeValuations();
			}
			SSE = 0;
			for (int i = 0; i < NumberOfObservations; i++)
			{
				var e = Data.Valuations[i] - Data.Outputs[i];
				SSE += e * e;
			}
			MSE = SSE / NumberOfObservations;
		}

		/// <summary>
		/// Implements standard algorithm of computing R2 and general standard error
		/// </summary>
		protected virtual void ComputeR2AndStandardError()
		{
			if (NumberOfObservations == 0)
				throw new InvalidOperationException("'Data' field should contains data for computing metrics");
			if (SSE == -1)
			{
				ComputeSseAndMse(); // Computing of R2 and standard error needs SSE counter
			}
			double sumSqrDevs = 0;
			double averageY = Data.Outputs.Average();
			for (int i = 0; i < NumberOfObservations; i++)
			{
				var dev = (averageY - Data.Outputs[i]);
				sumSqrDevs += dev * dev;
			}
			R2 = 1 - SSE / sumSqrDevs;
			StandardError = Math.Pow(SSE / NumberOfObservations, 0.5) / averageY;
		}

		// Method needs implementation is subclasses with Accord functionality
		protected virtual void ComputeStandardErrors() { }
	}
}
