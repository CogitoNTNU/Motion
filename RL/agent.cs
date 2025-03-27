using System;
using Motion;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;

namespace Motion
{
    public class Agent
    {
        public DQNModel model { get; private set; }
        private float epsilon;

        public Agent(int inputSize, int outputSize)
        {
            model = new DQNModel(inputSize, outputSize);
            epsilon = 1.0f;
        }

        public int SelectAction(Tensor state)
        {
            int action;
            if (new Random().NextDouble() < epsilon)
            {
                action = new Random().Next(0, 2);
            }
            else
            {
                using (var no_grad = torch.no_grad())
                {
                    Tensor qValues = model.forward(state);
                    action = qValues.argmax().item<int>();
                }
            }
            return action;
        }

        public void UpdateEpsilon(double decay, double minEpsilon)
        {
            epsilon = (float)Math.Max(epsilon * decay, minEpsilon);
        }
    }
}