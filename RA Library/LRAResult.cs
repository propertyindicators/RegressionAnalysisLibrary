using Accord;
using Accord.Math.Optimization.Losses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    /// <summary>
    /// субкласс оценки качества регрессии, специфицированный для линейного анализа
    /// </summary>
    public class LRAResult : RAResult
    {
        /// <summary>
        /// композиция (совместная) с объектом вызова машины Аккорд 
        /// </summary>
        RALinear ra_object;

        public LRAResult() { }

        public LRAResult(RALinear init) : base(init)
        {
            ra_object = init;
            var tres = ra_object.get_tempresult(); //ссылка на результат Аккорд
            double[] temp = new double[ra_object.get_tempresult().Weights.Length + 1];
            for (int i = 1; i < ra_object.get_tempresult().Weights.Length + 1; i++) { temp[i] = ra_object.get_tempresult().Weights[i - 1]; }
            temp[0] = ra_object.get_tempresult().Intercept;
            coefficients = temp;
            model = (coef, x) => { var t = coef[0]; for (int i = 0; i < x.Length; i++) t = t + coef[i + 1] * x[i]; return t; };//вычисление соответствует содержанию линейной функции
        }

        /// <summary>
        /// метод расчёта среднего значения через связанный Engine - ra_object
        /// </summary>
        public override void compute_valuations() { ra_object.valuations = ra_object.get_tempresult().Transform(ra_object.inputs); }

        /// <summary>
        /// стандартный метод расчёта sse для машины линейного анализа
        /// </summary>
        protected override void compute_sse_and_mse()
        {
            sse = (new SquareLoss(ra_object.outputs) { Mean = false }).Loss(ra_object.valuations);
            mse = sse / numofobserv;
        }

        /// <summary>
        /// стандартный метод расчёта r2 и общей стандартной ошибки для линейного анализа
        /// </summary>
        protected override void compute_r2_and_standart_error()
        {
            standart_error = ra_object.get_tempresult().GetStandardError(ra_object.inputs, ra_object.outputs);
            r2 = ra_object.get_tempresult().CoefficientOfDetermination(ra_object.inputs, ra_object.outputs);
        }

        /// <summary>
        ///  метод фиксации стандартных ошибок коэффициентов - реализуется через инструменты Аккорд на базе класса линейного анализа
        /// </summary>
        protected override void compute_standart_errors()
        {
            standart_errors = ra_object.get_tempresult().GetStandardErrors(mse, ra_object.get_engine().GetInformationMatrix());
        }

        /// <summary>
        /// реализует классический метод предсказания интервалов Аккорд для линейной регрессии 
        /// </summary>
        /// <param name="percent">вероятность попадания новых точек - главный метод в методике "отсева"</param>
        public override void computePredictions(double percent)
        {
            if (mse == -1) compute_sse_and_mse();//если mse ещё не посчитан - то надо посчитать
            predict = new RADoubleRange[numofobserv];
            for (int i = 0; i < numofobserv; i++)
            {
                DoubleRange r = ra_object.get_tempresult().GetPredictionInterval(
                input: ra_object.inputs[i],
                mse: mse,
                numberOfSamples: numofobserv,
                informationMatrix: ra_object.get_engine().GetInformationMatrix(),
                percent: percent);
                predict[i] = new RADoubleRange(r.Min, r.Max);
            }
        }
    }
}
