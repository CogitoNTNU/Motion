using System;
using System.Threading;
using Avalonia.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;
using TorchSharp;
using static TorchSharp.torch;

namespace Motion
{
    class GymRunner 
    {

        // Hyperparameters for training
        static double epsilon = 1.0;
        static double epsilonDecay = 0.995;
        static double epsilonMin = 0.01;
        static double gamma = 0.99; // Discount factor
        static int batchSize = 32;  // For mini-batch training
        public static void RunGymEnvironment()
        {
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); 
            PopulationManager popManager = new PopulationManager();
            var model = new DQNModel(4, 2);

            NDArray state = cp.Reset(); // Get initial state

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Step: " + i);

                double[] stateArray = state.ToArray<double>();
                Tensor stateTensor = torch.tensor(stateArray).unsqueeze(0);

                int action;
                if (new Random().NextDouble() < epsilon)
                {
                    action = new Random().Next(0, 2);
                }
                else 
                {
                    using (var noGrad = torch.no_grad())
                    {
                        Tensor qValues = model.forward(stateTensor);
                        action = qValues.argmax().item<int>();
                    }
                }

                var (nextState, reward, _done, information) = cp.Step(action);

                popManager.AddExperience(state, action, reward, nextState, _done);

                state = nextState;

                Dispatcher.UIThread.Post(() =>
                {
                    Image img = cp.Render(); 
                });

                Thread.Sleep(15);

                if (_done)
                {
                    state = cp.Reset();

                    epsilon = Math.Max(epsilon * epsilonDecay, epsilonMin);
                }
            }
            popManager.SaveToFile("/Users/edvard/Git/Motion/RL/populationData.json");
        }
    }
}