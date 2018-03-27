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
    public class NonLRAResult : RAResult
    {
        /// <summary>
        /// композиция (совместная) с объектом вызова машины Аккорд 
        /// </summary>
        RANonLinear ra_object;
        public NonLRAResult() { }

        public NonLRAResult(RANonLinear init) : base(init)
        {
            ra_object = init;
            coefficients = ra_object.get_tempresult().Coefficients;
            model = (x, y) => ra_object.get_tempresult().Function(x, y);
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
        ///  метод фиксации стандартных ошибок коэффициентов - реализуется через инструменты Аккорд на базе класса нелинейного анализа
        /// </summary>
        protected override void compute_standart_errors() { standart_errors = ra_object.get_tempresult().StandardErrors; }

        /// <summary>
        /// реализует классический метод предсказания интервалов Аккорд для линейной регрессии 
        /// </summary>
        /// <param name="percent">вероятность попадания новых точек - главный метод в методике "отсева"</param>
        public override void computePredictions(double percent)
        {
            throw new InvalidOperationException("NonLRAResult.computePredictions: требуется наполнение метода кодом!");
        }
    }

}
