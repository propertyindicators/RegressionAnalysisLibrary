using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RA_Library;

namespace Reganalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test code for demonstrating the use of the RA Library for pricessing linear and nonlinear regression analysis 
            //with a simple and intuitive interface and algorithm (all other reganalysis settings are encapsulated in the library classes).
            //In this example, we will test some hypothesis described by the regression model on some experimental data set 
            //(potential observations) using the linear and nonlinear regression solvers 

            //Non-linear analysis with reg.function y=k0*k1^ln(x0)*k2^x1*k3^x2 and LevenbergMarquardt minimization algorithm 
            //Having a set of experimental data, we create an object (virtual engine) for performing non-linear regression analysis
            RANonLinear nlra = new RANonLinear(DataSet.testdata);
            nlra.setModel(
                        f: (k, x) => k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2],x[1]) * Math.Pow(k[3], x[2]),//regression function
                        g: (k, x, r) =>
                        { //gradient function 
                            r[0] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]);
                            r[1] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * Math.Log(x[0]) / k[1];
                            r[2] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * x[1] / k[2];
                            r[3] = k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]) * x[2] / k[3]; ; 
                        },
                        n: 4,//number of observed coefficients
                        v: new[] { 150.0, 1, 1, 1 });
            //machine learning processing 
            nlra.computeRAresult();
            //metrics observation and reg.valuations (predictions)
            NonLRAResult nlres = nlra.getRAresult();
            nlres.computeRAMetrics();
            //results out
            Console.WriteLine("Non-linear solver result:");
            Tools.PrintRAResult(nlres);


            //Next, We solve the same problem using the solver of linear models. 
            // This is possible in the case when the non-linear model analytically can be transformed  to the linear model (as in this case)
            //To reduce the function y=k0*k1^ln(x0)*k2^x1*k3^x2 to a linear form, it is necessary to perform the logarithm of the right and left sides of equation
            // ln(y)=ln(k0)+ln(k1)*ln(x0)+ln(k2)*x1+ln(k3)*x2
            // ln(y) and ln(kn) are considered as coefficients of the linear model
            // Thus, in order to perform a linear analysis, the following transformations:
             RAData linedata = DataSet.testdata.CloneData();//clone of base lineardata
            linedata.outputsTransform(x => Math.Log(x));//perform the logarithm of y
            linedata.inputsTransform(0, x => Math.Log(x));//perform the logarithm of first parametr
            RALinear lra = new RALinear(linedata);
            lra.computeRAresult();
            RAResult lres = lra.getRAresult();
            lres.computeRAMetrics();
            //results out
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Linear solver result whith logarithmic data (direct result):");
            Tools.PrintRAResult(lres);

            //Further, to obtain metrics in the same format for a fundamental comparison of the results of linear and nonlinear analysis, 
            // we will create an object of nonlinear analysis with coefficients obtained by linear analysis (but in a nonlinear form)
            RAResult checklra = new RAResult(DataSet.testdata);//set base nonlinear dataset
            checklra.coefficients = new double[] { Math.Exp(lres.coefficients[0]), Math.Exp(lres.coefficients[1]), Math.Exp(lres.coefficients[2]), Math.Exp(lres.coefficients[3]) };
            checklra.model=(k, x) => k[0] * Math.Pow(k[1], Math.Log(x[0])) * Math.Pow(k[2], x[1]) * Math.Pow(k[3], x[2]);//set of nonlinar regression function
            checklra.computeRAMetrics();
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Linear solver result whith base nonlinear data and base nonlinear model");
            Tools.PrintRAResult(checklra);


            //This test demonstrates that even analytically identical transformation of nonlinear models to linear ones 
            //with further application of a linear solver significantly reduces the quality of statistical observations. 
            //If possible, it is advisable to use non-linear analysis, but it requires careful adjustment of the parametric properties. 
            //Confirmation of the thesis clearly demonstrates the results of observation, deduced in the Console of this aplication. 

            Console.ReadLine();
        }
    }
}
