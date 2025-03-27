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
        static double gamma = 0.99;
        static int batchSize = 32;

        public static void RunGymEnvironment()
        {
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory);
            PopulationManager popManager = new PopulationManager();

            var model = new DQNModel(4, 2);
            var agent = new Agent(4, 2);
            var optimizer = torch.optim.Adam(model.parameters(), lr: 0.001);

            NDArray state = cp.Reset();

            for (int i = 0; i < 1000; i++)
            {
                double[] stateArray = state.ToArray<double>();
                Tensor stateTensor = torch.tensor(stateArray).unsqueeze(0); // Shape [1,4]

                // ε-greedy action selection
                var action = agent.SelectAction(stateTensor);

                var (nextState, reward, done, info) = cp.Step(action);

                popManager.AddExperience(state, action, reward, nextState, done);

                state = nextState;

                Dispatcher.UIThread.Post(() => 
                {
                    Image img = cp.Render();
                });

                Thread.Sleep(15);

                if (done)
                {
                    state = cp.Reset();
                    agent.UpdateEpsilon(epsilonDecay, epsilonMin);
                }
            }

            // Save the collected experience data
            popManager.SaveToFile("/Users/edvard/Git/Motion/RL/populationData.json");
        }
    }
}