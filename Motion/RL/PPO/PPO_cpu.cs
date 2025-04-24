using TorchSharp;
using static TorchSharp.torch;
using RL.Environment;
using RL.PPO;
public class PPOCpu : PPOBase
{
    // CPU-specific fields (e.g., CPU device in TorchSharp)
    private readonly torch.Device _device;

    public PPOCpu(float learningRate, float gamma, int horizon)
        : base(learningRate, gamma, horizon)
    {
        _device = torch.CPU; // or detect CPU device
        // Initialize CPU-specific layers, optimizers, etc.
    }

    public override float RunEpisode(IEnvironment env)
    {
        // Implement training logic on CPU
        // Use _device or CPU-specific code
        float totalReward = 0f;
        // ...
        return totalReward;
    }

    public override void Save(string path)
    {
        // Save CPU model
    }

    public override void Load(string path)
    {
        // Load CPU model
    }
}
