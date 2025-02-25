using System;
using System.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.WinForm;

namespace Motion
{
    class Program
    {
        static void Main(string[] args)
        {
            CartPoleEnv cp = new CartPoleEnv(WinFormEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
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
                    var (observation, reward, _done, information) = cp.Step((i % 2)); // switching between left and right
                    done = _done;
                    // Do something with the reward and observation.
                }

                Image img = cp.Render(); // Returns the rendered image.
                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

            
        }
    }
}
