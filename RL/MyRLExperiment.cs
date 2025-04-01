using System;
using RL.Environment;
// Suppose you have a PPO base or a factory in RL.PPO

public class MyRLExperiment
{
    public void RunPPO(bool useCuda)
    {
        // 1. Create the environment via the adapter
        IEnvironment env = new GymCartPoleAdapter();

        // 2. Create your PPO agent (CPU or GPU) via your PPOFactory or direct constructor
        // Example:
        var ppoAgent = PPOFactory.CreatePPO(
            useCuda: false,         // or true if you have a CUDA build
            learningRate: 1e-3f,
            gamma: 0.99f,
            horizon: 2048
        );

        // 3. Train for some episodes
        for (int episode = 0; episode < 1000; episode++)
        {
            float totalReward = ppoAgent.RunEpisode(env);
            Console.WriteLine($"Episode {episode}, total reward = {totalReward}");
        }

        // 4. Optionally save your model
        ppoAgent.Save("ppoModel.pt");
        Console.WriteLine("Training complete!");
    }
}
