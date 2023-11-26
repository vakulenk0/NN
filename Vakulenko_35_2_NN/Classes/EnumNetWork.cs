using System;

namespace Vakulenko_35_2_NN.Classes
{
    enum NeuroType { Hidden, Output }
	// 15:70:33:10 Leaky Relu(функция активации)

    enum MemmoryMode //режим работы памяти
    {
        GET, SET, INIT
    }

    enum NetworkMode //режим работы сети
    {
        Train, Test, Demo
    }
}
