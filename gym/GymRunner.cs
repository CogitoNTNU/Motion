using System;
using System.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;
using Avalonia;

namespace Motion
{
    class GymRunner
    {
        public static void RunGymEnvironment()
        {   Agent agent = Agent.InitializeAgent(4,1,5);
        
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
            bool done = true;
            float prediction = 0;
            var action = 0;
            
            for (int i = 0; i < 100_000; i++)
            {
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    //var action = (i % 2); // switching between left and right
                    
                    
                    if(prediction < 0.5 ){action = 0;}
                    else{action = 1;}
    
                    var (observation, reward, _done, information) = cp.Step(action); 
                    done = _done;
                    prediction = (float)agent.ForwardPass((double[])observation)[0];
                    // Do something with the reward and observation.

                    // Console.WriteLine("reward:   " + reward);
                    Console.WriteLine("prediction:   " + prediction);
                    Console.WriteLine("action:   " + action);
                    
                   

                    
                    
                }

                SixLabors.ImageSharp.Image img = cp.Render(); //returns the image that was rendered.
                Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

          
        }
    }
}
