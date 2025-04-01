using RL.Environment; // Make sure this matches the namespace in IEnvironment.cs

namespace RL.PPO
{
    public abstract class PPOBase
    {
        public float LearningRate { get; protected set; }
        public float Gamma { get; protected set; }
        public int Horizon { get; protected set; }

        protected PPOBase(float learningRate, float gamma, int horizon)
        {
            LearningRate = learningRate;
            Gamma = gamma;
            Horizon = horizon;
        }

        // Use the fully qualified type for clarity:
        public abstract float RunEpisode(IEnvironment env);
        public abstract void Save(string path);
        public abstract void Load(string path);
    }
}
