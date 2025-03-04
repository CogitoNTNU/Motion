using System;
using System.Threading;
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
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    var action = (i % 2); // switching between left and right
                    var (observation, reward, _done, information) = cp.Step(action); 
                    done = _done;
                    // Do something with the reward and observation.

                    Console.WriteLine("reward:   " + reward);
                    Console.WriteLine("obs:   " + observation);
                   

                    
                    
                }

                SixLabors.ImageSharp.Image img = cp.Render(); //returns the image that was rendered.
                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

          
        }
    }
}
