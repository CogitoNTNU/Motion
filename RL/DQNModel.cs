using System;
using TorchSharp;
using TorchSharp.Modules;
using TorchSharp.Tensor;

namespace Motion
{
    public class DQNModel : torch.nn.Module
    {
        private Linear fc1, fc2, fc3;
        public DQNModel(int inputSize, int outputSize) : base("DQNModel")
        {
            fc1 = torch.nn.Linear(inputSize, 128);
            fc2 = torch.nn.Linear(128, 128);
            fc3 = torch.nn.Linear(128, outputSize);

            RegisterComponents();
        }

        public override torch.Tensor forward(torch.Tensor x)
        {
            x = torch.nn.functional.relu(fc1.forward(x));
            x = torch.nn.functional.relu(fc2.forward(x));
            x = fc3.forward(x);
            return x;
        }
    }

}