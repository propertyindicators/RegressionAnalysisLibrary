namespace RALibrary
{
	// These delegates are needed in order to get rid reference to Accord library in projects that use RA library

	/// <summary>
	/// Provide interface of regression function for nonlinear regressions
	/// </summary>
	/// <param name="coefficients">Coefficients (parameters) of regression function</param>
	/// <param name="input">Input data set of regression analysis</param>
	/// <returns></returns>
	public delegate double RARegressionFunction(double[] coefficients, double[] input);

	/// <summary>
	/// Provide interface of gradient function for nonlinear regressions
	/// </summary>
	/// <param name="coefficients">Coefficients (parameters) of regression function</param>
	/// <param name="input">Input data set of regression analysis</param>
	/// <param name="result">Gradient function vector</param>
	public delegate void RARegressionGradientFunction(double[] coefficients, double[] input, double[] result);
}
