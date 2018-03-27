using Accord.Statistics.Models.Regression.Linear;
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
    public class RALinear : RAData, IRAEngine
    {
        LRAResult raresult;

        /// <summary>
        /// пустой конструктор - инициализирует данные null
        /// </summary>
        public RALinear() : base() { }

        /// <summary>
        /// инициализирует данные реганализа композицей другого сета данных RAData или унаследованного класса по ссылке (без клонирования)
        /// </summary>
        /// <param name="init">инициализирующий сет данных, содержащий RAData</param>
        public RALinear(RAData init) : base(init) { }


        /// <summary>
        /// инициализирует данные реганализа композицей конкретных массивов с данными по ссылке (без клонирования)
        /// </summary>
        /// <param name="inputs_init">инициализирующий массив параметров</param>
        /// <param name="outputs_init">инициализирующий массив фактически наблюдаемых значений наблюдаемой функции, описывающей предметную область модели</param>
        /// <param name="valuations_init">инициализирующий массив регресионных значений (необязательный параметр)</param>
        public RALinear(double[][] inputs_init, double[] outputs_init, double[] valuations_init = null) : base(inputs_init, outputs_init, valuations_init) { }

        //внутренее свойство для работы с Аккорд tempresult - объект множественного линейного анализа класса Аккорд
        MultipleLinearRegression tempresult;
        public MultipleLinearRegression get_tempresult() { return tempresult; }
        //внутренее свойство для работы с Аккорд - объект анализа с использованием метода нименьших квадратов для линейных моделей
        OrdinaryLeastSquares engine = new OrdinaryLeastSquares() { UseIntercept = true };
        public OrdinaryLeastSquares get_engine() { return engine; }
        /// <summary>
        /// обще интерфейсный метод расчёта базового результата
        /// </summary>
        public void computeRAresult()
        {
            tempresult = engine.Learn(inputs, outputs);
            raresult = new LRAResult(this);
            raresult.compute_valuations();
        }

        /// <summary>
        /// возвращает базовый результат анализа в стандартизированном (на уровне библиотеки) формате LRAResult:RAResult
        /// </summary>
        /// <returns></returns>
        public LRAResult getRAresult() { return raresult; }

    }

}
