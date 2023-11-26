using static System.Math;

namespace Vakulenko_35_2_NN.Classes
{
    class Neyron
    {
        //Fields
        private double[] inputs;
        private double[] weights;
        private double output;
        NeuroType type;
        private double derivative;

        //Свойства
        public double[] Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        public double[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        public double Output
        {
            get { return output; }
        }

        public double Derivative
        {
            get { return derivative; }
        }

        //Конструктор
        public Neyron(double[] _weights, NeuroType _type)
        {
            weights = _weights;
            type = _type;
        }

        //Активатор
        public void Activator()
        {
            double sum = weights[0];

            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += weights[i + 1] * inputs[i];
            }
            switch (type)
            {
                case NeuroType.Hidden:
                    output = LeakyRelu(sum);
                    derivative = LeakyRelu_Derivator(sum);
                    break;
                case NeuroType.Output:
                    output = Exp(sum);
                    break;
                default:
                    break;
            }
        }

        private double LeakyRelu(double arg)
        {
            return arg > 0 ? arg : arg * 0.01;
        }

        private double LeakyRelu_Derivator(double arg)
        {
            //+ разобрать про SoftMax() (она не относится к этой фукнкции просто домашка)
            return arg > 0 ? 1 : 0.01;
        }
    }
}
