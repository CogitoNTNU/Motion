using RL.PPO;
public static class PPOFactory
{
    public static PPOBase CreatePPO(
        bool useCuda,
        float learningRate = 1e-3f,
        float gamma = 0.99f,
        int horizon = 2048
    )
    {
        if (useCuda)
        {
            // Check if CUDA is actually available if you like
            // if (!torch.cuda.is_available()) fallback to CPU
            return new PPOCpu(learningRate, gamma, horizon);
        }
        else
        {
            return new PPOCpu(learningRate, gamma, horizon);
        }
    }
}
