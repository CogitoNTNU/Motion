using System;
using System.Linq;
using NumSharp;
using OneOf;
using RL.Environment;                         // Your GymCartPoleAdapter
using RLMatrix.Common;                        // Contains PPOAgentOptions and TransitionPortable<T>
using RLMatrix.Agents.PPO.Implementations;    // Contains PPOAgentFactory<T>
using RLMatrix;
public class MyRLExperiment
{
    public void RunPPO()
    {
        // Define PPO hyperparameters using the PPOAgentOptions from RLMatrix.Common.
        var options = new PPOAgentOptions(
            batchSize: 16,
            memorySize: 10000,
            gamma: 0.99f,
            gaeLambda: 0.95f,
            lr: 1e-3f,
            depth: 2,
            width: 1024,
            clipEpsilon: 0.2f,
            vClipRange: 0.2f,
            cValue: 0.5f,
            ppoEpochs: 10,
            clipGradNorm: 0.5f,
            entropyCoefficient: 0.1f,
            useRNN: false
        );

        // For CartPole: action space is discrete with 2 actions, and state size is 4.
        int[] actionSizes = new int[] { 2 };
        OneOf<int, (int, int)> stateSize = 4;

        // Use float[] as the state type.
        var agent = PPOAgentFactory<float[]>.ComposeDiscretePPOAgent(options, actionSizes, stateSize);

        // Create your environment adapter.
        IEnvironment env = new GymCartPoleAdapter();

        // Training loop.
        for (int episode = 0; episode < 1000; episode++)
        {
            // Reset the environment and convert the state to a flat float array.
            NDArray stateND = env.Reset();
            double[] stateDoubles = stateND.reshape(-1).ToArray<double>();
            float[] stateFloats = Array.ConvertAll(stateDoubles, d => (float)d);
            float episodeReward = 0f;
            bool done = false;

            while (!done)
            {
                // The agent expects an array of states (float[][]), one per instance.
                float[][] stateBatch = new float[][] { stateFloats };

                // Select actions using the agent.
                int[][] actions = agent.SelectActions(stateBatch, isTraining: true);
                int action = actions[0][0];  // For CartPole, one action per state

                // Step the environment.
                var (nextStateND, reward, stepDone, _) = env.Step(action);
                episodeReward += reward;

                // Convert the next state NDArray to a flat float array.
                double[] nextStateDoubles = nextStateND.reshape(-1).ToArray<double>();

                float[] nextStateFloats = Array.ConvertAll(nextStateDoubles, d => (float)d);

                // Create a transition (using TransitionPortable<float[]> since our state is float[]).
                var transition = new TransitionPortable<float[]>(
                    Guid.NewGuid(),   // Unique identifier
                    stateFloats,      // Current state as float[]
                    new int[] { action },
                    new float[0],     // For continuous actions (none in CartPole), use an empty array.
                    reward,
                    null              // For simplicity, set nextTransitionGuid to null.
                );

                // Add the transition to the agent's memory.
                agent.AddTransition(new[] { transition }.ToList());

                // Move to the next state.
                stateFloats = nextStateFloats;
                done = stepDone;
            }

            // After each episode, optimize the model.
            agent.OptimizeModel();
            Console.WriteLine($"Episode {episode}, total reward = {episodeReward}");
        }

        // Save the trained model.
        agent.Save("ppoModel.pt");
        Console.WriteLine("Training complete!");
    }
}
