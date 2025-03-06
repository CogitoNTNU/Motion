
using System;
using System.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;

class Fitness{
    public static float FitnessFunction(int stepsize){
        CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
        bool done = true;
        float total_rerward = 0;
            for (int i = 0; i < 100_000; i++)
            {
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    var action = (i % 2);
                    var (observation, reward, _done, information) = cp.Step(action); // switching between left and right
                    done = _done;
                    // Do something with the reward and observation.
                    total_rerward += reward;
                    
                }

                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

        return total_rerward;
    }
}

