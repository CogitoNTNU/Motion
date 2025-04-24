using NumSharp;

namespace RL.Environment
{
    public interface IEnvironment
    {
        NDArray Reset();
        (NDArray nextState, float reward, bool done, object info) Step(int action);
        object Render();
    }
}
