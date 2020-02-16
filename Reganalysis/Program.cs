using System;
using System.Collections.Generic;
using RALibrary;

namespace Reganalysis
{
	// This test code uses the main functionality of RA Library.
	// Also implements experiment that demonstrates advantages of nonlinear regression analysis
	// vs linear regression analysis on example nonlinear model that could be transformed
	// to linear model: y=k0*k1^ln(x0)*k2^x1*k3^x2 <=> ln(y)=ln(k0)+ln(x0)*ln(k1)+x1*ln(k2)+x2*ln(k3).

	// Test algorithm:
	// 1) Analyze test dataset using tools for the nonlinear models.
	// 2) Transform test dataset to format for linear regression and analyze with tools for the linear models.
	// 3) Create RAResultBase with nonlinear the model but with model coefficients obtained using linear regression analysis.
	// 4) Compare results obtained in 1) vs results obtained in 3) with consistency (based on initial nonlinear model).

	// Conclusions:
	// A) RA Library wraps and significantly simplifies interfaces of regression analysis throw the Accord library.
	// B) Direct nonlinear analysis produce significantly more quality (accurate) result than analysis of adapted (to linear form) regression model.

	static class Program
	{
		private static void Main()
		{
			double baseNonlinearModel(double[] k, double[] x) => k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]);
			// 1) Analyze test data using tools for nonlinear model
			RAProcessorNonLinear nlra = new RAProcessorNonLinear(TestSet.Data);
			nlra.SetModel(
						f: baseNonlinearModel,
						g: (k, x, r) =>
						{ //gradient function 
							r[0] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]);
							r[1] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * Math.Log(x[0]) / k[1];
							r[2] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * x[1] / k[2];
							r[3] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * x[2] / k[3]; 
						},
						n: 4,//number of observed coefficients
						v: new[] { 150.0, 1, 1, 1 });
			nlra.ComputeRAResult();
			RAResultNonLinear nlres = nlra.GetRAresult();
			nlres.ComputeRAResultMetrics();
			PrintRAResult(nlres, "Non-linear analysis processing result:");

			// 2) Transform test dataset to format for linear regression and analyze with tools linear model
			RAData linedata = TestSet.Data.CloneData(); // Clone base test data
			linedata.OutputsTransform(Math.Log); //perform the logarithm of y
			linedata.InputsTransform(0, Math.Log);//perform the logarithm of first parametr
			RAProcessorLinear lra = new RAProcessorLinear(linedata);
			lra.ComputeRAResult();
			RAResultBase lres = lra.GetRAresult();
			lres.ComputeRAResultMetrics();
			PrintRAResult(lres, "Linear analysis processing result:");

			// 3) Create RAResultBase with nonlinear model but with model coefficients obtained using linear regression analysis
			RAResultBase checklra = new RAResultBase(TestSet.Data)
			{
				Coefficients = new double[]
				{
					Math.Exp(lres.Coefficients[0]),
					Math.Exp(lres.Coefficients[1]),
					Math.Exp(lres.Coefficients[2]),
					Math.Exp(lres.Coefficients[3])
				},
				Model = baseNonlinearModel
			};
			checklra.ComputeValuations();
			checklra.ComputeRAResultMetrics();
			PrintRAResult(checklra, "Non-linear analysis metrics with coefficients obtained with linear analysis:");

			Console.WriteLine("Test is passed.");
			Console.ReadKey();
		}

		public static void PrintRAResult(RAResultBase result, string title)
		{
			List<string> coefs = new List<string>();
			for (int i = 0; i < result.Coefficients.Length; i++) { coefs.Add("k" + i.ToString() + "=" + result.Coefficients[i].ToString()); }
			Console.WriteLine(title);
			Console.WriteLine("Coefficients:  " + String.Join("  ", coefs));
			Console.WriteLine();
			Console.WriteLine("SSE = " + result.SSE.ToString());
			Console.WriteLine("MSE = " + result.MSE.ToString());
			Console.WriteLine("R2 = " + result.R2.ToString());
			Console.WriteLine("St.Error = " + result.StandardError.ToString());
			Console.WriteLine();
			Console.WriteLine("valuations - outputs:");
			for (int i = 0; i < result.Data.Valuations.Length; i++)
			{ Console.WriteLine(result.Data.Valuations[i].ToString() + " - " + result.Data.Outputs[i].ToString()); }
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
