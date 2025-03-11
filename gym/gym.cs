using System;
using System.Threading;
using Avalonia.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;

namespace Motion
{
    class GymRunner
    {
        public static void RunGymEnvironment()
        {
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); 
            bool done = true;
            PopulationManager popManager = new PopulationManager();

            NDArray state = cp.Reset(); // Get initial state

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Step: " + i);

                int action = i % 2; // Placeholder: for algorithm

                var (nextState, reward, _done, information) = cp.Step(action);

                popManager.AddExperience(state, action, reward, nextState, _done);

                state = nextState;
                done = _done;


                Dispatcher.UIThread.Post(() =>
                {
                    Image img = cp.Render(); 
                });

                Thread.Sleep(15);

                if (done)
                {
                    state = cp.Reset();
                }
            }

            popManager.SaveToFile("/Users/edvard/Git/Motion/RL/populationData.json");
        }
    }
}