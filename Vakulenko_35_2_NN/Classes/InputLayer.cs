using System;

namespace Vakulenko_35_2_NN.Classes
{
    class InputLayer 
    {
        private Random random = new Random();
        private (double[], int)[] trainSet = new (double[], int)[100]; //100 - кол-во примеров

        public (double[], int)[] TrainSet { get =>  trainSet; }

        //Конструктор 
        public InputLayer(NetworkMode nm)
        {
            switch (nm)
            {
                case NetworkMode.Train:
                    //Здесс написать код считывания обучающего множества
                    //из файла и формирование массива TrainSet

                    //Перетасовка методом Фишера Йетса
                    break;
                case NetworkMode.Test:
                    break;
                case NetworkMode.Recognize:
                    break;
            }
        }
    }
}
