using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    /// <summary>
    /// Класс сета данных реганализа
    /// </summary>
    public class RAData
    {
        public double[][] inputs;
        public double[] outputs;
        public double[] valuations;

        /// <summary>
        /// пустой конструктор без инициализации данных
        /// </summary>
        public RAData() { }

        /// <summary>
        /// инициализирует данные реганализа композицей другого сета данных RAData или унаследованного класса по ссылке (без клонирования)
        /// </summary>
        /// <param name="init">инициализирующий сет данных, содержащий RAData</param>
        public RAData(RAData init)
        {
            inputs = init.inputs;
            outputs = init.outputs;
            valuations = init.valuations;
        }

        /// <summary>
        /// инициализирует данные реганализа композицей конкретных массивов с данными по ссылке (без клонирования)
        /// </summary>
        /// <param name="inputs_init">инициализирующий массив параметров</param>
        /// <param name="outputs_init">инициализирующий массив фактически наблюдаемых значений наблюдаемой функции, описывающей предметную область модели</param>
        /// <param name="valuations_init">инициализирующий массив регресионных значений (необязательный параметр)</param>
        public RAData(double[][] inputs_init, double[] outputs_init, double[] valuations_init = null)
        {
            inputs = inputs_init;
            outputs = outputs_init;
            valuations = valuations_init;
        }

        /// <summary>
        /// метод преобразования столбцов таблицы (двумерного массива double) параметров X
        /// </summary>
        /// <param name="ColumnOperator"></param>
        public void inputsTransform(int colindex, Func<double, double> func)
        {
            if (inputs == null)
                throw new InvalidOperationException("RAData.ColumnTransform: попытка трансформирования столбца таблицы параметров X не инициализированного объекта данных!");
            if (colindex > inputs[0].Length)
                throw new InvalidOperationException("RAData.ColumnTransform: попытка трансформирования не существующего столбца (индекс за пределами массива)!");
            foreach (double[] m in inputs) { m[colindex] = func(m[colindex]); }
        }

        /// <summary>
        /// метод преобразования столбцов массива  фактического результата наблюдения Y
        /// </summary>
        /// <param name="ColumnOperator"></param>
        public void outputsTransform(Func<double, double> func)
        {
            if (outputs == null)
                throw new InvalidOperationException("RAData.ColumnTransform: попытка трансформирования массива значений Y наблдений не инициализированного объекта данных");
            for (int i = 0; i < outputs.Length; i++) { outputs[i] = func(outputs[i]); }
        }

        /// <summary>
        /// метод клонирования базовых данных реганализа - с клонированием массивов
        /// </summary>
        /// <param name="toclone"></param>
        /// <returns></returns>
        public RAData CloneData()
        {
            //inputs
            RAData temp = new RAData();
            if (inputs != null)
                if (inputs.Length != 0)
                {
                    temp.inputs = new double[inputs.Length][];
                    for (int i = 0; i < inputs.Length; i++) { temp.inputs[i] = (double[])inputs[i].Clone(); }
                }
                else { temp.inputs = (double[][])inputs.Clone(); }
            //inputs
            if (outputs != null) { temp.outputs = (double[])outputs.Clone(); }
            //valuations
            if (valuations != null) { temp.valuations = (double[])valuations.Clone(); }
            return temp;
        }
    }
}
