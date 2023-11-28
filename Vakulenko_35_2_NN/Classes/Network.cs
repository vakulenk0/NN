using System;

namespace Vakulenko_35_2_NN.Classes
{
    class Network
    {
        //Массив для хранения вектора выходного сигнала нейросети
        public double[] Fact = new double[10];


        //Все слои нейросети
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuroType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(33, 70, NeuroType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 33, NeuroType.Output, nameof(output_layer));

        // Среднее значение энергии ошибки эпохи обучения
        private double e_error_avg;

        //Свойства
        public double E_error_avg
        {  get { return e_error_avg; } 
           set { e_error_avg = value; } 
        }

        //Конструктор
        public Network(NetworkMode nm)
        {
            input_layer = new InputLayer(nm);
        }

        //Прямой проход сигнала по нейросети
        public void ForwardPass(Network net, double[] net_input)
        {
            net.hidden_layer1.Data = net_input;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }

        //Метод обучения
        public void Train(Network net)
        {
            int epochs = 100; //эпохия обучения
            net.input_layer = new InputLayer(NetworkMode.Train);

            double tmpSumError; //Времення переменная суммы ошибок
            double[] errors; //Вектор сигнала ошибки выходного слоя
            double[] tmpGradSums1; //Вектор градиента первого скрытого слоя
            double[] tmpGradSums2; //Вектор градиента второго скрытого слоя

            for (int k = 0; k < epochs; k++)
            {
                e_error_avg = 0; //В начале каждой эпохи значение средней энергии ошибки обнуляется
                for (int i = 0; i < net.input_layer.TrainSet.Length; i++)
                {
                    //Прямой проход
                    ForwardPass(net, net.input_layer.TrainSet[i].Item1);

                    //Вычисление ошибки по итерации
                    tmpSumError = 0;
                    errors = new double[net.Fact.Length]; //net.Fact.Length - Длина массива фактического выхода н.с.
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.TrainSet[i].Item2)
                        {
                            errors[x] = -(net.Fact[x] - 1.0);
                        }
                        else
                        {
                            errors[x] = -net.Fact[x];
                        }

                        //Собираем энергию ошибки
                        tmpSumError += errors[x] * errors[x] / 2.0;
                    }

                    e_error_avg += tmpSumError / errors.Length; //Суммарное значение энергии оишбки эпох

                    //Обратный проход и коррекция весов
                    tmpGradSums2 = net.output_layer.BackwardPass(errors);
                    tmpGradSums1 = net.hidden_layer2.BackwardPass(tmpGradSums2);

                    //Корректируем 1 скрытый слой
                    net.hidden_layer1.BackwardPass(tmpGradSums1);
                }

                e_error_avg /= net.input_layer.TrainSet.Length; //Среднее значение энергии ошибки одной эпохи
                
                //Здесь написать код отображения среднего значения эпохи на графике
            }

            net.input_layer = null; //Обнуление входного слоя

            //Сохранение скорректированных весов
            net.hidden_layer1.WeightsInitialize(MemmoryMode.SET, nameof(hidden_layer1) + "_memory.scv");
            net.hidden_layer2.WeightsInitialize(MemmoryMode.SET, nameof(hidden_layer2) + "_memory.scv");
            net.output_layer.WeightsInitialize(MemmoryMode.SET, nameof(output_layer) + "_memory.scv");

        }
    }
}
// 15:70:33:10 Leaky Relu(функция активации)
//Прога должна что-то выводить(даже неправильное значение)