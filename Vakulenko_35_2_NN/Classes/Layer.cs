using System;
using System.IO;

namespace Vakulenko_35_2_NN.Classes { 

    abstract class Layer
    {
        //protected - ��������� ������ ������ ������ � ����� ������ � ���� ��� ������������
        protected string name_Layer; //������������ ����
        string pathDirWeights; //�������� ���� ��������, ��� ����� ��������� ����
        string pathFileWeights;
        protected int numofneurons; //����� �������� �� ���� ����
        protected int numofprevneurons; //����� �������� �� ���������� ����
        protected const double learningrate = 0.5; //�������� ��������
        protected const double momentum = 0.05; //��������� ������ ����������� ��������
        protected double[,] lastdeltaweights; //��������� ��������� �����
        private Neyron[] neurons;


        // ��������
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

        // �����������
        protected Layer(int non, int nopn, NeuroType nt, string nm_Layer)  // non - ���-�� �������� � ����,
                                                                            // nopn - ���-�� �������� � ���������� ����,
                                                                            // nm_layer - ��� ����
        {
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neyron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\"; //���� � ��������, ��� ��������� ����
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; //��������� ������ ������������� �����

            if (File.Exists(pathFileWeights)) // ����������, ���������� �� path
                Weights = WeightsInitialize(MemmoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                File.Create(pathFileWeights).Close();
                Weights = WeightsInitialize(MemmoryMode.INIT, pathFileWeights);
            }

            lastdeltaweights = new double[non, nopn + 1];

            for (int i = 0; i < non; ++i) //���� ������������ �������� ���� � �����
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j =0; j < nopn + 1; ++j)
                    tmp_weights[j] = Weights[i, j];

                Neurons[i] = new Neyron(tmp_weights, nt);
            }

        }

        public double[,] WeightsInitialize(MemmoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ', '_' }; //����������� ����
            string tmpStr = ""; //��������� ������ ��� ������
            string[] tmpStrWeights = { }; //��������� ������ �����
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
            switch (mm)
            {
                case MemmoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path); // ���������� ����� ���������� �����; ��� ��������� ���������� ������ ����� ��������
                    string[] memory_element;
                    for (int i = 0; i < numofneurons; ++i)
                    {
                        memory_element = tmpStrWeights[i].Split(delim); //��������� ������
                        for (int j = 0; j < numofprevneurons + 1; ++j) //�������������� ����� � double
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture); //������ , �� .) 
                        }
                    }
                    break;
                case MemmoryMode.SET:
                    tmpStrWeights = new string[numofneurons]; // ���������� ������ �����
                                                              // if (!File.Exists(path)  ���� ��� ����� ������������� �����
                                                              //{
                                                              //     MessageBox.Show()
                                                              // }
                    break;
                case MemmoryMode.INIT:
                    Random random = new Random();
                    double d = random.NextDouble();
                    /*������������� �����
                     * 1) ���� ���������������� ���������� ����������
                     * 2) ��� ������� (�� �������) ���� ����� ������� ������ ��������� 0 (���� �� �������� ���� ����� � 
                     * �������� ��� �������� �� ���� ����������� ����� � ������� 0)
                     * 3) ������� �������������� �������� �� ������ ��������� 1(���������� ��� ��� ����������� � ���������� �� ����������)
                     */
                    for (int i = 0; i < numofneurons; i++)
                    {
                        //���������� ����
                        for (int j = 1; j < numofprevneurons + 1; j++)
                            tmpStr += delim[0] + weights[i, j].ToString();

                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights); //������ ����� ���� ���������� � ����
                    
                    break;
                default:
                    break;
            }
            return weights;
        }

        abstract public void Recognize(Network net, Layer nextLayer); //��� ������� �������
        abstract public double[] BackwardPass(double[] stuff); //� ���������
    }
}
