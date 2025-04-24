using System;
using NumSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;

namespace RL.Environment
{
    public class GymCartPoleAdapter : IEnvironment
    {
        private CartPoleEnv _gymEnv;

        public GymCartPoleAdapter()
        {
            // Initialize the CartPole environment with Avalonia viewer if needed.
            _gymEnv = new CartPoleEnv(AvaloniaEnvViewer.Factory);
        }

        public NDArray Reset()
        {
            // Ensure _gymEnv.Reset() returns a NumSharp.NDArray.
            return _gymEnv.Reset();
        }

        public (NDArray nextState, float reward, bool done, object info) Step(int action)
        {
            var (nextState, reward, done, info) = _gymEnv.Step(action);
            Console.WriteLine($"Raw reward: {reward}");
            return (nextState, (float)reward, done, info);
        }


        public object Render()
        {
            return _gymEnv.Render();
        }
    }
}
