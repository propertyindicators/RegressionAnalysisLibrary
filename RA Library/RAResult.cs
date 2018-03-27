using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_Library
{
    public class RAResult
    {
        /// <summary>
        /// пустой базовый конструктор
        /// </summary>
        public RAResult() { }

        //Композиция с сэтом данных реганализа
        RAData data;
        public RAData get_data() { return data; }

        /// <summary>
        /// конструктор создаёт новый объект с привязкой к существующему объекту RAData
        /// </summary>
        /// <param name="init_data">существующий объект с данными реганализа</param>
        public RAResult(RAData init_data)
        {
            if (init_data == null)
                throw new ArgumentNullException("RAResult: попытка создать объект из пустого объекта с базовыми данными");
            //if (init_data.valuations == null || init_data.valuations.Length == 0)
            //    throw new ArgumentException("RAResult: попытка создать объект из  объекта с базовым объектом данных, который не содержит регресионных оценок (должны быть созданы на предыдущем уровне вычислений)");
            if (init_data.inputs == null || init_data.inputs.Length == 0)
                throw new ArgumentException("RAResult: попытка создать объект из  объекта с базовым объектом данных, который массива значений X (должны быть созданы на предыдущем уровне вычислений)");
            if (init_data.outputs == null || init_data.outputs.Length == 0)
                throw new ArgumentException("RAResult: попытка создать объект из  объекта с базовым объектом данных, который массива значений Y (должны быть созданы на предыдущем уровне вычислений)");
            if (!(init_data.inputs.Length == init_data.outputs.Length))
                throw new ArgumentException("RAResult: попытка создать объект из  объекта с базовым объектом данных, в котором массивы X, Y, V не согласованы по длине (должны иметь одинаковый размер)");
            //теперь можно инициализировать
            data = init_data;
            numofobserv = init_data.inputs.Length;
        }

        /// <summary>
        /// Метод для подсчёта количества наблюдений в основном сэте данных (RAData data) текущем объекте реганализа с определёнными параметрами
        /// </summary>
        /// <param name="rule">логическая функция (от типичного набора параметров единичного наблюдния), которая при возврате true будет увеличивать счётчик</param>
        /// <returns>обнаруженное количество наблюдений с указанными в функции rule параметрами</returns>             
        public int dataParametrizationCount(Func<double[], bool> rule)
        {
            int t = 0;
            foreach (double[] pars in data.inputs)
                if (rule(pars)) t++;
            return t;
        }

        //Композиция с функцией регрессионной модели
        public RARegressionFunction model { get; set; }
        public RAResult(RAData init_data, RARegressionFunction init_model) : this(init_data) { model = init_model; }
        //базовый инцициализатор коэффициентов - результатов анализа
        private double[] _coefficients;
        public double[] coefficients
        {
            get { return _coefficients; }
            set
            {
                if (value == null || value.Length == 0)
                    throw new ArgumentException("RAResult.coefficient.set: попытка инициализировать коэффициенты регрессии c пустым массивом");
                _coefficients = value;
                numofcoef = value.Length;
            }
        }
        //стандартные метрики,  определённые в базовом классе
        public double sse { get; set; } = -1;
        public double mse { get; set; } = -1;
        public double r2 { get; set; } = -1;
        public double standart_error { get; set; } = -1;
        public double[] standart_errors { get; set; }
        public int numofcoef = 0;
        public int numofobserv = 0;

        /// <summary>
        /// метод вычисляет регрессионные оценки и помещает их в объекте-свойстве data
        /// реализует свобственные алгоритм, в субклассах могут использоваться инструменты Аккорд
        /// </summary>
        public virtual void compute_valuations()
        {
            if (model == null)
                throw new ArgumentException("RAResult.compute_valuations: регресионная модель не задана в объекте");
            if (data == null || data.inputs.Length == 0)
                throw new ArgumentException("RAResult.compute_valuations: данные X и Y не инициализированы");
            data.valuations = new double[data.inputs.Length];
            for (int i = 0; i < data.inputs.Length; i++) {
                data.valuations[i] = model(coefficients, data.inputs[i]);
                var a = 0;
            }//вычисление регрессионной функции
        }

        /// <summary>
        /// диапазон доверительных интервалов для текущих данных реганализа
        /// </summary>
        public RADoubleRange[] predict;

        /// <summary>
        /// общий для всех реганализов метод взятия предстказательного интервала
        /// </summary>
        /// <param name="percent"></param>
        public virtual void computePredictions(double percent) { } ///требуется реализация

        /// <summary>
        /// метод вычисления метрик - всех основных показателей качества регрессии
        /// </summary>
        public virtual void computeRAMetrics()
        {
            compute_sse_and_mse();
            compute_r2_and_standart_error();
            compute_standart_errors();
        }

        /// <summary>
        /// стандартный метод расчёта sse и mse
        /// </summary>
        protected virtual void compute_sse_and_mse()
        {
            if (numofobserv == 0)
                throw new InvalidOperationException("toBaseSet.compute_sse: попытка вычислить sse без инициализированного и наполненного сэта данных RAData");
            if (data.valuations == null || data.valuations.Length == 0) compute_valuations();
            //расчёт sse
            sse = 0;
            for (int i = 0; i < numofobserv; i++) { var e = (data.valuations[i] - data.outputs[i]); sse = sse + (e * e); }
            //расчёт mse
            mse = sse / numofobserv;
        }

        /// <summary>
        /// стандартный метод расчёта r2
        /// </summary>
        protected virtual void compute_r2_and_standart_error()
        {
            if (numofobserv == 0)
                throw new InvalidOperationException("toBaseSet.compute_r2_and_se: попытка вычислить sse без инициализированного и наполненного сэта данных RAData");
            if (sse == -1) compute_sse_and_mse();//если sse ещё не считалось, его надо вначале посчитать 
            double ssdevy = 0;
            double avy = data.outputs.Average();
            for (int i = 0; i < numofobserv; i++) { var dev = (avy - data.outputs[i]); ssdevy = ssdevy + (dev * dev); }
            r2 = 1 - sse / ssdevy;
            standart_error = Math.Pow(sse / numofobserv, 0.5) / avy;
        }

        /// <summary>
        /// абстрактный метод фиксации стандартных ошибок коэффициентов - реализуется через инструменты Аккорд
        /// </summary>
        protected virtual void compute_standart_errors() { } ///требуется реализация
    }
}
