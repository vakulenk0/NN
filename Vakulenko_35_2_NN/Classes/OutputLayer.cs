namespace Vakulenko_35_2_NN.Classes
{
    class OutputLayer: Layer
    {
        public OutputLayer(int non, int nopn, NeuroType nt, string type) : base(non, nopn, nt, type) { }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < numofneurons; i++)
                e_sum += Neurons[i].Output;

            for (int i = 0; i < numofneurons; i++)
            {
                net.Fact[i] = Neurons[i].Output / e_sum; //Расчёт веткора выходных сигналов
            }
        }

        public override double[] BackwardPass(double[] erros)
        {
            double[] gr_sum = new double[numofprevneurons + 1];
            for (int j = 0; j < numofprevneurons + 1; j++) // Вычисление градиентых сумм 
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += Neurons[k].Weights[j] * erros[k];

                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneurons; i++) // Вычисление коррекции синаптических весов 
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaW = 0;
                    if (n == 0) //Для коррекции порогов
                        deltaW = momentum * lastdeltaweights[i, 0] + learningrate * erros[i];
                    else //Коррекция синаптических весов
                        deltaW = momentum * lastdeltaweights[i, n] +
                            learningrate * Neurons[i].Inputs[n - 1] * erros[i];

                    lastdeltaweights[i, n] = deltaW;
                    Neurons[i].Weights[n] += deltaW; //Коррекция весов
                }
            }
            return gr_sum;
        }

    }
}
