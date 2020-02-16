using System;

namespace RALibrary
{
	/// <summary>
	/// Standard data set for regression analysis
	/// </summary>
	public class RAData
	{
		public double[][] Inputs;
		public double[] Outputs;
		public double[] Valuations;

		public RAData() { } // Base constructor is needed for supporting clone functionality

		/// <summary>
		/// Creates new RAData object that refers to already existing RAData object
		/// </summary>
		/// <param name="initRAData">Initial RAData object</param>
		public RAData(RAData initRAData)
		{
			Inputs = initRAData.Inputs;
			Outputs = initRAData.Outputs;
			Valuations = initRAData.Valuations;
		}


		/// <summary>
		/// Transforms column of Inputs data set with predefined rule
		/// </summary>
		/// <param name="func">Transform function</param>
		public void InputsTransform(int colIndex, Func<double, double> func)
		{
			if (Inputs == null)
				throw new InvalidOperationException("Could not transform column of null or empty Inputs data set");
			if (colIndex > Inputs[0].Length)
				throw new ArgumentException("'colIndex' parameter is out of range");
			foreach (double[] m in Inputs)
			{
				m[colIndex] = func(m[colIndex]);
			}
		}

		/// <summary>
		/// Transforms Outputs array with predefined rule
		/// </summary>
		/// <param name="func">Transform function</param>
		public void OutputsTransform(Func<double, double> func)
		{
			if (Outputs == null)
				throw new InvalidOperationException("Could not transform null or empty Outputs array");
			for (int i = 0; i < Outputs.Length; i++)
			{
				Outputs[i] = func(Outputs[i]);
			}
		}

		/// <summary>
		/// Clones RAData object
		/// </summary>
		public RAData CloneData()
		{
			// Inputs
			RAData temp = new RAData();
			if (Inputs != null)
			{
				if (Inputs.Length != 0)
				{
					temp.Inputs = new double[Inputs.Length][];
					for (int i = 0; i < Inputs.Length; i++)
					{
						temp.Inputs[i] = (double[])Inputs[i].Clone();
					}
				}
				else
				{
					temp.Inputs = (double[][])Inputs.Clone();
				}
			}
			// Outputs
			if (Outputs != null)
			{
				temp.Outputs = (double[])Outputs.Clone();
			}
			// Valuations
			if (Valuations != null)
			{
				temp.Valuations = (double[])Valuations.Clone();
			}
			return temp;
		}
	}
}
