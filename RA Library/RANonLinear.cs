using Accord.Math.Optimization;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    /// <summary>
    /// класс для выполнения операций линейного анализа
    /// </summary>
    public class RANonLinear : RAData, IRAEngine
    {
        NonLRAResult raresult;

        /// <summary>
        /// пустой конструктор - инициализирует данные null
        /// </summary>
        public RANonLinear() : base() { }

        /// <summary>
        /// инициализирует данные реганализа композицей другого сета данных RAData или унаследованного класса по ссылке (без клонирования)
        /// </summary>
        /// <param name="init">инициализирующий сет данных, содержащий RAData</param>
        public RANonLinear(RAData init) : base(init) { }


        /// <summary>
        /// инициализирует данные реганализа композицей конкретных массивов с данными по ссылке (без клонирования)
        /// </summary>
        /// <param name="inputs_init">инициализирующий массив параметров</param>
        /// <param name="outputs_init">инициализирующий массив фактически наблюдаемых значений наблюдаемой функции, описывающей предметную область модели</param>
        /// <param name="valuations_init">инициализирующий массив регресионных значений (необязательный параметр)</param>
        public RANonLinear(double[][] inputs_init, double[] outputs_init, double[] valuations_init = null) : base(inputs_init, outputs_init, valuations_init) { }

        //внутренее свойство для работы с Аккорд tempresult - объект множественного нелинейного анализа класса Аккорд
        NonlinearRegression tempresult;
        /// <summary>
        /// объект временного результата нелинейного реганализа
        /// </summary>
        /// <returns></returns>
        public NonlinearRegression get_tempresult() { return tempresult; }

        //внутренее свойство для работы с Аккорд - объект анализа с использованием метода нименьших квадратов - для нелинейных моделей
        NonlinearLeastSquares engine = new NonlinearLeastSquares()
        {//инициализация базового алгоритма неким стандартным значением
            Algorithm = new LevenbergMarquardt()
            {
                MaxIterations = 10000,
                Tolerance = 0
            }
        };

        /// <summary>
        /// машина (engine) минимизации нелинейного реганализа реганализа
        /// </summary>
        /// <returns></returns>
        public NonlinearLeastSquares get_engine() { return engine; }

        /// <summary>
        /// Реализация вызова метода поиска регресионных коэффициентов
        /// </summary>
        public void computeRAresult()
        {
            tempresult = engine.Learn(inputs, outputs);
            raresult = new NonLRAResult(this);
            raresult.compute_valuations();
        }

        /// <summary>
        /// возвращает ранее наработанный результат реганализа с использованием класса RALinear - для использования  возвращаемого объекта типа
        /// MultipleLinearRegression необходима библиотека Accord в части Accord.Statistics.Models.Regression.Linear
        /// </summary>
        /// <returns>возвращает наработанный результат реганализа в вде ссылки на готовый объект NonlinearRegression  </returns>
        public NonLRAResult getRAresult() { return raresult; }

        /// <summary>
        /// Set метод класса для определения регрессионной функции - лямба выражение  (w,x)=>
        /// </summary>
        public void setFunction(RegressionFunction f) { engine.Function = f; }

        /// <summary>
        /// Set метод класса для определения градиента регрессионной функции лямба выражение  (w,x)=>
        /// </summary>
        public void setGradient(RegressionGradientFunction g) { engine.Gradient = g; }

        /// <summary>
        /// Set метод  класса для определения стартовых значений поиска
        /// </summary>
        public void setSrartValues(double[] v) { engine.StartValues = v; }

        /// <summary>
        /// Set метод класса для определения алгоритма поиска в системе Аккорд
        /// </summary>
        public void setAlgorithm(LevenbergMarquardt alg) { engine.Algorithm = alg; }

        /// <summary>
        /// Set метод класса для количества параметров модели
        /// </summary>
        public void setNumberOfParametrs(int n) { engine.NumberOfParameters = n; }

        /// <summary>
        /// Set метод класса для задания общей модели нелинейного регализа в системе Аккорд - из набора пользовательских данных
        /// </summary>
        /// <param name="f">лямбда выражение регрессионной функции - основной гипотезы (w,x)=> </param>
        /// <param name="g">лямбда выражение функции градиента регрессионной функции (w,x,r)=> </param>
        /// <param name="n">количество искомых параметров функции - регресионных коэффициентов</param>
        /// <param name="v">массив инициализирующих значений поиска (базовое предположение)</param>
        /// <param name="alg">алгоритм посика в системе классов Аккорд</param>
        public void setModel(RARegressionFunction f, RARegressionGradientFunction g, int n, double[] v = null)
        {
            engine.Function = (k,x) => f(k,x) ;
            engine.Gradient = (k,x,r)=>g (k,x,r);
            engine.NumberOfParameters = n;
            if (v != null) { engine.StartValues = v; }
        }

        /// <summary>
        /// Set метод класса для определения общей модели нелинейного реггализа в системе Аккорд из другого (уже созданного) экзепляра класса (композиция по ссылке)
        /// </summary>
        public void setModel(RANonLinear prot)
        {
            engine = prot.engine;
        }
    }
}
