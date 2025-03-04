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
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
            bool done = true;
            for (int i = 0; i < 100_000; i++)
            {
                Console.WriteLine("Hello world" + i); // Output line for testing
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    var (observation, reward, _done, information) = cp.Step((i % 2)); // switching between left and right
                    done = _done;
                    // Do something with the reward and observation.
                    
                }
                // Utfør UI-operasjoner på Avalonia-tråden
                Dispatcher.UIThread.Post(() =>
                {
                    Image img = cp.Render(); // Render bildet på UI-tråden
                });

                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

            
        }
    }
}
