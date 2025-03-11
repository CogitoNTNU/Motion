using System;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;

namespace Motion
{
    public class DQNModel : nn.Module 
    {
        private Linear fc1, fc2, fc3;

        public DQNModel(int inputSize, int outputSize) : base("DQNModel")
        {
            fc1 = nn.Linear(inputSize, 128);
            fc2 = nn.Linear(128, 128);
            fc3 = nn.Linear(128, outputSize);

            RegisterComponents();
        }

        public Tensor forward(Tensor x) 
        {
            x = nn.functional.relu(fc1.forward(x));
            x = nn.functional.relu(fc2.forward(x));
            x = fc3.forward(x);
            return x;
        }
    }
}