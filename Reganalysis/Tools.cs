using RA_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reganalysis
{
    public static class Tools
    {
        public static void PrintRAResult(RAResult result)
        {
            List<string> coefs = new List<string>();
            for (int i = 0; i < result.coefficients.Length; i++) { coefs.Add("k" + i.ToString() + "=" + result.coefficients[i].ToString()); }
            Console.WriteLine("Coefficients:  " + String.Join("  ", coefs));
            Console.WriteLine();
            Console.WriteLine("SSE=" + result.sse.ToString());
            Console.WriteLine("MSE=" + result.mse.ToString());
            Console.WriteLine("R2=" + result.r2.ToString());
            Console.WriteLine("St.Error=" + result.standart_error.ToString());
            Console.WriteLine();
            Console.WriteLine("valuations - outputs:");
            for (int i = 0; i < result.get_data().valuations.Length; i++)
            { Console.WriteLine(result.get_data().valuations[i].ToString() + " - " + result.get_data().outputs[i].ToString()); }
        }
    }
}
