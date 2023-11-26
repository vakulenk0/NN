namespace Vakulenko_35_2_NN.Classes
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuroType nt, string type) : base(non, nopn, nt, type) { }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
                hidden_out[i] = Neurons[i].Output;

            nextLayer.Data = hidden_out;
            
        }

        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < gr_sum.Length; j++) // Вычисление градиентых сумм 
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += Neurons[k].Weights[j] * Neurons[k].Derivative * gr_sums[k];

                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneurons; i++) // Вычисление коррекции синаптических весов 
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaW = 0;
                    if (n == 0) //Для коррекции порогов
                        deltaW = momentum * lastdeltaweights[i, 0] + learningrate * Neurons[i].Derivative * gr_sums[i];
                    else //Коррекция синаптических весов
                        deltaW = momentum * lastdeltaweights[i, n] + 
                            learningrate * Neurons[i].Inputs[n - 1] * Neurons[i].Derivative * gr_sums[i];
                    
                    lastdeltaweights[i, n] = deltaW;
                    Neurons[i].Weights[n] += deltaW; //Коррекция весов
                }
            }
            return gr_sum;        
        }
    }
}

//Доделать инициализацию в Layer + чтобы вся программа запускалась без ошибок
