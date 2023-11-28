using System;
using System.IO;

namespace Vakulenko_35_2_NN.Classes { 

    abstract class Layer
    {
        //protected - открывает доступ внутри класса к этому методу и всем его наследнкикам
        protected string name_Layer; //наименование слоя
        string pathDirWeights; //файловый путь каталога, где будут храниться веса
        string pathFileWeights;
        protected int numofneurons; //число нейронов на этом слое
        protected int numofprevneurons; //число нейронов на предыдщуем слое
        protected const double learningrate = 0.5; //скорость обучения
        protected const double momentum = 0.05; //настройка метода оптимизации обучения
        protected double[,] lastdeltaweights; //последние изменения весов
        private Neyron[] neurons;


        // Свойства
        public Neyron[] Neurons
        {
            get { return neurons; }
            set { neurons = value; }
        }
    
        public double[] Data
        {
            set
            {
                for (int i = 0; i < Neurons.Length; i++)
                {
                    Neurons[i].Inputs = value;
                    Neurons[i].Activator();
                }
            }
        }

        // Конструктор
        protected Layer(int non, int nopn, NeuroType nt, string nm_Layer)  // non - кол-во нейронов в слое,
                                                                            // nopn - кол-во нейронов в предыдущем слое,
                                                                            // nm_layer - имя слоя
        {
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neyron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\"; //путь к каталогу, где находятся веса
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; //временный массив синаптических весов

            if (File.Exists(pathFileWeights)) // определяет, существует ли path
                Weights = WeightsInitialize(MemmoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                File.Create(pathFileWeights).Close();
                Weights = WeightsInitialize(MemmoryMode.INIT, pathFileWeights);
            }

            lastdeltaweights = new double[non, nopn + 1];

            for (int i = 0; i < non; ++i) //цикл формирования нейронов слоя и весов
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j =0; j < nopn + 1; ++j)
                    tmp_weights[j] = Weights[i, j];

                Neurons[i] = new Neyron(tmp_weights, nt);
            }

        }

        public double[,] WeightsInitialize(MemmoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ', '_' }; //Разделитель слоёв
            string tmpStr = ""; //Временная строка для чтения
            string[] tmpStrWeights = { }; //Временный массив строк
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
            switch (mm)
            {
                case MemmoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path); // считывание строк текстового файла; тут получится одномерный массив строк нейронов
                    string[] memory_element;
                    for (int i = 0; i < numofneurons; ++i)
                    {
                        memory_element = tmpStrWeights[i].Split(delim); //разбивает строку
                        for (int j = 0; j < numofprevneurons + 1; ++j) //преобразование строк в double
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture); //Замена , на .) 
                        }
                    }
                    break;
                case MemmoryMode.SET:
                    tmpStrWeights = new string[numofneurons]; // построчный массив весов
                                                              // if (!File.Exists(path)  Если нет файла синаптических весов
                                                              //{
                                                              //     MessageBox.Show()
                                                              // }
                    break;
                case MemmoryMode.INIT:
                    Random random = new Random();
                    double d = random.NextDouble();
                    /*Инициализация весов
                     * 1) Веса инициализируются случайными величинами
                     * 2) Мат ождание (ср значени) всех весов нейрона должно равняться 0 (ищем ср значение всех весов и 
                     * отнимаем это значение от всех изначальных весов и получим 0)
                     * 3) Среднее квадратическое значение СВ должно равняться 1(посмотреть как оно вычисляется в конспектах по статистике)
                     */
                    for (int i = 0; i < numofneurons; i++)
                    {
                        //Вычисления выше
                        for (int j = 1; j < numofprevneurons + 1; j++)
                            tmpStr += delim[0] + weights[i, j].ToString();

                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights); //Создаёт новый файл записывает в него
                    
                    break;
                default:
                    break;
            }
            return weights;
        }

        abstract public void Recognize(Network net, Layer nextLayer); //для прямого прохода
        abstract public double[] BackwardPass(double[] stuff); //и обратного
    }
}
