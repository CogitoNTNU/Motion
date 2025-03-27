using System;
using TorchSharp;

namespace Motion
{
    public class Training
    {
    public static void TrainingDQN(DQNModel model, torch.optim.Optimizer optimizer, PopulationManager popManager, double gamma, int batchSize)
    {
        if (popManager.States.Count < batchSize)
        {
            Console.WriteLine("Not enough experiences to train");
            return;
        }

        Random rnd = new Random();
        List<int> indices = new List<int>();
        for (int i = 0; i < batchSize; i++)
        {
            indices.Add(rnd.Next(popManager.States.Count));
        }

        double[][] stateBatch = new double[batchSize][];
        double[][] nextStateBatch = new double[batchSize][];
        int[] actionsBatch = new int[batchSize];
        float[] rewardsBatch = new float[batchSize];
        float[] donesBatch = new float[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            int idx = indices[i];
            stateBatch[i] = popManager.States[idx].ToArray<double>();
            nextStateBatch[i] = popManager.NextStates[idx].ToArray<double>();
            actionsBatch[i] = popManager.Actions[idx];
            rewardsBatch[i] = popManager.Rewards[idx];
            donesBatch[i] = popManager.Dones[idx] ? 1.0f : 0.0f;
        }
    }

    }
}