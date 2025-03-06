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

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Hello world" + i); // Output line for testing
                NDArray observation;
                if (done)
                {
                    observation = cp.Reset();
                    done = false;
                }
                else
                {
                    int action = i % 2; // switching between left and right
                    var (obs, reward, _done, information) = cp.Step(action); 
                    observation = obs;
                    done = _done;

                    popManager.AddData(observation, reward, action);
                    
                }
                // Utfør UI-operasjoner på Avalonia-tråden
                Dispatcher.UIThread.Post(() =>
                {
                    Image img = cp.Render(); // Render bildet på UI-tråden
                });

                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

            popManager.SaveToFile("YOUR PATH FILE HERE"); // ENTER YOUR PATH FILE TO "populationData.json"
        }
    }
}
