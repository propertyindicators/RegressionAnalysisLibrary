using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    /// <summary>
    /// интерфейс метода получения результата реганализа
    /// </summary>
    public interface IRAEngine
    {
        void computeRAresult();
    }

    /// <summary>
    /// Делегат регрессионной функции с типичным интерфейсом вызова
    /// </summary>
    /// <param name="coefficients">массив коэффициентов регрессионного уравнения</param>
    /// <param name="input">массив входных параметров регрессионного уравнения</param>
    /// <returns></returns>
    public delegate double RARegressionFunction(double[] coefficients, double[] input);

    /// <summary>
    /// Делегат функции градиента регрессионной функции с типичным интерфейсом вызова
    /// </summary>
    /// <param name="coefficients">массив коэффициентов регрессионного уравнения</param>
    /// <param name="input">массив входных параметров регрессионного уравнения</param>
    /// <param name="result">функциональный вектор функции градиента</param>
    public delegate void RARegressionGradientFunction(double[] coefficients, double[] input, double[] result);
}
